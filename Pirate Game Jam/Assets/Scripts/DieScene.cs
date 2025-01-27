using System.Collections;
using JetBrains.Annotations;
using Unity.Cinemachine;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DieScene : MonoBehaviour
{
    //getting components
    Camera mainCam;
    private CinemachineCamera cam;
    private CinemachineConfiner2D camConfiner;
    GameObject playerCameraBounds;
    Coroutine coroutine;
    private Transform player;
    [SerializeField] float duration = 5f;
    [SerializeField] float cameraZoom = 3.68f;
    [SerializeField] float deathRadius;
    [SerializeField] GameObject explotion;

    public void Awake()
    {
       PlayerMovement.playerDies += Dies;
       cam = GetComponent<CinemachineCamera>();
       camConfiner = GetComponent<CinemachineConfiner2D>();
       playerCameraBounds = GameObject.FindWithTag("CameraBoundingBox");
       player = GameObject.FindWithTag("Player").GetComponent<Transform>();     
    }

    private void OnDestroy()
    {
        PlayerMovement.playerDies -= Dies;
    }
    void Dies()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(DieAnimation());
        }
    }
    public IEnumerator DieAnimation()
    {
        camConfiner.BoundingShape2D = null;
        this.transform.position = new Vector3(playerCameraBounds.transform.position.x, playerCameraBounds.transform.position.y, -10);
        PanelsManager.canReadInput = false;

        float temp = 0;
        float camInitialZoom = cam.Lens.OrthographicSize;

        //getting the camera position
        float cameraX = this.transform.position.x;
        float cameraY = this.transform.position.y;

        //getting the player X and Y 
        float playerX = player.position.x;
        float playerY = player.position.y;

        while (duration > temp)
        {
            temp += Time.unscaledDeltaTime;
            cam.Lens.OrthographicSize = Mathf.Lerp(camInitialZoom, cameraZoom, temp/duration);
            transform.position = new Vector3(Mathf.Lerp(cameraX, playerX, temp / duration), Mathf.Lerp(cameraY, playerY, temp / duration), -10);
            yield return null;
        }

        cam.Lens.OrthographicSize = cameraZoom;
        transform.position = new Vector3(player.position.x, player.position.y, -10);

        for (int i = 0; i < 20; i++)
        {
            DeathExplosion();
            yield return new WaitForSecondsRealtime(0.2f);
        }
        PanelsManager.Instance.GameOver();
    }

    void DeathExplosion()
    {
        Vector2 pos = Random.insideUnitCircle * deathRadius + (Vector2)transform.position;
        GameObject explosion = Instantiate(explotion, pos, Quaternion.identity);
        Destroy(explosion, .3f);
    }
}
