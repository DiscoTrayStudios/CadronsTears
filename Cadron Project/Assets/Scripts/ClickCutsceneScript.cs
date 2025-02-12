using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCutsceneScript : MonoBehaviour
{
    public string charname;
    public string[] dialogLines;
    public LetterDialog letterdialog;
    public Sprite portrait;
    public Texture2D cursortexture;
    private bool canStartDialog = false;
    //public Arrow arrow;

    public void OnMouseEnter(){
        if(GameManager.Instance.IsPaused() == false && GameManager.Instance.isBusy() == false){
            //arrow.Visible(false);
            Cursor.SetCursor(cursortexture, Vector2.zero, CursorMode.Auto);
        }
    }
    public void OnMouseDown(){
        if(GameManager.Instance.IsPaused() == false && GameManager.Instance.isBusy() == false){
           //arrow.Remove(charname);
           //arrow.Visible(false);
           Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
           letterdialog.SetDialog();
           GameManager.Instance.ToCutscene(dialogLines, charname, portrait);
           letterdialog.UpdateLetter();
        }
        
    }

    /*public void CheckMapIcon(){
        if(gameObject.Get)
    }*/

    public void OnMouseExit(){
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        //arrow.Visible(true);
    }
    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.rigidbody.gameObject.CompareTag("Player")) {
            canStartDialog = true;
            StartCoroutine(WaitToStart());
        }
    }
    IEnumerator WaitToStart(){
        while(canStartDialog){
            if(Input.GetKeyDown(KeyCode.E) && !GameManager.Instance.isBusy()){
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                letterdialog.SetDialog();
                GameManager.Instance.ToCutscene(dialogLines, charname, portrait);
                letterdialog.UpdateLetter();
                canStartDialog = false;
            }
            yield return null;
        }
    }
    public void OnCollisionExit2D(Collision2D collision) {
        if (collision.rigidbody.gameObject.CompareTag("Player"))
        {
            canStartDialog = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}