using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuText : MonoBehaviour
{
    [SerializeField] TMP_Text highscore = null;
    [SerializeField] TMP_Text gamesplayed = null;

    // Start is called before the first frame update
    void Start()
    {
        if (highscore != null) highscore.text = $"Highscore: {PlayerPrefs.GetInt("Highscore")}";
        if (gamesplayed != null) gamesplayed.text = $"Games Player: {PlayerPrefs.GetInt("GamesPlayed")}";
    }
}
