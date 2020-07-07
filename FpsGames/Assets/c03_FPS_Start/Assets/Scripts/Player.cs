using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    //这是主角的Transform
    public Transform playerTramsform;

    //添加摄像机的Transform
    public Transform cameraTransform;

    //添加摄像机的旋转角度（redirect camera）
    private Vector3 _cameraRotation;

    //摄像机高度，即主角的升高
    private float _playerHeight = 0.3f;

    //角色控制器组件
    private CharacterController _playerCharactor;

    //角色移动速度
    private float _playerMovespeed = 10.0f;

    //定义加速时间
    private float _speedUpTime = 10.0f;

    //定义加速的恢复时间
    //private float _SpeedUpRecoverTime = 3.0f;
    //主角的重力加速度
    //private float PlayerGravity = 9.80f;

    //定义主角的跳跃初速度
    //private float PlayerJumpSpeed = 4.50f;

    //主角生命值
    public float playerHealth = 100.0f;

    // Start is called before the first frame update
    //初始化摄像机的位置和旋转角度，并锁定鼠标
    void Start()
    {
        //初始化全屏
        Screen.fullScreen = true;
        //获取主角控制器组件
        playerTramsform = this.transform;
        _playerCharactor = this.GetComponent<CharacterController>();

        //获取摄像机
        //该脚本使用public的Transform在游戏中直接指定任何摄相机
        //CameraTransform = Camera.main.transform;

        //设置摄像机的初始位置，用TransformPoin偏移一定位置
        cameraTransform.position = playerTramsform.TransformPoint(0, _playerHeight, 0);

        //初始化时设置摄像机的旋转方向与主角一致
        cameraTransform.rotation = playerTramsform.rotation;
        //初始化时角度也要一致
        _cameraRotation = playerTramsform.eulerAngles;

        //不使用Sceen.lockCursor方法，使用新的Cursor类调用lockState方式来锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //锁定鼠标
        //Screen.lockCursor = true;
    }

    // Update is called once per frame
    void Update()
    {
        //按下esc键推出
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Screen.fullScreen = false;
        }
        else if (Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //如果生命值降为0，那么什么也不做
        if (playerHealth <= 0)
            return;
        Control();

        //注：GetKeyDown只能检测一次的shift键按下，故用GetKey函数
        if (Input.GetKey(KeyCode.LeftShift) && _speedUpTime > 0)
        {
            GameManager.Instance.speeding.text = "Speed up...";
            _playerMovespeed = 20.0f;
            _speedUpTime -= 1 * Time.deltaTime; //加速时间减少
            GameManager.Instance.slider2.value = _speedUpTime / 10.0f;
        }
        else
            _playerMovespeed = 10.0f;

        if (!Input.GetKey(KeyCode.LeftShift) && _speedUpTime <= 10.0f)
        {
            GameManager.Instance.speeding.text = "";
            _speedUpTime += 2 * Time.deltaTime; //只有在松开的时候才能恢复加速时间
            GameManager.Instance.slider2.value = _speedUpTime / 10.0f;
        }

//        if (Input.GetKeyDown(KeyCode.LeftShift))
//            Debug.Log("PressLeftShift");
        //每帧调用，显示主角生命值
        GameManager.Instance.playerHealth.text = "" + playerHealth;
    }

    //控制主角移动代码，并且在该函数中旋转摄像机时，移动主角后使得摄像机的角度和主角一致
    void Control()
    {
        //获取鼠标移动的距离
        float rh = Input.GetAxis("Mouse X");
        float vh = Input.GetAxis("Mouse Y");

        //旋转摄像机
        _cameraRotation.x -= vh; //回过头来重新看一下这两行代码的 -= 和 +=
        _cameraRotation.y += rh;
        //让摄像机的旋转角度参考鼠标的移动
        cameraTransform.eulerAngles = _cameraRotation;
        //然后使主角的面向方向与摄像机一致
        Vector3 camrotat = cameraTransform.eulerAngles;
        camrotat.x = 0;
        camrotat.z = 0;
        playerTramsform.eulerAngles = camrotat;

        //移动的方向和距离
        Vector3 motion = Vector3.zero;
        motion.z = Input.GetAxis("Vertical") * _playerMovespeed * Time.deltaTime;
        motion.x = Input.GetAxis("Horizontal") * _playerMovespeed * Time.deltaTime;

        //motion.y -= PlayerGravity * Time.deltaTime;

        //使用角色控制器提供的Move函数控制主角移动 该函数会提供自动检测碰撞
        _playerCharactor.Move(playerTramsform.TransformDirection(motion));
        //更新摄像机位置（使其与主角一致）
        cameraTransform.position = playerTramsform.TransformPoint(0, _playerHeight, 0);
    }

    public void OnDamage(int damage)
    {
        playerHealth -= damage;
        GameManager.Instance.slider.value = playerHealth / 100;

        //当主角生命值小于等于0时，取消锁定光标
        if (playerHealth <= 0)
            Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.playerHealth.text = "" + playerHealth;
    }
}