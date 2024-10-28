using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClickingSound : MonoBehaviour
{
    [SerializeField] GameObject audio_Click = null;

    public void ClickSound()
    {
        GameObject audio = Instantiate(audio_Click);
        DontDestroyOnLoad(audio);
    }
}
