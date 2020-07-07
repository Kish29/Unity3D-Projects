using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public Transform player;

    //定义一个三维向量去获取相机与主角间的偏移量，必须是三维！
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        //偏移量等于相机的位置减去主角的位置
        offset = this.transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //让这个实例化的gameobject的位置始终等于主角的位置+偏移量
        transform.position = player.transform.position + offset;
    }
}