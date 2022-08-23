using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Anima2D.SpriteMeshInstance[] body;
    [SerializeField] private int _id;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    public void SetId(int newId)
    {
        _id = newId;
    }

    public void SetPlayerCharacteristics(int newId)
    {
        for (int i = 0; i < body.Length; ++i)
        {
            body[i].color = gameManager.GetColorPlayer(newId);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < body.Length; ++i)
        {
            body[i].GetComponent<Anima2D.SpriteMeshInstance>();
        }

        SetPlayerCharacteristics(_id);
    }
}
