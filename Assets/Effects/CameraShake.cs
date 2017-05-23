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

    //void OnTriggerEnter(Collider collision)
    //{
    //    camShakeRef.Doshake();       
    //}



    //Call this on FLINCH in playerController
    public void Shake(float time, float decrease)
    {
        camShakeRef.Doshake(time, decrease);
    }
	
}
