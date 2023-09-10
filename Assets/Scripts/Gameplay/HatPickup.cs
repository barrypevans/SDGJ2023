using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{

    public class HatPickup : MonoBehaviour
    {
        private float _liveTime = 0f;
        private float _bobRate = 1f;
        private float _bobDist = 0.1f;
        private Vector3 _initialPos;

        private void Start()
        {
            _initialPos = transform.position;
            _liveTime = Random.value;
        }

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

        private void ClayUpdate()
        {
            _liveTime += Time.deltaTime * _bobRate;
            transform.position = _initialPos + Vector3.up * Mathf.Sin(_liveTime) * _bobDist;
        }
    }
}