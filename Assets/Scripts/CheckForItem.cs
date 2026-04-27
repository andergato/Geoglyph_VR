using System.Collections;
using System.Collections.Generic;
using UltimateXR.Avatar.Rig;
using UnityEngine;
using UltimateXR.Devices;
using UnityEngine.Animations;
using UltimateXR.Manipulation;
using UltimateXR.Avatar;

public class CheckForItem : MonoBehaviour
{
    public GameObject item;
    public UxrGrabber grabber;
    public UxrAvatar avatar;
    public GameObject npc;
    private float threshold = 5f;
    public GameObject textbox;
    //public UxrGrabbableObject grabbedObject;
    // Start is called before the first frame update
    void Start()
    {
        textbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //DialogueManager dialogueManagerInstance = new DialogueManager();
        Clickable clickableInstance = new Clickable();
        Vector3 avatarPos = avatar.CameraPosition;
        Vector3 npcPos = npc.transform.position;
        float distance = Vector3.Distance(avatarPos, npcPos);
        if (distance < threshold)
        {
            //Debug.Log("Avatar is close to NPC");
            if(grabber.GrabbedObject != null)
            {
                if(grabber.GrabbedObject.gameObject.transform.IsChildOf(item.transform))
                {
                    //dialogueManagerInstance.StartDialogue();
                    //clickableInstance.StartDialogue();
                    textbox.SetActive(true);
                    Debug.Log("Avatar is holding the item");

                }
                else
                {

                    Debug.Log("Avatar is holding something else");
                }
            }
            
        }
        //Debug.Log("Avatar is located at: " + avatarPos);
        //Debug.Log("Hand is holding: " + grabber.GrabbedObject.name);
        //Debug.Log("Avatar is located at: " + avatarPos);
        //Debug.Log("Avatar is located at: ");
    }
}
