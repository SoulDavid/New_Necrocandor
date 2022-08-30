using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableObject : MonoBehaviour
{
    [SerializeField] private ParticleSystem holdParticle;

    // Start is called before the first frame update
    void Start()
    {
        holdParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        holdParticle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
