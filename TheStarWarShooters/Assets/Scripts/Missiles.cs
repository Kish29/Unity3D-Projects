using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MyGame/rocket")]
public class Missiles : MonoBehaviour
{
    private float m_speed = 15.0f;

    public float m_attack = 2.0f;

    private Transform rocketTransform;

    private void OnBecameInvisible()
    {
        if (this.enabled) //if the rocket is being actived
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        rocketTransform = this.transform;
    }

    //add the function to control the shells actions
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "AdvancedEnemy" || other.tag == "EnemyRocket")
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rocketTransform.Translate(new Vector3(0, 0, m_speed * Time.deltaTime));
    }
}