using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    /// <summary>
    /// Puntos de ruta
    /// </summary>
    [SerializeField] private Transform[] points;
    public int pointsSize;

    /// <summary>
    /// Velocidad de movimiento
    /// </summary>
    [SerializeField] private float speed;

    /// <summary>
    /// Distancia minima en la que se considera que ha alcanzado el punto
    /// </summary>
    [SerializeField] private float minDistance = .5f;
    private int iterator = 0;

    /// <summary>
    /// Booleano para si la sierra va en dirección contraria
    /// </summary>
    private bool reverse;
    public bool Reverse { get => reverse; set => reverse = value; }

    /// <summary>
    /// Booleano que activa o desactiva el movimiento circular
    /// </summary>
    private bool circular;

    /// <summary>
    /// Booleano que activa el movimiento de las plataformas
    /// </summary>
    public bool activateMovement = false;

    /// <summary>
    /// Booleano para que reboten las plataformas
    /// </summary>
    public bool constant_Movement = false;

    public Transform startPoint;

    private SpriteRenderer platformRenderer;

    [SerializeField] private bool isInvisibleAtStart;

    // Start is called before the first frame update
    void Start()
    {
        platformRenderer = GetComponent<SpriteRenderer>();

        for (int i = 1; i < points.Length - 1; ++i)
        {
            points[i].GetComponent<SpriteRenderer>().enabled = false;
        }
        reverse = false;

        transform.position = startPoint.position;

        if(isInvisibleAtStart)
        {
            platformRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(activateMovement)
        {
            platformRenderer.enabled = true;
            FollowPoints();
        }
    }

    private void FollowPoints()
    {
        if (points.Length > 0)
        {
            //asignación del punto de destino actual
            Transform currentTarget = points[iterator];

            if (points[iterator] != null)
            {
                if (!constant_Movement)
                {
                    //si no se ha llegado al punto se mueve
                    if (Vector2.Distance(transform.position, currentTarget.position) > minDistance)
                    {
                        MoveToPoint(currentTarget);
                    }

                    if (reverse)
                    {
                        //siguiente punto
                        if (currentTarget != points[0])
                        {
                            currentTarget = points[iterator--];
                        }
                        else
                        {
                            currentTarget = points[0];

                        }
                        MoveToPoint(currentTarget);
                    }
                    else
                    {
                        //siguiente punto normal
                        if (currentTarget != points[points.Length - 1])
                        {
                            currentTarget = points[iterator++];
                        }

                        MoveToPoint(currentTarget);
                    }
                }


                else
                {

                    //si no se ha llegado al punto se mueve
                    if (Vector2.Distance(transform.position, currentTarget.position) > minDistance)
                    {
                        MoveToPoint(currentTarget);
                    }

                    else
                    {
                        //en el caso de llegar al punto, se cambia el de destino y se mueve hacia él
                        if (circular)
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
                        else if (reverse)
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
                            //siguiente punto normal
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
        }
    }

    private void MoveToPoint(Transform target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
