using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ColorCharacter : MonoBehaviour
{
    [SerializeField] private List<Color> colors = new List<Color>();
    [SerializeField] private int actualColor;
    [SerializeField] private Image[] body;
    [SerializeField] private Color newColor;
    [SerializeField] private PlayerInput playerInput;
    private GameManager gameManager;
    [SerializeField] private int _id;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        playerInput = GetComponent<PlayerInput>();

        newColor = colors[0];
        newColor.a = 1f;

        for (int i = 0; i < body.Length; i++)
        {
            body[i].GetComponent<Image>();
            body[i].color = newColor;
        }

        body[body.Length - 1].color = newColor;
        actualColor = 0;
    }

    public Color GetColor()
    {
        return newColor;
    }

    public void SetNumberOfPlayer(int newId)
    {
        _id = newId;
    }

    public void ColorSelected(InputAction.CallbackContext context)
    {
        if(context.action.triggered)
        {
            Debug.Log(playerInput.currentControlScheme);
            gameManager.SetCurrentControlSchemePlayer(playerInput.currentControlScheme, _id);
            gameManager.SetColorPlayer(newColor, _id);
        }

        if(gameManager.GetReadyToStart())
        {
            SceneManager.LoadScene("Scene_Kitchen");
        }
    }

    public void OnColorChangeRight(InputAction.CallbackContext context)
    {
        if(context.action.triggered)
        {
            if (actualColor >= colors.Count - 1)
                actualColor = -1;

            newColor = colors[actualColor + 1];
            newColor.a = 1f;
            for (int i = 0; i < body.Length - 1; i++)
            {
                body[i].color = newColor;
            }

            body[body.Length - 1].color = newColor;
            actualColor += 1;
        }
    }

    public void OnColorChangeLeft(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            if(actualColor <= 0)
                actualColor = colors.Count;

            newColor = colors[actualColor - 1];
            newColor.a = 1f;
            for (int i = 0; i < body.Length; i++)
            {
                body[i].color = newColor;
            }

            body[body.Length - 1].color = newColor;
            actualColor -= 1;
        }
    }
}
