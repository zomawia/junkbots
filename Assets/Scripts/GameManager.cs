using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public PlayerController player1 = null;
    public PlayerController player2 = null;

    public List<GameObject> heads;
    public List<GameObject> torsos;
    public List<GameObject> arms;

    public static float baseDamage = 5f;

    //Allows this to be accessed anywhere without needing to have a GameManager variable.
    public static GameManager instance = null;

    private void Awake()
    {
        //Set the instance to this...but only once
        if (instance == null)
            instance = this;
        //If an instance already exists and it's not this...
        //THEN THIS IS AN IMPOSTER
        //IT MUST COMMIT SEPPUKU TO RETAIN ITS HONOR
        else if (instance != this)
            Destroy(gameObject);

        //This makes it persistant between levels
        DontDestroyOnLoad(gameObject);
    }

    //Parameters refer to indices of lists
    public void updateParts(int head, int torso, int rightArm, int leftArm, bool isPlayer1)
    {
        if(isPlayer1)
        {
            player1.myBody.head = heads[head];
            player1.myBody.torso = torsos[torso];
            player1.myBody.leftArm = arms[leftArm];
            player1.myBody.rightArm = arms[rightArm];
            //player1.myBody.attachParts();
        }
        else
        {
            player2.myBody.head = heads[head];
            player2.myBody.torso = torsos[torso];
            player2.myBody.leftArm = arms[leftArm];
            player2.myBody.rightArm = arms[rightArm];
            //player2.myBody.attachParts();
        }
    }

    //Universally get the game manager
    public static GameManager getGameManager()
    {
        return GameObject.Find("GameManager").GetComponent<GameManager>();
    }
}
