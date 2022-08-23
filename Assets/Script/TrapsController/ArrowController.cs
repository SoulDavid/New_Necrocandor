using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private bool direction;
    [SerializeField] private Rigidbody2D body;

    private IObjectPool<ArrowController> arrowPool;

    public void SetPool(IObjectPool<ArrowController> _arrowPool)
    {
        arrowPool = _arrowPool;
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        if (direction)
            force = -force;
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = new Vector2(force, 0);
    }

    private void OnBecameInvisible()
    {
        arrowPool.Release(this);
    }
}
