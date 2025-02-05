using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MapPeopleDisplay : MonoBehaviour
{
    public Sprite bw;
    public Sprite glowing;
    public bool isFirst;
    public string sendsto;
    private string personname;
    Image img;
    // Start is called before the first frame update
    void Start()
    {
        personname = gameObject.name;
        img = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.HaveLetter(personname) && !GameManager.Instance.IsLetterDelivered(personname)){
            img.sprite = glowing;
        }
        else if(isFirst && (!GameManager.Instance.HaveLetter(sendsto)) && !GameManager.Instance.IsLetterDelivered(sendsto)){
            img.sprite = glowing;
        }
        else{
            img.sprite = bw;
        }
    }
}
