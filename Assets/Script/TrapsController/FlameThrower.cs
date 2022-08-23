using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    [SerializeField] private ParticleSystem fire;
    [SerializeField] private TrapController trap;

    // Start is called before the first frame update
    void Start()
    {
        trap = GetComponent<TrapController>();
        fire = transform.GetChild(1).GetComponent<ParticleSystem>();
        StartCoroutine(Damage());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Damage()
    {
        while(true)
        {
            fire.Stop();

            yield return new WaitForSeconds(1.5f);

            trap.SetActive();
            fire.Play();

            yield return new WaitForSeconds(1.5f);

            trap.SetActive();
        }
    }
}
