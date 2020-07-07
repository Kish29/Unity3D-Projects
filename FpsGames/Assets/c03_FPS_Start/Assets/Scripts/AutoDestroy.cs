using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    //定义销毁时间
    private float _destroyTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, _destroyTime);
    }
}