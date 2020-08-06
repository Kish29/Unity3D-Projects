using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    public static CameraPoint Instance;


    private void Awake()
    {
        Instance = this;
    }

    // 用 OnDrawGizmos显示一个图标
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "CameraPoint.tif"); 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
