using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingBlocks.DataTypes;
using TMPro;
using Monologue.Dialogue;
using System;
using Lean.Transition;
using SimpleMan.CoroutineExtensions;
using UnityEngine.SceneManagement;
using EasyTransition;
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _Product;
    [SerializeField] GameObject _Player;
    [SerializeField] Vector3 _SpawnLocation;
    [SerializeField] Vector3 _LerpLocation;
    [SerializeField] Vector3 _BackLerpLocation;
    [SerializeField] TMP_Text _ProductsLeft;
    [SerializeField] TMP_Text _Quota;
    public int Quota = 65827;
    [SerializeField] TMP_Text _Weight;
    public int Weight;
    public int CurrentQuota;
    [SerializeField] Animation _GreenButton;
    [SerializeField] Animation _RedButton;
    [SerializeField] Animation _LeftCover;
    [SerializeField] Animation _RightCover;
    [SerializeField] AnimationClip _GreenButtonPress;
    [SerializeField] AnimationClip _RedButtonPress;
    [SerializeField] AnimationClip _OpenRightSide;
    [SerializeField] AnimationClip _OpenLeftSide;
    [SerializeField] AnimationClip _CloseRightSide;
    [SerializeField] AnimationClip _CloseLeftSide;
    
    [SerializeField] Animator _PlayerAnimator;

    [SerializeField] List<TextAsset> _Conversations;

    [SerializeField] TextAsset _Start;
    [SerializeField] int _TotalConversations;
    GameObject _CurrentProduct;

    [SerializeField] TransitionSettings _CircleTransition;
    [SerializeField] MovementSideScroll _PlayerMovement;
    void OnEnable()
    {
        DialogueManager.OnDialogueEndEvent += StartGame;
        DialogueManager.OnChoiceEvent += SetTrialChamber;
        CursorManager.OnInteractableClickedEvent += DevineJudgement;
        StoryFunctions.OnAnimationEvent += StartPlayerAnimation;
    }

    void OnDisable()
    {
        // DialogueManager.OnDialogueEndEvent -= InspectProduct;
        DialogueManager.OnChoiceEvent -= SetTrialChamber;
        CursorManager.OnInteractableClickedEvent -= DevineJudgement;
        StoryFunctions.OnAnimationEvent -= StartPlayerAnimation;
    }

    void Start()
    {
        CinemachineCameraManager.Instance.SetCam("Workview");
        _Quota.text = $"{CurrentQuota}lb/{Quota}lb";
        _ProductsLeft.text = $"{_Conversations.Count - _TotalConversations}/{_Conversations.Count}";
        DialogueManager.Instance.EnterDialogMode(_Start);
    }
    void StartGame()
    {
        InspectProduct();
        DialogueManager.OnDialogueEndEvent -= StartGame;
    }
    private void InspectProduct()
    {
        if(_TotalConversations >= _Conversations.Count)
        {
            SendToFire();
            return;
        }   
        CinemachineCameraManager.Instance.SetCam("Workview");
        _CurrentProduct = Instantiate(_Product, _SpawnLocation, Quaternion.identity);
        _PlayerAnimator = _CurrentProduct.GetComponent<Animator>();
        Weight = UnityEngine.Random.Range(140,300);
        // _CurrentProduct.transform.LookAt(_Player.transform);
        _CurrentProduct.transform.rotation = Quaternion.Euler(0,90,0);
        ConveyerBeltManager.Instance.RunConveyerBelt(5f);
        this.Delay(1f, () =>
        {
            if(_CurrentProduct)
                _CurrentProduct.transform.localPositionTransition(_LerpLocation, 5f)
                    .JoinTransition()
                    .EventTransition(() => 
                    {
                        _Weight.text = $"{Weight}lb";
                        DialogueManager.Instance.EnterDialogMode(_Conversations[_TotalConversations]);
                    });
        });
    }

    // Job
    // list of people -> Number of people + dialogue
    // quota
    void SetTrialChamber(List<string> choices)
    {
        _LeftCover.clip = _OpenLeftSide;
        _LeftCover.Play();
        _RightCover.clip = _OpenRightSide;
        _RightCover.Play();
    }
    void DevineJudgement(GameObject interactable)
    {
        if(DialogueManager.Instance.ActiveDialoguePanel)
        {
            CursorManager.OnInteractableClickedEvent -= DevineJudgement;
            _LeftCover.clip = _CloseLeftSide;
            _LeftCover.Play();
            _RightCover.clip = _CloseRightSide;
            _RightCover.Play();
            CinemachineCameraManager.Instance.SetCam("Productview");
            if(interactable.name.Equals("no"))
            {
                _RedButton.clip = _RedButtonPress;
                _RedButton.Play();
                ConveyerBeltManager.Instance.RunConveyerBelt(-6f);
                DialogueManager.Instance.ChoiceSelected(1);

                _CurrentProduct.transform.localPositionTransition(_BackLerpLocation - new Vector3(3,0), 3f)
                    .JoinDelayTransition(3f)
                    .EventTransition(() => 
                    {
                        Destroy(_CurrentProduct);
                    });
            }
            else if(interactable.name.Equals("yes"))
            {
                _GreenButton.clip = _GreenButtonPress;
                _GreenButton.Play();
                CurrentQuota += Weight;
                ConveyerBeltManager.Instance.RunConveyerBelt(1f);
                DialogueManager.Instance.ChoiceSelected(0);
                _CurrentProduct.transform.localPositionTransition(_LerpLocation + new Vector3(3,0), 1f)
                    .JoinDelayTransition(3f)
                    .EventTransition(() => 
                    {
                        Destroy(_CurrentProduct);
                    });
            }
            _TotalConversations++;
            _ProductsLeft.text = $"{_Conversations.Count - _TotalConversations}/{_Conversations.Count}";
            _Quota.text = $"{CurrentQuota}lb/{Quota}lb";
            this.Delay(7f,() => {
                InspectProduct();
                CursorManager.OnInteractableClickedEvent += DevineJudgement;
            });
        }
    }
    string _CurrentPlayerAnimation;
    void StartPlayerAnimation(string animationString)
    {
        print("test");
        _CurrentPlayerAnimation = animationString;
        _PlayerAnimator.SetBool(_CurrentPlayerAnimation, true);
        DialogueManager.OnDialogueTryingToContinueEvent += StopPlayerAnimation;
    }

    void StopPlayerAnimation()
    {
        _PlayerAnimator.SetBool(_CurrentPlayerAnimation, false);
        DialogueManager.OnDialogueTryingToContinueEvent -= StopPlayerAnimation;
    }

    void SendToFire()
    {
        _PlayerMovement.CanMove = false;
        TransitionManager.Instance().Transition("Firing", _CircleTransition, 0f);
    }
}
