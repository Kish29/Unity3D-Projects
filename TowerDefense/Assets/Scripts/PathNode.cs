using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// PathNode 是一种链表形式的数据结构，保存下一个PathNode
/// </summary>
public class PathNode : MonoBehaviour
{
    public PathNode prev = null;

    public PathNode next = null;

    public bool isHead = false;

    // 圆锥属性
    private float _coneRadius = 0.1f;
    private float _coneHeight = 0.2f;
    private int _conePrecision = 24;

    public void AddToTail(PathNode node)
    {
        if (next != null)
        {
            node.next = next;
            next.prev = node;
        }

        next = node;
        node.prev = this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Node.tif");
        // 画出链表之间的线条
        if (next != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.TransformPoint(0, 0, 0), next.transform.TransformPoint(0, 0, 0));
            // 获取cone子物体
            GameObject cone = GetConeObject();
            if (cone != null)
            {
                var position = (transform.TransformPoint(0, 0, 0) +
                                next.transform.TransformPoint(0, 0, 0)) / 2;
                var direction = (next.transform.TransformPoint(0, 0, 0) -
                                 transform.TransformPoint(0, 0, 0)) / 2;
                cone.GetComponent<MeshFilter>().mesh =
                    ConeManager.CreateConeMesh(_coneRadius, _coneHeight, _conePrecision);
                cone.transform.position = position;
                // 将物体的x轴朝向设置为朝向下一个节点
                cone.transform.right = direction;
            }
        }
    }

    private GameObject GetConeObject()
    {
        // 在子物体中查找cone物体
        GameObject cone = null;
        foreach (var t in GetComponentsInChildren<Transform>())
        {
            if (String.Compare(t.name, "cone", StringComparison.Ordinal) == 0)
            {
                cone = t.gameObject;
            }
        }

        // 如果子物体中没有，创建新的cone物体
        if (cone == null)
        {
            GameObject newCone = new GameObject();
            newCone.name = "cone";
            // 设置其父物体为本物体
            newCone.transform.parent = transform;

            // 设置该物体的坐标在二者之间
            newCone.transform.position =
                (transform.TransformPoint(0, 0, 0) +
                 next.transform.TransformPoint(0, 0, 0)) / 2;

            // 添加mesh filter组件
            if (newCone.GetComponent<MeshFilter>() == null)
            {
                newCone.gameObject.AddComponent<MeshFilter>();
            }

            // 添加mesh renderer组件
            if (newCone.GetComponent<MeshRenderer>() == null)
            {
                newCone.gameObject.AddComponent<MeshRenderer>();
            }

            cone = newCone;
        }

        // 检查null，防止获取gameObject抛出异常
        if (cone == null)
        {
            return null;
        }

        return cone.gameObject;
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