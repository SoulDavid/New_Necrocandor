using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    private bool isDead;

    public bool GetIsDead()
    {
        return isDead;
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            if (collision.gameObject.transform.root.GetComponent<TrapController>().GetActive() && !isDead)
                StartCoroutine(DeadAction());
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
