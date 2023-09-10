using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{

    public class ScriptedBook : MonoBehaviour
    {
        public Transform Book, Target;
        public CollisionBroadcaster CollisionListener;
        public AnimationCurve FallCurve;

        private bool _fallen;
        private Vector3 _startPos;
        private Quaternion _startRot;

        void Start()
        {
            _startPos = Book.position;
            _startRot = Book.rotation;
            CollisionListener.CollisionEnter += ChildCollided;
        }

        private void ChildCollided(Collision col)
        {
            if (!_fallen)
                StartCoroutine(Fall());
        }

        IEnumerator Fall()
        {
            _fallen = true;
            float cursor = 0;
            while (Vector3.Distance(Book.transform.position, Target.transform.position) > 0.1f
                || Quaternion.Angle(Book.transform.rotation, Target.transform.rotation) > 2f)
            {
                cursor += Time.deltaTime;
                Book.transform.position = Vector3.Lerp(_startPos, Target.transform.position, FallCurve.Evaluate(cursor));
                Book.transform.rotation = Quaternion.Lerp(_startRot, Target.transform.rotation, FallCurve.Evaluate(cursor));
                yield return null;
            }
        }


        void Update()
        {

        }
    }
}