using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public static GameCamera Instance;

    // 摄像机距离地面的距离
    protected float DstToGrd = 15;

    // 摄像机的角度
    protected Vector3 CameraRotate = new Vector3(40, 0, 0);

    // 摄像机移动速度
    protected float CameraMoveSpeed = 60.0f;

    // 摄像机的移动距离
    protected float Vx = 0f;

    protected float Vy = 0f;

    // 摄像机焦点
    protected Transform CameraFocus;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 获得摄像机焦点
        CameraFocus = CameraPoint.Instance.transform;
        Follow();
    }

    // 跟随这个焦点
    private void Follow()
    {
        // 设置旋转角度
        transform.eulerAngles = CameraRotate;
        // 摄像机移动到指定位置
        transform.position = CameraFocus.TransformPoint(new Vector3(0, DstToGrd, 0));
        // 摄像头对准焦点
        // transform.LookAt(CameraFocus);
    }

    // 控制摄像机的移动
    public void Control(bool mouse, float mx, float my)
    {
        if (!mouse)
            return;
        CameraFocus.eulerAngles = Vector3.zero;
        CameraFocus.Translate(-mx, 0, -my);
    }

    private void LateUpdate()
    {
        Follow();
    }

    // Update is called once per frame
    void Update()
    {
    }
}