using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleCtr : MonoBehaviour
{
    private CharacterController _controller;

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

    // 定义切换视角的距离
    private int _viewDistance = 0;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _fixedFrameTime = Time.deltaTime;
        if (Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
            _cameraTransform.transform.position = transform.TransformPoint(0, 1.6f, _viewDistance);
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
            Debug.Log("I'm isGrounded!");
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
                    Debug.Log("我是分支！");
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
        Debug.Log(_parabolicSpeed);
        _controller.Move(transform.TransformDirection(motion));
        _cameraTransform.position = transform.TransformPoint(0, 1.6f, _viewDistance);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (_viewDistance == 0)
                _viewDistance = -5;
            else
                _viewDistance = 0;
        }

        Move();
    }

    private void FixedUpdate()
    {
    }
}