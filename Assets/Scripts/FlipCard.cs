using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

// This script is used to flip the card when the player clicks on it
// It is attached to the card prefab
public class FlipCard : MonoBehaviour, IPointerClickHandler
{
    private bool isFlipped = false;

    public void Flip()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Debug.Log("RectTransform: " + rectTransform.rotation.ToString());

        // Get current rotation and only modify the Y axis
        Vector3 currentRotation = rectTransform.localEulerAngles;
        Vector3 targetRotation = new Vector3(
            currentRotation.x,  // Keep existing X rotation
            isFlipped ? 0 : 180,  // Only flip Y axis
            currentRotation.z   // Keep existing Z rotation
        );

        rectTransform.DOLocalRotate(targetRotation, 0.5f);
        Debug.Log("RectTransform After: " + rectTransform.rotation.ToString());
        isFlipped = !isFlipped;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Card clicked!" + gameObject.name);
        Flip();
    }
}
