using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using SimpleMan.CoroutineExtensions;
using UnityEngine;

public class TutorialLeaveScene : MonoBehaviour
{
    [SerializeField] TransitionSettings _CircleTransition;
    [SerializeField] MovementSideScroll _Player;

    void OnTriggerEnter(Collider other)
    {
        _Player.CanMove = false;
        TransitionManager.Instance().Transition("Work", _CircleTransition, 1f);
    }
}
