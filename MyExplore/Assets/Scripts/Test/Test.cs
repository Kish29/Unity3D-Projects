using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    private float _agent;

    private Vector3 _cameraRotate;

    private Camera _camera;

    private float _moveSpeed = 5f;
    private float _jmpSpeed = 5.0f;
    private float _jmpTime = 0;

    private float _gravity = 9.8f;

// 抛物运动过程的速度
    private float _parabolicSpeed = 0;

    // 定义可以几段条的次数
    private int _jmpCounts;
    private const int JumpCountsValue = 3;

    private float _fixedFrameTime;

    // 定义切换视角的参数
    private float _cameraToggleSpeed = 0f;

    // 切换的方向，true为第三人称，false为第一人称
    private bool _cameraToggle = false;

    // 转换过程按V无效
    private bool _enableChangeView = true;
    private float _cameraToggleCounts = 0;
    private float _cameraToggleDistance = 16f;
    private float _cameraToggleTime = 0;

    private float _radius;

    private Transform _head;

    void Start()
    {
        _agent = 0f;
        foreach (Transform child in transform)
        {
            if (_camera == null)
                _camera = child.GetComponentInChildren<Camera>();
            if (child.CompareTag("head"))
            {
                _head = child;
            }
        }

        _fixedFrameTime = Time.deltaTime;
        _radius = transform.localScale.x / 2;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Control()
    {
        float mouseHorizontal = Input.GetAxis("Mouse X");
        float mouseVertical = Input.GetAxis("Mouse Y");
        _cameraRotate.x -= mouseVertical;
        _cameraRotate.y += mouseHorizontal;

        var control = _cameraRotate;
        control.x = 0;
        transform.eulerAngles = control;
        // 记住人物先旋转，相机后旋转，效果更好一些
        _camera.transform.eulerAngles = _cameraRotate;

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

        if (_cameraToggleTime > 0)
        {
            _cameraToggleSpeed += _gravity * _fixedFrameTime * 2;
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

        transform.Translate(motion);
        _camera.transform.position = _head.TransformPoint(0, -_cameraToggleCounts * 0.01125f, _cameraToggleCounts);
    }

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
            _cameraToggleTime = (float) Math.Sqrt(2 * _cameraToggleDistance / (_gravity * 2));
            _cameraToggleSpeed = 0;
            _enableChangeView = false;
        }

        Control();
        //_agent += Time.deltaTime * 50;
        /*
         * localEulerAngles是本物体的旋转角度，当其变为某个父物体的子物体时，角度是相对于父物体的
         * eulerAngles是本问题相对于世界的的角度，不受父物体的角度影响
         */
        //transform.localEulerAngles = new Vector3(0, _agent, 0);
    }
}