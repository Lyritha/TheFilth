using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Clock : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    // Update is called once per frame
    void FixedUpdate()
    {
        text.text = DateTime.Now.ToString("hh:mm tt");
    }
}
