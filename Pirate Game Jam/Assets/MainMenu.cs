using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() => GameManager.Instance.LoadScene(1);

    public void QuitGame ()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }


}
