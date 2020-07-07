using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvEnemyRocket : MonoBehaviour
{
    //define player's position
    private Transform _plyposition;

    //get the offset
    private float _offset = 0.0f;

    //define fly speed
    private float _fly_speed = 5.0f;

    //define its attack ability
    public float AdvEneAtta = 25.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

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
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            GameObject plyposition1 = GameObject.FindGameObjectWithTag("Player");
            if (plyposition1 != null)
            {
                _plyposition = plyposition1.transform;
            }

            Vector3 trackposition = this.transform.position - _plyposition.position;
            if (trackposition.x / trackposition.z < 1)
                this.transform.Translate(new Vector3(_offset * _fly_speed * Time.deltaTime, 0,
                    _fly_speed * Time.deltaTime));

            else if (trackposition.z / trackposition.x < 1)
                this.transform.Translate(new Vector3(_fly_speed * Time.deltaTime, 0,
                    _offset * _fly_speed * Time.deltaTime));

            else
                this.transform.Translate(new Vector3(_fly_speed * Time.deltaTime, 0,
                    _fly_speed * Time.deltaTime));
        }
    }
}