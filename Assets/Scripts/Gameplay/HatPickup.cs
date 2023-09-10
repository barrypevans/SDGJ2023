using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{

    public class HatPickup : MonoBehaviour
    {
        [SerializeField]
        HatType hatType;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<HatMgr>().SetHat(hatType);
                Destroy(gameObject);
            }
        }
    }
}