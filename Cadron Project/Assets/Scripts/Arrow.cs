using System.Dynamic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public List<GameObject> people;
    public GameObject current;
    public Vector3 playerpos;
    public Vector3 personpos;
    public GameObject player;
    public GameObject arrow;
    private bool here = true;
    public bool mouseExit = false;

    // Start is called before the first frame update
    void Start()
    {
        current = people[0];
        Vector3 playerpos = new Vector3();
        Vector3 personpos = new Vector3();
    }

    public void Remove(string charname) {
        if (current.name == charname) {
            people.Remove(current);
            //  gameObject.SetActive(false);
        }
    }
    public void itsMouseExit() { mouseExit = true; }

    public void Visible(bool b) { here = b; }

    // Update is called once per frame
    void Update() {
        if (people.Count > 0) { current = people[0]; }
        else { current = null; }
        playerpos = player.transform.position;
        if (current != null && here) {
            personpos = current.transform.position;
            Vector3 arrowdir = (personpos - playerpos).normalized;
            int RotationZ = (int) (Vector3.Angle(arrowdir, new Vector3(1f,0f,0f)));
            if (arrowdir.y < 0) { RotationZ = 360 - RotationZ; }
            transform.eulerAngles = new Vector3(0, 0, RotationZ);
            if ((personpos - playerpos).magnitude > 1f) {
                transform.position = playerpos + (arrowdir / 2);
            }
            else {
                transform.position = playerpos + (arrowdir * 10000f);
            }            
        }
        if (!here) {
                transform.position = playerpos + new Vector3(10000f, 10000f, 0f);
        } 
        if (!GameManager.Instance.isBusy()) { here = true; }
        if (current != null && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && 
            Mathf.Pow(playerpos.y - current.transform.position.y, 2) + 
            Mathf.Pow(playerpos.x - current.transform.position.x, 2) < 2.0f) { 
                ClickCutsceneScript clickCutsceneScript = current.GetComponent<ClickCutsceneScript>();
                if (clickCutsceneScript != null) {
                    clickCutsceneScript.OnMouseDown(); 
                    clickCutsceneScript.OnMouseEnter(); 
                    if (mouseExit) {
                       clickCutsceneScript.OnMouseExit(); 
                       mouseExit = false;
                    }
                }
        }
    }
}
