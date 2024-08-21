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
    
    [SerializeField] List<TextAsset> _Conversations;

    [SerializeField] TextAsset _Start;
    [SerializeField] int _TotalConversations;
    GameObject _CurrentProduct;
    void OnEnable()
    {
        DialogueManager.OnDialogueEndEvent += InspectProduct;
        DialogueManager.OnChoiceEvent += SetTrialChamber;
        CursorManager.OnInteractableClickedEvent += DevineJudgement;
    }

    void OnDisable()
    {
        DialogueManager.OnDialogueEndEvent -= InspectProduct;
        DialogueManager.OnChoiceEvent -= SetTrialChamber;
        CursorManager.OnInteractableClickedEvent -= DevineJudgement;
    }

    void Start()
    {
        CinemachineCameraManager.Instance.SetCam("Workview");
        DialogueManager.Instance.EnterDialogMode(_Start);
    }

    private void InspectProduct()
    {
        CinemachineCameraManager.Instance.SetCam("Workview");
        _CurrentProduct = Instantiate(_Product, _SpawnLocation, Quaternion.identity);
        Weight = UnityEngine.Random.Range(140,300);
        _Weight.text = $"{Weight}";
        _CurrentProduct.transform.LookAt(_Player.transform);
        ConveyerBeltManager.Instance.RunConveyerBelt(5f);
        this.Delay(2f, () =>
        {
            _CurrentProduct.transform.localPositionTransition(_LerpLocation, 5f)
                .JoinTransition()
                .EventTransition(() => 
                {
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
            if(_TotalConversations >= _Conversations.Count)
            {
                SceneManager.LoadScene("Canned");
            }
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

                _CurrentProduct.transform.localPositionTransition(_BackLerpLocation, 3f)
                    .JoinDelayTransition(5f)
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
                _CurrentProduct.transform.localPositionTransition(_LerpLocation + new Vector3(2,0), 1f)
                    .JoinTransition()
                    .EventTransition(() => 
                    {
                        Destroy(_CurrentProduct);
                    });
            }
            _TotalConversations++;
            _ProductsLeft.text = $"{_TotalConversations}/{_Conversations.Count}";
            _Quota.text = $"{CurrentQuota}lb/{Quota}lb";
        }
    }

    // Human gets orderer
    // conveyer belt
    // EnterDialogueMode dialogueMode;
    // covers go up.
     
}
