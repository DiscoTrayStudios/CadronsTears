using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLetterVisibility : MonoBehaviour
{

    private string lettername;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lettername = gameObject.transform.parent.gameObject.name;
        //Debug.Log(lettername);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.HaveLetter(lettername) && !GameManager.Instance.IsLetterDelivered(lettername)){
            //spriteRenderer.enabled = true;
        }
        else{
            spriteRenderer.enabled = false;
        }
    }
}
