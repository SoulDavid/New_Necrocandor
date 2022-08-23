using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons_InitialMenu : MonoBehaviour
{
    [SerializeField] private GameObject panelOptions;

    public void NewGame()
    {
        SceneManager.LoadScene("SelectionCharacterMenu");
    }

    public void ShowOptions()
    {
        panelOptions.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
