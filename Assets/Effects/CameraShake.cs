using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public DoShake camShakeRef;
    
    public bool Shaking;
    
    void Start()
    {
        Shaking = false;
    }

   

    void OnTriggerEnter(Collider collision)
    {
        

        camShakeRef.Doshake();
            

       
    }
	
}
