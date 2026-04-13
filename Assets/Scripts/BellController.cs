using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSimpleInteractable))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))] // 추가
public class BellController : MonoBehaviour
{
    [Header("Tag Settings")]
    [SerializeField] private string bellTag = "Bell";

    [Header("Color Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color activeColor = Color.red;

    [Header("Option")]
    [SerializeField] private bool resetOnHoverExit = false;

    [Header("Sound Settings")] // 추가
    [SerializeField] private AudioClip bellSound;

    private XRSimpleInteractable simpleInteractable;
    private AudioSource audioSource; // 추가

    private void Awake()
    {
        simpleInteractable = GetComponent<XRSimpleInteractable>();
        audioSource = GetComponent<AudioSource>(); // 추가
    }

    private void OnEnable()
    {
        simpleInteractable.hoverEntered.AddListener(OnHoverEntered);
        simpleInteractable.hoverExited.AddListener(OnHoverExited);
    }

    private void OnDisable()
    {
        simpleInteractable.hoverEntered.RemoveListener(OnHoverEntered);
        simpleInteractable.hoverExited.RemoveListener(OnHoverExited);
    }

    private void Start()
    {
        SetAllBellsColor(normalColor);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        SetAllBellsColor(activeColor);

        if (audioSource != null && bellSound != null)
        {
            audioSource.PlayOneShot(bellSound);
            Debug.Log("벨 사운드 재생됨");
        }
        else
        {
            Debug.LogWarning("AudioSource 또는 AudioClip이 연결되지 않음");
        }
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (resetOnHoverExit)
        {
            SetAllBellsColor(normalColor);
        }
    }

    private void SetAllBellsColor(Color targetColor)
    {
        GameObject[] bells = GameObject.FindGameObjectsWithTag(bellTag);

        foreach (GameObject bell in bells)
        {
            Renderer rend = bell.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = targetColor;
            }
        }
    }
}