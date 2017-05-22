using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(XboxController))]
public class XboxControllerTest : MonoBehaviour {

    public Transform pivotPoint;
    public Transform spawnPoint;
    public GameObject rightPunchTestObject;
    public GameObject leftPunchTestObject;
    public GameObject blockTestObject;
    public bool isPlayer1;

    private GameObject blockObj;
    private XboxController xbox;

    public bool isLeaningRight { get; set; }
    public bool isLeaningLeft{ get; set; }

    public bool isPunchRight { get; set; }
    public bool isPunchLeft { get; set; }

    public bool isBlocking { get; set; }

    public bool isCrouching { get; set; }

    public float axisThreshold = 0.75f;
    public float crouchAmount = 2f;

    // Use this for initialization
    void Start () {
        isLeaningLeft = false;
        isLeaningRight = false;

        isPunchLeft = false;
        isPunchRight = false;

        isBlocking = false;
        isCrouching = false;

        xbox = GetComponent<XboxController>();
        xbox.isPlayer1 = isPlayer1;

        blockObj = Instantiate(blockTestObject, spawnPoint);
        blockObj.transform.parent = spawnPoint;
        blockObj.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        RotateCheck();
        PunchCheck();
        CrouchCheck();
        BlockCheck();

        blockObj.SetActive(isBlocking);
    }

    void CrouchCheck()
    {
        if (xbox.GetButtonDown("Duck") && !isCrouching)
        {
            isCrouching = true;
            transform.Translate(0, -crouchAmount, 0, Space.World);
        }

        else if (xbox.GetButtonUp("Duck") && isCrouching)
        {
            isCrouching = false;
            transform.Translate(0, crouchAmount, 0, Space.World);
        }
    }

    void BlockCheck()
    {
        if(isCrouching)
        {
            isBlocking = false;
            return;
        }

        if(xbox.GetButtonDown("Block") && !isBlocking)
            isBlocking = true;

        else if (xbox.GetButtonUp("Block") && isBlocking)
            isBlocking = false;
    }

    void PunchCheck()
    {
        if (isBlocking)
            return;


        if (xbox.GetAxis("JoystickRightY") >= axisThreshold && !isPunchRight)
        {
            isPunchRight = true;
            isPunchLeft = false;
            DoPunchTest();
        }

        else if (xbox.GetAxis("JoystickRightY") <= -axisThreshold && !isPunchLeft)
        {
            isPunchRight = false;
            isPunchLeft = true;
            DoPunchTest();
        }

        else if ((xbox.GetAxis("JoystickRightY") < axisThreshold && xbox.GetAxis("JoystickRightY") > -axisThreshold) && (isPunchLeft || isPunchRight))
        {
            isPunchLeft = false;
            isPunchRight = false;
        }
    }

    void DoPunchTest()
    {

        if (isPunchLeft)
            Instantiate(leftPunchTestObject, spawnPoint);

        else if (isPunchRight)
            Instantiate(rightPunchTestObject, spawnPoint);
    }

    void RotateCheck()
    {
        if (xbox.GetAxis("JoystickLeftX") >= axisThreshold && !isLeaningRight)
        {
            isLeaningRight = true;
            isLeaningLeft = false;
            DoRotate();
        }

        else if (xbox.GetAxis("JoystickLeftX") <= -axisThreshold && !isLeaningLeft)
        {
            isLeaningRight = false;
            isLeaningLeft = true;
            DoRotate();
        }

        else if ((xbox.GetAxis("JoystickLeftX") < axisThreshold && xbox.GetAxis("JoystickLeftX") > -axisThreshold) && (isLeaningLeft || isLeaningRight))
        {
            isLeaningLeft = false;
            isLeaningRight = false;
            DoRotate();
        }
    }

    void DoRotate()
    {

        transform.rotation = new Quaternion(0,0,0,0);

        if (isLeaningLeft)
            transform.Rotate(45, 0, 0, Space.World);

        else if (isLeaningRight)
            transform.Rotate(-45, 0, 0, Space.World);

    }
}
