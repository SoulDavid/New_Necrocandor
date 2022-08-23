using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitch : MonoBehaviour
{
    [SerializeField] List<GameObject> players = new List<GameObject>();
    [SerializeField] List<GameObject> texts = new List<GameObject>();
    [SerializeField] private List<ColorCharacter> playersSelection = new List<ColorCharacter>();
    [SerializeField] private GameObject textToStart;
    private PlayerInputManager _InputManager;
    private int numberOfPlayers;

    private void Awake()
    {
        _InputManager = GetComponent<PlayerInputManager>();
        _InputManager.playerPrefab = players[0];
        numberOfPlayers = 0;
    }

    public void SwitchNextSpawnCharacter(PlayerInput input)
    {
        _InputManager.playerPrefab = players[1];
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playersSelection.Add(playerInput.gameObject.GetComponent<ColorCharacter>());
        playerInput.gameObject.transform.SetParent(this.gameObject.transform, false);
        playerInput.gameObject.GetComponent<ColorCharacter>().SetNumberOfPlayer(numberOfPlayers);
        texts[numberOfPlayers].SetActive(false);
        numberOfPlayers += 1;

        if (numberOfPlayers == 2)
            textToStart.SetActive(true);
    }
}
