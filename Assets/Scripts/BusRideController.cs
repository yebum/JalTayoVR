using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BusRideController : MonoBehaviour
{
    [Header("Assign in Inspector")]
    [SerializeField] private Transform passengerAnchor;

    [Header("Optional Debug")]
    [SerializeField] private bool showDebugLog = false;

    private Transform playerRoot;
    private bool isRiding = false;

    private void Awake()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        box.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform root = other.transform.root;

        if (!root.CompareTag("Player"))
            return;

        if (isRiding && playerRoot == root)
            return;

        playerRoot = root;
        isRiding = true;

        if (passengerAnchor != null)
        {
            playerRoot.SetParent(passengerAnchor, true);
        }

        if (showDebugLog)
        {
            Debug.Log("Player boarded bus.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform root = other.transform.root;

        if (!root.CompareTag("Player"))
            return;

        if (!isRiding || root != playerRoot)
            return;

        playerRoot.SetParent(null, true);

        isRiding = false;
        playerRoot = null;

        if (showDebugLog)
        {
            Debug.Log("Player left bus.");
        }
    }
}