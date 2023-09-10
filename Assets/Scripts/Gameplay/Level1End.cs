using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Level1End : MonoBehaviour
    {
        public float FallThreshold = 8f;
        public ScriptedBook ExitDoor;

        private bool _hasTriggered = false;
        void Update()
        {
            if (!_hasTriggered && transform.position.y < FallThreshold)
                TriggerLevelEnd();
        }
        private void TriggerLevelEnd()
        {
            if (_hasTriggered)
                return;

            _hasTriggered = true;

            ExitDoor.TriggerFall();

            // Todo: audio cues
        }
    }
}