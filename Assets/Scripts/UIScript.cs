using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    [SerializeField]
    ARCameraManager arCamera;
    [SerializeField]
    TMP_Text cookieText;
    bool cookieFound = false;

    [SerializeField]
    ARTrackedImageManager arTImanager;
    [SerializeField]
    ARFaceManager arFmanager;

    List<ARFace> faces = new List<ARFace>();
    [SerializeField]
    Button eatCookie;

    void OnEnable()
    {
        arTImanager.trackedImagesChanged += OnChanged;
        arFmanager.facesChanged += OnFaceChanged;
    }
    void OnDisable()
    {
        arTImanager.trackedImagesChanged -= OnChanged;
        arFmanager.facesChanged -= OnFaceChanged;
    }

    void OnFaceChanged(ARFacesChangedEventArgs eventArgs)
    {
        foreach (var newFace in eventArgs.added)
        {
            faces.Add(newFace);
        }
        foreach (var lostFace in eventArgs.removed)
        {
            faces.Remove(lostFace);
        }
    }

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            cookieFound = true;
        }
    }
    
     
    public void CameraSwitch(TMP_Text buttonText)
    {
        if (arCamera.currentFacingDirection == CameraFacingDirection.World)
        {
            arTImanager.enabled = false;
            arCamera.requestedFacingDirection = CameraFacingDirection.User;
            buttonText.text = "Rear";
            
        }
        else if (arCamera.currentFacingDirection == CameraFacingDirection.User)
        {
            arTImanager.enabled = true;
            arCamera.requestedFacingDirection = CameraFacingDirection.World;
            buttonText.text = "Front";
        }
    }

    private void Update()
    {
        if (cookieFound == true)
        {
            cookieText.text = "Cookie Found";
            cookieText.color = Color.green;
        }
        if (cookieFound == false)
        {
            cookieText.text = "Cookie Not Found";
            cookieText.color = Color.red;
        }

        foreach (var face in faces)
        {
            Vector3 pos = face.transform.TransformPoint(face.vertices[14]);
            Vector3 screenPos = arCamera.GetComponent<Camera>().WorldToScreenPoint(pos);
            eatCookie.transform.position = screenPos;

        }
    }
}
