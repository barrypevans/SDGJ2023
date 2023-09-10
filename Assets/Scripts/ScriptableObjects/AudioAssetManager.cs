using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/AssetManager")]
public class AudioAssetManager : ScriptableObject
{

    public AudioClip[] Music;

    public AudioClip[] CatChirps;

    public AudioClip[] CatReacts;

    public AudioClip[] ClayPickups;

    public AudioClip StretchLoop;

    public AudioClip[] Dialogue;

    public AudioClip DoorOpen;
}
