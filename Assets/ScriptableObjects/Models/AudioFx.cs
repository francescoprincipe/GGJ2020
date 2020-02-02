using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/WorkenchAudioFX", fileName = "WorkBenchAudioFx")]
public class AudioFx : ScriptableObject
{
    public List<AudioClip> buildClips = new List<AudioClip>();
    public List<AudioClip> dashClips = new List<AudioClip>();
    public List<AudioClip> destroyClips = new List<AudioClip>();
    public AudioClip iteraction;
    public AudioClip button;
}