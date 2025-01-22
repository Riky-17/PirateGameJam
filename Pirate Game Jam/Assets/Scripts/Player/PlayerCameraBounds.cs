using UnityEngine;

public class PlayerCameraBounds : MonoBehaviour
{
    Transform player;

    public bool isLockScreen;
    Vector3 savedPlayerPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraBoundsPosition();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LockScreen lockscreen = collision.GetComponent<LockScreen>();
        if (lockscreen != null)
        {
            savedPlayerPosition = new Vector3(player.position.x, 0, 0); // Stores player's x position when triggering a gameobject with lockscreen component
            isLockScreen = true;
        }
    }

    void CameraBoundsPosition()
    {
        if (isLockScreen)
        {
            transform.position = savedPlayerPosition; // Camera bounds freezes
        }
        else if (!isLockScreen)
        {
            transform.position = new Vector3(player.position.x, 0, 0); // Camera bounds moves horizontally with the player
        }
    }
}
