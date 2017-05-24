// PlayerParticleController
// Zomawia Sailo
// Sync up with the PlayerController to play particle effects at required states
// Also control other effects (screen shake, halo, lighting)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleController : MonoBehaviour {

    public ParticleSystem charge;   //windup
    public ParticleSystem plasma;   //flinch
    public ParticleSystem smoke;    //low health
    public ParticleSystem steam;    //low stamina
    public ParticleSystem sparks;   //regular hit
    public TrailRenderer trail;     //trails on fists

    Component halo; //charge
    CameraShake cameraShake;

    CharacterData myChar;
    PlayerController pc;
    PlayerController.AttackState attackState;
    PlayerController.MoveState moveState;
    PlayerController.PlayerState playerState;
    PlayerController.BlockState blockState;

    // Use this for initialization
    void Start()
    {
        pc = GetComponent<PlayerController>();
        myChar = GetComponent<CharacterData>();
        cameraShake = GetComponent<CameraShake>();
        halo = gameObject.GetComponent("Halo");

        attackState = pc.myAttackState;
        moveState = pc.myMoveState;
        playerState = pc.myState;
        blockState = pc.myBlockState;
    }

    void StopEffect(ParticleSystem part)
    {

    }

    void StopAllEffects()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (attackState)
        {
            case PlayerController.AttackState.WINDUP:

                halo.GetType().GetProperty("enabled").SetValue(halo, true, null);

                charge.gameObject.SetActive(true);
                charge.Play();

                break;
            case PlayerController.AttackState.RELEASE:

                halo.GetType().GetProperty("enabled").SetValue(halo, false, null);

                charge.Pause();
                charge.gameObject.SetActive(false);

                break;

            case PlayerController.AttackState.RECOVERY:
                break;
            case PlayerController.AttackState.IMPACT:
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

                plasma.time = 0;
                plasma.Stop();
                plasma.Play();

                cameraShake.Shake(.05f, .025f);

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
