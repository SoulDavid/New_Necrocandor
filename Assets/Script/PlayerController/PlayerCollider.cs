using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private GameObject elementToThrow;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private State statePlayer;

    private bool canDialogue;
    private bool isDead;
    private bool canHold;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        statePlayer = GetComponent<State>();
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public bool GetCanHold()
    {
        return canHold;
    }

    public bool GetCanDialogue()
    {
        return canDialogue;
    }

    public GameObject GetElementToThrow()
    {
        return elementToThrow;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Spawn"))
        {
            playerSpawn = collision.gameObject.transform;
        }

        if (collision.gameObject.CompareTag("Trap"))
        {
            if (collision.gameObject.transform.parent.GetComponent<TrapController>().GetActive() && !isDead)
                StartCoroutine(DeadAction());
        }

        if(collision.gameObject.CompareTag("DialogueTrigger"))
        {
            GetComponent<DialogueController>().SetStartFromScratch(true);
            canDialogue = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            if (collision.gameObject.transform.root.GetComponent<TrapController>().GetActive() && !isDead)
                StartCoroutine(DeadAction());
        }

        if (collision.gameObject.CompareTag("HoldableObject"))
        {
            canHold = true;
            elementToThrow = collision.gameObject;
        }

        if(collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Quitando kinematic");
            body.isKinematic = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HoldableObject"))
        {
            canHold = false;
            elementToThrow = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DialogueTrigger"))
        {
            canDialogue = false;
        }
    }

    private IEnumerator DeadAction()
    {
        isDead = true;

        yield return new WaitForSeconds(1.75f);

        this.transform.position = playerSpawn.position;

        isDead = false;
    }
}
