using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;
using Monologue.Dialogue;

// cuz like it shakes or something
public class RadioShack : MonoBehaviour
{
    [SerializeField] GameObject _Radio;
    Vector3 _InitialRadioPosition;
    [SerializeField] LeanShake _Shake;

    void OnEnable()
    {
        StoryFunctions.OnSpeakerEvent += RadioShaketh;
    }

    void OnDisable()
    {
        StoryFunctions.OnSpeakerEvent += RadioShaketh;
    }
    void Awake()
    {
        _InitialRadioPosition = _Radio.transform.position;
    }
    void RadioShaketh(string radioName)
    {
        if(radioName.Equals("radio"))
        {
            _Shake.Strength = 0.1f;
            DialogueManager.OnDialogueTryingToContinueEvent += GoodbyeVietnam;
        }
    }

    // you can really tell how drunk i am from the level of silly that are my function names.
    void GoodbyeVietnam()
    {
        _Shake.Strength = 0f;
        _Radio.transform.position = Vector3.Lerp(_Radio.transform.position, _InitialRadioPosition, 0.1f);
        DialogueManager.OnDialogueTryingToContinueEvent -= GoodbyeVietnam;
    }
}
