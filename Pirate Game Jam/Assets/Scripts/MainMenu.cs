using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button startGameButton;

    void Awake()
    {
        startGameButton.onClick.AddListener(PlayGame);
    }

    void PlayGame() 
    {
        AudioManager.Instance.PlayMusic(1);
        GameManager.Instance.LoadScene(1);
        PanelsManager.canReadInput = true;
    }

    public void QuitGame ()
    {
        Application.Quit();
    }


}
