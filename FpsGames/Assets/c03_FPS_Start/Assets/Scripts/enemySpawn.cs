using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class enemySpawn : MonoBehaviour
{
    //这个Transform与敌人相关联
    public Transform getEnemy;

    //生成敌人的间隔时间
    private float _enemySpawnTime;


    void Start()
    {
        _enemySpawnTime = Random.value * 5.0f;
    }

    //生成敌人的最大数量
    private int _maxEnemies = 20;

    private void _spawnEnemy() //使用协程函数创建敌人，且必须声明为virtual，才能重写子类的继承
    {
        var transform1 = this.transform;
        GameObject.Instantiate(getEnemy, transform1.TransformPoint(0, (float) 1.8, 0),
            transform1.rotation); //在当前位置生成新的敌人
        GameManager.enemyNumbers++; //更新敌人数量
    }

    //显示图片，方便场景中观察
    protected void OnDrawGizmos()
    {
        Gizmos.DrawIcon(this.transform.TransformPoint(0, (float) 1.2, 0), "item.png", true);
    }

    private void Update()
    {
        //如果达到最大敌人数量，停止生成敌人
        if (GameManager.enemyNumbers >= _maxEnemies)
            return;

        //每隔一段时间
        _enemySpawnTime -= Time.deltaTime;
        if (!(_enemySpawnTime <= 0)) return;
        _spawnEnemy();
        _enemySpawnTime = Random.value * 5;
    }
}