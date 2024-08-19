using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using SimpleMan.CoroutineExtensions;
using Monologue.Dialogue;

public class TutorialCameraController : MonoBehaviour
{
    [SerializeField] CinemachineCamera _Sideview;
    [SerializeField] EnterDialogueMode _StartDialogue;
    [SerializeField] CinemachineBasicMultiChannelPerlin _OverheadScreenShake;
    [SerializeField] Animator _PlayerAnimation;
    void OnEnable()
    {
        DialogueManager.OnDialogueEndEvent += BeginTutorial;
    }
    void OnDisable()
    {
        DialogueManager.OnDialogueEndEvent -= BeginTutorial;
    }
    void Start()
    {
        this.Delay(3.1f, () =>
        {
            StartCoroutine(SmoothlyDecreaseShake(1.2f));
        }
        );
        this.Delay(6f, () => 
        {
            _Sideview.Priority = 2;
            this.Delay(2.5f, () =>  _PlayerAnimation.Play("Player_Wake"));
            this.Delay(3f, () => _StartDialogue.EnterDialogue());
        });
    }
    private IEnumerator SmoothlyDecreaseShake(float duration)
    {
        float startTime = Time.time;
        float startAmplitude = _OverheadScreenShake.AmplitudeGain;
        float startFrequency = _OverheadScreenShake.FrequencyGain;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            _OverheadScreenShake.AmplitudeGain = Mathf.Lerp(startAmplitude, 0, t);
            _OverheadScreenShake.FrequencyGain = Mathf.Lerp(startFrequency, 0, t);
            yield return null;
        }

        _OverheadScreenShake.AmplitudeGain = 0;
        _OverheadScreenShake.FrequencyGain = 0;
    }

    void BeginTutorial()
    {
        
    }
}
