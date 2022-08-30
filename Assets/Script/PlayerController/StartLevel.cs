using System.Collections;
using System.Collections.Generic;
using Anima2D;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private SpriteMeshInstance[] body;
    [SerializeField] private int _id;
    [SerializeField] Color color;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        for (int i = 0; i < body.Length; ++i)
        {
            body[i].GetComponent<SpriteMeshInstance>();
        }

        SetPlayerCharacteristics(_id);

    }

    public void SetId(int newId)
    {
        _id = newId;
    }

    public void SetPlayerCharacteristics(int newId)
    {
        color = gameManager.GetColorPlayer(newId);

        for (int i = 0; i < body.Length; ++i)
        {
            body[i].sharedMaterial.SetColor("_Color", color);
            body[i].sharedMaterial.color = color;
        }
    }
}
