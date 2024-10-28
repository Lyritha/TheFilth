using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuScript : MonoBehaviour
{
    [SerializeField] Popup popup = null;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Cursor.visible = true;
        popup.EnablePopup();
    }

    public void TryAgain()
    {
        PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed") + 1);
        StartCoroutine(TryAgainAnim());
    }

    public void MainMenu()
    {
        StartCoroutine(MainMenuAnim());
    }

    IEnumerator MainMenuAnim()
    {
        popup.DisablePopup();

        yield return new WaitForSecondsRealtime(popup.DisableSpeed);

        PlayerPrefs.SetInt("PlayStartupAnim", 1);
        SceneManager.LoadScene("MainMenu");

        yield return null;
    }

    IEnumerator TryAgainAnim()
    {
        popup.DisablePopup();

        yield return new WaitForSecondsRealtime(popup.DisableSpeed);

        SceneManager.LoadScene("MainGame");

        yield return null;
    }
}
