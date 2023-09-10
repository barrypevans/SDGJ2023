using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

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
            ExitDoor.Book.GetComponent<Collider>().enabled = false;

            AudioController.Instance.TriggerDialogue(2);
            AudioController.Play(AudioController.Instance.Assets.DoorOpen, false);

            // Todo: audio cues
        }
    }
}