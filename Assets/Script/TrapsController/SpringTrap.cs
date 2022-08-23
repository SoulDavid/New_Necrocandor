using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringTrap : MonoBehaviour
{
    public enum Type { LEFT, CENTER, RIGHT };
    public Type directionType;

    [SerializeField] private float force;
    [SerializeField] private int multiplyerForce;
    [SerializeField] private Animator anim;
    [SerializeField] private TrapController trapController;
    [SerializeField] private Vector2 forceVector;
    private GameObject collidedWidth;
    private int typeDirection;

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        trapController = GetComponent<TrapController>();

        switch(directionType)
        {
            case Type.LEFT:
                forceVector = new Vector2(-force * multiplyerForce, force * multiplyerForce);
                typeDirection = 1;
                break;

            case Type.CENTER:
                forceVector = new Vector2(0, force * multiplyerForce);
                typeDirection = 2;
                break;

            case Type.RIGHT:
                forceVector = new Vector2(force * multiplyerForce, force * multiplyerForce);
                typeDirection = 3;
                break;
        }
    }

    public void StartCoroutineSpringTrap()
    {
        StartCoroutine(GetForceToPlayer());
    }

    public IEnumerator GetForceToPlayer()
    {
        anim.SetInteger("Type", typeDirection);
        collidedWidth = trapController.GetCollision();

        if (collidedWidth != null)
        {
            collidedWidth.transform.GetComponent<Rigidbody2D>().AddRelativeForce(forceVector);
        }

        trapController.SetActive();

        yield return new WaitForSeconds(1f);

        trapController.SetActive();
        anim.SetInteger("Type", 0);
    }


}
