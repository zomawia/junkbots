using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holds and processes all character data
//Including health, stamina, and anything else we need
public class CharacterData : MonoBehaviour {

    public float maxHealth;
    public float baseDamage;
    public float stamina = 100;    
    public float maxStamina = 100;

    public GameObject head;
    public GameObject torso;
    public GameObject piston;
    public GameObject neck;
    public GameObject upperArm;
    public GameObject leftArm;
    public GameObject rightArm;

    private GameObject headActive;
    private GameObject torsoActive;
    private GameObject pistonActive;
    private GameObject neckActive;
    private GameObject upperLeftArmActive;
    private GameObject upperRightArmActive;
    private GameObject leftArmActive;
    private GameObject rightArmActive;

    private Transform pistonBone;
    private Transform torsoBone;
    private Transform neckBone;
    private Transform headBone;
    private Transform rightShoulderBone;
    private Transform leftShoulderBone;
    private Transform rightElbowBone;
    private Transform leftElbowBone;

    private List<GameObject> activeParts;
    private GameManager manager;

    public bool useManager = true;
    public bool isPlayer1;

    public Canvas gameover;
    public ParticleSystem smoke;

    public float totalHealth
    {
        get
        {
            return headActive.GetComponent<BodyPart>().totalHealth +
                   torsoActive.GetComponent<BodyPart>().totalHealth +
                   leftArmActive.GetComponent<BodyPart>().totalHealth +
                   rightArmActive.GetComponent<BodyPart>().totalHealth;
        }
    }

    private void Start()
    {
        //Whenever gameover is used NONE OF THE REST OF THE START FUNCTION runs
        //if (headActive.GetComponent<BodyPart>().totalHealth >= 0 &&
        //          torsoActive.GetComponent<BodyPart>().totalHealth >= 0)
        //{
        //    gameover.gameObject.SetActive(false);
        //}

        //gameover.gameObject.SetActive(false);
        activeParts = new List<GameObject>();

        if(GameObject.Find("GameManager") != null)
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(useManager && isPlayer1)
        {
            head = manager.player1.myBody.head;
            torso = manager.player1.myBody.torso;
            rightArm = manager.player1.myBody.rightArm;
            leftArm = manager.player1.myBody.leftArm;
        }
        else if(useManager && !isPlayer1)
        {
            head = manager.player2.myBody.head;
            torso = manager.player2.myBody.torso;
            rightArm = manager.player2.myBody.rightArm;
            leftArm = manager.player2.myBody.leftArm;
        }

        pistonBone = transform.Find("robot_piston_ball_joint");
        var rollerBall = pistonBone.transform.Find("robot_roller_ball_joint");
        torsoBone = rollerBall.transform.Find("robot_chest_joint");
        neckBone = torsoBone.transform.Find("robot_neck_ball_joint");
        headBone = neckBone.transform.Find("robot_head_ball_joint");
        rightShoulderBone = torsoBone.transform.Find("robot_right_shoulder_joint");
        rightElbowBone = rightShoulderBone.transform.Find("robot_right_elbow_joint");
        leftShoulderBone = torsoBone.transform.Find("robot_left_shoulder_joint");
        leftElbowBone = leftShoulderBone.transform.Find("robot_left_elbow_joint");

        attachParts();
    }

    private void resetPieces()
    {
        //clear the parts
        if (headActive != null) Destroy(headActive);
        if (torsoActive != null) Destroy(torsoActive);
        if (leftArmActive != null) Destroy(leftArmActive);
        if (rightArmActive != null) Destroy(rightArmActive);
    }

    void Update()
    {
        if (stamina < maxStamina)
            stamina += Time.deltaTime * 10;

        if (stamina > maxStamina)
            stamina = maxStamina;

        if (stamina < 0)
            stamina = 0;
    }

    public void CheckHealth()
    {
        if (headActive.GetComponent<BodyPart>().totalHealth <= 0 || torsoActive.GetComponent<BodyPart>().totalHealth <= 0)
            Destroy(this.gameObject);
    }

    public void attachParts()
    {
        if(activeParts.Count > 0)
        {
            foreach (var p in activeParts)
            {
                activeParts.Remove(p);
                Destroy(p);
            }
        }

        pistonActive = Instantiate(piston, pistonBone);
        torsoActive = Instantiate(torso, torsoBone);
        neckActive = Instantiate(neck, neckBone);
        headActive = Instantiate(head, headBone);
        upperRightArmActive = Instantiate(upperArm, rightShoulderBone);
        upperRightArmActive.transform.Rotate(180, 0, 0);
        rightArmActive = Instantiate(rightArm, rightElbowBone);
        rightArmActive.transform.localRotation = Quaternion.Euler(0, -35, 0);
        rightArmActive.transform.localPosition = new Vector3(0, 0, rightArmActive.transform.localPosition.z * -1);

        upperLeftArmActive = Instantiate(upperArm, leftShoulderBone);
        leftArmActive = Instantiate(leftArm, leftElbowBone);

        activeParts.Add(pistonActive);
        activeParts.Add(torsoActive);
        activeParts.Add(neckActive);
        activeParts.Add(headActive);
        activeParts.Add(upperRightArmActive);
        activeParts.Add(rightArmActive);
        activeParts.Add(upperLeftArmActive);
        activeParts.Add(leftArmActive);

        if(headActive.GetComponent<BodyPart>().totalHealth <= 0 &&
                   torsoActive.GetComponent<BodyPart>().totalHealth <= 0)
        {
            gameover.gameObject.SetActive(true);
            smoke.gameObject.SetActive(true);
            PlayerController.Destroy(null);
        }
    }

}
