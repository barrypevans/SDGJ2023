using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Gameplay
{
    public class CaffeinePills : MonoBehaviour
    {
        public float CaffeineTime = 3f;
        public float Strength = 500f;
        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                JacobTempPlayerController controller = collision.gameObject.GetComponent<JacobTempPlayerController>();
                if (controller != null)
                {
                    controller.Caffeinate(CaffeineTime);
                    if (controller.GetMovementState != JacobTempPlayerController.MovementState.RETRACTING)
                    {
                        controller.GetComponent<Rigidbody>().AddForce(Vector3.up * Strength);
                        AudioController.Play(AudioController.Instance.Assets.CatReacts.GetRandom(), false);
                    }
                }
            }
        }
    }
}