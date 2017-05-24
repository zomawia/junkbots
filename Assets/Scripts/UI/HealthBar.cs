using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public CharacterData bot;
    public Part type;
    public Texture2D img;

    private BodyPart part;
    private Color healthColor;

	// Use this for initialization
	void Start () {
        healthColor = Color.red;

        //gameObject.GetComponent<Image>().material.SetTexture("_MainTex", img);
        StartCoroutine(StartWait());
        resetPart();        
    }

    //Wait for a wee bit to make sure the body parts have been applied to the character.
    IEnumerator StartWait()
    {
        yield return new WaitForSeconds(.5f);
    }

    public void resetPart()
    {
        switch (type)
        {
            case Part.head:
                part = bot.headActive.GetComponent<BodyPart>();
                break;
            case Part.torso:
                part = bot.torsoActive.GetComponent<BodyPart>();
                break;
            case Part.leftArm:
                part = bot.leftArmActive.GetComponent<BodyPart>();
                break;
            case Part.rightArm:
                part = bot.rightArmActive.GetComponent<BodyPart>();
                break;
        }
    }

    private void Update()
    {
        gameObject.GetComponent<Image>().material.SetFloat("_Progress", (part.totalHealth / part.maxHealth));
    }

}
