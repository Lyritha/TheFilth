using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomName : MonoBehaviour
{
    [SerializeField] string[] names = null;
    [SerializeField] TMP_Text text = null;

    // Start is called before the first frame update
    void Start()
    {
        text.text = names[Random.Range(0, names.Length)];
    }
}
