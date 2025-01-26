using System.Collections;
using Unity.Cinemachine;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DieScene : MonoBehaviour
{
    //getting components
    private CinemachineCamera cam;
    private Transform player;
    [SerializeField] float duration = 10f;
    [SerializeField] float cameraZoom = 3.68f;
    [SerializeField] float deathRadius;
    [SerializeField] GameObject explotion;

    public void Awake()
    {
       PlayerMovement.playerDies += Dies;
       cam = GetComponent<CinemachineCamera>();
       player = GameObject.FindWithTag("Player").GetComponent<Transform>();     
    }

    private void OnDestroy()
    {
        PlayerMovement.playerDies -= Dies;
    }
    void Dies()
    {
        StartCoroutine(DieAnimation());
    }
    public IEnumerator DieAnimation()
    {
        PanelsManager.canReadInput = false;
        yield return new WaitForSecondsRealtime(1);
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
            transform.position = new Vector3(Mathf.Lerp(cameraX, playerX, temp/duration), Mathf.Lerp(cameraY, playerY, temp / duration), -10);
            yield return null;
        }          
        cam.Lens.OrthographicSize = cameraZoom;
        DeathExplosion();
        yield return new WaitForSecondsRealtime(0.5f);
        DeathExplosion();
        yield return new WaitForSecondsRealtime(0.5f);
        DeathExplosion();
        yield return new WaitForSecondsRealtime(0.5f);
        PanelsManager.Instance.GameOver();
    }

    void DeathExplosion()
    {
        Vector2 pos = Random.insideUnitCircle * deathRadius + (Vector2)transform.position;
        GameObject explosion = Instantiate(explotion, pos, Quaternion.identity);
        Destroy(explosion, .3f);
    }
}
