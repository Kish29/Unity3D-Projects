using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var pos = transform.position;
        Gizmos.color = Color.blue;
        for (int i = 0; i < 4; i++)
        {
            var toward = transform.TransformPoint(i, 0, 3);
            Gizmos.DrawLine(transform.TransformPoint(i, 0, 0), toward);
            for (int j = 0; j < 3; j++)
            {
                Gizmos.color = new Color(1, 0, 0, 0.3f);
                Gizmos.DrawCube(transform.TransformPoint(i + 0.5f, 0,
                        j + 0.5f),
                    new Vector3(1, 0.2f, 1));
            }
        }
    }

    private int _targetMask;

    // Start is called before the first frame update
    void Start()
    {
        _targetMask = LayerMask.GetMask("target");
        print(_targetMask);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}