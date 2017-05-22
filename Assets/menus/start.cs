using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour {

    public void TaskOnClick()
    {
        SceneManager.LoadScene("CharacterCustom", LoadSceneMode.Single);
    }
}
