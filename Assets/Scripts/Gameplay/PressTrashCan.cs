using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PressTrashCan : MonoBehaviour
    {
        public Transform Lid, LidDestination;
        public AnimationCurve OpenCurve;

        private int _currentPlayerOverlaps;
        private float _openProgress;
        private Vector3 _startPos;
        private Quaternion _startRot;

        void Start()
        {
            _startPos = Lid.position;
            _startRot = Lid.rotation;
        }

        void Update()
        {
            if ((_openProgress != 0 && _currentPlayerOverlaps == 0) || _openProgress != 1 && _currentPlayerOverlaps !=0)
            {
                _openProgress += (_currentPlayerOverlaps > 0 ? 1 : -1) * Time.deltaTime * 2f;
                _openProgress = Mathf.Clamp01(_openProgress);
            }
            Lid.transform.position = Vector3.Lerp(_startPos, LidDestination.position, OpenCurve.Evaluate(_openProgress));
            Lid.transform.rotation = Quaternion.Lerp(_startRot, LidDestination.rotation, OpenCurve.Evaluate(_openProgress));
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _currentPlayerOverlaps++;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _currentPlayerOverlaps--;
            }
        }
    }
}