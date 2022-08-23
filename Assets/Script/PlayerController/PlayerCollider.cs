using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Spawn"))
        {
            playerSpawn = collision.gameObject.transform;
        }

        if (collision.gameObject.CompareTag("Trap"))
        {
            if (collision.gameObject.transform.parent.GetComponent<TrapController>().GetActive())
                this.transform.position = playerSpawn.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            if (collision.gameObject.transform.root.GetComponent<TrapController>().GetActive())
                this.transform.position = playerSpawn.position;
        }
    }
}
