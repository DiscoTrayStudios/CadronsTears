using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public GameObject dialogBox;
    public GameObject animalSpots;
    public TextMeshProUGUI dialogText;
    public GameObject curtain;
    public GameObject canvas;
    public GameObject creditsmenu;
    //public GameObject player;
    private Dictionary<string, bool> letters;
    private bool gamePaused = false;
    private bool raiseLower = false;
    public bool busy;
    private int tentscene;
    private Animator animator;

    private String nextscene;
    private bool newletter;

    void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);  
        } else {
        Destroy(gameObject);
        }
    }
    
    public List<Transform> getSpots(){
        Debug.Log("returning spots");
        StartCoroutine(WaitforSpots());
        return animalSpots.GetComponent<MakeSpotList>().getMoveSpots();
    }
    public IEnumerator WaitforSpots(){
        while(!animalSpots){
            yield return null;    
        }
    }

    public bool IsLetterDelivered(string key){
        if (letters.ContainsKey(key)){
            return letters[key];
        }
        else
        return false;
    }
    public bool HaveLetter(string key){
        return letters.ContainsKey(key);
    }
    public void UpdateLetterStatus(string key){
        if(letters.ContainsKey(key)){
            letters[key] = !letters[key];
        }
    }
    public void AddLetter(string k){
        letters.Add(k, false);
        newletter = true;
    }

    public void playerBusy(bool b) {
        /*if (player != null) {
            player.GetComponent<PlayerMovement>().SetIdle(b);*/
            busy = b; 
            /*if (b) { 
                Debug.Log("stoppingggg");
                animator = player.GetComponent<Animator>();
                player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll; 
                animator.SetFloat("horizontal", 0f);
                animator.SetFloat("vertical",   0f);
                player.GetComponent<PlayerMovement>().SetIdle(true);
                //Debug.Log(animator.horizontal);
                
            }
           // else   { 
                //Animator animator = player.GetComponent<Animator>(); 
                //player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation; 
            //}
        }*/
    }

    public bool isBusy () { return busy; }


    public void ClearLetters(){
        letters = new Dictionary<string, bool>();

    }
    IEnumerator ColorLerpFunction(bool fadeout, float duration)
    {
        float time = 0;
        raiseLower = true;
        Image curtainImg = curtain.GetComponent<Image>();
        Color startValue;
        Color endValue;
        if (fadeout) {
            startValue = new Color(0, 0, 0, 0);
            endValue = new Color(0, 0, 0, 1);
        } else {
            startValue = new Color(0, 0, 0, 1);
            endValue = new Color(0, 0, 0, 0);
        }
        while (time < duration)
        {
            curtainImg.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        curtainImg.color = endValue;
        raiseLower = false;
    }
     IEnumerator LoadYourAsyncScene(string scene)
    {
        curtain.SetActive(true);
        StartCoroutine(ColorLerpFunction(true, 1));
        while (raiseLower)
        {
            yield return null;
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

    while(!asyncLoad.isDone)
    {
        yield return null;
    }
    while(curtain == null){
        yield return null;
    }
    StartCoroutine(ColorLerpFunction(false, 1));
    
    }

    public void ChangeScene(string scene){
        StartCoroutine(LoadYourAsyncScene(scene));
        ClearLetters();
    }
    public void DialogShow(string text) {
        dialogBox.SetActive(true);
        playerBusy(true);
        //busy = true;
        StopAllCoroutines();
        StartCoroutine(TypeText(text));
    }
    IEnumerator TypeText(string text) {
        dialogText.text = "";
        foreach(char c in text.ToCharArray()) {
            dialogText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void DialogHide(){
        dialogBox.SetActive(false);
        playerBusy(false);
        busy = false;
        gamePaused = false;
        //Arrow arrow = player.transform.GetChild(0).GetComponent<Arrow>();
        //arrow.itsMouseExit();
    }

    public void ShowButtons(){
        canvas.transform.Find("BackpackButton").gameObject.SetActive(true);
        canvas.transform.Find("MapButton").gameObject.SetActive(true);
    }
    public void HideButtons(){
        
        canvas.transform.Find("BackpackButton").gameObject.SetActive(false);
        canvas.transform.Find("MapButton").gameObject.SetActive(false);
    }

    public void ToCutscene(string[] dialog, string name, Sprite portrait){
        CutSceneDialog dialogscript = dialogBox.GetComponent<CutSceneDialog>();
         dialogscript.lines = dialog;
         dialogBox.SetActive(true);
         DialogShow(dialog[0]);
         dialogscript.StartCutscene(name, portrait);
         gamePaused = true;
         if(!(SceneManager.GetActiveScene().name == "Cutscene")){
         HideButtons();     
         }
    //         Time.timeScale = 0f;
    }
    public bool CheckTextDone(String l){
        return dialogText.text.Equals(l);
    }
    public void FinishText(String l){
        dialogText.text = l;
        StopAllCoroutines();
    }

    public void PauseGame(){
        gamePaused = true;
        Time.timeScale = 0f;
        
    }
    public void UnpauseGame(){
        gamePaused = false;
        Time.timeScale = 1f;

    }
    public void EndCutscene(){
        // if the scenes name is cutscene, then transition to the Season2 scene here
        if (SceneManager.GetActiveScene().name == "Cutscene") { 
            if(nextscene != "Credits"){ChangeScene(nextscene); }
            else{
                ChangeScene("Menu");
                StartCoroutine(WaitforCreditsMenu());
            }
        }
        else{ShowButtons();}
        DialogHide();
        gamePaused = false;
        ShowButtons();
        if (newletter == true){
            GameObject bpb = canvas.transform.Find("BackpackButton").gameObject;
            bpb.GetComponent<BackpackLetterPopup>().ShowNewLetter();
            newletter = false;
        }
    //        Time.timeScale = 1f;
    }
    public IEnumerator WaitforCreditsMenu(){
        while(!creditsmenu){
            yield return null;    
        }
        creditsmenu.SetActive(true);
    }

    public bool IsPaused(){
        return gamePaused;
    }
    public Dictionary<string, bool> GetLetters(){
        return letters;
    }

    public int GetTentscene(){
        return tentscene;
    }
    public void StartTentscene(int c, String next){
        tentscene = c;
        ChangeScene("Cutscene");
        nextscene = next;

    }
    void Start()
    {
        //animator = player.GetComponent<Animator>(); 
        busy = false;
        letters = new Dictionary<string, bool>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
