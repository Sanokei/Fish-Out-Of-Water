using System;
using System.Collections;
using System.Collections.Generic;
using Monologue.Dialogue;
using SimpleMan.CoroutineExtensions;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] MovementSideScroll _Player;
    void Awake()
    {
        _Player.CanMove = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
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
        DialogueManager.OnDialogueEndEvent -= BeginTutorial;
        _Player.CanMove = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // activate phone with phone message.
        // this.Delay("")
    }
}
