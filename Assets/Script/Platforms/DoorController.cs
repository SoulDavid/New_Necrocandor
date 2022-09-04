using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private ParticleSystem portal;
    private bool isOpen;

    public void SetIsOpen(bool _isOpen)
    {
        isOpen = _isOpen;
    }

    public bool GetIsOpen()
    {
        return isOpen;
    }

    // Start is called before the first frame update
    void Start()
    {
        portal = transform.GetChild(0).GetComponent<ParticleSystem>();
        anim = GetComponent<Animator>();

        portal.Stop();
    }

    public void OpenDoor()
    {
        anim.SetBool("IsOpen", true);
        portal.Play();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
