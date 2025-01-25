using System.Collections;
using TMPro;
using UnityEngine;

public class CreditsScene : MonoBehaviour
{
    [SerializeField] private TMP_Text credits;
    [SerializeField] private TMP_Text thanks;

    private void Awake()
    {
        credits.gameObject.SetActive(false);
        thanks.gameObject.SetActive(true);
        StartCoroutine(playingCreditsScene());

    }
    IEnumerator playingCreditsScene()
    {
        yield return new WaitForSecondsRealtime(5f);
        credits.gameObject.SetActive(true);
        thanks.gameObject.SetActive(false);
    }
}
