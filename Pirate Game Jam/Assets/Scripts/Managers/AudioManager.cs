using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioClipsSO audioClips;
    public AudioSource musicSource;

    Scene scene;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        MusicVolume(0.5f);
        PlayMusic(scene.buildIndex);
    }

    void OnEnable()
    {
        //subscribing to the events
        WeaponSystem.onShotFired += ShotFired;
        ShotgunScript.onShotgunPump += ShotgunPump;
        EnemyWeapon.onShotFired += ShotFired;
        EnemyShotgun.onShotgunPump += ShotgunPump;
        GrenadeExplotion.onExplode += Explode;
        DieScene.onExplode += PlayerExplode;
        TankBoss.onExplode += Explode;
        HellicopBoss.onExplode += Explode;
        FortressBoss.onExplode += Explode;
        FortressWindow.onExplode += Explode;
        LivingGunBoss.onExplode += Explode;
    }

    void OnDisable()
    {
        //unsubscribing from the static events
        WeaponSystem.onShotFired -= ShotFired;
        ShotgunScript.onShotgunPump -= ShotgunPump;
        EnemyWeapon.onShotFired -= ShotFired;
        EnemyShotgun.onShotgunPump -= ShotgunPump;
        GrenadeExplotion.onExplode -= Explode;
        DieScene.onExplode -= PlayerExplode;
        TankBoss.onExplode -= Explode;
        HellicopBoss.onExplode -= Explode;
        FortressBoss.onExplode -= Explode;
        FortressWindow.onExplode -= Explode;
        LivingGunBoss.onExplode -= Explode;
    }

    public void PlayMusic(int sceneIndex)
    {
        musicSource.clip = audioClips.backgroundMusic[sceneIndex];
        musicSource.Play();
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    void PlaySound(AudioClip audioClip, Vector3 pos, float volume = 1) => AudioSource.PlayClipAtPoint(audioClip, pos, volume);

    void ShotFired(Transform trf) => PlaySound(audioClips.GetRandomAudioClip(audioClips.casingDrops), trf.position);

    void Explode(Transform trf) => PlaySound(audioClips.explosion, trf.position, 0.5f);

    void PlayerExplode(Transform trf) => PlaySound(audioClips.explosion, trf.position, 0.1f);

    private void ShotgunPump(Transform trf) => PlaySound(audioClips.GetRandomAudioClip(audioClips.shotgunPump), trf.position);
}
