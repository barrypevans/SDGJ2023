using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class JacobTempCameraController : MonoBehaviour
    {
        public float RotationSpeed = 2f;
        public Transform Player;
        public bool TrackPlayer = false;
        Vector3 lastmousepos;

        private void Start()
        {
            lastmousepos = Input.mousePosition;

            if (TrackPlayer)
                transform.position = Player.position;
        }
        void Update()
        {
            if (Input.GetKey(KeyCode.Mouse2))
            {
                Vector3 delta = Input.mousePosition - lastmousepos;
                float direction = delta.x;

                if (direction != 0)
                {
                    transform.Rotate(Vector3.up, direction*RotationSpeed);
                }
            }
            lastmousepos = Input.mousePosition;

            if (TrackPlayer)
                transform.position = Vector3.Lerp(transform.position, Player.position, Time.deltaTime * 4f);
        }
    }
}