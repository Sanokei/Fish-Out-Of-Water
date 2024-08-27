using System;
using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using Monologue.Dialogue;
using SimpleMan.CoroutineExtensions;
using UnityEngine;

public class FiringScene : MonoBehaviour
{
    [SerializeField] TextAsset _InkJSON;
    [SerializeField] TransitionSettings _CircleTransition;
    void OnEnable()
    {
        DialogueManager.OnDialogueEndEvent += EndGame;
    }

    void OnDisable()
    {
        DialogueManager.OnDialogueEndEvent -= EndGame;
    }

    private void EndGame()
    {
       TransitionManager.Instance().Transition("Canned",_CircleTransition, 1f);
    }

    void Start()
    {
       DialogueManager.Instance.EnterDialogMode(_InkJSON);
    }
}
