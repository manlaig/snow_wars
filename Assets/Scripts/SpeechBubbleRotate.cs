using UnityEngine;
using System.Collections;

public class SpeechBubbleRotate : MonoBehaviour
{
    Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }
}