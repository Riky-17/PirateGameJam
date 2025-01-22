using System.Collections.Generic;
using UnityEngine;

public class LockScreen : MonoBehaviour
{
    PlayerCameraBounds playerCameraBounds;

    public List<Enemy> Enemies { get; private set; } = new();

    bool isCompleted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCameraBounds = GameObject.FindWithTag("CameraBoundingBox").GetComponent<PlayerCameraBounds>();
    }

    // Update is called once per frame
    void Update()
    {
        Completed();
    }

    void Completed()
    {
        if (isCompleted || Input.GetKeyDown(KeyCode.F)) // F key input is temporary until implementations to set isCompleted to true
        { 
            playerCameraBounds.isLockScreen = false;
            Destroy(gameObject);
        }
    }
}
