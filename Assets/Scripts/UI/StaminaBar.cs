using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour {

    public CharacterData bot;

    private Color healthColor;

    // Use this for initialization
    void Start()
    {
        healthColor = Color.red;

        StartCoroutine(StartWait());
    }

    //Wait for a wee bit to make sure the body parts have been applied to the character.
    IEnumerator StartWait()
    {
        yield return new WaitForSeconds(.5f);
    }

    private void Update()
    {
        gameObject.GetComponent<Image>().material.SetFloat("_Progress", (bot.stamina / bot.maxStamina));
    }

}

