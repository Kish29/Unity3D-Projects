using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI; //UI名称域
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;

//关卡名称域

[AddComponentMenu("Game/GameManager")]
public class GameManager : MonoBehaviour
{
    //静态GameManager实例（很重要！！！）
    public static GameManager Instance = null;

    //获取mainCamera组件
    private Transform _mainCamera;

    //游戏得分
    private long _score = 0;

    //弹药数量
    private int _shellNumbers = 200;

    //游戏主角
    private Player _player;

    //游戏敌人
    private Enemies _enemies;

    //UI文字
    private Text _shellnumbers;

    public Text playerHealth;

    private Text _scoreText;

    private Text _coldingTime;

    public Text speeding;

    private Button _restartButton;

    //三个slider
    public Slider slider;
    [FormerlySerializedAs("Slider2")] public Slider slider2;
    private Slider _coldTimer;

    //枪口的Transform
    private Transform _weaponFirePoint;

    //射击时，射线能碰到的碰撞层
    public LayerMask layerMask;

    //射击音效
    private AudioSource _audioSource;
    public AudioClip audioClip;

    //保存射线检测的结果
    private RaycastHit _raycastHit;

    //射击点的粒子特效
    public Transform shootEffect;

    //生成敌人的数量
    public static int enemyNumbers = 0;

// Start is called before the first frame update
    void Start()
    {
        Instance = this;

        //定义一个camera再获取枪口组件
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

        //初始化枪口位置
        _weaponFirePoint = _mainCamera.Find("M16/weapon/muzzlepoint").transform;

        //初始话音效文件
        _audioSource = this.GetComponent<AudioSource>();

        //获得主角
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //获得UI文字
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");

        //foreach遍历获得组件
        foreach (Transform all in canvas.transform.GetComponentsInChildren<Transform>())
        {
            //获得弹药数量
            if (String.Compare(all.name, "ShellNumbers", StringComparison.Ordinal) == 0)
            {
                _shellnumbers = GameObject.FindGameObjectWithTag("numbers").GetComponent<Text>();
            }
            //获得主角血量
            else if (String.Compare(all.name, "textHealth", StringComparison.Ordinal) == 0)
            {
                playerHealth = GameObject.FindGameObjectWithTag("health").GetComponent<Text>();
            }
            //获得分数
            else if (String.Compare(all.name, "score", StringComparison.Ordinal) == 0)
            {
                _scoreText = all.GetComponent<Text>();
                _scoreText.text = "Scores-><color=yellow>" + _score + "</color>";
            }
            //获得重新开始组件
            else if (String.Compare(all.name, "ButtonRestart", StringComparison.Ordinal) == 0)
            {
                _restartButton = all.GetComponent<Button>();
                _restartButton.onClick.AddListener(delegate
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                });
                //游戏初期隐藏重新开始按钮
                _restartButton.gameObject.SetActive(false);
            }
            //获取Slider按钮
            else if (String.Compare(all.name, "Slider", StringComparison.Ordinal) == 0)
            {
                slider = all.GetComponent<Slider>();
            }
            else if (String.Compare(all.name, "Slider2", StringComparison.Ordinal) == 0)
            {
                slider2 = all.GetComponent<Slider>();
                speeding = GameObject.FindGameObjectWithTag("Speeding").GetComponent<Text>();
            }
            else if (String.Compare(all.name, "ColdTimer", StringComparison.Ordinal) == 0)
            {
                _coldTimer = all.GetComponent<Slider>();
                _coldingTime = GameObject.FindGameObjectWithTag("coldingTime").GetComponent<Text>();
            }
        }
    }

    //更新分数
    public void ScoreUpdate(int currentscore)
    {
        _score += currentscore;
        _scoreText.text = "Scores-><color=yellow>" + _score + "</color>";
    }

    //装填弹药时间
    private float _shellNumberUpdateTime = 3.5f;

    //装填弹药
    //并且需要一个flag
    private string _shellUpdateFlag = "No_F";

    private void _ShellNumberUpdate()
    {
        if (_shellNumberUpdateTime <= 0)
        {
            _shellNumbers = 200;
            _coldTimer.value = 1;
            _coldingTime.text = "";
            _shellNumberUpdateTime = 3.5f;
            _shellUpdateFlag = "No_F";
            return;
        }

        //每一帧更新
        _shellNumberUpdateTime -= 1 * Time.deltaTime;
        _coldTimer.value = _shellNumberUpdateTime / 3.5f;

        //显示当前冷却剩余时间，保留2位小数
        _coldingTime.text = String.Format("{0:N2}", _shellNumberUpdateTime);
    }

    //定义发射时间
    private float _coldTime = 0.10f;

// Update is called once per frame
    private void Update()
    {
        //主角生命值小于等于0，什么也不做
        if (_player.playerHealth <= 0)
        {
            _restartButton.gameObject.SetActive(true);
            return;
        }

        //按下鼠标左键弹药减少（注意判断条件）
        if (Input.GetMouseButton(0) && _shellNumbers > 0 && Math.Abs(_shellNumberUpdateTime - 3.5f) <= 0)
        {
            _coldTime -= Time.deltaTime;
            //冷却时间到了才可以发射
            if (_coldTime <= 0)
            {
                _shellNumbers--;
                _audioSource.PlayOneShot(audioClip);

                //从muzzlepoint的位置，向摄像机的正前方射出一根射线，并且射线只能与layerMask所指定的层碰撞
                bool ishit = Physics.Raycast(_weaponFirePoint.transform.position,
                    _mainCamera.TransformDirection(Vector3.forward), out _raycastHit, 100, layerMask);
                //注：_raycastHit检测的结果是返回碰撞到的物体的信息                                                            

                //如果射线碰到了物体
                if (ishit)
                {
                    //如果射线射中了敌人
                    if (String.Compare(_raycastHit.transform.tag, "Enemy", StringComparison.Ordinal) == 0)
                    {
                        Enemies enemies = _raycastHit.transform.GetComponent<Enemies>();
                        enemies.onDamage();
                    }

                    //在射中的地方释放一个射击特效
                    Instantiate(shootEffect, _raycastHit.point, _raycastHit.transform.rotation);
                }

                _coldTime = 0.10f;
            }
        }

        //实时更新弹药数据
        _shellnumbers.text = _shellNumbers + "/200";
        //按下F键补充弹药
        if (Input.GetKeyDown(KeyCode.F))
        {
            //设置标志
            _shellUpdateFlag = "Yes_F";
        }

        //标志成立时每帧调用
        if (_shellUpdateFlag == "Yes_F")
            _ShellNumberUpdate();
    }
}