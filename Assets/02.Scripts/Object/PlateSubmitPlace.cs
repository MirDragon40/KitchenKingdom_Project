using DG.Tweening;
using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlateSubmitPlace : MonoBehaviour
{
    private FoodCombination _foodCombo;
    public bool IsServeable = false;
    private CharacterHoldAbility _holdability;
    private string _plateContent = string.Empty;
    public TMP_Text ScoreUI;
    private PhotonView _pv;

    // 음식별 점수를 저장하는 Dictionary
    private Dictionary<string, int> foodScores = new Dictionary<string, int>
    {
        { "burgerCokeFry", 50 },
        { "burgerCoke", 30 },
        { "burger", 15 }
    };

    private void Awake()
    {
        _pv = GetComponent<PhotonView>();
        ScoreUI.text = string.Empty;
    }

    private void LateUpdate()
    {
        if (IsServeable && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(_plateContent);
            _pv.RPC("SubmitPlate", RpcTarget.All);
            SoundManager.Instance.PlayAudio("Reappearance", false, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ShowScoreUI(25);
        }
    }

    private void ShowScoreUI(int score)
    {
        ScoreUI.text = $"+{score}pts";
        ScoreUI.alpha = 1.0f;
        ScoreUI.color = new Color(0, 0.2f, 0);
        RectTransform scoreRectTransform = ScoreUI.GetComponent<RectTransform>();
        scoreRectTransform.anchoredPosition = new Vector2(0, 0);
        scoreRectTransform.DOAnchorPosY(50, 2f).SetEase(Ease.OutBounce);
        ScoreUI.DOColor(Color.green, 2f);
        ScoreUI.DOFade(0, 2.5f);
    }

    [PunRPC]
    private void SubmitPlate()
    {
        bool isMatchingOrder = OrderManager.Instance.SubmitOrder(_plateContent);
        if (isMatchingOrder)
        {
            int score = 0;
            if (foodScores.TryGetValue(_plateContent, out score))
            {
                ShowScoreUI(score);
                if (PhotonNetwork.IsMasterClient)
                {
                    OrderManager.Instance.RequestAddDirtyPlates();
                }
            }
        }

        if (_foodCombo != null)
        {
            if (_foodCombo.PV.IsMine)
            {
                PhotonNetwork.Destroy(_foodCombo.gameObject);
            }
        }
        _foodCombo = null;
        _plateContent = string.Empty;

        _holdability._pv.RPC("Drop", RpcTarget.All);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out _holdability))
        {
            if (_holdability.HoldableItem != null && _plateContent == string.Empty)
            {
                if (_holdability.HoldableItem.TryGetComponent(out _foodCombo))
                {
                    if (_foodCombo.IsReadyServe)
                    {
                        if (_foodCombo.Ingrediants["burger"] && _foodCombo.Ingrediants["coke"] && _foodCombo.Ingrediants["fry"])
                        {
                            _plateContent = "burgerCokeFry";
                        }
                        else if (_foodCombo.Ingrediants["burger"] && _foodCombo.Ingrediants["coke"])
                        {
                            _plateContent = "burgerCoke";
                        }
                        else if (_foodCombo.Ingrediants["burger"] && _foodCombo.Ingrediants["fry"])
                        {
                            _plateContent = "burgerFry";
                        }
                        else if (_foodCombo.Ingrediants["burger"])
                        {
                            _plateContent = "burger";
                        }
                        else
                        {
                            _plateContent = string.Empty;
                        }
                        IsServeable = true;
                        _holdability.IsServeable = true;
                    }
                    else
                    {
                        IsServeable = false;
                        _holdability.IsServeable = true;
                    }
                }
            }
        }
        else
        {
            IsServeable = false;
            _plateContent = string.Empty;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IsServeable = false;
        _plateContent = string.Empty;
        if (_holdability != null)
        {
            _holdability.IsServeable = false;
        }
    }
}