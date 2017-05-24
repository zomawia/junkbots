// AudioController
// Zomawia Sailo
// Hooks up with AudioManager (stores and plays the audio clips)
// and syncs with PlayerController to get the states from the FSM

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour {

    AudioManager audioManager;
  
    PlayerController pc;
    PlayerController.AttackState attackState;
    PlayerController.MoveState moveState;
    PlayerController.PlayerState playerState;
    PlayerController.BlockState blockState;

	// Use this for initialization
	void Start () {
        pc = GetComponent<PlayerController>();
        audioManager = GetComponent<AudioManager>();
        
	}
	
	// Update is called once per frame
	void Update () {
        switch (attackState)
        {
            case PlayerController.AttackState.WINDUP:                
                audioManager.PlayWindUp();
                break;
            case PlayerController.AttackState.RELEASE:        
                break;
            case PlayerController.AttackState.RECOVERY:
                break;
            case PlayerController.AttackState.IMPACT:
                audioManager.punch();
                break;
            case PlayerController.AttackState.IDLE:
            default:
                break;
        }

        switch (moveState)
        {
            case PlayerController.MoveState.DODGERAM:
                break;
            case PlayerController.MoveState.RETURN:
                break;
            case PlayerController.MoveState.DEFAULT:
            default:
                break;
        }

        switch (playerState)
        {
            case PlayerController.PlayerState.FLINCH:
                audioManager.Flinch();
                break;
            case PlayerController.PlayerState.IDLE:
            default:
                break;
        }

        switch (blockState)
        {
            case PlayerController.BlockState.ENTER:
                break;
            case PlayerController.BlockState.BLOCK:
            case PlayerController.BlockState.BLOCK_LOW:
                break;
            case PlayerController.BlockState.EXIT:
                break;
            case PlayerController.BlockState.NONE:
            default:
                break;
        }
	}
}
