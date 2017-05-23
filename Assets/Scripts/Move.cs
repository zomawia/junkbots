// Move script
// Zomawia Sailo
// For use with the Robot ring girl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    float timer = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(-transform.forward * Time.deltaTime);

        timer += Time.deltaTime;

        if (timer > 30)
            gameObject.SetActive(false);

    }
}
