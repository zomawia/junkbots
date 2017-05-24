using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Part { head, torso, leftArm, rightArm};

[RequireComponent(typeof(Collider))]
public class BodyPart : MonoBehaviour {

    public string partName;
    public Part part;

    public float maxHealth = 1;

    public float flinchDuration = 1;
    public float flinchThreshold = 1;

    public float moveSpeed = 1;
    public float cooldownSpeed = 1;
    public float staminaRegen = 1;

    public float punchSpeed = 1;
    public float punchDamage = 1;
    public float blockAmount = 1;

    public float totalHealth;

    public CharacterData character { get; set; }

	// Use this for initialization
	void Start () {
        totalHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CheckHealth()
    {
        character.CheckHealth();

        //if (totalHealth <= 0)
        //    Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<DamageScript>() != null)
        {
            DamageScript dmg = other.gameObject.GetComponent<DamageScript>();
            totalHealth -= dmg.damageAmount;
            CheckHealth();
        }
    }
}
