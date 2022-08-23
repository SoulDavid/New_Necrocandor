using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    private enum State
    {
        Idle,
        InAir,
        Walk,
        Hooking,
        Impulsing,
        Impulsed
    }

    [SerializeField]
    State currentState = State.Idle;

    private Ground ground;
    private bool onGrounded;
    public bool onImpulse = false;
    private Rigidbody2D body;
    private Animator anim;

    public void Jump()
    {
        switch(currentState)
        {
            case State.Idle:
                currentState = State.InAir;
                break;

            case State.Walk:
                currentState = State.InAir;
                break;

            case State.Impulsed:
                currentState = State.InAir;
                break;
        }
    }

    public void Hook()
    {
        if(onGrounded)
        {
            switch (currentState)
            {
                case State.Idle:
                    currentState = State.Hooking;
                    break;

                case State.Hooking:
                    currentState = State.Hooking;
                    break;

                case State.Walk:
                    currentState = State.Hooking;
                    break;
            }
        }
    }

    public void Land()
    {
        switch(currentState)
        {
            case State.InAir:
                currentState = State.Idle;
                break;
        }
    }

    public void Walk()
    {
        if(onGrounded)
        {
            switch (currentState)
            {
                case State.Idle:
                    currentState = State.Walk;
                    break;
                case State.InAir:
                    if(onGrounded)
                        currentState = State.InAir;
                    break;
            }
        }
    }

    public void Idle()
    {
        switch (currentState)
        {
            case State.Idle:
                currentState = State.Idle;
                break;
            case State.Walk:
                currentState = State.Idle;
                break;
            case State.Hooking:
                currentState = State.Idle;
                break;
            case State.InAir:
                currentState = State.Idle;
                break;
        }
        
    }

    public void Impulsing()
    {
        if(onGrounded)
        {
            onImpulse = true;

            switch(currentState)
            {
                case State.Idle:
                    currentState = State.Impulsing;
                    break;
                case State.Walk:
                    currentState = State.Impulsing;
                    break;
            }
        }
    }

    public void Impulsed()
    {
        if(onGrounded)
        {
            switch(currentState)
            {
                case State.Impulsing:
                    currentState = State.Impulsed;
                    break;
            }

        }
    }

    public string CheckState()
    {
        switch(currentState)
        {
            case State.Idle:
                return "Idle";
            case State.Hooking:
                return "Hooking";
            case State.InAir:
                return "InAir";
            case State.Walk:
                return "Walk";
            case State.Impulsing:
                return "Impulsing";
            case State.Impulsed:
                return "Impulsed";
        }

        return "No State";
    }

    private void StateAnim()
    {
        switch (currentState)
        {
            case State.Idle:
                anim.SetInteger("State", 0);
                break;
            case State.Hooking:
                anim.SetInteger("State", 3);
                break;
            case State.InAir:
                anim.SetInteger("State", 2);
                break;
            case State.Walk:
                anim.SetInteger("State", 1);
                break;
            case State.Impulsing:
                anim.SetInteger("State", 4);
                break;
            case State.Impulsed:
                break;
        }
    }

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        ground = GetComponent<Ground>();
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        onGrounded = ground.GetOnGround();

        if (onGrounded && body.velocity.x == 0 && !onImpulse && CheckState()!= "Hooking")
        {
            Idle();
        }

        StateAnim();
    }
}
