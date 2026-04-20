using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Clickable clickableInstance;
    public GameObject textbox;
    // Start is called before the first frame update
    void Start()
    {
        textbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void StartDialogue()
    //{
    //    clickableInstance.clickButton(textbox);
    //}
}
