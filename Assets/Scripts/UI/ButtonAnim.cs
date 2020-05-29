using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnim : MonoBehaviour
{
    public Sprite   b_Normal;
    public Sprite   b_Pressed;
    public Image    buttonImage;

    private string  fireString;

    // Start is called before the first frame update
    void Start()
    {
        switch (gameObject.name)
        {
            case "Q":
                fireString = "Fire2";
                break;
            case "W":
                fireString = "Fire1";
                break;
            case "E":
                fireString = "Fire3";
                break;
            case "R":
                fireString = "Fire4";
                break;
        }

        buttonImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(fireString))
            buttonImage.sprite = b_Pressed;
        else
            buttonImage.sprite = b_Normal;
    }
}
