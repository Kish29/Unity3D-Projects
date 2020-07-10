using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    private float _agent;

    void Start()
    {
        _agent = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        _agent += Time.deltaTime * 50;
        /*
         * localEulerAngles是本物体的旋转角度，当其变为某个父物体的子物体时，角度是相对于父物体的
         * eulerAngles是本问题相对于世界的的角度，不受父物体的角度影响
         */
        transform.localEulerAngles = new Vector3(0, _agent, 0);
    }
}