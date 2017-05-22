using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class back : MonoBehaviour {

	public void TaskOnClick()
    {
        SceneManager.LoadScene("main menu", LoadSceneMode.Single);
    }
}
