using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class PlaySong : MonoBehaviour
{
    public int Track;
    void Start()
    {
        AudioController.Instance.PlayTrack(Track);
    }
}
