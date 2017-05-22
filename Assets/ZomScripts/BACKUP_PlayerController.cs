//// JunkBot PlayerController
//// by Zomawia Sailo

//// Holds logic and actions for JunkBot Combat System
//// Hooks up with an Animator to sync states with animations

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerController : MonoBehaviour {

//    public enum PlayerState
//    {
//        IDLE,
//        ATTACK,
//        COUNTER,
//        COMBO,
//        BLOCK,
//        FLINCH
//    }

//    public enum AttackState
//    {
//        IDLE,
//        WINDUP,
//        RELEASE,
//        RECOVERY,
//        IMPACT,
//        MISS
//    }

//    public enum BlockState
//    {
//        ENTER,
//        BLOCK,
//        BLOCK_LOW,
//        EXIT,
//        NONE
//    }

//    public enum Zone
//    {
//        NONE, HIGH_LEFT, HIGH_RIGHT, LOW_LEFT, LOW_RIGHT
//    }

//    public CharacterData myBody;

//    public PlayerController opponentCtrl;

//    float leftTrigger = 0f;
//    float rightTrigger = 0f;
//    float yAxisRightJoystick = 0f;
//    float xAxisRightJoystick = 0f;
//    float yAxisLeftJoystick = 0f;
//    float xAxisLeftJoystick = 0f;
//    float axisThreshold = .3f;

//    //For physics layers
//    //1 is player1, 2 is player 2
//    int playerLayer = 0;
//    int opponentLayer = 0;

//    float leanAxis = 0f;

//    int comboCount = 0;
//    int comboLimit = 4;
//    float comboWindowTime = .8f;
//    float comboSpeedBuff = .2f;
//    public float punchPower = 0f;
//    float maxPunchCharge = 1.5f;
//    float recoveryWindowTime = .5f;
//    float releaseTime = 1f;

//    float buttonHoldTimer = 0f;
//    float timedBlockThreshold = .7f;

//    //TODO: Sync up time with animations or use animator event system
//    float blockEnterTime = .3f;
//    float blockExitTime = .7f;

//    public float flinchThreshold = 5f;
//    float flinchDuration = 1f;    

//    bool isFlinched = false;

//    bool lastSwingMissed = false;
//    bool successfulPunch = false;
//    bool canCounterPunch = false;

//    string lastButtonPressed;

//    float animPlayTimer = 0f;

//    float timer = 0f;

//    XboxController xbox;

//    public PlayerState myState;
//    public AttackState myAttackState;
//    public BlockState myBlockState;
//    public Zone myZone;

//    Animator anim;
//    //Animation myAnimation;
//    //AnimatorStateInfo animState;
//    //AnimatorClipInfo[] animClip;

//    //Transforms positions to check collisions when punches are thrown
//    public Transform leftHand;
//    public Transform rightHand;

//    // Use this for initialization
//    void Start() {

//        if (GetComponent<Animator>())
//            anim = GetComponent<Animator>();

//        if (GetComponent<XboxController>())        
//            xbox = GetComponent<XboxController>();        

//        if (xbox != null)
//        {
//            if (xbox.isPlayer1)
//            {
//                playerLayer = 8;
//                opponentLayer = 16;
//            }
//            else
//            {
//                playerLayer = 16;
//                opponentLayer = 8;
//            }
//        }

//        myState = PlayerState.IDLE;

//    }

//    void ClearStuff()
//    {
//        punchPower = 0f;
//        buttonHoldTimer = 0f;
//        isFlinched = false;
//        lastSwingMissed = false;
//        canCounterPunch = false;
//        animPlayTimer = 0f;
//        comboCount = 0;
//        timer = 0;
//        myAttackState = AttackState.IDLE;
//        myState = PlayerState.IDLE;
//        myZone = Zone.NONE;
//        myBlockState = BlockState.NONE;

//    }

//    bool IsPunchCollide()
//    {
//        Transform tra;

//        if (myZone == Zone.HIGH_LEFT || myZone == Zone.LOW_LEFT)
//        {
//            tra = leftHand;
//        }
//        else
//            tra = rightHand;

//        Collider[] hits = Physics.OverlapSphere(tra.position, 0.5f, opponentLayer);

//        for (int i = 0; i < hits.Length; ++i)
//        {
//            if (hits[i] != null)
//            {
//                Debug.Log("HIT DETECTED");
//                return true;
//            }
//        }

//        return false;
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        // TODO: Placeholder if statement until we get naming conventions for body parts
//        if (other.gameObject.GetComponent<Collider>().tag == "hand")
//        {
//            //Debug.Log("I'm hit!");
//            if ((myState == PlayerState.ATTACK && 
//                (myAttackState == AttackState.WINDUP || myAttackState == AttackState.RECOVERY)) ||
//                myState == PlayerState.IDLE)
//            {
//                //Debug.Log(gameObject);                

//                if (opponentCtrl.punchPower >= flinchThreshold)
//                {
//                    myState = PlayerState.FLINCH;
//                }
//                else
//                {
//                    myState = PlayerState.IDLE;
//                }

//                if (opponentCtrl.myZone == Zone.HIGH_LEFT)
//                {
//                    myBody.head.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, false);
//                }

//                if (opponentCtrl.myZone == Zone.HIGH_RIGHT)
//                {
//                    myBody.head.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, true);
//                }

//                if (opponentCtrl.myZone == Zone.LOW_LEFT)
//                {
//                    myBody.torso.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, false);
//                }

//                if (opponentCtrl.myZone == Zone.LOW_RIGHT)
//                {
//                    myBody.torso.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, true);
//                }
//            }

//            // Receive a punch during a BLOCK
//            if (myState == PlayerState.BLOCK)
//            {
//                if (myBlockState == BlockState.BLOCK)
//                {
//                    if (opponentCtrl.myZone == Zone.HIGH_LEFT)
//                    {
//                        myBody.rightArm.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, false, true);
//                        if (timedBlockThreshold <= buttonHoldTimer)
//                        {
//                            canCounterPunch = true;
//                            punchPower = opponentCtrl.punchPower;
//                        }
//                    }
//                    if (opponentCtrl.myZone == Zone.HIGH_RIGHT)
//                    {
//                        myBody.leftArm.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, true, true);
//                        if (timedBlockThreshold <= buttonHoldTimer)
//                        {
//                            canCounterPunch = true;
//                            punchPower = opponentCtrl.punchPower;
//                        }
//                    }
//                    if (opponentCtrl.myZone == Zone.LOW_LEFT)
//                    {
//                        myBody.torso.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, false);
//                        if (opponentCtrl.punchPower >= flinchThreshold)
//                        {
//                            myState = PlayerState.FLINCH;
//                        }
//                        else
//                        {
//                            myState = PlayerState.IDLE;
//                        }
//                    }

//                    if (opponentCtrl.myZone == Zone.LOW_RIGHT)
//                    {
//                        myBody.torso.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, true);
//                        if (opponentCtrl.punchPower >= flinchThreshold)
//                        {
//                            myState = PlayerState.FLINCH;
//                        }
//                        else
//                        {
//                            myState = PlayerState.IDLE;
//                        }
//                    }
//                }

//                if (myBlockState == BlockState.BLOCK_LOW)
//                {
//                    if (opponentCtrl.myZone == Zone.LOW_LEFT)
//                    {
//                        myBody.rightArm.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, false, true);
//                        if (timedBlockThreshold <= buttonHoldTimer)
//                        {
//                            canCounterPunch = true;
//                            punchPower = opponentCtrl.punchPower;
//                        }
//                    }

//                    if (opponentCtrl.myZone == Zone.LOW_RIGHT)
//                    {
//                        myBody.leftArm.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, true, true);
//                        if (timedBlockThreshold <= buttonHoldTimer)
//                        {
//                            canCounterPunch = true;
//                            punchPower = opponentCtrl.punchPower;
//                        }
//                    }

//                    if (opponentCtrl.myZone == Zone.HIGH_LEFT)
//                    {
//                        myBody.head.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, false);
//                        if (opponentCtrl.punchPower >= flinchThreshold)
//                        {
//                            myState = PlayerState.FLINCH;
//                        }
//                        else
//                        {
//                            myState = PlayerState.IDLE;
//                        }
//                    }

//                    if (opponentCtrl.myZone == Zone.HIGH_RIGHT)
//                    {
//                        myBody.head.GetComponent<BodyPart>().totalHealth -= DamageCalculations.GetDamage(this, opponentCtrl, true);
//                        if (opponentCtrl.punchPower >= flinchThreshold)
//                        {
//                            myState = PlayerState.FLINCH;
//                        }
//                        else
//                        {
//                            myState = PlayerState.IDLE;
//                        }
//                    }
//                }                
                
//            }
//        }
//    }

//    void DoPunch(string stateName, bool isCounter = false, bool isCombo = false)
//    {
//        lastButtonPressed = stateName;
//        punchPower = 0f;

//        switch (stateName)
//        {
//            case "RightPunchHigh":
//                myZone = Zone.HIGH_RIGHT;
//                break;
//            case "LeftPunchHigh":
//                myZone = Zone.HIGH_LEFT;
//                break;
//            case "RightPunchLow":
//                myZone = Zone.LOW_RIGHT;
//                break;
//            case "LeftPunchLow":
//                myZone = Zone.LOW_LEFT;
//                break;
//            default:
//                Debug.Log("ERROR PUNCH STATENAME");
//                myZone = Zone.NONE;
//                break;

//        }

//        // Counter punches skip windup state
//        if (isCounter)
//        {
//            myAttackState = AttackState.RELEASE;
//        }


//        // Combo punches decrease release times
//        else if (myState == PlayerState.COMBO)
//        {
//            myAttackState = AttackState.WINDUP;
//            comboCount++;
//        }

//        //Regular punch
//        else
//        {
//            myAttackState = AttackState.WINDUP;
//        }
//    }

//    void HandleInput()
//    {
//        // TODO: hook up leanAxis to lean animation
//        //leanAxis = Input.GetAxis("Horizontal") * Time.deltaTime;
//        //anim.SetFloat("Lean", leanAxis);

//        yAxisRightJoystick = xbox.GetAxis("JoystickRightY");
//        xAxisRightJoystick = xbox.GetAxis("JoystickRightX");
//        yAxisLeftJoystick = xbox.GetAxis("JoystickLeftY");
//        xAxisLeftJoystick = xbox.GetAxis("JoystickLeftX");
//        leftTrigger = xbox.GetAxis("LeftPunchHighAxis");
//        rightTrigger = xbox.GetAxis("RightPunchHighAxis");

//        anim.SetFloat("leanAxis", xAxisLeftJoystick);
//        anim.SetFloat("duckAxis", yAxisLeftJoystick);

//        if (myState == PlayerState.IDLE)
//        {
//            // front buttons
//            if (xbox.GetButtonDown("RightPunchHigh") || xbox.GetButtonDown("RightPunchLow") ||
//                xbox.GetButtonDown("LeftPunchHigh") || xbox.GetButtonDown("LeftPunchLow") || 
//                leftTrigger > axisThreshold || rightTrigger > axisThreshold)
//            {
//                myState = PlayerState.ATTACK;
//                Debug.Log("IDLE: Attack pressed, going to ATTACK state");
//            }

//            if (xbox.GetButton("Block"))
//            {
//                Debug.Log("IDLE: Block pressed, going to BLOCK state");
                
//                myBlockState = BlockState.ENTER;
//                myState = PlayerState.BLOCK;

                
//            }

//        }

//        if (myState == PlayerState.ATTACK || myState == PlayerState.COMBO || myState == PlayerState.COUNTER)
//        {
//            // Attack Cancelling
//            if (xbox.GetButton("Block") && myAttackState == AttackState.WINDUP)
//            {
//                //ClearStuff();
//                Debug.Log("ATTACK CANCELED - GOING TO BLOCK");
//                //anim.SetBool("LEFT", false);
//                //anim.SetBool("RIGHT", false);
//                myAttackState = AttackState.IDLE;                
//                myState = PlayerState.IDLE;                
//            }

//            if (myAttackState == AttackState.IDLE)
//            {
//                if (xbox.GetButtonDown("RightPunchHigh") || rightTrigger > axisThreshold)
//                {
//                    DoPunch("RightPunchHigh");
//                }
//                else if (xbox.GetButtonDown("RightPunchLow") ||
//                    (xbox.GetButton("Duck") && xbox.GetAxis("RightPunchHighAxis") > axisThreshold))
//                {
//                    DoPunch("RightPunchLow");
//                }
//                else if (xbox.GetButtonDown("LeftPunchHigh") || leftTrigger > axisThreshold)
//                {
//                    DoPunch("LeftPunchHigh");
//                }
//                else if (xbox.GetButtonDown("LeftPunchLow") ||
//                    (xbox.GetButton("Duck") && xbox.GetAxis("LeftPunchHighAxis") > axisThreshold))
//                {
//                    DoPunch("LeftPunchLow");
//                }
//            }

//            // Attack finished released, start/continue a combo
//            if (myAttackState == AttackState.IMPACT || myAttackState == AttackState.RECOVERY)
//            {
//                if (comboCount <= comboLimit)
//                {

//                    if (xbox.GetButtonDown("RightPunchHigh") && lastButtonPressed != "RightPunchHigh")
//                    {
//                        Debug.Log("ComboCount: " + comboCount);
//                        if (myState == PlayerState.ATTACK)
//                        {
//                            myState = PlayerState.COMBO;
//                        }
//                        DoPunch("RightPunchHigh");

//                    }
//                    else if ((xbox.GetButtonDown("RightPunchLow") && lastButtonPressed != "RightPunchLow") ||
//                                (xbox.GetButton("Duck") && rightTrigger > axisThreshold && lastButtonPressed != "RightPunchHigh"))
//                    {
//                        Debug.Log("ComboCount: " + comboCount);
//                        if (myState == PlayerState.ATTACK)
//                        {
//                            myState = PlayerState.COMBO;
//                        }
//                        DoPunch("RightPunchLow");
//                    }

//                    else if (xbox.GetButtonDown("LeftPunchHigh"))
//                    {
//                        Debug.Log("ComboCount: " + comboCount);
//                        if (myState == PlayerState.ATTACK)
//                        {
//                            myState = PlayerState.COMBO;
//                        }
//                        DoPunch("LeftPunchHigh");
//                    }

//                    else if ((xbox.GetButtonDown("RightPunchLow") && lastButtonPressed != "RightPunchLow") ||
//                                (xbox.GetButton("Duck") && leftTrigger > axisThreshold && lastButtonPressed != "LeftPunchHigh"))                            
//                    {
//                        Debug.Log("ComboCount: " + comboCount);
//                        if (myState == PlayerState.ATTACK)
//                        {
//                            myState = PlayerState.COMBO;
//                        }
//                        DoPunch("LeftPunchLow");
//                    }
//                }
//                else
//                {
//                    Debug.Log("Can't combo anymore. Going to IDLE");
//                    myState = PlayerState.IDLE;
//                }
//            }

//            if (myAttackState == AttackState.WINDUP)
//            {
//                if (xbox.GetButton(lastButtonPressed) || leftTrigger > axisThreshold || rightTrigger > axisThreshold)
//                {
//                    punchPower += Time.deltaTime;
//                    //myBody.stamina -= Time.deltaTime;
//                }

//                if (xbox.GetButtonUp(lastButtonPressed) || punchPower >= maxPunchCharge)
//                {
//                    myAttackState = AttackState.RELEASE;
//                }
//            }
//        }

//        if (myState == PlayerState.BLOCK)
//        {
//            if (xbox.GetButtonUp("Block"))
//            {
//                Debug.Log("BLOCK: Block button released.");
//                myBlockState = BlockState.EXIT;                
//            }

//            if (xbox.GetButton("Block") && myBlockState != BlockState.EXIT)
//            {
//                Debug.Log("BLOCK: Holding BLOCK button");
//                buttonHoldTimer += Time.deltaTime;
//            }

//            if (myBlockState != BlockState.ENTER && myBlockState != BlockState.EXIT)
//            {
//                if (xbox.GetButton("Block") && yAxisRightJoystick < -(axisThreshold))
//                {                    
//                    Debug.Log("BLOCK: Blocking LOW");
//                    myBlockState = BlockState.BLOCK_LOW;
//                }
//                else
//                {
//                    Debug.Log("BLOCK: Blocking HIGH");
//                    myBlockState = BlockState.BLOCK;
//                }
//            }

//            if (canCounterPunch)
//            {
//                if (xbox.GetButtonDown("RightPunchHigh"))
//                {
//                    DoPunch("RightPunchHigh", true);
//                }
//                else if (xbox.GetButtonDown("RightPunchLow") ||
//                    (rightTrigger > axisThreshold && xbox.GetButton("Duck")))
//                {
//                    DoPunch("RightPunchLow", true);
//                }
//                else if (xbox.GetButtonDown("LeftPunchHigh"))
//                {
//                    DoPunch("LeftPunchHigh", true);
//                }
//                else if (xbox.GetButtonDown("LeftPunchLow") ||
//                    (leftTrigger > axisThreshold && xbox.GetButton("Duck")))
//                {
//                    DoPunch("LeftPunchLow", true);
//                }
//            }
//        }     
//    }

//    void Update () {        

//        HandleInput();        

//        // For automated logic updates. Input and user actions should go into HandleInput
//        switch (myState)
//        {
//            case PlayerState.ATTACK:
//            case PlayerState.COMBO:
//                //Debug.Log("ATTACKSTATE: " + myAttackState);
//                if (myAttackState == AttackState.WINDUP)
//                {
//                    // TODO: Play small portion of animation
//                    if (myZone == Zone.HIGH_RIGHT || myZone == Zone.LOW_RIGHT)
//                    {
//                        anim.SetBool("RIGHT", true);
//                    }
//                    else if (myZone == Zone.HIGH_LEFT || myZone == Zone.LOW_LEFT)
//                    {
//                        anim.SetBool("LEFT", true);
//                    }

//                    // TODO: Spawn in some particle effects

//                    timer += Time.deltaTime;
//                    if (timer >= maxPunchCharge)
//                    {
//                        timer = 0;
//                        myAttackState = AttackState.RELEASE;
//                    }

//                }

//                if (myAttackState == AttackState.RELEASE)
//                {
//                    if (anim.GetBool("RIGHT"))
//                    {
//                        anim.SetBool("RIGHT", false);
//                    }

//                    else if (anim.GetBool("LEFT"))
//                    {
//                        anim.SetBool("LEFT", false);
//                    }



//                    if (IsPunchCollide() == true)
//                    {                        
//                        myAttackState = AttackState.IMPACT;
//                    }

//                    // TODO: Play 25-75% of animation or something like that

//                    timer += Time.deltaTime;
//                    if (timer >= releaseTime + punchPower)
//                    {
//                        timer = 0;                      
//                        myAttackState = AttackState.RECOVERY;                        
//                    }
//                }

//                if (myAttackState == AttackState.RECOVERY || myAttackState == AttackState.IMPACT)
//                {
//                    //TODO: Play rest of punch animation

//                    timer += Time.deltaTime;
//                    if (timer >= comboWindowTime)
//                    {
//                        Debug.Log("Not going into Combo. Back to Idle");
//                        timer = 0;                        
//                        myState = PlayerState.IDLE;
//                    }
//                }
//                break;

//            case PlayerState.COUNTER:
//                Debug.Log("STATE: I am in COUNTER");
//                // Wait to see if player will do input to go into ATTACK

//                // TODO: Add juice to show player is throwing a counter
//                timer += Time.deltaTime;
//                if (timer >= 2f && myAttackState != AttackState.RELEASE)
//                {
//                    myState = PlayerState.IDLE;
//                }
//                break;

//            case PlayerState.BLOCK:

//                //Debug.Log("BLOCKSTATE: " + myBlockState);

//                anim.SetBool("LEFT", false);
//                anim.SetBool("RIGHT", false);

//                if (myBlockState == BlockState.ENTER)
//                {
//                    //Debug.Log("BLOCKSTATE: Blockstate ENTER");

//                    anim.SetBool("block", true);

//                    timer += Time.deltaTime;

//                    if (timer >= blockEnterTime)
//                    {
//                        Debug.Log("BLOCKSTATE: Timer is done, can Block now.");
//                        timer = 0;
//                        myBlockState = BlockState.BLOCK;
//                    }
//                }

//                if (myBlockState == BlockState.BLOCK_LOW)
//                {
//                    //TODO: play low block animation
//                }
//                if (myBlockState == BlockState.BLOCK)
//                {
//                    //TODO: play high block animation
//                }

//                if (myBlockState == BlockState.NONE)
//                {
//                    //TODO: play idle
//                }

//                if (myBlockState == BlockState.EXIT)
//                {
//                    anim.SetBool("block", false);

//                    timer += Time.deltaTime;
                    
//                    if (timer >= blockExitTime)
//                    {
//                        Debug.Log("BLOCKSTATE: Exiting Block");                        
//                        timer = 0;
//                        myBlockState = BlockState.NONE;
//                        myState = PlayerState.IDLE;
//                    }
//                }

//                break;

//            case PlayerState.FLINCH:
//                //Debug.Log("STATE: I am in FLINCH");

//                anim.SetBool("flinch", true);
//                timer += Time.deltaTime;
//                if (timer >= flinchDuration + punchPower)
//                {
//                    timer = 0;
//                    anim.SetBool("flinch", false);

//                    myState = PlayerState.IDLE;
//                }
//                break;

//            case PlayerState.IDLE:
//            default:
//                //Debug.Log("STATE: I am in IDLE");
//                if (!xbox.isPlayer1)
//                {
//                    //TODO: Play idle2 animation
//                }
//                ClearStuff();
//                break;
                            
                
//        }
//    }
//}
