using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamic_Saw : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private TrapController trapController;
    [SerializeField] private ParticleSystem sparkle;

    [SerializeField] private Transform[] points;
    [SerializeField] private float speedSaw;
    int iterator = 0;
    float minDistance = 0.5f;
    private bool reverse;
    private bool circular;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        trapController = transform.parent.GetComponent<TrapController>();
        sparkle = transform.GetChild(0).GetComponent<ParticleSystem>();

        for(int i = 1; i < points.Length; i++)
        {
            points[i].GetComponent<SpriteRenderer>().enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        FollowPoints();
    }

    private void FollowPoints()
    {
        //while(trapController.GetActive())
        //{
        if (points.Length > 0)
        {
            //asignación del punto de destino actual
            Transform currentTarget = points[iterator];

            if (points[iterator] != null)
            {
                //si no se ha llegado al punto se mueve
                if (Vector2.Distance(transform.position, currentTarget.position) > minDistance && trapController.GetActive())
                {
                    MoveToPoint(currentTarget);
                }
                //en el caso de llegar al punto, se cambia el de destino y se mueve hacia él
                else if (circular)
                {
                    if (currentTarget != points[points.Length - 1])
                    {
                        currentTarget = points[iterator++];
                        Debug.Log(currentTarget);
                    }
                    else
                    {
                        currentTarget = points[0];
                        iterator = 0;
                    }
                    MoveToPoint(currentTarget);

                }
                else
                {
                    if (reverse)
                    {
                        //siguiente punto
                        if (currentTarget != points[0])
                        {
                            currentTarget = points[iterator--];
                        }
                        else
                        {
                            reverse = false;
                        }
                    }

                    else
                    {
                        //siguiente punto
                        if (currentTarget != points[points.Length - 1])
                        {
                            currentTarget = points[iterator++];
                        }
                        else
                        {
                            reverse = true;
                        }
                    }

                    MoveToPoint(currentTarget);
                }

            }

        }

        //}
    }

    private void MoveToPoint(Transform target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speedSaw * Time.deltaTime);
    }

    IEnumerator Damage()
    {
        sparkle.Play();
        anim.SetBool("Damaged", true);
        trapController.SetActive();

        yield return new WaitForSeconds(2f);

        sparkle.Stop();
        anim.SetBool("Damaged", false);
        trapController.SetActive();
        FollowPoints();
    }

    private void StartMyCoroutine()
    {
        StartCoroutine(Damage());
    }

    public void GotDamaged()
    {
        StartMyCoroutine();
    }
}
