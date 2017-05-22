using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuSwitch : MonoBehaviour
{

    public int idxhead;
    public Image[] heads = new Image[4];
   
   
    public void TaskOnClick()
    {
        for(int i = 0; i < heads.Length; i++)
        {
            idxhead = (idxhead + 1) % heads.Length;
        }
    }

}
