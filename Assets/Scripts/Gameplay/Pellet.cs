using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Pellet : MonoBehaviour
    {
        public int Reward = 5;
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                JacobTempPlayerController controller = other.GetComponent<JacobTempPlayerController>();
                if (controller != null)
                {
                    controller.MaxBodyPoints += Reward;
                    Object.Destroy(gameObject);
                }
            }
        }
    }
}