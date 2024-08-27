using System;
using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using Monologue.Dialogue;
using SimpleMan.CoroutineExtensions;
using UnityEngine;

public class OfficeScene : MonoBehaviour
{
    [SerializeField] TextAsset _InkJSON;
    [SerializeField] TransitionSettings _CircleTransition;
    void OnEnable()
    {
        DialogueManager.OnDialogueEndEvent += BeginTutorial;
    }

    void OnDisable()
    {
        DialogueManager.OnDialogueEndEvent -= BeginTutorial;
    }

    private void BeginTutorial()
    {
        
       TransitionManager.Instance().Transition("Work",_CircleTransition, 0f);
    }

    void Start()
    {
       DialogueManager.Instance.EnterDialogMode(_InkJSON);
    }
}
