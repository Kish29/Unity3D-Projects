using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleCtr : MonoBehaviour
{
    //private CharacterController _controller;

    private float _moveSpeed = 3.0f;

    private float _jmpSpeed = 5.0f;

    private float _jmpTime = 0;

    // 抛物运动过程的速度
    private float _parabolicSpeed = 0;
    private float _fixedFrameTime;

    private Transform _cameraTransform;

    private Vector3 _cameraRotate;

    private float _gravity = 9.8f;

    // 定义可以几段条的次数
    private int _jmpCounts;
    private const int JumpCountsValue = 3;

    // 定义切换视角的参数
    private float _cameraToggleSpeed = 0f;

    // 切换的方向，true为第三人称，false为第一人称
    private bool _cameraToggle = false;

    // 转换过程按V无效
    private bool _enableChangeView = true;
    private float _cameraToggleCounts = 0;
    private float _cameraToggleDistance = 2.5f;
    private float _cameraToggleTime = 0;

    private float _radius;

    // Start is called before the first frame update
    void Start()
    {
        //_controller = GetComponent<CharacterController>();
        _fixedFrameTime = Time.deltaTime;
        _radius = transform.localScale.x / 2;
        if (Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
            _cameraTransform.transform.position = transform.TransformPoint(0, 0, 0);
            _cameraTransform.rotation = transform.rotation;
            // 相机的旋转角度与role一致
            _cameraTransform.eulerAngles = transform.eulerAngles;
            // 获得摄像机角度
            _cameraRotate = _cameraTransform.eulerAngles;
            _cameraRotate.z = 0;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Move()
    {
        // 旋转视角时更新摄像机的角
        /*
         * 注意理解，Mouse X是鼠标左右平移的距离，动作应当是绕Y轴旋转
         * Mouse Y是鼠标相上下平移的距离，动作应当是绕X轴旋转
         * 旋转角度为正：顺时针， 负：逆时针
         * 方向从轴的正方向看过去（箭头指向屏幕向外）
         */
        float mouseHorizontal = Input.GetAxis("Mouse X");
        float mouseVertical = Input.GetAxis("Mouse Y");
        // Vertical为正时，说明向上看，Mouse Y是正，但是要逆时针旋转
        _cameraRotate.x -= mouseVertical;
        _cameraRotate.y += mouseHorizontal;
        _cameraTransform.eulerAngles = _cameraRotate;
        // 使主角的方向与摄像机一致
        var roleRotate = _cameraRotate;
        roleRotate.x = 0; // 主角物体不绕x轴旋转，记得置零
        transform.eulerAngles = roleRotate;

        Vector3 motion = Vector3.zero;
        motion.x = Input.GetAxis("Horizontal") * _moveSpeed * _fixedFrameTime;
        motion.z = Input.GetAxis("Vertical") * _moveSpeed * _fixedFrameTime;

        // 在地面时可以起跳
        if (IsGrounded())
        {
            // 地面恢复次数
            _jmpCounts = JumpCountsValue;
            // 上抛初速度置零
            _parabolicSpeed = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                // 总的上升时间
                _jmpTime = _jmpSpeed / _gravity;
                // 做抛物运动的初速度
                _parabolicSpeed = _jmpSpeed;
                // 跳跃次数-1
                _jmpCounts--;

                // 上升
                motion.y += _parabolicSpeed * _fixedFrameTime;
                _jmpTime -= _fixedFrameTime;
                _parabolicSpeed -= _gravity * _fixedFrameTime;
            }
        }
        else
        {
            // 如果处于继续上升的时间段
            if (_jmpTime > 0)
            {
                // 竖直方向上是匀减速运动
                motion.y += _parabolicSpeed * _fixedFrameTime;
                _jmpTime -= _fixedFrameTime;
                _parabolicSpeed -= _gravity * _fixedFrameTime;
            }
            else
            {
                // 如果不在地面
                if (!IsGrounded())
                {
                    // 下降是匀加速运动
                    motion.y -= _parabolicSpeed * _fixedFrameTime;
                    _parabolicSpeed += _gravity * _fixedFrameTime;
                }
            }

            /* n段跳
            * 因为这里是每一帧调用，所以如果使用GetKey的话就会造成上一次的空格还没释放，函数运行到这儿的时候仍然检测到有空格键
            * 所以要用GetKeyDown*/
            if (Input.GetKeyDown(KeyCode.Space) && _jmpCounts != 0)
            {
                _jmpTime = _jmpSpeed / _gravity;
                _parabolicSpeed = _jmpSpeed;
                _jmpCounts--;
            }
        }

        /*TransformDirection返回motion相对于世界坐标的相对偏移量！！！！！
        * Move需要传入的参数是相对于世界坐标的偏移量！！！！！(想了好久，md！)
        */
        // 相应的这儿就要用Translate函数
        transform.Translate(motion);
        //_controller.Move(transform.TransformDirection(motion));
        // 切换视角的移动平滑移动效果
        if (_cameraToggleTime > 0)
        {
            _cameraToggleSpeed += _gravity * _fixedFrameTime;
            _cameraToggleTime -= _fixedFrameTime;
            if (_cameraToggle)
                _cameraToggleCounts -= _cameraToggleSpeed * _fixedFrameTime;
            else
                _cameraToggleCounts += _cameraToggleSpeed * _fixedFrameTime;
        }
        else
        {
            _enableChangeView = true;
        }

        _cameraTransform.position = transform.TransformPoint(0, -_cameraToggleCounts * 0.2f, _cameraToggleCounts);
    }

    // 因为CharacterController的isGrounded每次调用Move函数后默认置为false，会出bug，所以用长度为物体半径的向下的射线来检测碰撞自定义地面逻辑
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _radius);
    }

// Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.V) && _enableChangeView)
        {
            _cameraToggle = !_cameraToggle;
            _cameraToggleTime = (float) Math.Sqrt(2 * _cameraToggleDistance / _gravity);
            _cameraToggleSpeed = 0;
            _enableChangeView = false;
        }

        Move();
    }

    private void FixedUpdate()
    {
    }
}