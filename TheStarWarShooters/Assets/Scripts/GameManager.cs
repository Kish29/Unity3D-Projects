using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[AddComponentMenu("MyGame/GameManager")]
public class GameManager : MonoBehaviour
{
    //静态GameManager实例
    public static GameManager Instance;

    //
    private long P_scoretimer = 50;
    private float P_scoretimer1 = 3;
    private bool P_scoretimer2 = false;

    //创建游戏分数，游戏结束的UI画面
    public Transform canvas_menu;
    public Transform gameover_ui;

    //创建游戏分数和生命的txt文档
    private Text gamescorer; //分数文字
    private Text playerlife; //生命文字
    private Text enemylife; //敌人生命文字
    private Text bestscore; //最高分文字
    private Button musichange; //切换音乐按钮

    //创建得分数值、最高分数值，获取主角实体
    private long P_score = 0;
    public static long P_bestscore = 0;
    private Player P_player;

    //创建游戏声音
    public AudioClip gameaudio;
    private AudioSource gameaudiosource;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        gameaudiosource = this.gameObject.AddComponent<AudioSource>(); //这儿的语句可以直接在实例化对象中添加AudioSource组件
        gameaudiosource.clip = gameaudio; //获取声音文件
        gameaudiosource.loop = true; //默认循环播放
        gameaudiosource.Play(); //开始播放音乐

        //获取主角实体，通过Tag来进行查找
        P_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //获取分数的UI组件
        gamescorer = canvas_menu.transform.Find("Text_Score").GetComponent<Text>();
        playerlife = canvas_menu.transform.Find("Text_Life").GetComponent<Text>();
        bestscore = canvas_menu.transform.Find("Text_Best").GetComponent<Text>();
        enemylife = canvas_menu.transform.Find("Text_Enemy_Life").GetComponent<Text>();
        //或取音乐切换按钮组件
        musichange = canvas_menu.transform.Find("Music_Button").GetComponent<Button>();
        //对这些组件进行初始化
        gamescorer.text = string.Format("分数  {0}", P_score);
        playerlife.text = string.Format("生命  {0}", P_player.heath);
        bestscore.text = string.Format("最高分  {0}", P_bestscore);
        //获取gameover的UI组件
        var restart_button = gameover_ui.transform.Find("Button_Restart").GetComponent<Button>();
        restart_button.onClick.AddListener(delegate() //设置重新开始游戏按钮事件
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }); //重新开始当前关卡
        gameover_ui.gameObject.SetActive(false);
    }

    //获取敌人组件，然后再collider函数调用
    public void GetEnemyHealth(float enemylife)
    {
        this.enemylife.text = string.Format("敌人血量  {0}", enemylife);
    }

    //设置增加分数
    public void AddScore(long point)
    {
        P_score += point; //分数会随着杀死敌人的数量进行更新
        if (P_bestscore < P_score)
        {
            P_bestscore = P_score;
        }

        gamescorer.text = string.Format("分数  {0}", P_score);
        bestscore.text = string.Format("最高分  {0}", P_bestscore);
    }

    //改变生命值UI显示
    public void ChangeLife(float health)
    {
        playerlife.text = string.Format("生命  {0}", health); //更新生命值
        if (health <= 0)
        {
            gameover_ui.gameObject.SetActive(true); //生命归为0后显示结束画面
            this.P_scoretimer2 = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        P_scoretimer1 -= Time.deltaTime;
        if (P_scoretimer1 <= 0 && !P_scoretimer2)
        {
            AddScore(P_scoretimer);
            P_scoretimer1 = 3;
        }
    }
}