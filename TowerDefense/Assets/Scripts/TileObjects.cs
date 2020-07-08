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
    public int[] tileData;

    // 不再inspector窗口显示 
    [HideInInspector] public int tileId = 0;

    public bool debug = false;

    private void Awake()
    {
        Instance = this;
    }

    // 重置地图数据
    public void Reset()
    {
        tileData = new int[xTileNum * zTileNum];
    }

    // 获得某个坐标对应的tileData的索引
    private int GetDataIndexFromPosition(float posX, float posZ)
    {
        var position = transform.position;
        int index = (int) ((posX - position.x) / tileSize) * zTileNum +
                    (int) ((posZ - position.z) / tileSize);
        if (index < 0 || index >= tileData.Length)
            return 0;
        return index;
    }

    // 获得相应的tile数值
    public int GetDataFromPosition(float posX, float posZ)
    {
        int index = GetDataIndexFromPosition(posX, posZ);
        return tileData[index];
    }

    // 设置相应的tile数值
    public void SetDataFromPosition(float poxX, float posZ, int val)
    {
        int index = GetDataIndexFromPosition(poxX, posZ);
        tileData[index] = val;
    }

    // 在编辑模式显示帮助信息
    private void OnDrawGizmos()
    {
        if (!debug)
            return;
        if (tileData == null)
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
            for (int j = 0; j < zTileNum; j++) // x轴方向的辅助线
            {
            }
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