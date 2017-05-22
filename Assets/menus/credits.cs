using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class credits : MonoBehaviour {

    public void TaskOnClick()
    {
        SceneManager.LoadScene("credits", LoadSceneMode.Single);
    }
}
