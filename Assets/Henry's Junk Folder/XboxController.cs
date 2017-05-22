using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XboxController : MonoBehaviour {

    public bool isPlayer1;
    Dictionary<string, string> axes;

    // Use this for initialization
    void Start () {
        axes = new Dictionary<string, string>();

        populateAxes();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void populateAxes()
    {
        //Clear this thing out first
        if (axes.Count > 0)
            axes.Clear();

        if (isPlayer1)
        {
            axes.Add("JoystickLeftX", "JoystickLeftX");
            axes.Add("JoystickLeftY", "JoystickLeftY");
            axes.Add("JoystickRightX", "JoystickRightX");
            axes.Add("JoystickRightY", "JoystickRightY");

            axes.Add("Jump", "Jump");
            axes.Add("Submit", "Submit");
            axes.Add("Pause", "Pause");

            axes.Add("RightPunchHighAxis", "RightPunchHighAxis");
            axes.Add("LeftPunchHighAxis", "LeftPunchHighAxis");

            axes.Add("RightPunchHigh", "RightPunchHigh");
            axes.Add("RightPunchLow", "RightPunchLow");
            axes.Add("LeftPunchHigh", "LeftPunchHigh");
            axes.Add("LeftPunchLow", "LeftPunchLow");

            axes.Add("Block", "Block");
            axes.Add("Duck", "Duck");
        }
        else
        {
            axes.Add("JoystickLeftX", "P2JoystickLeftX");
            axes.Add("JoystickLeftY", "P2JoystickLeftY");
            axes.Add("JoystickRightX", "P2JoystickRightX");
            axes.Add("JoystickRightY", "P2JoystickRightY");

            axes.Add("Jump", "P2Jump");
            axes.Add("Submit", "P2Submit");
            axes.Add("Pause", "P2Pause");

            axes.Add("RightPunchHighAxis", "P2RightPunchHighAxis");
            axes.Add("LeftPunchHighAxis", "P2LeftPunchHighAxis");

            axes.Add("RightPunchHigh", "P2RightPunchHigh");
            axes.Add("RightPunchLow", "P2RightPunchLow");
            axes.Add("LeftPunchHigh", "P2LeftPunchHigh");
            axes.Add("LeftPunchLow", "P2LeftPunchLow");

            axes.Add("Block", "P2Block");
            axes.Add("Duck", "P2Duck");
        }
    }

    public float GetAxis(string axisName)
    {
        return Input.GetAxis(axes[axisName]);
    }
    public bool GetButton(string axisName)
    {
        return Input.GetButton(axes[axisName]);
    }
    public bool GetButtonDown(string axisName)
    {
        return Input.GetButtonDown(axes[axisName]);
    }
    public bool GetButtonUp(string axisName)
    {
        return Input.GetButtonUp(axes[axisName]);
    }

    public void printAxes()
    {
        foreach (var kvp in axes)
        {
            Debug.Log(kvp.Key + ", " + kvp.Value);
        }
    }
}
