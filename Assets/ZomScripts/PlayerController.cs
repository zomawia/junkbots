using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    enum PlayerState
    {
        IDLE,
        ATTACK,
        COMBO,
        BLOCK
    }

    float leanAxis = 0f;

    PlayerState myState;
    Animator anim;
    AnimatorStateInfo animState;
    AnimatorClipInfo[] animClip;

	// Use this for initialization
	void Start () {
        myState = PlayerState.IDLE;
        anim = GetComponent<Animator>();
	}
	
    void DoPunch(string stateName)
    {
        //windup
        anim.Play(stateName);
        //release

        //recovery
    }

    void HandleInput()
    {
        leanAxis = Input.GetAxis("Horizontal") * Time.deltaTime;
        // hook up leanAxis to lean animation
        anim.SetFloat("Lean", leanAxis);

        //Punch from Idle
        if (myState == PlayerState.IDLE)
        {
            if (Input.GetKeyDown("RightPunchHigh"))
            {
                //jab
                if (Input.GetKeyUp("RightPunchHigh"))
                {
                    DoPunch("RightPunchHigh");
                }
            }
        }
        

    }

	// Update is called once per frame
	void Update () {

        HandleInput();

        switch (myState)
        {
            case PlayerState.ATTACK:
                break;

            case PlayerState.COMBO:
                break;

            case PlayerState.BLOCK:
                break;

            case PlayerState.IDLE:
            default:
                break;
        }
    }
}
