using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioClipsSO audioClips;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        //subscribing to the events
        WeaponSystem.onShotFired += ShotFired;
        ShotgunScript.onShotgunPump += ShotgunPump;
        EnemyWeapon.onShotFired += ShotFired;
        EnemyShotgun.onShotgunPump += ShotgunPump;
    }

    void OnDisable()
    {
        //unsubscribing from the static events
        WeaponSystem.onShotFired -= ShotFired;
        ShotgunScript.onShotgunPump -= ShotgunPump;
        EnemyWeapon.onShotFired -= ShotFired;
        EnemyShotgun.onShotgunPump -= ShotgunPump;
    }

    void PlaySound(AudioClip audioClip, Vector3 pos, float volume = 1) => AudioSource.PlayClipAtPoint(audioClip, pos, volume);

    void ShotFired(Transform trf) => PlaySound(audioClips.GetRandomAudioClip(audioClips.casingDrops), trf.position);

    private void ShotgunPump(Transform trf) => PlaySound(audioClips.GetRandomAudioClip(audioClips.shotgunPump), trf.position);
}
