using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerImpulse : MonoBehaviour
{
    private State stateController;
    private Rigidbody2D body;
    private Ground ground;

    private bool onGround;
    private bool Impulse;

    private Vector2 velocity;

    [SerializeField] private float TimeJumpCanalization = 0f;
    [SerializeField] private float[] JumpCanalizationPhase;
    [SerializeField] private float[] JumpMultiplifyer;
    [SerializeField] private float jumpForce;

    private void Awake()
    {
        stateController = GetComponent<State>();
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
    }

    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();
        velocity = body.velocity;

        if(stateController.GetState() == "Impulsing") TimeJumpCanalization += Time.deltaTime;
        if(stateController.GetState() == "Impulsed")
        {
            ImpulseAction();
            TimeJumpCanalization = 0f;
        }

        body.velocity = velocity;
    }

    private void ImpulseAction()
    {
        if(onGround)
        {
            float jumpForce = ForceImpulse(TimeJumpCanalization);
            velocity.y += jumpForce;
        }
    }

    private float ForceImpulse(float TimeJumpCanalization)
    {
        if(TimeJumpCanalization <= JumpCanalizationPhase[0])
        {
            return jumpForce * JumpMultiplifyer[0];
        }
        else if(TimeJumpCanalization > JumpCanalizationPhase[0] && TimeJumpCanalization <= JumpCanalizationPhase[1])
        {
            return jumpForce * JumpMultiplifyer[1];
        }
        else
        {
            return jumpForce * JumpMultiplifyer[2];
        }
    }
}
