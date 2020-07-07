using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Enemies : MonoBehaviour
{
    //敌人控件
    [FormerlySerializedAs("Enemy")] public Transform enemy;

    //定义主角控件
    private Player _player;

    //寻路组件
    private NavMeshAgent _enemyAgent;

    //移动速度
    private float EnemySpeed = 2.5f;

    //旋转速度
    private float _EnemyRotateSpeed = 5.0f;

    //设置敌人的生命值
    private float _enemyHealth = 20.0f;

    //脚本控制动画的过渡
    private Animator _animator;

    //设置更新敌人寻找主角的时间间隔（1.5s）
    private float _chaseTime = 1.5f;

    //定义一个slider
    public Slider enemyHealth;

    //定义敌人死亡的前0.01s
    private bool _deathFormer;
    private float _deathFormerTime = 2.99f;

    //敌人攻击动画时间
    private float _attackTime = 0.8f;

    // Start is called before the first frame update
    private void Start()
    {
        //获取敌人组件
        enemy = this.transform;

        //获取主角
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //寻路组件
        _enemyAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        //设置寻路器的行走速度
        _enemyAgent.speed = EnemySpeed;

        //获得动画播放器
        _animator = this.GetComponent<Animator>();

        //初始化状态
        _animator.SetBool("idle", true);
        _animator.SetBool("run", false);
        _animator.SetBool("attack", false);
        _animator.SetBool("death", false);

        _deathFormer = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //设置寻路目标
        //EnemyAgent.destination = player.transform.position;

        //如果主角生命值为0，那么什么也不做
        if (_player.playerHealth <= 0)
        {
            _animator.SetBool("idle", true);
            _animator.SetBool("attack", false);
            _animator.SetBool("run", false);
            return;
        }

        //获取当前动画状态
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        //更新重新定位时间
        _chaseTime -= 1 * Time.deltaTime;
        //更新时间到，并且不是出于攻击状态、与主角距离大于一定值
        if (_chaseTime <= 0 && stateInfo.fullPathHash != Animator.StringToHash("Base Layer.attack") &&
            Vector3.Distance(enemy.transform.position, _player.transform.position) > 4.157f)
        {
            //进入待机状态
            //_animator.SetBool("idle", true);
            //旋转至新方向
            RotateTo();
            //重新定位
            _enemyAgent.SetDestination(_player.transform.position);
            _chaseTime = 1.5f;
        }

        //如果此时动画出于待机状态并且不处于过渡状态
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.idle") && !_animator.IsInTransition(0))
        {
            _animator.SetBool("idle", false);
            _animator.SetBool("run", true);
        }

        //当二者之间的距离小于一定范围时，攻击
        if (Vector3.Distance(enemy.transform.position, _player.transform.position) <= 2.157f)
        {
            //停止寻路
            _enemyAgent.ResetPath();
            _animator.SetBool("run", false);
            _animator.SetBool("idle", true);
            _animator.SetBool("attack", true);
        }
        else
        {
            _animator.SetBool("attack", false);
            _animator.SetBool("run", true);
        }

        //如果敌人出于攻击状态并且距离主角小于4.157，主角生命值减少
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.attack") &&
            Vector3.Distance(enemy.transform.position, _player.transform.position) <= 2.157f)
        {
            _attackTime -= Time.deltaTime;
            //判断攻击动画是否结束
            if (_attackTime <= 0)
            {
                _player.OnDamage(20);
                _attackTime = 0.8f;
            }
        }

        //销毁对象前的0.01s加5000分
        if (_deathFormer == true)
        {
            _deathFormerTime -= 1 * Time.deltaTime;
            if (_deathFormerTime <= 0)
                GameManager.Instance.ScoreUpdate(5000);
        }
    }

    //敌人血量减少的函数
    public void onDamage()
    {
        this._enemyHealth -= 1.5f;
        enemyHealth.value = _enemyHealth / 20.0f;
        if (_enemyHealth <= 0)
        {
            //死亡后敌人数量减少1
            GameManager.enemyNumbers--;
            _deathFormer = true;
            _animator.SetBool("death", true);
            _enemyAgent.ResetPath();
            Destroy(this.gameObject, 3);
        }

        //更新分数
        if (_enemyHealth > 0)
        {
            GameManager.Instance.ScoreUpdate(100);
        }
    }

    private void RotateTo()
    {
        // 获取目标（Player）的方向
        Vector3 targetdir = _player.transform.position - this.transform.position;

        //计算出新方向
        Vector3 newdir =
            Vector3.RotateTowards(this.transform.forward,
                targetdir,
                _EnemyRotateSpeed * Time.deltaTime,
                0.0f);

        //旋转至新方向
        this.transform.rotation = Quaternion.LookRotation(newdir);
    }
}