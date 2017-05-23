using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchData : MonoBehaviour {

    public enum ActionUsed
    {
        NONE, BUTTON, LTRIGGER, RTRIGGER, JOYSTICK
    }

    public enum Zone
    {
        NONE, HIGH_LEFT, HIGH_RIGHT, LOW_LEFT, LOW_RIGHT
    }

    public ActionUsed action;
    public Zone zone;

    bool isCounter = false;


}
