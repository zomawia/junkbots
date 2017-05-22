using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoShake : MonoBehaviour {

    public bool Shaking;
    public float amnt;
    public float decreaseFactor;
    public Quaternion rot;
    Vector3 pos;
    
void Update()
    {
        if (amnt > 0)
        {
            transform.position = pos + Random.insideUnitSphere * amnt;
            transform.rotation = new Quaternion(rot.x * .2f,
                                rot.y  * .2f,
                                rot.z  * .2f,
                                rot.w * .2f);

            amnt -= decreaseFactor;
        }
        else if (Shaking)
        {
            Shaking = false;
        }
    }
    public void Doshake()
    {
        pos = transform.position;
        rot = transform.rotation;
        amnt = 0.3f;
        decreaseFactor = 0.2f;
        Shaking = true;
    }

}
