using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

[AddComponentMenu("MyGame/Player")]
public class Player : MonoBehaviour
{
    //定义向量类，用于获取主角位置
    protected Vector3 player_location;

    private Collider Quad_MeshCollier;

    //定义鼠标射线的碰撞层
    public LayerMask m_inputMask;

    private float m_timer = 0;

    public float heath = 100;

    //add the audio source
    public AudioClip shootaudio; //声音文件要设为public，这样才能在Unity中拖动音效文件进行关联

    private AudioSource _audioSource; //make sure that the audio source is private, prevent to being edited
    //声音源设为私有类型，防止外部将声音源进行修改，声音源用来播放声音

    //define its defend
    public float playerdefend = 30;

    //firstly define player's fly speed
    private float m_speed = 5.0f;

    private Transform fireposition;

    private Transform m_transform;

    //为主角添加一个爆炸特效，由于是生成特效，故用Transform组件
    public Transform explosioneffect;

    //create a transform preferred to rocket
    public Transform r_transform;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = this.transform; //m_transformde的所有信息来自于这个脚本添加到的实例化对象上
        fireposition = transform.Find("fireposition");
        _audioSource = this.GetComponent<AudioSource>(); //声音源直接从Unity中Player添加的AudioSource组件中获得
        //初始化player_location的位置
        player_location = m_transform.position;
        Quad_MeshCollier = GameObject.FindGameObjectWithTag("Quad").GetComponent<MeshCollider>();
    }

    //添加鼠标可控住主角移动的函数
    private void MoveTo()
    {
        //重新定义一个速度，让鼠标点击移动的速度快一点
        float Mouse_speed = 50.0f;

        Vector3 ms = Input.mousePosition; //获得鼠标屏幕位置

        Ray ray = Camera.main.ScreenPointToRay(ms); //讲屏幕位置转化为射线

        RaycastHit hitinfo; //用来记录射线碰撞信息

        bool iscast = Physics.Raycast(ray, out hitinfo, 1000, m_inputMask);
        //如果从鼠标发出的射线射到了m_inputMask所关联的层上（应该是竖直向下射出），然后将射到的层上的点儿的位置信息返回给
        //hitinfo，整个Raycast函数返回的是bool类型的值

        if (iscast)
        {
            //如果射中目标，则记录射线碰撞点
            player_location = hitinfo.point;
        }

        //用Vector3提供的MoveTowards函数，获得朝目标移动的位置
        //注：这个函数是让m_transform的position朝着player_locationm_speed*Time.deltaTime的位置移动
        //速度是m_speed*Time.deltaTime
        Vector3 pos = Vector3.MoveTowards(m_transform.position, player_location, Mouse_speed * Time.deltaTime);
        m_transform.position = pos;
        //重新更新主角的位置
    }


    //create the function to control player's heath
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyRocket")
        {
            EnemyRoket enemyRoket = other.GetComponent<EnemyRoket>();
            this.heath -= enemyRoket.enemyattack;
            GameManager.Instance.ChangeLife(this.heath);
            if (this.heath <= 0)
            {
                Destroy(this.gameObject);
                GameObject.Instantiate(explosioneffect, m_transform.position, Quaternion.identity);
                // Quaternion.identity坐标是Quaternion(0,0,0,0)是旋转前的一个角度，固定值
            }
        }
        else if (other.tag == "AdvEnemyRocket")
        {
            AdvEnemyRocket enemyRoket = other.GetComponent<AdvEnemyRocket>();
            this.heath -= enemyRoket.AdvEneAtta;
            GameManager.Instance.ChangeLife(this.heath);
            if (this.heath <= 0)
            {
                Destroy(this.gameObject);
                GameObject.Instantiate(explosioneffect, m_transform.position, Quaternion.identity);
                // Quaternion.identity坐标是Quaternion(0,0,0,0)是旋转前的一个角度，固定值
            }
        }
        else if (other.tag == "AdvancedEnemy")
        {
            this.heath -= 40;
            GameManager.Instance.ChangeLife(this.heath);
            if (this.heath <= 0)
            {
                Destroy(this.gameObject);
                GameObject.Instantiate(explosioneffect, m_transform.position, Quaternion.identity);
            }
        }
        else if (other.tag == "PlayerRocket")
            return;
        else if (other == Quad_MeshCollier)
            return;
        else
        {
            this.heath -= 20;
            GameManager.Instance.ChangeLife(this.heath);
            if (this.heath <= 0)
            {
                Destroy(this.gameObject);
                GameObject.Instantiate(explosioneffect, m_transform.position, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float move_v = 0; //the vertical distance
        float move_h = 0; //the horizontal distance

        //fire the rocket
        m_timer -= Time.deltaTime;
        if (m_timer < 0)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            {
                GameObject.Instantiate(r_transform, fireposition.position, fireposition.rotation);
                _audioSource.PlayOneShot(shootaudio); //括号里面的是文件，_audioSource用来播放该文件的声音
            }

            m_timer = 0.1f;
        }

        //get the uparrow's message
        if (Input.GetKey(KeyCode.UpArrow))
        {
            move_v += m_speed * Time.deltaTime;
        }

        //get the downarrow's message
        if (Input.GetKey(KeyCode.DownArrow))
        {
            move_v -= m_speed * Time.deltaTime;
        }

        //get the right distance
        if (Input.GetKey(KeyCode.RightArrow))
        {
            move_h += m_speed * Time.deltaTime;
        }

        //get the left distance
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            move_h -= m_speed * Time.deltaTime;
        }

        //apply to unity's game object
        m_transform.Translate(new Vector3(move_h, 0, move_v));

        //每帧调用MoveTo()函数
        //MoveTo();
        //或者只有当点击鼠标左键的时候才调用
        if (Input.GetMouseButton(0))
            MoveTo();
    }
}