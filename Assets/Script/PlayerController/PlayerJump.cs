using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    private bool Jump = false; 

    [SerializeField, Range(0f, 10f)] private float jumpHeight = 3f;
    [SerializeField, Range(0f, 5f)] private int maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f;

    private Rigidbody2D body;
    private Ground ground;
    private Vector2 velocity;

    [SerializeField]
    private int jumpPhase;
    private float defaultGravityScale;

    private bool desiredJump;
    [SerializeField]
    private bool onGround;

    private State stateController;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        stateController = GetComponent<State>();
        defaultGravityScale = 1f;
    }

    public void GetJumpTrigger(InputAction.CallbackContext context)
    {
        Jump = context.action.triggered;
    }

    // Update is called once per frame
    void Update()
    {
        desiredJump |= Jump;
    }

    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();
        velocity = body.velocity;

        if (onGround && !desiredJump && jumpPhase > 0)
        {
            jumpPhase = 0;
        }

        if(desiredJump)
        {
            desiredJump = false;
            Jump = false;
            JumpAction();
        }

        if(stateController.GetState() != "isDead")
        {
            if (body.velocity.y > 0)
                body.gravityScale = upwardMovementMultiplier;
            else if (body.velocity.y < 0)
                body.gravityScale = downwardMovementMultiplier;
            else if (body.velocity.y == 0)
                body.gravityScale = defaultGravityScale;
        }


        body.velocity = velocity;
    }

    private void JumpAction()
    {
        if (stateController.GetState() != "isDead" && stateController.GetState() != "Impulsing" && stateController.GetState() != "Hooking")
        {
            if (onGround || jumpPhase < maxAirJumps)
            {
                jumpPhase += 1;
                float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
                velocity.y += jumpSpeed;
            }
        }

    }
}
