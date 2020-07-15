using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    private float _jmpSpeed;

    private float _jmpTime;

    private float _gravity = 9.8f;

    private Rigidbody _rigidbody;

    private Vector3 rtDes = Vector3.zero;

    private float _rotateSpeed = 100f;

    private float _moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        _jmpSpeed = 0;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var des = Vector3.zero;
        //des.x += Input.GetAxis("Horizontal") * 5f * Time.deltaTime;
        des.z += Input.GetAxis("Vertical") * _moveSpeed * Time.deltaTime;

        if (Input.GetAxis("Vertical") < 0)
            rtDes.y -= Input.GetAxis("Horizontal") * _rotateSpeed * Time.deltaTime;
        else
            rtDes.y += Input.GetAxis("Horizontal") * _rotateSpeed * Time.deltaTime;
        transform.eulerAngles = rtDes;

        if (Input.GetKey(KeyCode.Space) && _rigidbody.useGravity)
        {
            _jmpSpeed = 5f;
            _jmpTime = _jmpSpeed / _gravity;
            des.y += _jmpSpeed * Time.deltaTime;
            _jmpTime -= Time.deltaTime;
            _jmpSpeed -= _gravity * Time.deltaTime;
            _rigidbody.useGravity = false;
        }
        else
        {
            if (_jmpTime > 0)
            {
                des.y += _jmpSpeed * Time.deltaTime;
                _jmpTime -= Time.deltaTime;
                _jmpSpeed -= _gravity * Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _jmpSpeed = 5f;
                    _jmpTime = _jmpSpeed / _gravity;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _rigidbody.useGravity = false;
                    _jmpSpeed = 5f;
                    _jmpTime = _jmpSpeed / _gravity;
                }
            }
        }

        transform.Translate(des);
    }
}