using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class State : MonoBehaviour
{
    private Animator anim;
    private Ground ground;
    private PlayerInput playerControls;
    private PlayerHook playerHook;
    private Rigidbody2D body;
    private PlayerCollider playerLife;
    StateBehavior currentState;
    [SerializeField] private string state;

    private void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        playerControls = GetComponent<PlayerInput>();
        ground = GetComponent<Ground>();
        playerHook = GetComponent<PlayerHook>();
        playerLife = GetComponent<PlayerCollider>();
        currentState = new Idle(anim, playerControls, ground, body, playerHook, playerLife);
    }

    private void Update()
    {
        state = GetState();
        currentState = currentState.Process();
    }

    public string GetState()
    {
        return currentState.GetState();
    }
}

public class StateBehavior
{
    public enum STATE
    {
        IDLE, WALKING, HOOKING, ONAIR, IMPULSING, IMPULSED, ISDEAD, HOLDING, THROWING
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected Animator anim;
    protected PlayerInput playerControls;
    protected Ground onGround;
    protected Rigidbody2D body;
    protected PlayerHook playerHook;
    protected PlayerCollider playerLife;
    [SerializeField] protected StateBehavior nextState;

    public StateBehavior(Animator _anim, PlayerInput _playerControls, 
        Ground _ground, Rigidbody2D _body, PlayerHook _playerHook, PlayerCollider _playerLife)
    {
        anim = _anim;
        playerControls = _playerControls;
        onGround = _ground;
        body = _body;
        playerHook = _playerHook;
        playerLife = _playerLife;
        stage = EVENT.ENTER;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public StateBehavior Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public string GetState()
    {
        switch(name)
        {
            case STATE.IDLE:
                return "Idle";
            case STATE.WALKING:
                return "Walk";
            case STATE.ONAIR:
                return "OnAir";
            case STATE.IMPULSING:
                return "Impulsing";
            case STATE.IMPULSED:
                return "Impulsed";
            case STATE.HOOKING:
                return "Hooking";
            case STATE.ISDEAD:
                return "isDead";
            case STATE.HOLDING:
                return "Holding";
            case STATE.THROWING:
                return "Throwing";
        }

        return "No State";
    }
}

public class Idle : StateBehavior
{
    private bool isJumping = false;

    public Idle(Animator _anim, PlayerInput _playerControls, Ground _ground, Rigidbody2D _body, PlayerHook _playerHook, PlayerCollider _playerLife)
        : base(_anim, _playerControls, _ground, _body, _playerHook, _playerLife)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        anim.SetTrigger("isIdle");
        base.Enter();
    }

    public override void Update()
    {
        if(!playerLife.GetIsDead())
        {
            if (playerControls.actions["Movement"].ReadValue<float>() != 0)
            {
                nextState = new Walking(anim, playerControls, onGround, body, playerHook, playerLife);
                stage = EVENT.EXIT;
            }
            if (playerControls.actions["Jump"].triggered)
            {
                isJumping = true;
            }
            if (playerControls.actions["Impulse"].triggered)
            {
                nextState = new Impulsing(anim, playerControls, onGround, body, playerHook, playerLife);
                stage = EVENT.EXIT;
            }
            if (playerControls.actions["Hook"].triggered)
            {
                nextState = new Hooking(anim, playerControls, onGround, body, playerHook, playerLife);
                stage = EVENT.EXIT;
            }

            if(playerControls.actions["Hold"].triggered && playerLife.GetCanHold())
            {
                nextState = new Holding(anim, playerControls, onGround, body, playerHook, playerLife);
                stage = EVENT.EXIT;
            }

            if (isJumping && !onGround.GetOnGround())
            {
                nextState = new Jumping(anim, playerControls, onGround, body, playerHook, playerLife);
                stage = EVENT.EXIT;
            }
        }
        else
        {
            nextState = new Dead(anim, playerControls, onGround, body, playerHook, playerLife);
            stage = EVENT.EXIT;
        }

    }

