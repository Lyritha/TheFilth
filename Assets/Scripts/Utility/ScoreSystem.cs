using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] int score = 0;
    [SerializeField] TMP_Text text = null;

    PlayerShoot playershoot;

    private void Start()
    {
        playershoot = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>();
    }

    public void AddScore(int pAddScore)
    {
        score += pAddScore;
        UpdateScore();
        UpdatePlayerPref();

        if (score % 30 == 0)
        {
            playershoot.AddSpecial();
        }
    }

    void UpdateScore()
    {
        text.text = $"Score: {score}";
    }

    void UpdatePlayerPref()
    {
        int highscore = PlayerPrefs.GetInt("Highscore");

        if (score > highscore)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
    }


}
