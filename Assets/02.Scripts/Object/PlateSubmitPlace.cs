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
        { "burger", 15 },
        { "burgerFry", 40 },
        { "cheeseBurger", 15 },
        { "cheeseBurgerCoke", 30 },
        { "cheeseBurgerCokeFry", 50 },
        { "cheeseBurgerFry", 40 },
        { "tomatoBurger", 15 },
        { "tomatoBurgerCoke", 30 },
        { "tomatoBurgerCokeFry", 50 },
        { "tomatoBurgerFry", 40 },
        { "chickenCokeFry", 40 },
        { "chickenCoke", 20 },
        { "chicken", 15 },
        { "cokeFry", 30 },
        { "fry", 15 },
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
                SoundManager.Instance.PlayAudio("Reappearance", false, true);
            }

            if (_foodCombo != null)
            {
                if (_foodCombo.PV.IsMine)
                {
                    PhotonNetwork.Destroy(_foodCombo.gameObject);
                }
            }
        }
        else
        {
            // 잘못된 음식 제출 시 사운드 재생
            SoundManager.Instance.PlayAudio("Wrong", false, true);
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
                        _plateContent = string.Empty;
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
                        
                        if (_foodCombo.Ingrediants["tomatoBurger"] && _foodCombo.Ingrediants["coke"] && _foodCombo.Ingrediants["fry"])
                        {
                            _plateContent = "tomatoBurgerCokeFry";
                        }
                        else if (_foodCombo.Ingrediants["tomatoBurger"] && _foodCombo.Ingrediants["coke"])
                        {
                            _plateContent = "tomatoBurgerCoke";
                        }
                        else if (_foodCombo.Ingrediants["tomatoBurger"] && _foodCombo.Ingrediants["fry"])
                        {
                            _plateContent = "tomatoBurgerFry";
                        }
                        else if (_foodCombo.Ingrediants["tomatoBurger"])
                        {
                            _plateContent = "tomatoBurger";
                        }  
                        
                        if (_foodCombo.Ingrediants["cheeseBurger"] && _foodCombo.Ingrediants["coke"] && _foodCombo.Ingrediants["fry"])
                        {
                            _plateContent = "cheeseBurgerCokeFry";
                        }
                        else if (_foodCombo.Ingrediants["cheeseBurger"] && _foodCombo.Ingrediants["coke"])
                        {
                            _plateContent = "cheeseBurgerCoke";
                        }
                        else if (_foodCombo.Ingrediants["cheeseBurger"] && _foodCombo.Ingrediants["fry"])
                        {
                            _plateContent = "cheeseBurgerFry";
                        }
                        else if (_foodCombo.Ingrediants["cheeseBurger"])
                        {
                            _plateContent = "cheeseBurger";
                        }               
                        
                        if (_foodCombo.Ingrediants["chicken"] && _foodCombo.Ingrediants["coke"] && _foodCombo.Ingrediants["fry"])
                        {
                            _plateContent = "chickenCokeFry";
                        }
                        else if (_foodCombo.Ingrediants["chicken"] && _foodCombo.Ingrediants["coke"])
                        {
                            _plateContent = "chickenCoke";
                        }
                        else if (_foodCombo.Ingrediants["chicken"])
                        {
                            _plateContent = "chicken";
                        }

                        if (_foodCombo.Ingrediants["burger"] && _foodCombo.Ingrediants["tomatoBurger"] && _foodCombo.Ingrediants["cheeseBurger"] && !_foodCombo.Ingrediants["chicken"])
                        {
                            if (_foodCombo.Ingrediants["coke"] && _foodCombo.Ingrediants["fry"])
                            {
                                _plateContent = "cokeFry";
                            }
                            else if (_foodCombo.Ingrediants["fry"])
                            {
                                _plateContent = "fry";
                            }
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