using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI控件命名空间的引用
using UnityEngine.Events; // UI事件命名空间的引用
using UnityEngine.EventSystems; // UI事件命令空间的引用

public class GameManager : MonoBehaviour
{
    // 实例
    public static GameManager Instance;

    public LayerMask groundLayer; // 地面碰撞层

    public int gameRound = 1; // 进攻波数

    public int maxGameRound = 10; // 最大波数

    public int roleLife = 10; // 角色生命值

    public int coinsNum = 30; // 铜钱数量

    // UI 文字控件
    private Text _round;

    private Text _health;

    private Text _coins;

    private Button _retry; // 重试按钮

    private bool _isSelect; // 当前是否选中的创建防守单位的按钮

    private void Awake()
    {
        // 实例化本类
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _isSelect = false;
        // 创建UnityAction，在OnButtonCreateDefenderDown函数中相应按钮按下的事件
        var downAction = new UnityAction<BaseEventData>(OnButtonCreateDefenderDown);
        // 创建UnityAction，在OnButtonCreateDefenderDown函数中相应按钮抬起的事件
        var upAction = new UnityAction<BaseEventData>(OnButtonCreateDefenderUp);

        // 按钮按下事件
        var down = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerDown;
        down.callback.AddListener(downAction);

        // 按钮抬起事件
        var up = new EventTrigger.Entry();
        up.eventID = EventTriggerType.PointerUp;
        up.callback.AddListener(upAction);

        // 查找所有子物体，根据名称获取UI控件
        foreach (var t in GetComponentsInChildren<Transform>())
        {
            // 波数
            if (String.Compare(t.name, "Round", StringComparison.Ordinal) == 0)
            {
                _round = t.GetComponent<Text>();
                SetRound(1);
            }
            else if (String.Compare(t.name, "Health", StringComparison.Ordinal) == 0)
            {
                _health = t.GetComponent<Text>();
                // 设置生命值
                _health.text = $"生命：<color=yellow>{roleLife}</color>";
            }
            else if (String.Compare(t.name, "Coins", StringComparison.Ordinal) == 0)
            {
                _coins = t.GetComponent<Text>();
                _coins.text = $"铜钱：<color=yellow>{coinsNum}</color>";
            }
            // 重置游戏按钮
            else if (String.Compare(t.name, "ButtonRetry", StringComparison.Ordinal) == 0)
            {
                _retry = t.GetComponent<Button>();
                _retry.onClick.AddListener(
                    delegate { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
                );
                // 隐藏重置按钮
                _retry.gameObject.SetActive(false);
            }
            // 按钮控件----创建防守单位
            else if (t.name.Contains("ButtonPlayer"))
            {
                EventTrigger trigger = t.gameObject.AddComponent<EventTrigger>();
                trigger.triggers = new List<EventTrigger.Entry>();
                trigger.triggers.Add(down);
                trigger.triggers.Add(up);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 将鼠标控制摄像机移动的事件传递给摄像机
        if (_isSelect) // 选中角色进行布置操作时不做出响应
            return;
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            // ios版本的逻辑控制
            // android版本的逻辑控制
            bool press = Input.touches.Length > 0 ? true: false;
            float mx = 0f;
            float my = 0f;
            if (press) {  
                if(Input.GetTouch(0).phase == TouchPhase.Moved) {    // 获得手指移动的位置
                    mx = Input.GetTouch(0).deltaPosition.x * 0.01f;
                    my = Input.GetTouch(0).deltaPosition.y * 0.01f;
                }
            }
#else
        bool press = Input.GetMouseButton(0);
        // 获得鼠标移动距离
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
#endif
        GameCamera.Instance.Control(press, mx, my);
    }

    private void OnButtonCreateDefenderDown(BaseEventData data)
    {
        _isSelect = true;
    }

    private void OnButtonCreateDefenderUp(BaseEventData data)
    {
        GameObject go = data.selectedObject;
    }

    private void SetRound(int val)
    {
        gameRound = val;
        _round.text = $"波数：<color=yellow>{gameRound}/{maxGameRound}</color>";
    }

    // 减少生命
    public void SetDamage(int damage)
    {
        roleLife -= damage;
        if (roleLife <= 0)
        {
            roleLife = 0;
            _retry.gameObject.SetActive(true); // 显示重新开始游戏按钮
        }

        _health.text = $"生命：<color=yellow>{roleLife}</color>";
    }

    // 设置铜钱
    private bool SetCoins(int coins)
    {
        if (coinsNum + coins <= 0)
        {
            return false;
        }

        coinsNum += coins;
        _coins.text = $"铜钱：<color=yellow>{coinsNum}</color>";
        return true;
    }
}