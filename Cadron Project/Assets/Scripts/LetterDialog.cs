using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterDialog : MonoBehaviour
{
    public string sendsletterto;
    public string[] deliverLetterDialog;
    public string[] afterletterdialog;

    public bool sendsLetter;
    public bool receivesLetter;

    public bool sendbeforereceive;


    public ClickCutsceneScript dialogScript;
    private string charname;
    private GameManager GM;
    public void SetDialog(){
        if(receivesLetter){
            if(GM.HaveLetter(charname)){
                if (!GM.IsLetterDelivered(dialogScript.charname)) {
                    dialogScript.dialogLines = deliverLetterDialog;
                }
                else {
                    dialogScript.dialogLines = afterletterdialog;
                }
            }
            else if (sendbeforereceive && GM.HaveLetter(sendsletterto)){
                dialogScript.dialogLines = afterletterdialog;
            }
        }
        
        else {
            if(GM.HaveLetter(sendsletterto)){
                dialogScript.dialogLines = afterletterdialog;
            }
        }
        
    }
    public void UpdateLetter(){
        if(receivesLetter){
            if(GM.HaveLetter(charname)){
                if(!GM.IsLetterDelivered(charname)){
                    GM.UpdateLetterStatus(charname);
                    if(!GM.HaveLetter(sendsletterto) && sendsLetter){
                      GM.AddLetter(sendsletterto);  
                    }
                }
            } 
            else if (sendbeforereceive && !GM.HaveLetter(sendsletterto)) {
                GM.AddLetter(sendsletterto);
            }
        }
        
        else if(sendsLetter && !GM.HaveLetter(sendsletterto)){
            GM.AddLetter(sendsletterto);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GM = GameManager.Instance;
        charname = dialogScript.charname;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
