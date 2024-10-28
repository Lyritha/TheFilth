using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPopupHandler : MonoBehaviour
{
    [SerializeField] Popup popup;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("HelpPopup") == 0)
        {
            popup.EnablePopup();
        }
    }

    // Update is called once per frame
    void Update()
    {
        int isHelpPopupOn = PlayerPrefs.GetInt("HelpPopup");

        if (Input.GetKeyDown(KeyCode.Tab) && isHelpPopupOn == 0)
        {
            popup.DisablePopup();
            PlayerPrefs.SetInt("HelpPopup", 1);
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isHelpPopupOn == 1)
        {
            popup.EnablePopup();
            PlayerPrefs.SetInt("HelpPopup", 0);
        }
    }
}
