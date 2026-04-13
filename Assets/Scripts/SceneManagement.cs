using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class SceneManagement : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string targetScene;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void LoadScene()
    {
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        // 효과음 재생
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
            yield return new WaitForSeconds(clickSound.length);
        }

        // 씬 이동
        SceneManager.LoadScene(targetScene);
    }
}