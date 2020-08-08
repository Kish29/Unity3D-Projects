using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 设置敌人的通用属性
    public PathNode nextPathNode;
    public int health = 15;

    public int maxHealth = 15;

    public int moveSpeed = 2;

    public float rotateSpeed = 120f;
    public System.Action<Enemy> onDeath; // 敌人的死亡事件

    public void RotateTo()
    {
        var moveDirection = nextPathNode.transform.position - transform.position;
        moveDirection.y = 0;
        var rotateDirection = Quaternion.LookRotation(moveDirection); // 获得朝向moveDirection的
        float next = Mathf.MoveTowardsAngle(transform.eulerAngles.y, rotateDirection.eulerAngles.y,
            rotateSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, next, 0);
    }

    public void MoveTo()
    {
        var pos1 = transform.position;
        var pos2 = nextPathNode.transform.position;
        float distance = Vector2.Distance(new Vector2(pos1.x, pos1.z), new Vector2(pos2.x, pos2.z));

        if (distance < 1.0f) //到达
        {
            if (nextPathNode.next == null)
            {
                GameManager.Instance.SetDamage(1);
                DestroyMe();
            }
            else
            {
                nextPathNode = nextPathNode.next;
            }
        }

        transform.Translate(new Vector3(0, 0, moveSpeed * Time.deltaTime));
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 主要调用两个函数，一个是RotateTo()，转向玩家，另一个是MoveTo()，向玩家移动
        RotateTo();
        MoveTo();
    }
}