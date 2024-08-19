using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using SimpleMan.CoroutineExtensions;

public class TutorialCameraController : MonoBehaviour
{
    [SerializeField] CinemachineCamera _Sideview;
    [SerializeField] MovementSideScroll _Player;
    void Awake()
    {
        _Player.CanMove = false;

    }
    void Start()
    {
        this.Delay(2f, () => 
        {
            _Sideview.Priority = 2;
            this.Delay(6f, () => BeginTutorial());
        });
    }

    void BeginTutorial()
    {
        
    }
}
