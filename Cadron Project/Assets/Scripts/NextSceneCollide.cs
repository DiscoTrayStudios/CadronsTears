using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NextSceneCollide : MonoBehaviour
{
    public string nextscene;
    public int whichscene;
    public string requiredletter;
    bool faded = false;

    public GameObject icon;
    void Start()
    {
        icon = gameObject.transform.Find("Icon").gameObject;
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            if((requiredletter == "") || GameManager.Instance.IsLetterDelivered(requiredletter))
            {
            
            //print to the console
            Debug.Log("Player has exited the trigger");
            GameManager.Instance.StartTentscene(whichscene, nextscene);
            }
        }
    }

    void Update()
    {
        if(GameManager.Instance.IsLetterDelivered(requiredletter)){
            icon.GetComponent<SpriteRenderer>().enabled = true;
            if(!faded && !GameManager.Instance.isBusy()){
                GameManager.Instance.EndPopup();
                faded = true;
            }

        }
        else{
            icon.GetComponent<SpriteRenderer>().enabled = false;
        }
    }


}
