using UnityEngine;

public class InteractiveObject3D : MonoBehaviour
{
    [SerializeField]
    Canvas messageCanvas;

    void Start()
    {
        messageCanvas.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            TurnOnMessage();
        }
    }

    private void TurnOnMessage()
    {
        messageCanvas.enabled = true;
    }


    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            TurnOffMessage();
        }
    }

    private void TurnOffMessage()
    {
        messageCanvas.enabled = false;
    }
}
