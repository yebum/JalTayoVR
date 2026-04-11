using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSimpleInteractable))]
[RequireComponent(typeof(Collider))]
public class BellController : MonoBehaviour
{
    [Header("Tag Settings")]
    [SerializeField] private string bellTag = "Bell";

    [Header("Color Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color activeColor = Color.red;

    [Header("Option")]
    [SerializeField] private bool resetOnHoverExit = false;

    private XRSimpleInteractable simpleInteractable;

    private void Awake()
    {
        simpleInteractable = GetComponent<XRSimpleInteractable>();
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