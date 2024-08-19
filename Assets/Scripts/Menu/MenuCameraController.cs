using System.Collections;
using System.Collections.Generic;
using SimpleMan.CoroutineExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Unity.Cinemachine;

public class MenuCameraController : MonoBehaviour
{
    [SerializeField] CinemachineCamera _Ocean;
    [SerializeField] CinemachineCamera _Dark;
    [SerializeField] Camera _MainCamera;
    [SerializeField] VideoPlayer _BubbleTransition;

    string _QueuedSceneName;
    void OnEnable()
    {
        CinemachineCameraManager.Instance.SetCam(_Ocean);
    }
    public void GoToScene(string sceneName)
    {
        _Ocean.Priority = 0;
        _Dark.Priority = 1;
        _QueuedSceneName = sceneName;
        this.Delay(5f,() =>
        {
            _BubbleTransition.Play();
            StartCoroutine(ChangeColor(new Color(0.08810966f, 0.1572813f, 0.4150943f, 1f),Color.black,4.5f,() => SceneManager.LoadScene(_QueuedSceneName)));
        }
        );
    }

    IEnumerator ChangeColor(Color startColor, Color endColor, float duration, System.Action callback)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            _MainCamera.backgroundColor = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is set
        _MainCamera.backgroundColor = endColor;
        callback();
    }
}
