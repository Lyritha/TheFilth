using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Popup mainPopup = null;
    [SerializeField] Popup ConfirmationPopup = null;
    [SerializeField] GameObject navBar = null;
    [SerializeField] Image blackFadeImage = null;

    [SerializeField] GameObject audio_Startup = null;
    [SerializeField] GameObject audio_Static = null;
    [SerializeField] GameObject audio_Music = null;

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
        if (PlayerPrefs.GetInt("PlayStartupAnim") == 0) StartCoroutine(TurnOnAnim());
        else
        {
            StartCoroutine(MainMenuAnim());
            PlayerPrefs.SetInt("PlayStartupAnim", 0);
        }
    }

    // Start is called before the first frame update
    public void StartGame()
    {
        PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed") + 1);
        SceneManager.LoadScene("MainGame");
    }

    public void QuitGame()
    {
        mainPopup.DisablePopup();
        ConfirmationPopup.EnablePopup();
    }

    public void ConfirmYes()
    {
        Application.Quit();
    }

    public void ConfirmNo()
    {
        ConfirmationPopup.DisablePopup();
        mainPopup.EnablePopup();
    }

    IEnumerator MainMenuAnim()
    {
        navBar.transform.localPosition = new(navBar.transform.localPosition.x, navBar.transform.localPosition.y + 70);
        StartCoroutine(FadeFromBlack(0.5f));
        mainPopup.EnablePopup();

        yield return null;
    }

    IEnumerator TurnOnAnim()
    {
        yield return new WaitForSecondsRealtime(2f);

        GameObject audio = Instantiate(audio_Static);
        DontDestroyOnLoad(audio);

        yield return new WaitForSecondsRealtime(1f);

        GameObject audio1 = Instantiate(audio_Startup);
        DontDestroyOnLoad(audio1);
        AudioHandler audioHandler = audio1.GetComponent<AudioHandler>();

        StartCoroutine(FadeFromBlack(audioHandler.AudioLength / 2));

        yield return new WaitForSecondsRealtime(audioHandler.AudioLength / 4);

        StartCoroutine(NavBarAnim(audioHandler.AudioLength / 4));

        yield return new WaitForSecondsRealtime(audioHandler.AudioLength / 4);

        mainPopup.EnablePopup(audioHandler.AudioLength / 8);

        GameObject audio2 = Instantiate(audio_Music);
        DontDestroyOnLoad(audio2);

        yield return null;
    }

    IEnumerator NavBarAnim(float animTime)
    {
        float StartY = navBar.transform.localPosition.y;
        float TargetY = navBar.transform.localPosition.y + 70;

        float time = 0;
        while (time <= animTime)
        {
            float t = time / animTime;
            navBar.transform.localPosition = new Vector2(navBar.transform.localPosition.x, EaseInOutLerpFloat(StartY, TargetY, t));

            time += Time.unscaledDeltaTime;

            yield return null;
        }

        navBar.transform.localPosition = new(navBar.transform.localPosition.x, TargetY);

        yield return null;
    }

    IEnumerator FadeFromBlack(float fadeTime)
    {
        Color currentColor = blackFadeImage.color;
        Color targetColor = new(0,0,0,0);

        float time = 0;
        while (time <= fadeTime)
        {
            float t = time / fadeTime;
            blackFadeImage.color = EaseInLerpColor(currentColor, targetColor, t);

            time += Time.unscaledDeltaTime;

            yield return null;
        }

        blackFadeImage.color = targetColor;

        yield return null;
    }

    Color EaseInLerpColor(Color a, Color b, float t)
    {
        t = t * t; // Squaring the t parameter creates the easing effect
        return Color.Lerp(a, b, t);
    }

    float EaseInOutLerpFloat(float a, float b, float t)
    {
        if (t < 0.5f)
        {
            t = 4 * t * t * t; // Ease-in for the first half
        }
        else
        {
            t = 1 - Mathf.Pow(-2 * t + 2, 3) / 2; // Ease-out for the second half
        }
        return Mathf.Lerp(a, b, t);
    }

}
