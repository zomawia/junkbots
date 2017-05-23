using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle : MonoBehaviour {

    public ParticleSystem psytem;
    

    void Awake()
    {
        psytem = GetComponentInChildren<ParticleSystem>();
        
    }

    void OnTriggerEnter(Collider collision)
    {
        psytem.Play();

    }

    
}
