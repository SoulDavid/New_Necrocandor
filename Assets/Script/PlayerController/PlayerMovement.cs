using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float inputMovement = 0f;

    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D body;
    private Ground ground;
    private State stateController;

    private float maxSpeedChange;
    private float acceleration;
    private bool OnGround;

    public enum Direction { LEFT, RIGHT };
    public Direction currentDirection;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        stateController = GetComponent<State>();
    }

    public void GetPlayerMovement(InputAction.CallbackContext context)
    { 
        inputMovement = context.ReadValue<float>();

        if(stateController.GetState() != "Hooking" && stateController.GetState() != "Impulsing" && stateController.GetState() != "isDead")
        {
            if (inputMovement > 0)
                SetDirection("RIGHT");
            else if (inputMovement < 0)
                SetDirection("LEFT");
        }
    }

    private void SetDirection(string newDirection)
    {
        switch(newDirection)
        {
            case "LEFT":
                currentDirection = Direction.LEFT;
                transform.GetChild(0).localScale = new Vector3(1, 1);
                break;
            case "RIGHT":
                currentDirection = Direction.RIGHT;
                transform.GetChild(0).localScale = new Vector2(-1, 1);
                break;
            default:
                Debug.LogError("Esta direccion de movimiento no existe");
                break;
        }
    }

    public int GetDirection()
    {
        if (currentDirection == Direction.RIGHT)
            return 0;
        else
            return 1;
    }

    // Update is called once per frame
    void Update()
    {
        desiredVelocity = new Vector2(inputMovement, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(), 0f);
    }

    private void FixedUpdate()
    {
        OnGround = ground.GetOnGround();
        velocity = body.velocity;

        if(stateController.GetState() != "Hooking" && stateController.GetState() != "Impulsing" && stateController.GetState() != "isDead") 
        {
            acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
            maxSpeedChange = acceleration * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            body.velocity = velocity;
        }
        else
        {
            body.velocity = Vector2.zero;
        }
    }
}
