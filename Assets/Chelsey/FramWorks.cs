using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramWorks : MonoBehaviour
{
    // Entering a state means instantiating the prefab for that state
    // Character Selection
    // Game State

    GameObject menu;
    GameObject game;
    GameObject victory;
    GameObject pause;

    void disableAll()
    {
        pause.SetActive(false);
        victory.SetActive(false);
        menu.SetActive(false);
        game.SetActive(true);
    }

    void continueGame()
    {
        disableAll();

        game.SetActive(true);
    }
    
    void doStartGame(int head, int torso, int left, int right, int head2, int torso2, int left2, int right2)
    {
        disableAll();

        game.SetActive(true);

        //game.GetComponentInChildren<GameManager>().startGame(head,torso,left,right,head2,torso2,left2,right2);
        // ...
    }

    void doStartGameOver(int victor)
    {
       // if(victor == 1) // player 1 won
        //if(victor == 2) // player 2 won
    }


    enum States
    {
        Intro,
        MainMenu,
        PlayerCreation,
        GameStateIntro,
        GameState,
        GameOver,
        Exit,
        Credits,
    }

    States SetState;
    /// <summary>
    /// lets use namespace in our c# scripts so the enumC# can just call your funtions instead of copy and pasting everyones shit into here?
    /// </summary>
    void doIntro()
    {
        //cinematic Game Intro video?//
    }
    void doMainMenu()
    {
        
        //Gingers Menue screen?
        // PressStart, moving logo?
    }
    void doPlayerCreation()
    {
        //player1 and player 2 
        // creates there battle bots
        // once bother players select there battle bots creation player one hits start?
        //super smashBros style? like when selection charaters?(just an idea)
    }
    void doGameStateIntro()
    {
        // entering arena cinamatic. when completed goes to gate state/
        // if intrupted by a skip buytton? to start imeditly (spelling rocks)
    }
    void doGameState()
    {
       // combat,damage, ect... scripts?
    }
    void doGameOver()
    {
        //winner screen bla bla 
    }
    void doExit()
    {
        // play again? idk what to put here gameover state could prob take care fo this stuff honestly
    }
    void doCredits()
    {
       // credits of the awesome people working onn this whooooo!!!
    }



    void stateUpdate()
    {
        switch(SetState)
        {
            case States.Intro:
                doIntro();
                break;

            case States.MainMenu:
                doMainMenu();
                break;


            case States.PlayerCreation:
                doPlayerCreation();
                break;


            case States.GameStateIntro:
                doGameStateIntro();
                break;

            case States.GameState:
                doGameState();
                break;

            case States.GameOver:
                doGameOver();
                break;

            case States.Exit:
                doExit();
                break;

            case States.Credits:
                doCredits();
                break;

        }
    }

}