    public override void Exit()
    {
        anim.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class Walking : StateBehavior
{
    private bool isJumping = false;

    public Walking(Animator _anim, PlayerInput _playerControls, Ground _ground, Rigidbody2D _body, PlayerHook _playerHook, PlayerCollider _playerLife)
        : base(_anim, _playerControls, _ground, _body, _playerHook, _playerLife)
    {
        name = STATE.WALKING;
    }

    public override void Enter()
    {
        anim.SetTrigger("isWalking");
        base.Enter();
    }

    public override void Update()
    {
        if(!playerLife.GetIsDead())
        {
            if (playerControls.actions["Movement"].ReadValue<float>() == 0)
            {
                nextState = new Idle(anim, playerControls, onGround, body, playerHook, playerLife);
                stage = EVENT.EXIT;
            }
            if (playerControls.actions["Jump"].triggered)
            {
                isJumping = true;
            }
            if (playerControls.actions["Impulse"].triggered)
            {
                nextState = new Impulsing(anim, playerControls, onGround, body, playerHook, playerLife);
                stage = EVENT.EXIT;
            }
            if (playerControls.actions["Hook"].triggered)
            {
                nextState = new Hooking(anim, playerControls, onGround, body, playerHook, playerLife);
                stage = EVENT.EXIT;
            }

            if (isJumping && !onGround.GetOnGround())
            {
                nextState = new Jumping(anim, playerControls, onGround, body, playerHook, playerLife);
                stage = EVENT.EXIT;
            }
        }
        else
        {
            nextState = new Dead(anim, playerControls, onGround, body, playerHook, playerLife);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isWalking");
        base.Exit();
    }
}

public class Hooking : StateBehavior
{
    public Hooking(Animator _anim, PlayerInput _playerControls, Ground _ground, Rigidbody2D _body, PlayerHook _playerHook, PlayerCollider _playerLife)
        : base(_anim, _playerControls, _ground, _body, _playerHook, _playerLife)
    {
        name = STATE.HOOKING;
    }

    public override void Enter()
    {
        anim.SetTrigger("isHooking");
        base.Enter();
    }

    public override void Update()
    {
        if(!playerLife.GetIsDead())
        {
            if (!playerHook.hookCreated)
            {
                if (playerControls.actions["Movement"].ReadValue<float>() == 0)
                {
                    nextState = new Idle(anim, playerControls, onGround, body, playerHook, playerLife);
                    stage = EVENT.EXIT;
                }
                if (onGround.GetOnGround() && playerControls.actions["Movement"].ReadValue<float>() != 0)
                {
                    nextState = new Walking(anim, playerControls, onGround, body, playerHook, playerLife);
                    stage = EVENT.EXIT;
                }
            }
        }
        else
        {
            nextState = new Dead(anim, playerControls, onGround, body, playerHook, playerLife);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isHooking");
        base.Exit();
    }
}

public class Impulsing : StateBehavior
{
    public Impulsing(Animator _anim, PlayerInput _playerControls, Ground _ground, Rigidbody2D _body, PlayerHook _playerHook, PlayerCollider _playerLife)
        : base(_anim, _playerControls, _ground, _body, _playerHook, _playerLife)
    {
        name = STATE.IMPULSING;
    }

    public override void Enter()
    {
        anim.SetTrigger("isImpulsing");
        base.Enter();
    }

    public override void Update()
    {
        if(!playerLife.GetIsDead())
        {
            if (playerControls.actions["Impulse"].WasReleasedThisFrame())
            {
                nextState = new Impulsed(anim, playerControls, onGround, body, playerHook, playerLife);
                stage = EVENT.EXIT;
            }
        }
        else
        {
            nextState = new Dead(anim, playerControls, onGround, body, playerHook, playerLife);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isImpulsing");
        base.Exit();
    }
}

public class Impulsed : StateBehavior
{
    public Impulsed(Animator _anim, PlayerInput _playerControls, Ground _ground, Rigidbody2D _body, PlayerHook _playerHook, PlayerCollider _playerLife)
        : base(_anim, _playerControls, _ground, _body, _playerHook, _playerLife)
    {
        name = STATE.IMPULSED;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if(!playerLife.GetIsDead())
        {
            if (!onGround.GetOnGround())
            {
                nextState = new Jumping(anim, playerControls, onGround, body, playerHook, playerLife);
                stage = EVENT.EXIT;
            }
        }
        else
        {
            nextState = new Dead(anim, playerControls, onGround, body, playerHook, playerLife);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Jumping : StateBehavior
{
    public Jumping(Animator _anim, PlayerInput _playerControls, Ground _ground, Rigidbody2D _body, PlayerHook _playerHook, PlayerCollider _playerLife)
        : base(_anim, _playerControls, _ground, _body, _playerHook, _playerLife)
    {
        name = STATE.ONAIR;
    }

    public override void Enter()
    {
        base.Enter();
        anim.SetTrigger("isOnAir");
    }

    public override void Update()
    {
        if(!playerLife.GetIsDead())
        {
            if (onGround.GetOnGround())
            {
                if (playerControls.actions["Movement"].ReadValue<float>() == 0)
                {
                    nextState = new Idle(anim, playerControls, onGround, body, playerHook, playerLife);
                    stage = EVENT.EXIT;
                }
                if (playerControls.actions["Movement"].ReadValue<float>() != 0)
                {
                    nextState = new Walking(anim, playerControls, onGround, body, playerHook, playerLife);
                    stage = EVENT.EXIT;
                }
            }
        }
        else
        {
            nextState = new Dead(anim, playerControls, onGround, body, playerHook, playerLife);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isOnAir");
        base.Exit();
    }
}

public class Dead : StateBehavior
{
    public Dead(Animator _anim, PlayerInput _playerControls, Ground _ground, Rigidbody2D _body, PlayerHook _playerHook, PlayerCollider _playerLife)
        : base(_anim, _playerControls, _ground, _body, _playerHook, _playerLife)
    {
        name = STATE.ISDEAD;
    }

    public override void Enter()
    {
        anim.SetTrigger("isDead");
        base.Enter();
    }

    public override void Update()
    {
        if(onGround.GetOnGround() && !playerLife.GetIsDead())
        {
            nextState = new Idle(anim, playerControls, onGround, body, playerHook, playerLife);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isDead");
        base.Exit();
    }
}

public class Holding : StateBehavior
{
    public Holding(Animator _anim, PlayerInput _playerControls, Ground _ground, Rigidbody2D _body, PlayerHook _playerHook, PlayerCollider _playerLife)
        : base(_anim, _playerControls, _ground, _body, _playerHook, _playerLife)
    {
        name = STATE.HOLDING;
    }

    public override void Enter()
    {
        anim.SetTrigger("isHolding");
        base.Enter();
    }

    public override void Update()
    {
        if (!playerLife.GetIsDead())
        {
            if (playerControls.actions["Hold"].WasReleasedThisFrame())
            {
                nextState = new Idle(anim, playerControls, onGround, body, playerHook, playerLife);
                stage = EVENT.EXIT;
            }
        }
        else
        {
            nextState = new Dead(anim, playerControls, onGround, body, playerHook, playerLife);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isHolding");
        base.Exit();
    }
}

//public class Throwing : StateBehavior
//{
//    public Throwing(Animator _anim, PlayerInput _playerControls, Ground _ground, Rigidbody2D _body, PlayerHook _playerHook, PlayerCollider _playerLife)
//        : base(_anim, _playerControls, _ground, _body, _playerHook, _playerLife)
//    {
//        name = STATE.THROWING;
//    }

//    public override void Enter()
//    {
//        anim.SetTrigger("isThrowing");
//        base.Enter();
//    }

//    public override void Update()
//    {
//        if (!playerLife.GetIsDead())
//        {
//            if (!playerLife.GetCanHold() && playerLife.GetElementToThrow() == null)
//            {
//                nextState = new Idle(anim, playerControls, onGround, body, playerHook, playerLife);
//                stage = EVENT.EXIT;
//            }
//        }
//        else
//        {
//            nextState = new Dead(anim, playerControls, onGround, body, playerHook, playerLife);
//            stage = EVENT.EXIT;
//        }
//    }

//    public override void Exit()
//    {
//        anim.SetTrigger("isThrowing");
//        base.Exit();
//    }
//}