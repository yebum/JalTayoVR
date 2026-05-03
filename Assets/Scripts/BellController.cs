using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSimpleInteractable))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class BellController : MonoBehaviour
{
    [Header("Tag Settings")]
    [SerializeField] private string bellTag = "Bell";

    [Header("Color Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color activeColor = Color.red;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip bellSound;

    [Header("Bus Route")]
    [SerializeField] private BusRoute busRoute;

    private XRSimpleInteractable simpleInteractable;
    private AudioSource audioSource;
    private bool isPressed = false;

    private void Awake()
    {
        simpleInteractable = GetComponent<XRSimpleInteractable>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        simpleInteractable.hoverEntered.AddListener(OnHoverEntered);
    }

    private void OnDisable()
    {
        simpleInteractable.hoverEntered.RemoveListener(OnHoverEntered);
    }

    private void Start()
    {
        SetAllBellsColor(normalColor);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (isPressed) return;

        isPressed = true;

        SetAllBellsColor(activeColor);

        if (audioSource != null && bellSound != null)
        {
            audioSource.PlayOneShot(bellSound);
        }

        if (busRoute != null)
        {
            busRoute.RequestStop();
        }

        Debug.Log("하차벨 입력: 다음 정류장 정차 요청");
    }

    public void ResetBell()
    {
        isPressed = false;
        SetAllBellsColor(normalColor);
        Debug.Log("하차벨 초기화");
    }

    private void SetAllBellsColor(Color targetColor)
    {
        GameObject[] bells = GameObject.FindGameObjectsWithTag(bellTag);

        foreach (GameObject bell in bells)
        {
            Renderer rend = bell.GetComponent<Renderer>();

            if (rend == null)
            {
                rend = bell.GetComponentInChildren<Renderer>();
            }

            if (rend != null)
            {
                rend.material.color = targetColor;
            }
        }
    }
}