using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance { get => instance; }

    [SerializeField] private Color[] colorPlayers;
    [SerializeField] private string[] currentControlSchemes;
    [SerializeField] private bool[] ReadyToStart;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        colorPlayers = new Color[2];
        currentControlSchemes = new string[2];
        ReadyToStart = new bool[2];

        for (int i = 0; i < ReadyToStart.Length; ++i)
            ReadyToStart[i] = false;
    }

    public void SetColorPlayer(Color _color, int _id)
    {
        colorPlayers[_id] = _color;
        ReadyToStart[_id] = true;
    }

    public Color GetColorPlayer(int _id)
    {
        return colorPlayers[_id];
    }

    public void SetCurrentControlSchemePlayer(string _controlScheme, int _id)
    {
        currentControlSchemes[_id] = _controlScheme;
    }

    public string GetCurrentControlSchemePlayer(int _id)
    {
        return currentControlSchemes[_id];
    }

    public bool GetReadyToStart()
    {
        for(int i = 0; i < ReadyToStart.Length; ++i)
        {
            if (!ReadyToStart[i])
                return false;
        }

        return true;
    }
}
