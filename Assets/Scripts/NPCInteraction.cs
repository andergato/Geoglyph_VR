using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.UI.UnityInputModule;
//using UltimateXR.Interaction.Pointing;
using TMPro;

[System.Serializable]
public class NPCInteraction : MonoBehaviour
{
    UxrPointerEventData pointerData;
    public GameObject textbox;
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void OnPointerClick(UxrPointerEventData data)
    {
        pointerData = data;
        Debug.Log("Clicked on NPC");
        textbox.SetActive(true);
    }

    public void OnPointerEnter(UxrPointerEventData pointerEventData) { }
    public void OnPointerExit(UxrPointerEventData pointerEventData) { }
    public void OnPointerDown(UxrPointerEventData pointerEventData) { }
    public void OnPointerUp(UxrPointerEventData pointerEventData) { }

}
