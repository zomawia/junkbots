using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Text))]
public class HealthText : MonoBehaviour {

    public CharacterData player;

    BodyPart head;
    BodyPart torso;
    BodyPart leftArm;
    BodyPart rightArm;

    Text myText;

    // Use this for initialization
    void Start () {
        head = player.head.GetComponent<BodyPart>();
        torso = player.torso.GetComponent<BodyPart>();
        leftArm = player.leftArm.GetComponent<BodyPart>();
        rightArm = player.rightArm.GetComponent<BodyPart>();

        myText = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        myText.text = head.name + ": " + head.totalHealth + "\n" +
            torso.name + ": " + torso.totalHealth + "\n" +
            leftArm.name + ": " + leftArm.totalHealth + "\n" +
            rightArm.name + ": " + rightArm.totalHealth;

    }
}
