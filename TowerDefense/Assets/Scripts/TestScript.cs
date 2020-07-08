using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(transform.position, 1f);
        var pos = transform.position;
        // Gizmos.color = Color.blue;
        // var drawPosZ = pos + new Vector3(0, pos.y, 0);
        // var drawPosZ1 = drawPosZ;
        // drawPosZ1.z += 3;
        // Gizmos.DrawLine(drawPosZ, drawPosZ1);            
        Gizmos.color = Color.blue;
        for (int i = 0; i < 4; i++)
        {
            var toward = transform.TransformPoint(i, 0, 3);
            Gizmos.DrawLine(transform.TransformPoint(i, 0, 0), toward);
        }

        // Debug.Log(toward.ToString());
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