using UnityEngine;

namespace Temp
{
    public class QuaternionTest : MonoBehaviour
    {
        public bool setRotate = false;

        private bool _isSetRotate = false;

        public bool setLook = false;

        public PathNode nextNode;

        private float _rotateSpeed = 120f;

        // Start is called before the first frame update
        void Start()
        {
        }

        private void QuaternionSetRotate()
        {
            var q = new Quaternion(0, Mathf.Cos(Mathf.Deg2Rad * 90 / 2), 0, Mathf.Cos(Mathf.Deg2Rad * 90 / 2));
            transform.rotation = transform.rotation * q;
        }


        private void QuaternionLookRotation()
        {
            if (nextNode != null)
            {
                var position = nextNode.transform.position - transform.position;
                position.y = 0;
                var targetRotation = Quaternion.LookRotation(position);
                Debug.Log(position);
                Debug.Log(targetRotation);
                float next = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y,
                    _rotateSpeed * Time.deltaTime);
                Debug.Log(next);
                transform.eulerAngles = new Vector3(0, next, 0);
            }
        }

        private void OnDrawGizmos()
        {
            if (setRotate && !_isSetRotate)
            {
                _isSetRotate = true;
                QuaternionSetRotate();
            }

            if (!setRotate)
            {
                _isSetRotate = false;
            }

            QuaternionLookRotation();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}