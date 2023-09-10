using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Cactus : MonoBehaviour
    {
        public float RetractDistance = 2f;
        private void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                JacobTempPlayerController controller = collision.gameObject.GetComponent<JacobTempPlayerController>();
                if (controller != null)
                {
                    controller.ForceRetract(RetractDistance);
                }
            }
        }
    }
}