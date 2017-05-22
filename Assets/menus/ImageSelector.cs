using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelector : MonoBehaviour {

    public Sprite[] images;
    public int currentImage = 0;
    public Image image;
    public string text;
    //0 = head, 1 = torso, 3 = left arm, 4 = right arm
    public int index;
    public bool isPlayer1;

    private CharacterSelectManager manager;

    void Start()
    {
        if (transform.parent.GetComponent<CharacterSelectManager>() != null)
            manager = transform.parent.GetComponent<CharacterSelectManager>();
        else
            Debug.Log("ImageSelector: parent does not contain CharacterSelectManager!");

        image.sprite = images[currentImage];
    }

	public void Next()
    {
        currentImage++;
        currentImage %= images.Length;

        image.sprite = images[currentImage];

        applyPart();
    }

    public void Prev()
    {
        currentImage--;
        
        if(currentImage < 0)
            currentImage = images.Length-1;

        image.sprite = images[currentImage];

        applyPart();
    }

    void OnValidate()
    {
        GetComponentInChildren<Text>().text = text;
    }

    void applyPart()
    {
        if (isPlayer1)
            manager.p1Parts[index] = currentImage;
        else
            manager.p2Parts[index] = currentImage;

        manager.ApplySelection();
    }
}
