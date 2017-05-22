using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRandomizer : MonoBehaviour {

    public List<GameObject> heads;
    public List<GameObject> torsos;
    public List<GameObject> arms;

    void randomize(out CharacterData character)
    {
        CharacterData newChar = new CharacterData();
        int partNum = 0;

        partNum = Random.Range(0, heads.Count);
        newChar.head = heads[partNum];

        partNum = Random.Range(0, torsos.Count);
        newChar.torso = torsos[partNum];

        partNum = Random.Range(0, arms.Count);
        newChar.rightArm = arms[partNum];

        partNum = Random.Range(0, arms.Count);
        newChar.leftArm = arms[partNum];

        newChar.baseDamage = GameManager.baseDamage;

        character = newChar;
    }
}
