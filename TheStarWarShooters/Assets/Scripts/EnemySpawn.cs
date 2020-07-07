using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    //这个Transform与敌人相关联
    public Transform getenemy;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnenemy()); //执行协程函数
    }

    protected virtual IEnumerator spawnenemy()  //使用协程函数创建敌人，且必须声明为virtual，才能重写子类的继承
    {
        while (true)  //整个游戏过程都会循环制造新的敌人
        {
            yield return new WaitForSeconds(Random.Range(5,15));
            GameObject.Instantiate(getenemy, this.transform.position, this.transform.rotation);//在当前位置生成新的敌人
        }
    }
    
    //显示图片，方便场景中观察
    protected void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position,"item.png",true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
