using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    // 相机震动方向，这里只获得一个方向
    private Vector3 _shakeRotation;
    private Vector3 _shakeRotation2;

    public Transform cameraTransform;
    // 相机震动时间
    private float shakeTime = 0.8f;

    // 相机震动强度
    private float _shakeValue;

    private float _currenTime;
    private float _toatalTime;

    private void Start()
    {
        _currenTime = 0.0f;
        _toatalTime = 0.0f;
        _shakeValue = 50.0f;
        var eulerAngles = cameraTransform.transform.eulerAngles;
        _shakeRotation = eulerAngles;
        _shakeRotation2 = eulerAngles;
    }

    public void Trigger()
    {
        _toatalTime = shakeTime;
        _currenTime = shakeTime;
    }

    public void ShakeInplement()
    {
        if (_currenTime > 0.0f && _toatalTime > 0.0f)
        {
            //每一帧减少相应的时间            
            _currenTime -= Time.deltaTime;

            //得到当前的时间百分比
            var percent = _currenTime / _toatalTime;
            //需要旋转的量
            var rotationValue = 1.0f - percent;
            _shakeRotation.x -= rotationValue * _shakeValue;
            cameraTransform.transform.eulerAngles = _shakeRotation;
        }
        else
        {
            _currenTime = 0.0f;
            _toatalTime = 0.0f;
            cameraTransform.transform.eulerAngles = _shakeRotation2;
            _shakeRotation = _shakeRotation2;
        }
    }

    private void Update()
    {
        ShakeInplement();
    }
}