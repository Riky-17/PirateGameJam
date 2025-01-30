using System.Collections;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelsManager : MonoBehaviour
{
    //panels
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject quitConfirmationPanel;
    [SerializeField] public GameObject gameOverPanel;
    [SerializeField] TMP_Text countDown;
    float timer = 3f;
    bool isPaused = false;
    public static bool canReadInput = true;
    public static PanelsManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        countDown.gameObject.SetActive(false);
        quitConfirmationPanel.gameObject.SetActive(false);
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) ||
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
    public void onClickMenu()
    {
        quitConfirmationPanel.gameObject.SetActive(true);
    }
    public void ButtonNo()
    {
        quitConfirmationPanel.gameObject.SetActive(false);
    }

    public void loadingSceneOnclick(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Single);
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
    public IEnumerator ContinueDelay()
    {
        timer = 3f;
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        while (timer > 0)
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
    public void RestartScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        canReadInput = true;
    }

    
    public  void GameOver() => gameOverPanel.gameObject.SetActive(true);
}
