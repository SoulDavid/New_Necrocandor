using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class TrapController : MonoBehaviour
{
    [SerializeField] private bool active = false;
    [SerializeField] private GameObject colliderTrap;
    [SerializeField] UnityEvent CollisionAction;
    [SerializeField] UnityEvent TriggerAction;
    [SerializeField] private string[] tagsToCheck;
    [SerializeField] private GameObject collisionGameObject;

    private void Awake()
    {
        collisionGameObject = null;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetActive()
    {
        return active;
    }

    public void SetActive()
    {
        if (active)
            active = false;
        else
            active = true;

        colliderTrap.SetActive(active);
    }

    public GameObject GetCollision()
    {
        return collisionGameObject;
    }

    public void SetCollisionGameObject(GameObject newCollision)
    {
        collisionGameObject = newCollision;
    }

    public void CollisionDetected(TrapCollision childScript, GameObject collision)
    {
        if(tagsToCheck.Contains(collision.gameObject.tag))
        {
            SetCollisionGameObject(collision);
            if (CollisionAction != null)
                CollisionAction.Invoke();
        }

    }

    public void TriggerDetected(TrapCollision childScript, GameObject collision)
    {
        if (tagsToCheck.Contains(collision.gameObject.tag))
        {
            SetCollisionGameObject(collision);
            if (TriggerAction != null)
                TriggerAction.Invoke();
        }
    }
}
