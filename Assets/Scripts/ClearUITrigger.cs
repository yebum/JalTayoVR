using UnityEngine;
using System.Collections;

public class ClearUITrigger : MonoBehaviour
{
    [Header("Clear UI")]
    public GameObject clearUICanvas;

    [Header("Display")]
    public float displayDuration = 2f;

    [Header("Tag Filter")]
    public string targetTag = "Player";

    private bool hasTriggered = false;

    private void Start()
    {
        if (clearUICanvas != null)
            clearUICanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;
        if (hasTriggered) return;

        hasTriggered = true;
        StartCoroutine(ClearUISequence());
    }

    private IEnumerator ClearUISequence()
    {
        clearUICanvas.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        clearUICanvas.SetActive(false);
    }
}