using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RawImage targetRawImage;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.red;

    void Start()
    {
        if (targetRawImage == null)
            targetRawImage = GetComponentInChildren<RawImage>();

        targetRawImage.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetRawImage != null)
        {
            targetRawImage.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetRawImage != null)
        {
            targetRawImage.color = normalColor;
        }
    }

    public void ResetHoverState()
    {
        if (targetRawImage != null)
        {
            targetRawImage.color = normalColor;
        }
    }
}
