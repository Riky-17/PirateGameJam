using UnityEngine;

public class LockScreen : MonoBehaviour
{
    PlayerCameraBounds playerCameraBounds;

    public GameObject boss;
    public GameObject bossHP;

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
        CheckLockScreen();
    }

    void Completed()
    {
        if (boss == null)
        { 
            //playerCameraBounds.isLockScreen = false;
            //Debug.Log("LockScreen Destroyed");
            Destroy(gameObject);
        }
    }

    void CheckLockScreen()
    {
        if (playerCameraBounds.isLockScreen)
        {
            boss.SetActive(true);
            bossHP.SetActive(true);
        }
    }
}
