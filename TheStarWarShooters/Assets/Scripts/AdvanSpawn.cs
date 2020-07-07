using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanSpawn : EnemySpawn
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnenemy());
    }

    //重写spawnenemy函数，注意公私有与基类相同
    protected override IEnumerator spawnenemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(60, 90));
            GameObject.Instantiate(getenemy, this.transform.position, this.transform.rotation);
            //生成对象与生成器位置相同
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}