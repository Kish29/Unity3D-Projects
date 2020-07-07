using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRenderer : MonoBehaviour
{
    public AdvancedEnemy m_enemy;
    // Start is called before the first frame update
    void Start()
    {
        m_enemy = this.GetComponentInParent<AdvancedEnemy>();    //通过父物体获得enemy的脚本
    }

    private void OnBecameVisible()    //当模型进入屏幕的时候
    {
        m_enemy.m_Active = true;    //让模型更新父类的脚本状态
        m_enemy.m_rendener = this.GetComponent<Renderer>();    //使父类enemy获得这个模型的渲染效果
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
