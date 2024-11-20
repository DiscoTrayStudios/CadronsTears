using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public GameObject MapButton;
    public GameObject LocationBox;

    private Dictionary<string, bool> letters;
    //public GameObject[] letterlocations;
    public void OpenMap(){
        if(!GameManager.Instance.isBusy()){
            GameManager.Instance.playerBusy(true);
            gameObject.SetActive(true);
            MapButton.SetActive(false);
            letters = GameManager.Instance.GetLetters();
            foreach(Transform letter in LocationBox.transform){
                if((GameManager.Instance.HaveLetter(letter.name)) && (GameManager.Instance.IsLetterDelivered(letter.name) == false)){
                        letter.gameObject.SetActive(true);
                }
                else{
                        letter.gameObject.SetActive(false);
                }
                }
        }
    }

    public void CloseMap(){
        gameObject.SetActive(false);
        GameManager.Instance.playerBusy(false);
        MapButton.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        //letterlocations = LocationBox.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
