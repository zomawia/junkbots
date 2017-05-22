using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour {

    bool isP1Ready, isP2Ready;
    GameManager gm;

    public List<int> p1Parts, p2Parts;

	// Use this for initialization
	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        //populate lists
        for(int i = 0; i < 4; i++)
            p1Parts.Add(0);
        for (int i = 0; i < 4; i++)
            p2Parts.Add(0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ApplySelection()
    {
        gm.updateParts(p1Parts[0], p1Parts[1], p1Parts[2], p1Parts[3], true);
        gm.updateParts(p2Parts[0], p2Parts[1], p2Parts[2], p2Parts[3], true);
    }
    
    public void p1Ready()
    {
        isP1Ready = true;
        GameObject.Find("ready1").GetComponent<Image>().color = Color.green;
        doTransition();                
    }
    public void p2Ready()
    {
        isP2Ready = true;
        GameObject.Find("ready2").GetComponent<Image>().color = Color.green;
        doTransition();
    }

    //???
    public static class GameSettings
    {
        public static CharacterData save;
    }

    void doTransition()
    {
        if (!(isP1Ready && isP2Ready)) return;

        

        DontDestroyOnLoad(transform.gameObject);

        SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
        //SceneManager.LoadScene("Test Scene - Henry", LoadSceneMode.Single);
        /*
         * [] Get our data about character selection ready. 
         * 
         * [] Pass it on to the next prefab/scene.
         * 
         * [X] Close the menu and trigger the fight.
         * 
         */

    }
}
