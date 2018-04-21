using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundMap
{
    public Sound Sound;
    public AudioClip[] Clips;
}

[CreateAssetMenu(fileName = "Sound Context", menuName = "Context/Sound Context")]
public class SoundContext : ScriptableObject
{
    public SoundMap[] Map;
}
