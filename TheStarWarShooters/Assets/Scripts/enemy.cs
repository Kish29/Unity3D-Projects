using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MyGame/enemy")]
public class enemy : MonoBehaviour
{   
    public float hp = 6;

    private float fly_speed = 1;

    //普通敌人将会每3秒发射速度更慢的子弹
    private float enemyfiretime = 3;

    //获取敌人的发射位置
    private Transform enemyfireposition;

    //获取主角的位置
    private Transform playerposition;

    //敌人获取子弹的生成
    public Transform enemy_shell;

    //敌人发射子弹没有声音，故这儿不创建敌人的声音源，创建敌人的爆炸特效
    public Transform enemy_explosion;

    //private float m_rotationspeed = 30;

    // Start is called before the first frame update
    void Start()
    {
        enemyfireposition = transform.Find("EnemyFirePosition2");
    }


    internal bool m_Active = false;

    private void OnBecameVisible()
    {
        m_Active = true;
    }

    private void OnBecameInvisible()
    {
        if (m_Active)
            Destroy(this.gameObject);
        return;
    }

    //define a function to control enemy's actions
    protected virtual void UpdateMove()
    {
        float rx = Mathf.Sin(Time.time) * Time.deltaTime;
        transform.Translate(new Vector3(rx, 0, fly_speed * Time.deltaTime));
    }

    //create a function to control the interaction between enemy and player
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerRocket") //if spaceship run into the shell
        {
            Missiles rocket = other.GetComponent<Missiles>(); //decrease its health
            if (rocket != null)
            {
                GameManager.Instance.GetEnemyHealth(hp);  //碰到就要显示血量
                hp -= rocket.m_attack;
                GameManager.Instance.GetEnemyHealth(hp);  //减少也要显示血量变化
                if (this.hp <= 0)
                {
                    GameManager.Instance.AddScore(100);  //通过已经实例化的GameManager的Instance调用AddScore函数，而且摧毁一架敌机加一百分
                    Destroy(this.gameObject); //destroy itself
                    GameObject.Instantiate(enemy_explosion, this.transform.position, this.transform.rotation);
                    //直接在敌人的位置生成爆炸特效
                }
            }
        }

        //if spaceship run into the player, destroy itself immediately
        if (other.tag == "Player")
        {
            GameManager.Instance.AddScore(100);
            Destroy(this.gameObject);
            GameObject.Instantiate(enemy_explosion, this.transform.position, this.transform.rotation);
            //直接在敌人的位置生成爆炸特效
        }
    }

    // Update is called once per frame
    void Update()
    { 
        enemyfiretime -= Time.deltaTime;
        if (enemyfiretime <= 0)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {

                if (playerposition != null)
                {
                    Vector3 _fireposition = playerposition.position - enemyfireposition.position;

                    GameObject.Instantiate(enemy_shell, enemyfireposition.position,
                        Quaternion.LookRotation(_fireposition));
                } //记得将Prefab拖到脚本的组建上，不然当代码找不到enemy_shell的时候，代码就会在这儿卡住
                //下一条指令UpdateMove()也不会执行
                else
                {
                    GameObject _playerposition = GameObject.FindGameObjectWithTag("Player");
                    if (_playerposition != null)
                    {
                        playerposition = _playerposition.transform;
                    }
                }

                enemyfiretime = 3;
            }
        }
        UpdateMove();
    }
}