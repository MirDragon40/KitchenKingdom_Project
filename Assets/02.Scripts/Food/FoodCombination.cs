using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FoodCombination : MonoBehaviour 
{
    public int Stage = 1;
    public List<GameObject> AvailableIngrediants = new List<GameObject>(); // 스테이지별 가능한 재료조합 미리 list에 prefab으로 넣어놓음
    public List<Image> UI_FoodIcon = new List<Image>();
    public Dictionary<string, bool> Ingrediants = new Dictionary<string, bool>();
    public bool IsSubmitable;
    private IHoldable _holdableObject;
    public bool IsReadyServe = false;
    [HideInInspector]
    public PhotonView PV;
    private Character _nearbyCharacter;

    private void Start()
    {
        Init();
        PV = GetComponent<PhotonView>();
    }
    public void Init()
    {
        Stage = GameManager.Instance.Stage;
        foreach (GameObject ingrediant in AvailableIngrediants)
        {
            ingrediant.SetActive(false);
        }
        foreach (var icon in UI_FoodIcon)
        {
            icon.gameObject.SetActive(false);
        }
        switch (Stage)
        {
            case 1:
                Ingrediants["bread"] = false;
                Ingrediants["patty"] = false;
                Ingrediants["lettuce"] = false;
                Ingrediants["coke"] = false;
                Ingrediants["fry"] = false;
                Ingrediants["burger"] = false;

                break;
            case 2:
                break;
            default:
                break;
        }
    }
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsSubmitable && _holdableObject != null)
        {
            FoodObject ingrediant = null;
            PanObject panObject = null;
            if (_holdableObject.TryGetComponent<FoodObject>(out ingrediant))
            {
                 SubmitIngrediant(ingrediant);
            }
            else if (_holdableObject.TryGetComponent<PanObject>(out panObject))
            {
                if (panObject.GrillingIngrediant != null)
                {
                    SubmitIngrediant(panObject.GrillingIngrediant.GetComponent<FoodObject>());
                }
            }
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            CharacterHoldAbility holdability = other.GetComponent<CharacterHoldAbility>();
            if (holdability.HoldableItem != null)
            {
                _nearbyCharacter = other.GetComponent<Character>();
                _holdableObject = holdability.HoldableItem;
                if (holdability.SelectedDish == null)
                {
                    holdability.SelectedDish = this;
                }
                if (holdability.SelectedDish == this)
                {
                    IsSubmitable = true;
                }
                else
                {
                    IsSubmitable = false;
                }
            }
        }
        else if (!GetComponent<BoxCollider>().enabled)
        {
            IsSubmitable = false;
        }

    }
    private void OnTriggerExit(Collider other)
    {

        CharacterHoldAbility holdability = other.GetComponent<CharacterHoldAbility>();
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            holdability.SelectedDish = null;
            _nearbyCharacter = other.GetComponent<Character>();
            IsSubmitable = false;
        }
    }
    [PunRPC]
    private void SetActiveIngrediant(string key)
    {
        Ingrediants[key] = true;
    }

    private void SubmitIngrediant(FoodObject submittedFood)
    {
        if (submittedFood.ItemType == EItemType.Food && submittedFood.FoodType == FoodType.Bread && !Ingrediants["bread"])
        {
            //Ingrediants["bread"] = true;
            PV.RPC("SetActiveIngrediant", RpcTarget.All, "bread");

            PhotonNetwork.Destroy(submittedFood.gameObject);
            
            PV.RPC("RefreshPlate", RpcTarget.All);
        }
        else if (submittedFood.ItemType == EItemType.Food && submittedFood.FoodType == FoodType.Lettuce && !Ingrediants["lettuce"] && submittedFood.State == FoodState.Cut)
        {
            PV.RPC("SetActiveIngrediant", RpcTarget.All, "lettuce");
            //Ingrediants["lettuce"] = true;

            PhotonNetwork.Destroy(submittedFood.gameObject);

            PV.RPC("RefreshPlate", RpcTarget.All);
        }
        else if (submittedFood.ItemType == EItemType.Coke && !Ingrediants["coke"])
        {
            PV.RPC("SetActiveIngrediant", RpcTarget.All, "coke");
            //Ingrediants["coke"] = true;

            PhotonNetwork.Destroy(submittedFood.gameObject);

            PV.RPC("RefreshPlate", RpcTarget.All);
        }
        else if (submittedFood.ItemType == EItemType.Food && !Ingrediants["patty"] && submittedFood.State == FoodState.Grilled)
        {
            PV.RPC("SetActiveIngrediant", RpcTarget.All, "patty");
            // Ingrediants["patty"] = true;
            PhotonNetwork.Destroy(submittedFood.gameObject);

            PV.RPC("RefreshPlate", RpcTarget.All);

        }

    }
    [PunRPC]
    private void RefreshPlate()
    {
        switch (Stage)
        {
            case 1: // stage 1
                if (Ingrediants["bread"] && Ingrediants["patty"] && Ingrediants["lettuce"])
                {
                    AvailableIngrediants[0].SetActive(true);
                    AvailableIngrediants[1].SetActive(true);
                    AvailableIngrediants[2].SetActive(true);
                    AvailableIngrediants[3].SetActive(true);
                    Ingrediants["burger"] = true;

                    IsReadyServe = true;
                    UI_FoodIcon[0].gameObject.SetActive(false);
                    UI_FoodIcon[1].gameObject.SetActive(false);
                    UI_FoodIcon[2].gameObject.SetActive(false);
                    UI_FoodIcon[5].gameObject.SetActive(true);
                }
                if (!Ingrediants["burger"])
                {
                    if (Ingrediants["bread"])
                    {
                        AvailableIngrediants[0].SetActive(true);
                        AvailableIngrediants[3].SetActive(true);
                        UI_FoodIcon[0].gameObject.SetActive(true);
                    }
                    if (Ingrediants["patty"])
                    {
                        AvailableIngrediants[1].SetActive(true);
                        UI_FoodIcon[2].gameObject.SetActive(true);
                    }
                    if (Ingrediants["lettuce"])
                    {
                        AvailableIngrediants[2].SetActive(true);
                        UI_FoodIcon[1].gameObject.SetActive(true);
                    }
                }

                if (Ingrediants["coke"])
                {
                    UI_FoodIcon[3].gameObject.SetActive(true);
                    AvailableIngrediants[4].SetActive(true);
                }
                if (Ingrediants["fry"])
                {
                    UI_FoodIcon[4].gameObject.SetActive(true);
                    AvailableIngrediants[5].SetActive(true);
                }
                break;
        }

    }
}
