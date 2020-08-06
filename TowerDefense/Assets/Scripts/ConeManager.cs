using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeManager : MonoBehaviour
{
    /// <summary>
    /// 创建圆锥
    /// </summary>
    /// <param name="radius">圆锥底面半径</param>
    /// <param name="height">圆锥的高</param>
    /// <param name="precision">绘制的圆的精细度</param>
    /// <returns>Mesh对象</returns>
    /*
     * 注意，Unity3D中，绘制平面是不共点的！！！！！
     * 因为两点之间构成的法线是不同的
     */
    public static Mesh CreateConeMesh(float radius, float height, int precision)
    {
        // 限制一下精度，不然爆掉内存
        if (!(precision >= 12 && precision <= 48))
        {
            return null;
        }

        var vertices = new List<Vector3>(); // 顶点
        var indices = new List<int>(); //存储各个顶点的索引

        vertices.Add(Vector3.zero); // 圆锥底坐标
        vertices.Add(Vector3.right * height); // 圆锥顶点坐标

        var temp = new List<Vector3>();

        // precision是精度，也就是圆底面要分成多少份
        var radPerCircle = 360.0f / precision;

        // 以每30°，获得底面圆上的点
        for (var i = 0.0f; i < 360.0f; i += radPerCircle)
        {
            var rad = Mathf.Deg2Rad * i; // deg2rad = degree to raid，将度数转化为弧度制
            // y坐标
            var y = radius * Mathf.Cos(rad);
            var z = radius * Mathf.Sin(rad);
            temp.Add(new Vector3(0, y, z));
        }

        // AddRange添加多个元素
        vertices.AddRange(temp);
        vertices.AddRange(temp);

        // 添加12个圆底的索引
        // 0、1索引所对的分别是圆锥底和圆锥顶
        var loop1 = precision + 2 - 1;
        for (var i = 2; i <= loop1; i++)
        {
            // 绘制正向圆底面
            indices.Add(0);
            indices.Add(i + 1);
            indices.Add(i);

            // 绘制反向的圆底面
            indices.Add(i);
            indices.Add(i + 1);
            indices.Add(0);
        }

        loop1++; //更新loop1索引
        var loop2 = loop1 + precision - 1;
        for (int i = loop1; i <= loop2; i++)
        {
            // 绘制朝外的三角面
            indices.Add(i);
            indices.Add(1);
            if (i < loop2)
            {
                indices.Add(i + 1);
            }
            else
            {
                indices.Add(loop1);
            }
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(indices, 0);
        mesh.RecalculateNormals();

        return mesh;
    }
}