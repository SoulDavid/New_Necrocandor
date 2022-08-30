using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAddedAtScene : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private GameManager gameManager;
    [SerializeField] private List<GameObject> prefabPlayer = new List<GameObject>();
    [SerializeField] private List<Transform> prefabSpawns = new List<Transform>();
    private int playerSpawned = 0;
    private int _id;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        inputManager = GetComponent<PlayerInputManager>();

        inputManager.playerPrefab = prefabPlayer[0];
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SwitchNextPlayer(PlayerInput playerInput)
    {
        inputManager.playerPrefab = prefabPlayer[1];
        playerSpawned++;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.currentControlScheme == gameManager.GetCurrentControlSchemePlayer(0))
            _id = 0;
        else
            _id = 1;

        playerInput.gameObject.transform.root.GetComponent<State>().SetId(_id);
        //playerInput.gameObject.transform.GetChild(0).GetComponent<StartLevel>().SetId(_id);
        playerInput.gameObject.transform.position = prefabSpawns[playerSpawned].position;
    }
}
