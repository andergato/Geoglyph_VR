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
    public string condition;
    //public UxrGrabbableObject grabbedObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
                    QuestFlags.Set(condition);
                    Debug.Log("Avatar is holding the item");
                }
                else
                {

                    Debug.Log("Avatar is holding something else");
                }
            }
            
        }
    }
}
