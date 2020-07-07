using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glamspMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //使摄像机永远是一个正方形，前两个参数表示XY位置，后两个参数是XY的大小
        float ratio = (float) Screen.width / Screen.height;
        GetComponent<Camera>().rect = new Rect(1 - 0.2f, 1 - 0.2f * ratio, 0.2f, 0.4f * ratio);
    }

    // Update is called once per frame
    void Update()
    {
    }
}