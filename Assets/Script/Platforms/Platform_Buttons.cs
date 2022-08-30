using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Platform_Buttons : MonoBehaviour
{
    public enum Type_Of_Button { ONE_USE, KEEP_PUSHING, TWO_BUTTONS }
    public Type_Of_Button typeButton;

    [SerializeField] private List<MovablePlatform> platforms_asigned = new List<MovablePlatform>();

    public Platform_Buttons pairButtons;

    [SerializeField] private bool is_activated;
    [SerializeField] private string[] tagsToCheck;
    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        is_activated = false;        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        is_activated = true;
        anim.SetBool("isPressed", is_activated);

        if(tagsToCheck.Contains(collision.tag))
        {
            switch(typeButton)
            {
                case Type_Of_Button.ONE_USE:
                    foreach (MovablePlatform platform in platforms_asigned)
                    {
                        platform.activateMovement = true;
                    }
                    break;
                case Type_Of_Button.KEEP_PUSHING:
                    foreach(MovablePlatform platform in platforms_asigned)
                    {
                        platform.activateMovement = true;

                        if (!platform.constant_Movement)
                            platform.Reverse = false;
                    }
                    break;
                case Type_Of_Button.TWO_BUTTONS:
                    if(pairButtons != null && pairButtons.is_activated)
                    {
                        foreach(MovablePlatform platform in platforms_asigned)
                        {
                            platform.activateMovement = true;

                            if (!platform.constant_Movement)
                                platform.Reverse = false;
                        }
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(tagsToCheck.Contains(collision.tag))
        {
            switch(typeButton)
            {
                case Type_Of_Button.ONE_USE:
                    break;
                case Type_Of_Button.KEEP_PUSHING:
                    foreach (MovablePlatform platform in platforms_asigned)
                    {
                        if (platform.constant_Movement)
                            platform.activateMovement = false;
                        else
                            platform.Reverse = true;
                    }
                    is_activated = false;
                    anim.SetBool("isPressed", is_activated);

                    break;
                case Type_Of_Button.TWO_BUTTONS:
                    foreach (MovablePlatform platform in platforms_asigned)
                    {
                        if (platform.constant_Movement)
                            platform.activateMovement = false;
                        else
                            platform.Reverse = true;
                    }
                    is_activated = false;
                    anim.SetBool("isPressed", is_activated);
                    break;
            }
        }
    }
}
