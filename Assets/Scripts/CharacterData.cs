using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holds and processes all character data
//Including health, stamina, and anything else we need
public class CharacterData : MonoBehaviour {

    public float maxHealth;
    public float baseDamage;
    public float stamina = 100;
    public float maxStamina = 100;

    public GameObject head;
    public GameObject torso;
    public GameObject leftArm;
    public GameObject rightArm;

    public Transform headPos;
    public Transform torsoPos;
    public Transform leftArmPos;
    public Transform rightArmPos;

    private GameObject headActive;
    private GameObject torsoActive;
    private GameObject leftArmActive;
    private GameObject rightArmActive;

    public float totalHealth
    {
        get { return headActive.GetComponent<BodyPart>().totalHealth +
                     torsoActive.GetComponent<BodyPart>().totalHealth +
                     leftArmActive.GetComponent<BodyPart>().totalHealth +
                     rightArmActive.GetComponent<BodyPart>().totalHealth; }
    }

    // Use this for initialization

    private void OnValidate()
    {
        if (headActive != null)
        {
            headActive.transform.SetPositionAndRotation(headPos.position, headPos.rotation);
        }
        if (torsoActive != null)
        {
            torsoActive.transform.SetPositionAndRotation(torsoPos.position, torsoPos.rotation);
        }
            
        if (leftArmActive != null)
        {
            leftArmActive.transform.SetPositionAndRotation(leftArmPos.position, leftArmPos.rotation);
            leftArmActive.GetComponent<BodyPart>().part = Part.leftArm;
        }
            
        if (rightArmActive != null)
        {
            rightArmActive.transform.SetPositionAndRotation(rightArmPos.position, rightArmPos.rotation);
            rightArmActive.GetComponent<BodyPart>().part = Part.rightArm;
        }
            
    }

    void Start () {
        attachParts();
    }

    void Update()
    {
        if (stamina < maxStamina)
            stamina += Time.deltaTime;

        if (stamina > maxStamina)
            stamina = maxStamina;
    }

    private void Awake()
    {
        attachParts();
    }

    public void CheckHealth()
    {
        if (headActive.GetComponent<BodyPart>().totalHealth <= 0 || torsoActive.GetComponent<BodyPart>().totalHealth <= 0)
            Destroy(this.gameObject);
    }

    public void attachParts()
    {
        //TODO: attach animations/rig

        //clear the parts
        if(headActive != null) Destroy(headActive);
        if(torsoActive != null) Destroy(torsoActive);
        if (leftArmActive != null) Destroy(leftArmActive);
        if (rightArmActive != null) Destroy(rightArmActive);

        Debug.LogWarning("CharacterData customization not yet implemented.");
        return;

        headActive = Instantiate(head, headPos);
        torsoActive = Instantiate(torso, torsoPos);
        leftArmActive = Instantiate(leftArm, leftArmPos);
        rightArmActive = Instantiate(rightArm, rightArmPos);
        rightArmActive.transform.Rotate(180, 180, 0);

        if (headActive != null) headActive.GetComponent<BodyPart>().character = this;
        if (torsoActive != null) torsoActive.GetComponent<BodyPart>().character = this;
        if (leftArmActive != null) leftArmActive.GetComponent<BodyPart>().character = this;
        if (rightArmActive != null) rightArmActive.GetComponent<BodyPart>().character = this;
    }

}
