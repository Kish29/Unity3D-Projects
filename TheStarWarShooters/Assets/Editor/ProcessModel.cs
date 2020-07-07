using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    public class ProcessModel : AssetPostprocessor
    {
        //该函数在导入模型时会自动调用
        private void OnPostprocessModel(GameObject input)
        {
            //只处理文件名为xxx的模型，这里是Enemy2b的fbx文件
            if (input.name != "EnemyAdv")
                return;

            //取得导入模型的相关信息
            ModelImporter importer = assetImporter as ModelImporter;

            //将该模型从工程中读出来
            GameObject target = AssetDatabase.LoadAssetAtPath<GameObject>(importer.assetPath);

            //将这个模型创建为prefab
            GameObject prefab = PrefabUtility.CreatePrefab("Assets/prefabs/EnemyAdv.prefab",target);

            //资源刷新
            AssetDatabase.Refresh();

            //设置Prefab的tag
            prefab.tag = "AdvancedEnemy";

            //查找碰撞模型
            foreach (Transform obj in prefab.GetComponentsInChildren<Transform>()) //查找相关组件
            {
                if (obj.name == "col") //如果找到了col组件
                {
                    //取消mesh renderer(渲染)
                    MeshRenderer m = obj.GetComponent<MeshRenderer>(); //获得网格渲染组件
                    m.enabled = false;

                    //并添加Mesh碰撞体
                    if (obj.gameObject.GetComponent<MeshCollider>() == null) //如果该组件还没有mesh碰撞体
                        obj.gameObject.AddComponent<MeshCollider>();

                    //设置碰撞体的tag
                    obj.tag = "AdvancedEnemy";
                }
            }

            //设置刚体
            Rigidbody rigidbody = prefab.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            //为prefab添加声音组件
            prefab.AddComponent<AudioSource>();

            //先获得高级敌人的脚本
            AdvancedEnemy adve = prefab.AddComponent<AdvancedEnemy>();

            //然后获得声音源件
            AudioClip adc = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/AudioandEffects/shoot.WAV");
            //获得爆炸组件
            Transform explosion = AssetDatabase.LoadAssetAtPath<Transform>("Assets/prefabs/Explosion Variant.prefab");
            //获得敌人的子弹组件
            Transform enemymissles = AssetDatabase.LoadAssetAtPath<Transform>("Assets/prefabs/EnemyRocket.prefab");


            //然后赋值
            adve.AdvancedAudio = adc;
            adve.AdvancedenemyExplosionEffects = explosion;
            adve.r_rocketfireposition = enemymissles;
        }
    }
}