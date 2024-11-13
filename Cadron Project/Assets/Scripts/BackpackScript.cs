using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BackpackScript : MonoBehaviour
{
    public GameObject LetterPrefab;

    public TextMeshProUGUI LetterTextPrefab;
    public GameObject Backpackbutton;

    public GameObject letterbox;
    private Dictionary<string, bool> letters;
   
    public void OpenBackpack(){
        if(!GameManager.Instance.isBusy()){
        
        GameManager.Instance.playerBusy(true); 
        letters = GameManager.Instance.GetLetters();
        Vector2 letterpos = new Vector2(-170,135);
        foreach(KeyValuePair<string, bool> letter in letters){
            string to = letter.Key;
            if(letter.Value == false){
                GameObject newletter = Instantiate(LetterPrefab);
                newletter.transform.SetParent(letterbox.transform);
                newletter.GetComponent<RectTransform>().anchoredPosition = letterpos;
                newletter.name = to;
                letterpos.x = letterpos.x + 170;
                if(letterpos.x > 170){
                    letterpos.y = letterpos.y - 105;
                    letterpos.x = -170;
                }
            }
        }
        gameObject.SetActive(true);
        }
        

    }
    public void CloseBackpack(){
        gameObject.SetActive(false);
        GameManager.Instance.playerBusy(false);
        Backpackbutton.SetActive(true);
        foreach(Transform child in letterbox.transform){
            GameObject.Destroy(child.gameObject);
        }
    
    }
    void Awake(){
        if(GameManager.Instance.isBusy()){
            Backpackbutton.SetActive(false);
        }
        else
        Backpackbutton.SetActive(true);
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
