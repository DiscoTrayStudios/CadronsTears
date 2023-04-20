using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;
    public GameObject curtain;
    public GameObject canvas;
    public GameObject player;
    private Dictionary<string, bool> letters;
    private bool gamePaused = false;
    private bool raiseLower = false;

     void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        } else {
        Destroy(gameObject);
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
    
    StartCoroutine(ColorLerpFunction(false, 1));
    
    }

    public void ChangeScene(string scene){
        StartCoroutine(LoadYourAsyncScene(scene));
    }
    public void DialogShow(string text) {
        dialogBox.SetActive(true);
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
    }

    public void ToCutscene(string[] dialog, string name, Sprite portrait){
        CutSceneDialog dialogscript = dialogBox.GetComponent<CutSceneDialog>();
         dialogscript.lines = dialog;
         dialogBox.SetActive(true);
         DialogShow(dialog[0]);
         dialogscript.StartCutscene(name, portrait);
         gamePaused = true;
    }

    public void PauseGame(){
        gamePaused = true;
    }
    public void UnpauseGame(){
        gamePaused = false;
    }
    public void EndCutscene(){
        DialogHide();
        gamePaused = false;
    }

    public bool IsPaused(){
        return gamePaused;
    }
    public Dictionary<string, bool> GetLetters(){
        return letters;
    }
    void Start()
    {
        letters = new Dictionary<string, bool>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}