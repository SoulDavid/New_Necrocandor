using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHold : MonoBehaviour
{
    private PlayerCollider playerCollider;
    private State stateController;

    private Ground ground;
    private bool onGround;

    [SerializeField] private Transform siteToGrab;
    private GameObject elementToThrow = null;
    [SerializeField] private bool isHolding;
    [SerializeField] private float speed = 1f;
    private float startTime;
    private float journeyLength;


    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GetComponent<PlayerCollider>();
        stateController = GetComponent<State>();
        ground = GetComponent<Ground>();
    }

    public void HoldItem(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(onGround && playerCollider.GetCanHold())
            {
                HoldUp();
            }
        }
        else if(context.canceled)
        {
            HoldDown();
        }
    }

    private void HoldUp()
    {
        elementToThrow = playerCollider.GetElementToThrow();
        elementToThrow.GetComponent<Rigidbody2D>().isKinematic = true;
        elementToThrow.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        MovementObject();
        //elementToThrow.transform.position = Vector3.Lerp(elementToThrow.transform.position, siteToGrab.position, Time.deltaTime);
        elementToThrow.transform.SetParent(transform);
    }

    private void MovementObject()
    {
        if(!isHolding)
        {
            startTime = Time.time;
            journeyLength = Vector3.Distance(elementToThrow.transform.position, siteToGrab.position);
            isHolding = true;
        }
    }

    private void HoldDown()
    {
        isHolding = false;
        elementToThrow.GetComponent<Rigidbody2D>().isKinematic = false;
        elementToThrow.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        elementToThrow.transform.SetParent(null);
        isHolding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHolding)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionJourney = distCovered / journeyLength;
            elementToThrow.transform.position = Vector3.Lerp(elementToThrow.transform.position, siteToGrab.position, fractionJourney);
        }
    }

    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();
    }
}
