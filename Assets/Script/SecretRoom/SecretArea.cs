using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretArea : MonoBehaviour
{
    public SpriteRenderer[] wallElements;
    float alphaValue = 1;

    //Tiempo en el que desaparece la pared
    public float disappearRate = 1f;
    bool playerEntered = false;

    //El area secreta se mantenga abierta o cerrada una vez el jugador salga de esta
    public bool toggleWall = false;

    // Update is called once per frame
    void Update()
    {
        if(playerEntered)
        {
            alphaValue -= Time.deltaTime * disappearRate;

            if (alphaValue <= 0)
                alphaValue = 0;

            foreach(SpriteRenderer wallItem in wallElements)
            {
                wallItem.color = new Color(wallItem.color.r, wallItem.color.g, wallItem.color.b, alphaValue);
            }
        }
        else
        {
            alphaValue += Time.deltaTime * disappearRate;

            if (alphaValue >= 1)
                alphaValue = 1;

            foreach (SpriteRenderer wallItem in wallElements)
            {
                wallItem.color = new Color(wallItem.color.r, wallItem.color.g, wallItem.color.b, alphaValue);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerEntered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && toggleWall)
        {
            playerEntered = false;
        }
    }
}
