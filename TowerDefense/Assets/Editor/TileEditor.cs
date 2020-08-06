using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TileObjects))]
    public class TileEditor : UnityEditor.Editor
    {
        // 是否处于编辑模式
        protected bool EditMode = false;

        // 受编辑器影响的Tile脚本
        protected TileObjects TileObjects;

        // 获得编辑器脚本
        private void OnEnable()
        {
            TileObjects = (global::TileObjects) target;
        }

        // 更改场景中的操作
        public void OnSceneGUI()
        {
            // 如果处于编辑器模式
            if (EditMode)
            {
                // 取消编辑器的选择功能（即其它物体不能被鼠标选中）
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                // 画出格子辅助线
                TileObjects.debug = true;
                // 获取Input事件
                Event e = Event.current;
                // 如果是鼠标左击或者拖拽
                if (e.button == 0 && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && !e.alt)
                {
                    // 获取由鼠标位置产生的射线
                    Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                    // 计算碰撞，获得鼠标左键点击时位置对应的tile，并设置value(value会被Inspector中的Toolbar改变)
                    Debug.Log(Physics.Raycast(ray, out var hitInfo, 2000, TileObjects.tileLayer));
                    if (Physics.Raycast(ray, out hitInfo, 2000, TileObjects.tileLayer))
                    {
                        TileObjects.SetDataFromPosition(hitInfo.point.x, hitInfo.point.z, TileObjects.dataId);
                    }
                }
            }

            HandleUtility.Repaint();
        }

        // 自定义Inspector的窗口UI
        public override void OnInspectorGUI()
        {
            GUILayout.Label("Tile Editor"); // 编辑器名称
            EditMode = EditorGUILayout.Toggle("Edit", EditMode); // 编辑模式
            TileObjects.debug = EditorGUILayout.Toggle("Debug", TileObjects.debug); // 画出辅助线
            string[] editorDataStr = {"Dead", "Road", "Guard"};
            // Toolbar对dataId对应字符串的值分别为0, 1, 2（对应字符串的数组下标）
            TileObjects.dataId = GUILayout.Toolbar(TileObjects.dataId, editorDataStr);

            // 添加一道分割线
            EditorGUILayout.Separator();
            if (GUILayout.Button("Reset"))
            {
                TileObjects.Reset();
            }

            DrawDefaultInspector();
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
}