using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HookController : MonoBehaviour
{
    public string[] tagsToCheck;
    public float speed, returnSpeed, range, stopRange;

    [SerializeField]
    private float forceToGoToWall = 10f;

    //[HideInInspector]
    public Transform caster, collidedWidth;
    [HideInInspector]
    private Rigidbody2D rb_caster;
    [SerializeField]
    private LineRenderer line;
    [SerializeField]
    private SpriteRenderer spriteRender;
    private bool hasCollided;

    private int direction;
    private bool HookCreated = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRender = transform.GetChild(0).GetComponent<SpriteRenderer>();
        line = transform.GetChild(1).GetComponent<LineRenderer>();
    }

    public void Init(int _direction, Transform _caster, Rigidbody2D _rbCaster)
    {
        if(!HookCreated)
        {
            direction = _direction;
            caster = _caster;
            rb_caster = _rbCaster; 
            HookCreated = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(caster)
        {
            line.SetPosition(0, caster.position);
            line.SetPosition(1, transform.position);

            if(hasCollided)
            {
                transform.LookAt(caster);

                var dist = Vector2.Distance(transform.position, caster.position);

                if(dist < stopRange)
                {
                    caster.root.gameObject.GetComponent<PlayerHook>().hookCreated = false;
                    Destroy(gameObject); 
                }
            }
            else
            {
                var distancia = Vector2.Distance(transform.position, caster.position);

                if(distancia > range)
                {
                    Collision(null, tagsToCheck.Length + 1);
                }
            }

            switch (direction)
            {
                case 0:
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                    break;
                case 1:
                    spriteRender.flipX = false;
                    transform.Translate(Vector2.left * speed * Time.deltaTime);
                    break;
            }

            if (collidedWidth)
                collidedWidth.transform.position = transform.position;
        }
        else
        {
            caster.root.gameObject.GetComponent<PlayerHook>().hookCreated = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!hasCollided && tagsToCheck.Contains(collision.tag))
        {
            int positionTag = System.Array.IndexOf(tagsToCheck, collision.tag);
            Collision(collision.gameObject, positionTag);
        }
    }

    void Collision(GameObject col, int item)
    {
        speed = returnSpeed;
        hasCollided = true;

        if(col)
        {
            switch(item)
            {
                case 0:
                    if(direction == 0)
                    {
                        Vector2 VectorForceRight = new Vector2(forceToGoToWall, 0);
                        rb_caster.velocity = VectorForceRight;
                    }
                    else
                    {
                        Vector2 VectorForceLeft = new Vector2(-forceToGoToWall, 0);
                        rb_caster.velocity = VectorForceLeft;
                    }

                    break;

                case 1:
                    caster.root.gameObject.GetComponent<PlayerHook>().hookCreated = false;
                    Destroy(gameObject);
                    break;

                case 2:
                    caster.root.gameObject.GetComponent<PlayerHook>().hookCreated = false;
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
