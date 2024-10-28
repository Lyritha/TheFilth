using UnityEngine;
using TMPro;
using System.Collections;

public class WavePopup : MonoBehaviour
{
    [SerializeField] int wavePauseDuration = 3;
    [SerializeField] Popup popup = null;
    [SerializeField] TMP_Text timerText = null;
    [SerializeField] GameObject audio_Error = null;

    SpawnEnemies spawnEnemies = null;
    bool isEnabled = false;

    private void Start()
    {
        spawnEnemies = GameObject.FindGameObjectWithTag("GameController").GetComponent<SpawnEnemies>();
    }

    public void WavePause()
    {
        GameObject audio = Instantiate(audio_Error);
        DontDestroyOnLoad(audio);

        isEnabled = true;
        timerText.text = wavePauseDuration.ToString();

        popup.EnablePopup();
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        int currentTime = wavePauseDuration;

        while (currentTime != 0)
        {
            yield return new WaitForSecondsRealtime(1f);

            currentTime--;

            timerText.text = currentTime.ToString();

            yield return null;
        }

        WaveContinue();

        yield return null;
    }

    private void WaveContinue()
    {
        popup.DisablePopup();
        spawnEnemies.SpawnEnemiesGroup();
        isEnabled = false;
    }

    public bool IsEnabled { get { return isEnabled; } }
}
