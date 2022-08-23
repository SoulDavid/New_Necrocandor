using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons_Options_Menu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPage;
    [SerializeField] private GameObject ControlsPage;
    [SerializeField] private GameObject AudioPage;

    public void ShowControls()
    {
        ControlsPage.SetActive(true);
    }

    public void CloseControls()
    {
        ControlsPage.SetActive(false);
    }

    public void ShowAudio()
    {
        AudioPage.SetActive(true);
    }

    public void CloseAudio()
    {
        AudioPage.SetActive(false);
    }

    public void ShowKeyboardControls()
    {
        ControlsPage.transform.GetChild(1).gameObject.SetActive(false);
        ControlsPage.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ShowControllerControls()
    {
        ControlsPage.transform.GetChild(0).gameObject.SetActive(false);
        ControlsPage.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void Exit()
    {
        optionsPage.SetActive(false);
    }
}
