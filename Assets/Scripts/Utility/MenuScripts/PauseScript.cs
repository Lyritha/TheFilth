using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    [SerializeField] Popup mainPopup;
    [SerializeField] Popup confirmPopup;
    
    private void Start()
    {
        StartCoroutine(EnablePopupAnim());
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Resume();
        }
    }
    public void Resume() => StartCoroutine(ResumeAnim());

    public void Quit()
    {
        mainPopup.DisablePopup();
        confirmPopup.EnablePopup();
    }

    IEnumerator ResumeAnim()
    {
        Time.timeScale = 1;

        mainPopup.DisablePopup();

        yield return new WaitForSecondsRealtime(mainPopup.DisableSpeed);

        Cursor.visible = false;
        SceneManager.UnloadSceneAsync("PauseScreen");

        yield return null;
    }

    IEnumerator QuitAnim()
    {
        Time.timeScale = 1;

        mainPopup.DisablePopup();

        yield return new WaitForSecondsRealtime(mainPopup.DisableSpeed);

        PlayerPrefs.SetInt("PlayStartupAnim", 1);
        SceneManager.LoadScene("MainMenu");

        yield return null;
    }

    IEnumerator EnablePopupAnim()
    {
        Time.timeScale = 0;

        mainPopup.EnablePopup();

        yield return new WaitForSecondsRealtime(mainPopup.DisableSpeed);

        Cursor.visible = true;
    }

    public void ConfirmYes()
    {
        StartCoroutine(QuitAnim());
    }

    public void ConfirmNo()
    {
        confirmPopup.DisablePopup();
        mainPopup.EnablePopup();
    }
}
