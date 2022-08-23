using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHook : MonoBehaviour
{
    private bool Hook = false;
    private bool desiredHook = false;

    private Ground ground;
    private bool onGround;

    private PlayerMovement player;
    private State StateController;

    private Rigidbody2D body;

    [SerializeField]
    private GameObject hookObj;
    [SerializeField]
    private Transform RightHookSpawn;
    [SerializeField]
    private Transform LeftHookSpawn;

    public bool hookCreated;

    public int MaxHooksAtSameTime = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        player = GetComponent<PlayerMovement>();
        StateController = GetComponent<State>();
    }

    public void GetHook(InputAction.CallbackContext context)
    {
        Hook = context.action.triggered;
    }

    // Update is called once per frame
    void Update()
    {
        desiredHook |= Hook;
    }

    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();

        if(desiredHook)
        {
            HookAction();
            desiredHook = false;
        }

        if(StateController.GetState() != "Hooking") 
        {
            MaxHooksAtSameTime = 0;
        }
    }
    public void HookAction()
    {
        if(MaxHooksAtSameTime <= 0 && onGround)
        {
            MaxHooksAtSameTime++;
            hookCreated = true;
            StartCoroutine(hookTime());
        }
    }

    private IEnumerator hookTime()
    {
        yield return new WaitForSeconds(0.09f);

        if (player.GetDirection() == 0)
        {
            var hookRight = Instantiate(hookObj, RightHookSpawn.position, RightHookSpawn.rotation);
            hookRight.GetComponent<HookController>().Init(0, RightHookSpawn, body);
        }
        else
        {
            var hookRight = Instantiate(hookObj, LeftHookSpawn.position, LeftHookSpawn.rotation);
            hookRight.GetComponent<HookController>().Init(1, LeftHookSpawn, body);
        }
    }
}
