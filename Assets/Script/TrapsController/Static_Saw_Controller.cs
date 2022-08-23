using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static_Saw_Controller : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private TrapController trap;
    [SerializeField] private ParticleSystem sparkle;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        sparkle = transform.GetChild(1).GetComponent<ParticleSystem>();  
    }

    IEnumerator Damage()
    {
        sparkle.Play();
        anim.SetBool("Damaged", true);
        trap.SetActive();
            
        yield return new WaitForSeconds(2f);

        sparkle.Stop();
        anim.SetBool("Damaged", false);
        trap.SetActive();
    }

    private void StartMyCoroutine()
    {
        StartCoroutine(Damage());
    }

    public void ParentScript()
    {
        StartMyCoroutine();
    }
}
