using System.Collections;
using System.Collections.Generic;
using UltimateXR.Avatar;
using UnityEngine;
using UltimateXR.Devices;
using UnityEngine.Animations;

public class Clickable : MonoBehaviour
{
    public GameObject promptBox;
    public GameObject resultBox;
    //Start is called before the first frame update
    void Start()
    {
        promptBox.SetActive(false);
        resultBox.SetActive(false);
    }

    //Update is called once per frame
    void Update()
    {

    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ray"))
    //    {
    //        if (UxrAvatar.LocalAvatarInput.GetButtonsPress(UltimateXR.Core.UxrHandSide.Right, UxrInputButtons.Menu))
    //        {
    //            if (infoCanvas.activeSelf)
    //            {
    //                infoCanvas.SetActive(false);
    //            }
    //            else
    //            {
    //                infoCanvas.SetActive(true);
    //            }
    //        }
    //    }
    //}


    public void ClickButton()
    {
        if (promptBox != null)
        {
            // This line toggles the state (true to false, or false to true)
            promptBox.SetActive(!promptBox.activeSelf);
        }
    }

    public void StartDialogue()
    {
        resultBox.SetActive(true);
    }
}
