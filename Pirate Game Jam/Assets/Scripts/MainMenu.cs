using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() {
        GameManager.Instance.LoadScene(1);
        PanelsManager.canReadInput = true;
     }

    public void QuitGame ()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }


}
