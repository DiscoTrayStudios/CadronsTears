using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonMapIcon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.HaveLetter(gameObject.transform.parent.GetComponent<LetterDialog>().sendsletterto)){
            gameObject.SetActive(false);
        }
    }
}
