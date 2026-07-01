using UnityEngine;
using UnityEngine.UI;

public class ZoneTriggerController : MonoBehaviour
{
    [Header("Canvas Image")]
    public Image targetImage;
    public Sprite defaultSprite;
    public Sprite triggerSprite;

    [Header("Tag Filter")]
    public string targetTag = "Player";

    private bool hasTriggered = false;
    private Vector2 originalSize;

    private void Start()
    {
        if (targetImage != null && defaultSprite != null)
        {
            targetImage.sprite = defaultSprite;
            originalSize = targetImage.rectTransform.sizeDelta;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;
        if (hasTriggered) return;

        hasTriggered = true;

        if (targetImage != null && triggerSprite != null)
        {
            targetImage.sprite = triggerSprite;
            targetImage.rectTransform.sizeDelta = originalSize;
        }
    }
}