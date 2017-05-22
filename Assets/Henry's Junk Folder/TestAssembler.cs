using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAssembler : MonoBehaviour {

    public PlayerController player1;
    public PlayerController player2;
    CharacterData bot1;
    CharacterData bot2;
    GameManager gm;

    void Start()
    {
        gm = GameManager.getGameManager();
        bot1 = gm.player1.myBody;
        bot2 = gm.player2.myBody;
        buildBots();
    }

    public void buildBots()
    {
        player1.myBody = bot1;
        player1.myBody.attachParts();

        player2.myBody = bot2;
        player2.myBody.attachParts();
    }
}
