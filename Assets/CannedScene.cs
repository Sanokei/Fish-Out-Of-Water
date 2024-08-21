using System.Collections;
using System.Collections.Generic;
using Lean.Transition;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CannedScene : MonoBehaviour
{
    [SerializeField] CanvasGroup _CannedCanvas;
    [SerializeField] CanvasGroup _CannedText;
    [SerializeField] Transform _Shelves;
    [SerializeField] Transform _Credits;
    [SerializeField] Transform _CanOfTuna;
    [SerializeField] CinemachineSplineDolly _SplineAutoDolly;

    void Start()
    {
        _CanOfTuna.positionTransition_y(0.91f,1.5f, LeanEase.Elastic)
            .JoinTransition()
            .EventTransition(()=> {
                _CannedText.alphaTransition(1,2.5f, LeanEase.Accelerate)
                    .JoinTransition()
                    .EventTransition(()=>
                    {
                        _CannedCanvas.alphaTransition(0,4f, LeanEase.Accelerate)
                            .JoinTransition()
                            .EventTransition(()=>
                            {
                                _Shelves.transform.positionTransition_z(-24.302f,1f, LeanEase.Smooth)
                                    .JoinDelayTransition(2f)
                                    .EventTransition( ()=>
                                    {
                                        _SplineAutoDolly.AutomaticDolly.Enabled = true;
                                    })
                                    .JoinDelayTransition(10f)
                                    .EventTransition(()=>{
                                        _Credits.transform.localPositionTransition_y(1000f, 10f, LeanEase.Smooth)
                                        .JoinDelayTransition(0.5f)
                                        .EventTransition(()=>{
                                            SceneManager.LoadScene("Menu");
                                    });
                                });
                            }
                        );
                    });
            }
        );
            
            
    }
}
