using UnityEngine;

// [CreateAssetMenu(fileName = "NewAudioManager", menuName = "ScriptableObject/AudioManager")]
public class AudioClipsSO : ScriptableObject
{
    public AudioClip multipleBullets;
    public AudioClip[] individualBullets;
    public AudioClip[] casingDrops;
    public AudioClip explosion;
    public AudioClip[] farExplosions;
    public AudioClip[] shotgunPump;
    public AudioClip[] shotgunReload;
    public AudioClip[] sniperReload;
    public AudioClip[] backgroundMusic;

    public AudioClip GetRandomAudioClip(AudioClip[] audioClips) => audioClips[Random.Range(0, audioClips.Length)];
}
