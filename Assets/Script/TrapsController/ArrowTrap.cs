using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private ArrowController arrowScriptPrefab;
    [SerializeField] private int maxArrowInScreen; 
    private IObjectPool<ArrowController> arrowPool;

    public int[] Patron = new int[] { 1, 2, 3, 3, 2, 1, 1, 3, 2 };
    public Transform[] spawnPoints = new Transform[3];
    public float TimeSimpleSet;
    public float TimeFinishedSet;
    private int contadorArrow = 1;
    private Vector3 position;

    private void Awake()
    {
        arrowPool = new ObjectPool<ArrowController>(
            CreateBullet,
            OnGet,
            OnRelease,
            OnDestroyArrow,
            maxSize: maxArrowInScreen
            );

        StartCoroutine(Shoot());
    }

    private ArrowController CreateBullet()
    {
        ArrowController arrow = Instantiate(arrowScriptPrefab, GetPosition(), transform.rotation);
        arrow.SetPool(arrowPool);
        return arrow;
    }

    private void OnGet(ArrowController arrow)
    {
        arrow.gameObject.SetActive(true);
        arrow.transform.position = GetPosition();
    }

    private void OnRelease(ArrowController arrow)
    {
        arrow.gameObject.SetActive(false);
    }

    private void OnDestroyArrow(ArrowController arrow)
    {
        Destroy(arrow.gameObject);
    }

    private void SetPosition(Vector3 _newPos)
    {
        position = _newPos;
    }

    private Vector3 GetPosition()
    {
        return position;
    }

    IEnumerator Shoot()
    {
        while(true)
        {
            for (int i = 0; i < Patron.Length; ++i)
            {
                Debug.Log(i);
                if (contadorArrow % 4 != 0)
                {
                    yield return new WaitForSeconds(TimeSimpleSet);
                    SetPosition(spawnPoints[Patron[i] - 1].position);
                    arrowPool.Get();
                }
                else
                {
                    yield return new WaitForSeconds(TimeFinishedSet);
                    for (int j = 0; j < 3; ++j)
                    {
                        SetPosition(spawnPoints[j].position);
                        arrowPool.Get();
                    }
                    --i;
                }
                contadorArrow++;
            }
        }
    }
}
