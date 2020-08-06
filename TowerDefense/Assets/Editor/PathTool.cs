using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PathTool : ScriptableObject
    {
        public static PathNode head = null;

        [MenuItem("PathTool/Create PathNode %m")]
        public static void CreatePathNode()
        {
            // 创建一个新的路点
            GameObject go = new GameObject();
            go.AddComponent<PathNode>();
            go.name = "pathnode";
            // set tag
            go.tag = "pathnode";
            // 使新创建的路点处于选择状态
            Selection.activeTransform = go.transform;
        }

        [MenuItem("PathTool/Set Head %q")] // 快捷键设置为Ctr + q
        public static void SetHead()
        {
            // 如果没有选中任何物体或者选择的物体多于一个
            if (!Selection.activeGameObject || Selection.GetTransforms(SelectionMode.Unfiltered).Length > 1)
                return;
            if (String.Compare(Selection.activeGameObject.tag, "pathnode", StringComparison.Ordinal) == 0)
            {
                head = Selection.activeGameObject.GetComponent<PathNode>();
                head.isHead = true;
            }
        }

        [MenuItem("PathTool/Add to List %w")] // 快捷键设置为Ctr + w
        public static void SetNext()
        {
            if (!Selection.activeGameObject || head == null || Selection.GetTransforms(SelectionMode.Unfiltered).Length > 1)
                return;
            if (String.Compare(Selection.activeGameObject.tag, "pathnode", StringComparison.Ordinal) == 0)
            {
                head.AddToTail(Selection.activeGameObject.GetComponent<PathNode>());
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
}