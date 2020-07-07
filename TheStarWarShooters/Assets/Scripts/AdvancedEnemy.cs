using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedEnemy : MonoBehaviour
{
    private float fly_speed = 0.2f;

    //get the firepostion
    private Transform fireposition;

    //高级敌人发射子弹为他加上声音
    //声音文件
    public AudioClip AdvancedAudio;

    //声音源
    private AudioSource AdvancedAudioSource;

    //给高级敌人加上爆炸特效
    public Transform AdvancedenemyExplosionEffects;

    //get enemy's rocket
    public Transform r_rocketfireposition;

    private Transform flyrailway;

    //find the player's position
    private Transform playerposition;

    //define its heath and attach and activities 
    public float heathlevel = 100;

    public float enemydefend = 20;

    private float firetime = 1.5f;

    internal Renderer m_rendener;

    internal bool m_Active = false;

    // Start is called before the first frame update
    void Start()
    {
        //初始化里获取声音源
        AdvancedAudioSource = this.gameObject.GetComponent<AudioSource>();
        fireposition = transform.Find("FirePosition");
        flyrailway = this.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerRocket")
        {
            GameManager.Instance.GetEnemyHealth(heathlevel); //碰到就要显示血量
            Missiles mis = other.GetComponent<Missiles>();
            this.heathlevel -= mis.m_attack;
            //decrease its health level
            GameManager.Instance.GetEnemyHealth(this.heathlevel); //减少也要显示血量变化
            if (this.heathlevel <= 0)
            {
                GameManager.Instance.AddScore(1000); //通过已经实例化的GameManager的Instance调用AddScore函数，而且摧毁一架高级敌机加一千分
                Destroy(this.gameObject); //if the advanced enemy being run into the player's rocket
                GameObject.Instantiate(AdvancedenemyExplosionEffects, this.transform.position, this.transform.rotation);
                //在当前位置生成爆炸效果
            }
        }

        if (other.tag == "Player")
        {
            GameManager.Instance.GetEnemyHealth(heathlevel); //碰到就要显示血量
            Player player = other.GetComponent<Player>();
            heathlevel -= (player.playerdefend - enemydefend);
            GameManager.Instance.GetEnemyHealth(this.heathlevel); //减少也要显示血量变化
            if (heathlevel <= 0)
            {
                GameManager.Instance.AddScore(1000); //通过已经实例化的GameManager的Instance调用AddScore函数，而且摧毁一架高级敌机加一千分

                Destroy(this.gameObject);

                GameObject.Instantiate(AdvancedenemyExplosionEffects, this.transform.position, this.transform.rotation);
                //在当前位置生成爆炸效果
            }

            //if the advancedenemy run into the player, decrease the heathlevel
        }
    }

    // Update is called once per frame
    void Update()
    {
        firetime -= Time.deltaTime;
        if (firetime <= 0)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                if (playerposition != null)
                {
                    Vector3 _fireposition = playerposition.position - fireposition.position;

                    GameObject.Instantiate(r_rocketfireposition, fireposition.position,
                        Quaternion.LookRotation(_fireposition));

                    AdvancedAudioSource.PlayOneShot(AdvancedAudio);
                }
                else
                {
                    GameObject _playerposition = GameObject.FindGameObjectWithTag("Player");
                    if (_playerposition != null)
                    {
                        playerposition = _playerposition.transform;
                    }
                }

                firetime = 2;
            } //the enemy will fire the shells after every 2 seconds
        }

        flyrailway.Translate(new Vector3(0, 0, -fly_speed * Time.deltaTime));
    }
}