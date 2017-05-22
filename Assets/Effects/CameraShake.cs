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
        //if(collision.gameObject.name == "Player2" || collision.gameObject.name == "Player1")

        camShakeRef.Doshake();
            

       
    }
	
}
