using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    public float maxNum;
    public float currentNum { get; set; }
	// Use this for initialization
	void Start () {
        currentNum = maxNum;
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Image>().material.SetFloat("_Progress", (currentNum/maxNum));
    }
}
