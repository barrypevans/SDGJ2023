using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Pellet : MonoBehaviour
    {

        public int Reward = 5;
        private Vector3 _initialPos;
        private float _liveTime = 0f;
        private float _bobRate = 1f;
        private float _bobDist = 0.1f;

        private void Start()
        {
            _initialPos = transform.position;
            _liveTime = Random.value;
        }

        private void Update()
        {
            _liveTime += Time.deltaTime* _bobRate;
            transform.position = _initialPos + Vector3.up * Mathf.Sin(_liveTime)*_bobDist;
        }

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