using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Avatar;
using UltimateXR.Avatar.Rig;
using UltimateXR.Devices;
using UnityEngine.Animations;
using UltimateXR.Manipulation;

public class FacePlayerSmooth : MonoBehaviour
{
    public UxrAvatar avatar;
    public Transform NPC;
    private Transform avatarTransform;
    public float rotationSpeed = 3.0f;
    public float rotationTolerance = 5.0f;

    void Update()
    {

        
        avatarTransform = avatar.CameraTransform;

        //NPC.LookAt(avatarTransform);

        //NPC.rotation.x = 0;
        //NPC.rotation.y = 0;

        Vector3 avatarPos = avatar.CameraPosition;
        if (avatarTransform == null) return;

        Vector3 direction = avatarTransform.position - transform.position;
        direction.y = 0; // Keep the NPC upright
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        //float angle = Quaternion.Angle(transform.rotation, targetRotation);

        // Rotate smoothly
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
}