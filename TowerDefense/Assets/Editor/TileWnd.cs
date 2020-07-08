using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class TileWnd : EditorWindow
    {
        protected static TileObjects TileObjects;

        [MenuItem("Tools/Tile Window")]
        private static void ShowWindow()
        {
            // 定制窗口的一些信息
            var window = GetWindow<TileWnd>();
            window.titleContent = new GUIContent("Tile Editor");
            window.Show();

            // 选中带有TileObject脚本实例的TileObjects实例
            if (Selection.activeTransform != null)
                TileObjects = Selection.activeTransform.GetComponent<TileObjects>();
        }

        // 更新选中的新物体
        private void OnSelectionChange()
        {
            if (Selection.activeTransform != null)
                TileObjects = Selection.activeTransform.GetComponent<TileObjects>();
        }

        // 显示窗口GUI，大部分函数都在GUILayout和EditorGUILayout内
        private void OnGUI()
        {
            // 只有含有TileObjects实例的物体才进行渲染
            if (TileObjects == null)
                return;
            // 在工程目录中读取一张2D贴图
            var sticker = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/GUI/player1.png");
            // 将贴图显示在窗口内
            GUILayout.Label(sticker);
            // 是否绘制辅助线
            TileObjects.debug = EditorGUILayout.Toggle("Debug", TileObjects.debug);
            // 切换tile的数据，原理同TileEditor脚本中Toolbar
            string[] editorDataStr = {"Dead", "Road", "Guard"};
            // Toolbar对dataId对应字符串的值分别为0, 1, 2（对应字符串的数组下标）
            TileObjects.dataId = GUILayout.Toolbar(TileObjects.dataId, editorDataStr);
            // 添加一道分割线
            EditorGUILayout.Separator();
            // 重置按钮
            if (GUILayout.Button("Reset"))
            {
                TileObjects.Reset();
            }
        }
    }
}