// JunkBot PlayerController
// by Zomawia Sailo

// Holds logic and actions for JunkBot Combat System
// Hooks up with an Animator to sync states with animations

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public enum PlayerState
    {
        IDLE,
        ATTACK,
        COUNTER,
        COMBO,
        BLOCK,
        FLINCH
    }

    public enum AttackState
    {
        IDLE,
        WINDUP,
        RELEASE,
        RECOVERY,
        IMPACT,
        MISS
    }

    public enum BlockState
    {
        ENTER,
        BLOCK,
        BLOCK_LOW,
        EXIT,
        NONE
    }

    public enum Zone
    {
        NONE, HIGH_LEFT, HIGH_RIGHT, LOW_LEFT, LOW_RIGHT
    }

    public enum MoveState
    {
        DEFAULT, DODGERAM              
    }

    //One more ENUM for good luck
    public enum ActionUsed
    {
        NONE, BUTTON, LTRIGGER, RTRIGGER, JOYSTICK
    }

    public CharacterData myBody;
    public PlayerController opponentCtrl;

    //For sanity
    public float leftTrigger = 0f;
    public float rightTrigger = 0f;
    public float yAxisRightJoystick = 0f;
    public float xAxisRightJoystick = 0f;
    public float yAxisLeftJoystick = 0f;
    public float xAxisLeftJoystick = 0f;
    public float axisThreshold = .3f;

    //For physics layers    
    int playerLayer = 0;
    int opponentLayer = 0;

    float leanAxis = 0f;


    // Punch variables
    ushort comboCount = 0;
    ushort comboLimit = 4;
    float comboWindowTime = .5f;
    float comboSpeedBuff = .2f;

    public float punchPower = 0f;
    float maxPunchCharge = 1.5f;

    float recoveryWindowTime = .5f;
    float releaseTime = 1f;

    float buttonHoldTimer = 0f;
    float timedBlockThreshold = .7f;
    
    float blockEnterTime = .3f;
    float blockExitTime = .7f;

    public float flinchThreshold = 5f;
    float flinchDuration = 1f;            
    
    bool canCounterPunch = false;

    string lastButtonPressed;    

    float animPlayTimer = 0f;

    float timer = 0f;

    public XboxController xbox { get; set; }

    public PlayerState myState;
    public AttackState myAttackState;
    public BlockState myBlockState;
    public Zone myZone;
    public ActionUsed myActionUsed;

    // For charging punch graphic
    Component halo;

    public Animator anim { get; set; }

    //Transforms positions to check collisions when punches are thrown
    public Transform leftHand;
    public Transform rightHand;
    
    // Use this for initialization
    void Start() {

        //Set animator
        if (GetComponent<Animator>())
            anim = GetComponent<Animator>();

        // Punch Charge visual indicator
        halo = gameObject.GetComponent("Halo");

        //Set input controller
        if (GetComponent<XboxController>())        
            xbox = GetComponent<XboxController>();        

        //Determine if player 1 or player 2
        if (xbox != null)
        {
            if (xbox.isPlayer1)
            {
                playerLayer = 8;
                opponentLayer = 16;
            }
            else
            {
                playerLayer = 16;
                opponentLayer = 8;
            }
        }

        //Make sure the player is idling
        myState = PlayerState.IDLE;

    }
    bool isDucking()
    {
        if (yAxisLeftJoystick < -axisThreshold)
            return true;
        return false;
    }

    bool isLeftAttacksPressed()
    {
        if (xbox.GetButtonDown("LeftPunchHigh") || leftTrigger > axisThreshold ||
            (xAxisRightJoystick <= -axisThreshold && yAxisRightJoystick <= -axisThreshold))
        {
            return true;
        }
        return false;
    }

    bool isRightAttackPressed()
    {
        if (xbox.GetButtonDown("RightPunchHigh") || rightTrigger > axisThreshold ||
                    (xAxisRightJoystick >= -axisThreshold && yAxisRightJoystick <= -axisThreshold))
        {
            return true;
        }
        return false;
    }

    // This one's pretty obvious
    // Called on IDLE but can be used when transitioning from outside of FSM scope
    void ClearStuff()
    {
        punchPower = 0f;
        buttonHoldTimer = 0f;        
        
        canCounterPunch = false;
        animPlayTimer = 0f;
        comboCount = 0;
        timer = 0;

        myAttackState = AttackState.IDLE;
        myState = PlayerState.IDLE;
        myZone = Zone.NONE;
        myBlockState = BlockState.NONE;
        myActionUsed = ActionUsed.NONE;

        anim.SetBool("LEFT", false);        
        anim.SetBool("RIGHT", false);        
    }

    bool IsAPunchAxisCharging()
    {
        if (myActionUsed == ActionUsed.JOYSTICK)
        {            
            if (yAxisRightJoystick <= -axisThreshold)
            {
                return true;
            }
            return false;
        }
        if (myActionUsed == ActionUsed.RTRIGGER)
        {
            if (rightTrigger > axisThreshold)
            {
                return true;
            }
            return false;
        }
        if (myActionUsed == ActionUsed.LTRIGGER)
        {
            if (leftTrigger > axisThreshold)
            {
                return true;
            }
            return false;
        }
        
        return false;
    }    

    // Required for the different types of controls for punch charging.
    public ActionUsed GetActionUsed()
    {
        if (leftTrigger > axisThreshold)
        {
            return ActionUsed.LTRIGGER;
        }
        if (rightTrigger > axisThreshold)
        {
            return ActionUsed.RTRIGGER;
        }

        if (yAxisRightJoystick < -axisThreshold)
        {
            return ActionUsed.JOYSTICK;
        }

        return ActionUsed.BUTTON;
    }

    //Is working now
    public bool IsPunchCollide()
    {
        Transform tra;

        //use left hand if zone is left
        if (myZone == Zone.HIGH_LEFT || myZone == Zone.LOW_LEFT)
        {
            tra = leftHand;
        }
        //otherwise use right hand
        else
            tra = rightHand;

        //use a sphere to determine if the punch hits things
        Collider[] hits = Physics.OverlapSphere(tra.position, 0.5f);

        //figure out if any of the hits are valid.
        //If they are, GREAT.  If not, less great.
        for (int i = 0; i < hits.Length; ++i)
        {
            if (hits[i] != null)
            {                
                return true;
            }
        }

        return false;
    }

    //Getting punched
    void OnTriggerEnter(Collider other)
    {
        //IF a 'hand' collided with us...
        if (other.gameObject.GetComponent<Collider>().tag == "hand" && 
            (opponentCtrl.myState == PlayerState.ATTACK || 
            opponentCtrl.myState == PlayerState.COMBO || 
            opponentCtrl.myState == PlayerState.COUNTER))
        {            
            //Flinch if we got punched while we weren't blocking, combo-ing, or already flinching
            if (myAttackState == AttackState.WINDUP || 
                myAttackState == AttackState.RECOVERY || 
                myState == PlayerState.IDLE)
            {
                //Flinch if the punch was powerful enough to flinch us
                if (opponentCtrl.punchPower >= flinchThreshold)
                {
                    myState = PlayerState.FLINCH;
                }
                else
                {
                    anim.SetTrigger("hit");
                    //myState = PlayerState.IDLE;
                }

                //Take damage
                if (opponentCtrl.myZone == Zone.HIGH_LEFT)
                {
                    myBody.head.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, false);
                }

                if (opponentCtrl.myZone == Zone.HIGH_RIGHT)
                {
                    myBody.head.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, true);
                }

                if (opponentCtrl.myZone == Zone.LOW_LEFT)
                {
                    myBody.torso.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, false);
                }

                if (opponentCtrl.myZone == Zone.LOW_RIGHT)
                {
                    myBody.torso.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, true);
                }
            }

            // Receive a punch during a BLOCK
            if (myState == PlayerState.BLOCK)   //if we're blocking...
            {
                if (myBlockState == BlockState.BLOCK) //and the block isn't in enter/exit...
                {
                    if (opponentCtrl.myZone == Zone.HIGH_LEFT)  //and the other guy is punching high
                    {
                        //get damaged
                        myBody.rightArm.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, false, true);
                        if (timedBlockThreshold <= buttonHoldTimer) //do a timed block if the block was quick enough
                        {
                            //enable coutner punch and instantly set punch power to the other guy's punch power.
                            canCounterPunch = true;
                            punchPower = opponentCtrl.punchPower;
                        }
                    }
                    if (opponentCtrl.myZone == Zone.HIGH_RIGHT) //same as above but with the opponent's right arm.
                    {
                        myBody.leftArm.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, true, true);
                        if (timedBlockThreshold <= buttonHoldTimer)
                        {
                            canCounterPunch = true;
                            punchPower = opponentCtrl.punchPower;
                        }
                    }
                    if (opponentCtrl.myZone == Zone.LOW_LEFT)   //if opponent punches LOW and we blocked HIGH we get punched
                    {
                        myBody.torso.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, false);
                        //check for flinch
                        if (opponentCtrl.punchPower >= flinchThreshold)
                        {
                            myState = PlayerState.FLINCH;
                        }
                        else
                        {
                            myState = PlayerState.IDLE;
                        }
                    }

                    if (opponentCtrl.myZone == Zone.LOW_RIGHT)  //same as above but with the opponent's right arm.
                    {
                        myBody.torso.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, true);
                        if (opponentCtrl.punchPower >= flinchThreshold)
                        {
                            myState = PlayerState.FLINCH;
                        }
                        else
                        {
                            myState = PlayerState.IDLE;
                        }
                    }
                }

                //same as above but blocking low
                if (myBlockState == BlockState.BLOCK_LOW)
                {
                    if (opponentCtrl.myZone == Zone.LOW_LEFT)
                    {
                        myBody.rightArm.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, false, true);
                        if (timedBlockThreshold <= buttonHoldTimer)
                        {
                            canCounterPunch = true;
                            punchPower = opponentCtrl.punchPower;
                        }
                    }

                    if (opponentCtrl.myZone == Zone.LOW_RIGHT)
                    {
                        myBody.leftArm.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, true, true);
                        if (timedBlockThreshold <= buttonHoldTimer)
                        {
                            canCounterPunch = true;
                            punchPower = opponentCtrl.punchPower;
                        }
                    }

                    if (opponentCtrl.myZone == Zone.HIGH_LEFT)
                    {
                        myBody.head.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, false);
                        if (opponentCtrl.punchPower >= flinchThreshold)
                        {
                            myState = PlayerState.FLINCH;
                        }
                        else
                        {
                            myState = PlayerState.IDLE;
                        }
                    }

                    if (opponentCtrl.myZone == Zone.HIGH_RIGHT)
                    {
                        myBody.head.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, true);
                        if (opponentCtrl.punchPower >= flinchThreshold)
                        {
                            myState = PlayerState.FLINCH;
                        }
                        else
                        {
                            myState = PlayerState.IDLE;
                        }
                    }
                }                
                
            }
        }
    }

    //Punching
    public void DoPunch(string stateName, bool isCounter = false, bool isCombo = false)
    {
        myActionUsed = GetActionUsed();
        if (myActionUsed == ActionUsed.BUTTON)
            lastButtonPressed = stateName;

        punchPower = 0f;

        //Do a different punch based on button state
        switch (stateName)
        {
            case "RightPunchHigh":
                myZone = Zone.HIGH_RIGHT;
                break;
            case "LeftPunchHigh":
                myZone = Zone.HIGH_LEFT;
                break;
            case "RightPunchLow":
                myZone = Zone.LOW_RIGHT;
                break;
            case "LeftPunchLow":
                myZone = Zone.LOW_LEFT;
                break;
            default:
                Debug.Log("ERROR PUNCH STATENAME");
                myZone = Zone.NONE;
                break;

        }

        // Counter punches skip windup state
        if (isCounter)
        {
            myAttackState = AttackState.RELEASE;
        }


        // Combo punches decrease release times
        else if (myState == PlayerState.COMBO)
        {
            myAttackState = AttackState.WINDUP;
            comboCount++;
        }

        //Regular punch
        else
        {
            myAttackState = AttackState.WINDUP;
        }
    }

    void HandleInput()
    {
        yAxisRightJoystick = xbox.GetAxis("JoystickRightY");
        xAxisRightJoystick = xbox.GetAxis("JoystickRightX");
        yAxisLeftJoystick = xbox.GetAxis("JoystickLeftY");
        xAxisLeftJoystick = xbox.GetAxis("JoystickLeftX");
        leftTrigger = xbox.GetAxis("LeftPunchHighAxis");
        rightTrigger = xbox.GetAxis("RightPunchHighAxis");

        anim.SetFloat("leanAxis", xAxisLeftJoystick);
        anim.SetFloat("duckAxis", yAxisLeftJoystick);

        

        //If the character isn't doing anything...
        if (myState == PlayerState.IDLE)
        {
            // If any attack buttons are pressed
            if (isLeftAttacksPressed() || isRightAttackPressed())
            {
                myState = PlayerState.ATTACK;                
            }

            //Block
            if (xbox.GetButton("Block"))
            {   
                myBlockState = BlockState.ENTER;
                myState = PlayerState.BLOCK;                
            }

        }

        //If we're attacking in some way...
        if (myState == PlayerState.ATTACK || myState == PlayerState.COMBO || myState == PlayerState.COUNTER)
        {
            // Attack Cancelling - cancel attack if block is pressed.
            if (xbox.GetButton("Block") && myAttackState == AttackState.WINDUP)
            {                
                anim.SetTrigger("attackCancel");
                myState = PlayerState.IDLE;
                //anim.ResetTrigger("attackCancel");
            }

            //If the attack hasn't triggered yet, do a punch in the given zone.
            if (myAttackState == AttackState.IDLE)
            {
                if (isRightAttackPressed())
                {
                    if (isDucking())
                        DoPunch("RightPunchLow");
                    else
                        DoPunch("RightPunchHigh");
                }

                if (isLeftAttacksPressed())
                {
                    if (isDucking())
                        DoPunch("LeftPunchLow");
                    else
                        DoPunch("LeftPunchHigh");                    
                }                
            }

            //CHARGED PUNCH
            if (myAttackState == AttackState.WINDUP)
            {
                //Only charge punch if the respective punch button is held down.
                    switch (myActionUsed)
                    {
                        case ActionUsed.BUTTON:
                            if (xbox.GetButton(lastButtonPressed))
                            {
                                punchPower += Time.deltaTime;
                                myBody.stamina -= Time.deltaTime;
                            }
                            if (!xbox.GetButton(lastButtonPressed) || punchPower >= maxPunchCharge)
                            {
                                myBody.stamina -= punchPower;
                                myAttackState = AttackState.RELEASE;
                            }
                            break;
                        case ActionUsed.JOYSTICK:
                        case ActionUsed.LTRIGGER:
                        case ActionUsed.RTRIGGER:
                            if (IsAPunchAxisCharging())
                            {
                                punchPower += Time.deltaTime;
                                myBody.stamina -= Time.deltaTime;
                            }
                            if (!IsAPunchAxisCharging() || punchPower >= maxPunchCharge)
                            {
                                myBody.stamina -= punchPower;
                                myAttackState = AttackState.RELEASE;
                            }
                            break;

                        case ActionUsed.NONE:
                        default:
                            Debug.Log("It shouldn't be possible to get here!!!");
                            break;                        
                    }

                }            

            // Attack finished released, start/continue a combo
            if (myAttackState == AttackState.IMPACT || myAttackState == AttackState.RECOVERY)
            {
                if (comboCount <= comboLimit)   //Only combo if the combo limit hasn't been reached
                {                    
                    if (isRightAttackPressed())
                    {
                        if (isDucking())
                            DoPunch("RightPunchLow");
                        else
                            DoPunch("RightPunchHigh");
                    }

                    if (isLeftAttacksPressed())
                    {
                        if (isDucking())
                            DoPunch("LeftPunchLow");
                        else
                            DoPunch("LeftPunchHigh");
                    }
                }
                //break combo if combo limit reached
                else
                {
                    Debug.Log("Can't combo anymore. Going to IDLE");
                    myState = PlayerState.IDLE;
                }
            }
        }

        //If we're blocking...
        if (myState == PlayerState.BLOCK)
        {
            //exit block if block button released
            if (xbox.GetButtonUp("Block"))
            {
                myBlockState = BlockState.EXIT;                
            }

            //Continue block if block is not being exited.
            if (xbox.GetButton("Block") && myBlockState != BlockState.EXIT)
            {
                buttonHoldTimer += Time.deltaTime;
            }

            if (myBlockState == BlockState.ENTER)
            {
                if ((xbox.GetButton("Block") && isDucking()) ||
                    (xbox.GetButton("Block") && yAxisRightJoystick < -axisThreshold))
                {
                    anim.SetBool("blockLow", true);
                }
                else
                {
                    anim.SetBool("block", true);
                }
            }

            //Enter block when enter phase is over
            if (myBlockState != BlockState.ENTER && myBlockState != BlockState.EXIT)
            {
                //do a low block
                if ((xbox.GetButton("Block") && isDucking()) || 
                    (xbox.GetButton("Block") && yAxisRightJoystick < -axisThreshold))
                {                    
                    myBlockState = BlockState.BLOCK_LOW;
                }
                //or a high block
                else
                {
                    myBlockState = BlockState.BLOCK;
                }
            }

            //COUNTER-PUNCH, a punch that comes directly out of block mode            
            // WIP
            if (canCounterPunch)
            {
                if (isRightAttackPressed())
                {
                    anim.SetBool("block", false);
                    myBlockState = BlockState.NONE;
                    myState = PlayerState.ATTACK;
                    if (isDucking())
                        DoPunch("RightPunchLow", true);
                    else
                        DoPunch("RightPunchHigh", true);
                }

                if (isLeftAttacksPressed())
                {
                    anim.SetBool("block", false);
                    myBlockState = BlockState.NONE;
                    myState = PlayerState.ATTACK;
                    if (isDucking())
                        DoPunch("LeftPunchLow", true);
                    else
                        DoPunch("LeftPunchHigh", true);
                }
            }
        }     
    }

    // For automated logic updates and animations. Input and user actions should go into HandleInput.
    void Update () {

        HandleInput();        

        //The State Machine
        switch (myState)
        {
            case PlayerState.ATTACK:
            case PlayerState.COMBO:
                //Debug.Log("ATTACKSTATE: " + myAttackState);
                //Windup animations and timer increase
                if (myAttackState == AttackState.IDLE)
                {                    
                    
                }

                if (myAttackState == AttackState.WINDUP)
                {
                    Component halo = gameObject.GetComponent("Halo");                    
                    
                    if (myZone == Zone.HIGH_RIGHT || myZone == Zone.LOW_RIGHT)
                    {
                        anim.SetBool("RIGHT", true);
                    }
                    else if (myZone == Zone.HIGH_LEFT || myZone == Zone.LOW_LEFT)
                    {
                        anim.SetBool("LEFT", true);
                    }

                    halo.GetType().GetProperty("enabled").SetValue(halo, true, null);

                    timer += Time.deltaTime;
                    //Release punch if fully charged
                    if (timer >= maxPunchCharge)
                    {
                        timer = 0;
                        myAttackState = AttackState.RELEASE;
                        
                    }

                }

                //Punch release
                if (myAttackState == AttackState.RELEASE)
                {                    
                    halo.GetType().GetProperty("enabled").SetValue(halo, false, null);

                    //Animation stuff
                    if (anim.GetBool("RIGHT"))
                    {
                        anim.SetBool("RIGHT", false);
                    }

                    if (anim.GetBool("LEFT"))
                    {
                        anim.SetBool("LEFT", false);
                    }

                    anim.SetFloat("punchPower", punchPower);

                    //Punch collision
                    if (IsPunchCollide() == true)
                    {                        
                        myAttackState = AttackState.IMPACT;
                    }                    

                    //Go to recovery when the punch is over.
                    timer += Time.deltaTime;
                    if (timer >= releaseTime)
                    {
                        timer = 0;                      
                        myAttackState = AttackState.RECOVERY;                        
                    }
                }

                //Combo timer while punch is in impact or recovery
                if (myAttackState == AttackState.RECOVERY || myAttackState == AttackState.IMPACT)
                {
                    timer += Time.deltaTime;
                    if (timer >= comboWindowTime)
                    {
                        //Debug.Log("Not going into Combo. Back to Idle");
                        timer = 0;                        
                        myState = PlayerState.IDLE;
                    }
                }
                break;

            case PlayerState.COUNTER:
                Debug.Log("STATE: I am in COUNTER");
                // Wait to see if player will do input to go into ATTACK

                // TODO: Add juice to show player is throwing a counter
                // TODO: Switch from hard-coded 2f to variable
                timer += Time.deltaTime;
                //End counter if timer is up
                if (timer >= 2f && myAttackState != AttackState.RELEASE)
                {
                    myState = PlayerState.IDLE;
                }
                break;

            case PlayerState.BLOCK:
                //Block enter timer.
                //Is this necessary?
                if (myBlockState == BlockState.ENTER)
                {
                    anim.SetBool("blockEnter", true);
                    timer += Time.deltaTime;
                    if (timer >= blockEnterTime)
                    {
                        //Debug.Log("BLOCKSTATE: Timer is done, can Block now.");
                        timer = 0;                        
                        myBlockState = BlockState.NONE;
                    }
                }

                //Block animations
                if (myBlockState == BlockState.BLOCK_LOW)
                {                    
                    anim.SetBool("blockLow", true);
                    
                }
                if (myBlockState == BlockState.BLOCK)
                {                    
                    anim.SetBool("blockLow", false);
                }

                if (myBlockState == BlockState.NONE)
                {
                    //TODO: play idle
                }

                //End block
                if (myBlockState == BlockState.EXIT)
                {
                    anim.SetBool("block", false);
                    anim.SetBool("blockLow", false);
                    anim.SetBool("blockEnter", false);

                    timer += Time.deltaTime;
                    
                    if (timer >= blockExitTime)
                    {
                       // Debug.Log("BLOCKSTATE: Exiting Block");                        
                        timer = 0;
                        myBlockState = BlockState.NONE;
                        myState = PlayerState.IDLE;
                    }
                }

                break;

            case PlayerState.FLINCH:
                //Debug.Log("STATE: I am in FLINCH");

                anim.SetBool("flinch", true);
                timer += Time.deltaTime;
                //End flinch when timer is up
                if (timer >= flinchDuration)
                {
                    timer = 0;
                    anim.SetBool("flinch", false);

                    myState = PlayerState.IDLE;
                }
                break;

            case PlayerState.IDLE:
            default:
                ClearStuff();
                break;              
        }
    }
}
