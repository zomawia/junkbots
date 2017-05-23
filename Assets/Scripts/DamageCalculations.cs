using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculations : MonoBehaviour {

    public static float GetDamage(PlayerController self, PlayerController other, bool isRightArm, bool isBlock = false)
    {
        BodyPart damageArm = isRightArm ? other.myBody.rightArm.GetComponent<BodyPart>() : other.myBody.leftArm.GetComponent<BodyPart>();

        float baseDamage = other.myBody.baseDamage;
        float damageMult = damageArm.punchDamage;
        float punchCharge = other.punchPower;
        float dmg = damageMult * punchCharge * baseDamage;

        if (isBlock)
        {
            float armor = self.myBody.torso.GetComponent<BodyPart>().blockAmount;
            dmg *= (1 - (armor / 10));
        }     

        return dmg;
    }

}
