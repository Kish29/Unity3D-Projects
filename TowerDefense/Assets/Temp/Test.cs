using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private int _targetMask;

    // Start is called before the first frame update
    void Start()
    {
        _targetMask = LayerMask.GetMask("target");
        print(_targetMask);
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main != null)
        {
            // 创建一条从相机到屏幕的射线，并且穿过屏幕的position(x, y)，起始位置是屏幕中鼠标的位置(z坐标被忽略)
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // RaycastHit是一个结构体，用来存储射线返回的信息
            // 如果击中了LayerMask为target的物体
            if (Physics.Raycast(ray, out var hit, 100f, _targetMask))
            {
                var offset = new Vector3(1, 0, 0);
                var position = hit.transform.position;
                Debug.Log(position.ToString());
                position += offset;
                hit.transform.position = position;
            }
        }
    }
}