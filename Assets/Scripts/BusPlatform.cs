using UnityEngine;

public class BusPlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
            Debug.Log("승객이 버스에 탑승했습니다.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
            Debug.Log("승객이 버스에서 하차했습니다.");
        }
    }
}