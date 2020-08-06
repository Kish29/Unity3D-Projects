using System;
using UnityEngine;

public class TileObjects : MonoBehaviour
{
    // 静态实例化成员
    public static TileObjects Instance = null;

    // tile的碰撞层
    public LayerMask tileLayer;

    // tile的大小
    public float tileSize = 1.0f;

    // x轴方向的tile数量
    public int xTileNum = 2;

    // z轴方向的tile数量
    public int zTileNum = 2;

    // 所有的格子信息
    private int[] _tileData = new int[4];

    // 子物体plane
    private Transform _plane;

    // 不在inspector窗口显示 
    [HideInInspector] public int dataId = 0;

    [HideInInspector] public bool debug = false;

    private void Awake()
    {
        Instance = this;
    }

    // 重置地图数据
    public void Reset()
    {
        _tileData = new int[xTileNum * zTileNum];
        // 重新设置plane的大小
        foreach (var t in GetComponentsInChildren<Transform>())
        {
            // 获取plane的Transform
            if (String.Compare(t.name, "TileBasic", StringComparison.Ordinal) == 0)
                _plane = t.transform;
        }

        _plane.localScale = new Vector3((float) xTileNum / 10, 1, (float) zTileNum / 10);
        _plane.localPosition = new Vector3((float) xTileNum / 2, 0, (float) zTileNum / 2);
    }

    // 获得某个坐标对应的tileData的索引
    private int GetDataIndexFromPosition(float posX, float posZ)
    {
        var position = transform.position;
        // 鼠标落在外面是无法编辑的
        if (posX - position.x < 0 || posZ - position.z < 0)
            return -1;
        int index = (int) ((posX - position.x) / tileSize) * zTileNum +
                    (int) ((posZ - position.z) / tileSize);
        if (index >= _tileData.Length)
            return -1;
        return index;
    }

    // 获得相应的tile数值
    public int GetDataFromPosition(float posX, float posZ)
    {
        int index = GetDataIndexFromPosition(posX, posZ);
        return index == -1 ? -1 : _tileData[index];
    }

    // 设置相应的tile数值
    public void SetDataFromPosition(float poxX, float posZ, int val)
    {
        int index = GetDataIndexFromPosition(poxX, posZ);
        if (index != -1)
            _tileData[index] = val;
    }

    // 在编辑模式显示帮助信息
    private void OnDrawGizmos()
    {
        if (!debug)
            return;
        if (_tileData == null)
        {
            Debug.Log("Please Reset Data First!");
            return;
        }

        Vector3 pos = transform.position;

        // 为格子画上辅助线
        for (int i = 0; i < xTileNum; i++) // z轴方向的辅助线
        {
            Gizmos.color = Color.blue;
            // 线长度
            var drawLengthZ = tileSize * zTileNum;
            Gizmos.DrawLine(transform.TransformPoint(tileSize * i, 0, 0),
                transform.TransformPoint(tileSize * i, 0, drawLengthZ));
            // 自身调用TransformPoint函数，在传入的参数不是new Vector3的情况下，
            // 将自生的坐标加上传入的坐标x、y、z再转化成世界坐标返回

            // 画出置位的格子的轮廓
            for (int j = 0; j < zTileNum; j++)
            {
                int index = i * zTileNum + j;
                if (index < _tileData.Length) // 绘制的格子在网格的范围内
                {
                    Color[] cubeColor = new Color[3];
                    // 根据不同的状态绘制不同颜色的Cube
                    cubeColor[0] = new Color(1, 0, 0, 0.5f);
                    cubeColor[1] = new Color(0, 1, 0, 0.5f);
                    cubeColor[2] = new Color(0, 0, 1, 0.5f);
                    // 根据格子的状态来显示不同的颜色
                    Gizmos.color = cubeColor[_tileData[index]];
                    // DrawCube两个参数，第一个是Cube的中心坐标，第二个是Cube的大小
                    Gizmos.DrawCube(transform.TransformPoint(i * tileSize + tileSize * 0.5f, 0,
                            j * tileSize + tileSize * 0.5f),
                        new Vector3(tileSize, 0.2f, tileSize));
                }
            }
        }

        for (int j = 0; j < zTileNum; j++) // x轴方向的辅助线
        {
            Gizmos.color = Color.red;
            var drawLengthX = tileSize * xTileNum;
            Gizmos.DrawLine(transform.TransformPoint(0, 0, tileSize * j),
                transform.TransformPoint(drawLengthX, 0, tileSize * j));
        }
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