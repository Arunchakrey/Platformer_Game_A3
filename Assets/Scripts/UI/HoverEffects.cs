using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Optional visuals")]
    public Image targetImage;          // leave null to skip color change
    public Color hoverColor = new Color(1f, 1f, 1f, 1f);
    private Color _normalColor;

    [Header("Scale")]
    public bool scaleOnHover = true;
    public float hoverScale = 1.08f;
    private Vector3 _startScale;

    void Awake()
    {
        _startScale = transform.localScale;
        if (targetImage != null) _normalColor = targetImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (scaleOnHover) transform.localScale = _startScale * hoverScale;
        if (targetImage != null) targetImage.color = hoverColor;
        // play a hover sound here if you like
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scaleOnHover) transform.localScale = _startScale;
        if (targetImage != null) targetImage.color = _normalColor;
    }
}
