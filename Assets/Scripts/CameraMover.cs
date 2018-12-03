using UnityEngine;
using UnityEngine.EventSystems;

/*
/// <summary>
/// Camera mover prototype.
/// </summary>
public class CameraMover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private CameraController.WindowEdges windowEdge;
    private CameraController C;

    /// <summary>
    /// Raises the pointer enter event.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (windowEdge)
        {
            case CameraController.WindowEdges.Left:
                C.Move(Vector3.left);
                break;
            case CameraController.WindowEdges.Right:
                C.Move(Vector3.right);
                break;
            case CameraController.WindowEdges.Top:
                C.Move(Vector3.forward);
                break;
            case CameraController.WindowEdges.Bottom:
                C.Move(Vector3.back);
                break;
            case CameraController.WindowEdges.LeftTop:
                C.Move(Vector3.left + Vector3.forward);
                break;
            case CameraController.WindowEdges.LeftBottom:
                C.Move(Vector3.left + Vector3.back);
                break;
            case CameraController.WindowEdges.RightTop:
                C.Move(Vector3.right + Vector3.forward);
                break;
            case CameraController.WindowEdges.RightBottom:
                C.Move(Vector3.right + Vector3.back);
                break;
            default:
                Debug.LogError("Window Edge not set.");
                break;
        }
    }

    /// <summary>
    /// Raises the pointer exit event.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        C.CancelMove();
    }

    void Start()
    {
        C = Camera.main.GetComponent<CameraController>();
    }
}
*/