using System.Collections;
using TMPro;
using UnityEngine;

public class PanelsManager : MonoBehaviour
{
    //panels
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] TMP_Text countDown;
    float timer = 3f;
    bool isPaused = false;
    public static bool canReadInput = true;
    private void Awake()
    {
        countDown.gameObject.SetActive(false);
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)||
            Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();               
            }
            else if (isPaused)
            {
                UnpauseGame();
            }
        }
    }

    void PauseGame()
    {
        countDown.gameObject.SetActive(true);
        pausePanel.SetActive(true);
        gamePanel.SetActive(false);
        Time.timeScale = 0;
        AudioListener.pause = true;
        isPaused = true;
        canReadInput = false;
    }
    public void UnpauseGame()
    {
        StartCoroutine(ContinueDelay());         
    }
    IEnumerator ContinueDelay()
    {
        timer = 3f;
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        while(timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            countDown.text = Mathf.Ceil(timer).ToString();
            yield return null;
        }
        countDown.gameObject.SetActive(false);
        Time.timeScale = 1;
        AudioListener.pause = false;
        isPaused = false;
        canReadInput = true;
    }
}
