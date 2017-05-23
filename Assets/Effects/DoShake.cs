using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoShake : MonoBehaviour {

    public bool Shaking;
    public float amnt;
    public float decreaseFactor;
    public Quaternion rot;
    Vector3 pos;
    Vector3 startPos;

void Start()
    {
        startPos = transform.position;
    }
    
void Update()
    {
        if (amnt > 0)
        {
            

            transform.position = pos + Random.insideUnitSphere * amnt;
            transform.rotation = new Quaternion(rot.x * .01f,
                                rot.y * .01f,
                                rot.z * .01f,
                                rot.w * .01f);

            amnt -= decreaseFactor;
        }
        else if (Shaking)
        {
            Shaking = false;
        }
        
        if (amnt <= 0)
        {
            transform.position = startPos;
        }

    }
    public void Doshake(float time = .3f, float decrease = .2f)
    {
        pos = transform.position;
        rot = transform.rotation;
        amnt = time;
        decreaseFactor = decrease;
        Shaking = true;
    }

}
