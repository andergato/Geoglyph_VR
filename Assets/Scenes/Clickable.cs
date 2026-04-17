using System.Collections;
using System.Collections.Generic;
using UltimateXR.Avatar;
using UnityEngine;
using UltimateXR.Devices;
using UnityEngine.Animations;

public class Clickable : MonoBehaviour
{
    public GameObject infoCanvas;
    // Start is called before the first frame update
    void Start()
    {
        infoCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if(collision.gameObject.CompareTag("Ray"))
    //    {
    //        if (UxrAvatar.LocalAvatarInput.GetButtonsPress(UltimateXR.Core.UxrHandSide.Right, UxrInputButtons.Menu))
    //        {
    //            if(infoCanvas.activeSelf)
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


    public void clickButton()
    {
        if (infoCanvas.activeSelf)
        {
            infoCanvas.SetActive(false);
        }
        else
        {
            infoCanvas.SetActive(true);
        }
    }
}
