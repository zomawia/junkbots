// JunkBot PlayerController
// by Zomawia Sailo

// Holds player states, fighting system state machine
// Hooks up with an Animator state machine to sync states, actions with animations

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

    enum AttackState
    {
        IDLE,
        WINDUP,
        RELEASE,
        RECOVERY
    }

    float leanAxis = 0f;
    float powerPunchTimer = 0f;
    float maxPowerPunch = 1.25f;

    float animPlayTimer = 0f;

    PlayerState myState;
    AttackState myAttackState;

    Animator anim;
    Animation myAnimation;
    AnimatorStateInfo animState;
    AnimatorClipInfo[] animClip;

	// Use this for initialization
	void Start () {

        myState = PlayerState.IDLE;

        if (anim != null)
        {
            anim = GetComponent<Animator>();
            animState = anim.GetCurrentAnimatorStateInfo(0);
            animClip = anim.GetCurrentAnimatorClipInfo(0);
            myAnimation = GetComponent<Animation>();
        }

	}
	
    //Hit detection
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Hand")
        {
            if (myAttackState == AttackState.WINDUP && myState == PlayerState.ATTACK)
            {
                //do flinch
                
                //stop current animation
            }
        }
    }

    void DoPunch(string stateName)
    {
        //windup
        anim.Play(stateName, 0, 0f);
        myAttackState = AttackState.WINDUP;
        
        if ((animClip[0].clip.length % 1 ) >= .25f && myAttackState == AttackState.WINDUP)
        {
            //release
            anim.Play(stateName, 0, .25f);
            myAttackState = AttackState.RELEASE;
        }

        if ((animClip[0].clip.length % 1) >= .75f && myAttackState == AttackState.RELEASE)
        {
            //recovery
            anim.Play(stateName, 0, .75f);
            myAttackState = AttackState.RECOVERY;
        }                    
    }

    void HandleInput()
    {
        leanAxis = Input.GetAxis("Horizontal") * Time.deltaTime;
        // hook up leanAxis to lean animation
        anim.SetFloat("Lean", leanAxis);
                
        if (myState == PlayerState.IDLE)
        {
            // idle to punch
            if (Input.GetKeyDown("RightPunchHigh") || Input.GetKeyDown("RightPunchLow") || 
                Input.GetKeyDown("LeftPunchHigh") || Input.GetKeyDown("LeftPunchLow") )
            {
                myState = PlayerState.ATTACK;
            }
        }

        else if (myState == PlayerState.ATTACK)
        {
            // decide to throw normal punch or charge up
            if (Input.GetKeyUp("RightPunchHigh"))
            {
                DoPunch("RightPunchHigh");
            }
            else if (Input.GetKeyUp("RightPunchLow"))
            {
                DoPunch("RightPunchLow");
            }
            else if (Input.GetKeyUp("LeftPunchHigh"))
            {
                DoPunch("LeftPunchHigh");
            }
            else if (Input.GetKeyUp("LeftPunchLow"))
            {
                DoPunch("LeftPunchLow");
            }

            //we can go back to idle or start combo
        }     
        
        else if (myState == PlayerState.COMBO)
        {

        }
        
        else if (myState == PlayerState.BLOCK)
        {
            if (Input.GetKeyDown("RightPunchHigh") || Input.GetKeyDown("RightPunchLow") ||
                    Input.GetKeyDown("LeftPunchHigh") || Input.GetKeyDown("LeftPunchLow"))
            {
                //do nothing
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
