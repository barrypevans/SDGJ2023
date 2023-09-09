using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class JacobTempCameraController : MonoBehaviour
    {
        public float RotationSpeed = 2f;
        void Update()
        {
            int direction = 0;
            if (Input.GetKey(KeyCode.A))
                direction = -1;
            else if (Input.GetKey(KeyCode.D))
                direction = 1;

            if (direction != 0)
            {
                transform.Rotate((direction == -1 ? Vector3.down : Vector3.up), Time.deltaTime * RotationSpeed);
            }
        }
    }
}