using System.Collections;
using System.Collections.Generic;
using Monologue.Dialogue;
using UnityEngine;

public class TutorialItemsDialogue : MonoBehaviour
{
    [SerializeField] EnterDialogueMode _Painting;
    [SerializeField] EnterDialogueMode _Clock;

    void OnEnable()
    {
        CursorManager.OnInteractableClickedEvent += InteractableItemHit;
    }

    void OnDisable()
    {
        CursorManager.OnInteractableClickedEvent -= InteractableItemHit;
    }

    void InteractableItemHit(GameObject obj)
    {
        if(DialogueManager.Instance.ActiveDialoguePanel)
            return;
        // I just realized you can really tell I came from Java when I still use String.Equals for string comparisons.
        // the trauma.. 
        if(obj.name.Equals("clock"))
            _Clock.EnterDialogue();

        if(obj.name.Equals("painting"))
            _Painting.EnterDialogue();
    }
}
