using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Gameplay
{
    public class Pellet : MonoBehaviour
    {
        public bool isFinalPellete = false;
        public float AddedLength = 2.5f;
        private Vector3 _initialPos;
        private float _liveTime = 0f;
        private float _bobRate = 1f;
        private float _bobDist = 0.1f;

        private void Start()
        {
            _initialPos = transform.position;
            _liveTime = Random.value;
        }

        private void ClayUpdate()
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
                    controller.MaxBodyLength += AddedLength;
                    Object.Destroy(gameObject);

                    if (controller.MaxBodyLength > 60) //mid bathroom
                        AudioController.Instance.TriggerDialogue(3);
                    else if (controller.MaxBodyLength > 27) //mid bedroom
                        AudioController.Instance.TriggerDialogue(1);
                    else if (controller.MaxBodyLength > 5) // first thing
                        AudioController.Instance.TriggerDialogue(0);
                    // Lvl 1 end handled by lvl 1 end script

                    AudioController.Play(AudioController.Instance.Assets.ClayPickups.GetRandom(), false);
                    if(Random.value>0.9f)
                        AudioController.Play(AudioController.Instance.Assets.CatChirps.GetRandom(), false).volume = 0.4f;
                }
                if (isFinalPellete)
                {
                    var subtitleObj = GameObject.Find("final-image");
                    if (subtitleObj && subtitleObj.GetComponent<TitleSplash>())
                    {
                        subtitleObj.GetComponent<TitleSplash>().ShowText(5.0f);
                    }
                }
            }
        }
    }
}