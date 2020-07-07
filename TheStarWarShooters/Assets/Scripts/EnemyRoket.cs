using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoket : MonoBehaviour
{
    private float enemyfly = 5;

    private Transform enemyrocket;

    //define its attack
    public float enemyattack = 15;

    // Start is called before the first frame update
    void Start()
    {
        enemyrocket = this.transform;
    }

    //when the shells became invisible, destroy itself
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "PlayerRocket")
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemyrocket.transform.Translate(new Vector3(0, 0, enemyfly * Time.deltaTime));
    }
}