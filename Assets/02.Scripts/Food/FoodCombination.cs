using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FoodCombination : MonoBehaviour 
{
    public int Stage => GameManager.Instance.CurrentStage;
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
        foreach (GameObject ingrediant in AvailableIngrediants)
        {
            ingrediant.SetActive(false);
        }
        foreach (var icon in UI_FoodIcon)
        {
            icon.gameObject.SetActive(false);
        }

        Ingrediants["bread"] = false;
        Ingrediants["patty"] = false;
        Ingrediants["lettuce"] = false;
        Ingrediants["tomato"] = false;
        Ingrediants["cheese"] = false;
        Ingrediants["chicken"] = false;
        Ingrediants["coke"] = false;
        Ingrediants["fry"] = false;
        Ingrediants["burger"] = false;
        Ingrediants["tomatoBurger"] = false;
        Ingrediants["cheeseBurger"] = false;



    }
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsSubmitable && _holdableObject != null)
        {
            FoodObject ingrediant = null;
            PanObject panObject = null;
            BasketObject basketObject = null;
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
            else if (_holdableObject.TryGetComponent<BasketObject>(out basketObject))
            {
                if (basketObject.FryingIngrediant != null)
                {
                    SubmitIngrediant(basketObject.FryingIngrediant.GetComponent<FoodObject>());
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
        if (key == "cheese" || key == "tomato" || key == "lettuce") // 3가지 재료들은 서로 겹쳐서 존재할수 없음
        {
            Ingrediants["cheese"] = false;
            Ingrediants["tomato"] = false;
            Ingrediants["lettuce"] = false;
        }
        Ingrediants[key] = true;
    }

    private void SubmitIngrediant(FoodObject submittedFood)
    {
        if (submittedFood.ItemType == EItemType.Food && submittedFood.FoodType == FoodType.Bread && !Ingrediants["bread"])
        {
            PV.RPC("SetActiveIngrediant", RpcTarget.All, "bread");

            PhotonNetwork.Destroy(submittedFood.gameObject);
            
            PV.RPC("RefreshPlate", RpcTarget.All);
        }
        else if (submittedFood.ItemType == EItemType.Food && submittedFood.FoodType == FoodType.Lettuce && !Ingrediants["lettuce"] && submittedFood.State == FoodState.Cut)
        {
            PV.RPC("SetActiveIngrediant", RpcTarget.All, "lettuce");

            PhotonNetwork.Destroy(submittedFood.gameObject);

            PV.RPC("RefreshPlate", RpcTarget.All);
        }
        else if (submittedFood.ItemType == EItemType.Coke && !Ingrediants["coke"])
        {
            PV.RPC("SetActiveIngrediant", RpcTarget.All, "coke");

            PhotonNetwork.Destroy(submittedFood.gameObject);

            PV.RPC("RefreshPlate", RpcTarget.All);
        }
        else if (submittedFood.ItemType == EItemType.Food && submittedFood.FoodType == FoodType.Patty && !Ingrediants["patty"] && submittedFood.State == FoodState.Grilled)
        {
            PV.RPC("SetActiveIngrediant", RpcTarget.All, "patty");
            // Ingrediants["patty"] = true;
            PhotonNetwork.Destroy(submittedFood.gameObject);

            PV.RPC("RefreshPlate", RpcTarget.All);
        }
        else if (submittedFood.ItemType == EItemType.Food &&  submittedFood.FoodType == FoodType.Potato && !Ingrediants["fry"] && submittedFood.State == FoodState.Fried)
        {
            PV.RPC("SetActiveIngrediant", RpcTarget.All, "fry");
            PhotonNetwork.Destroy(submittedFood.gameObject);

            PV.RPC("RefreshPlate", RpcTarget.All);
        }
        else if (submittedFood.ItemType == EItemType.Food && submittedFood.FoodType == FoodType.Tomato && !Ingrediants["tomato"] && submittedFood.State == FoodState.Cut)
        {
            PV.RPC("SetActiveIngrediant", RpcTarget.All, "tomato");
            PhotonNetwork.Destroy(submittedFood.gameObject);

            PV.RPC("RefreshPlate", RpcTarget.All);
        }       
        else if (submittedFood.ItemType == EItemType.Food && submittedFood.FoodType == FoodType.Cheese && !Ingrediants["cheese"])
        {
            PV.RPC("SetActiveIngrediant", RpcTarget.All, "cheese");
            PhotonNetwork.Destroy(submittedFood.gameObject);
            PV.RPC("RefreshPlate", RpcTarget.All);
        }
        else if (submittedFood.ItemType == EItemType.Food && submittedFood.FoodType == FoodType.Chicken && !Ingrediants["chicken"] && submittedFood.State == FoodState.Fried)
        {
            PV.RPC("SetActiveIngrediant", RpcTarget.All, "chicken");
            PhotonNetwork.Destroy(submittedFood.gameObject);
            PV.RPC("RefreshPlate", RpcTarget.All);
        }

    }
    [PunRPC]
    private void RefreshPlate()
    {
        foreach (var ingrediant in AvailableIngrediants) // refresh할때 초기에 전부다 끔
        {
            ingrediant.SetActive(false);
        }
        foreach (var icon in UI_FoodIcon) // 아이콘도 전부다끔
        {
            icon.gameObject.SetActive(false);
        }

        if (Ingrediants["bread"] && Ingrediants["patty"] && (Ingrediants["lettuce"] || Ingrediants["tomato"])
        {
            AvailableIngrediants[0].SetActive(true); // 0 빵 1 패티 2 양상추 3 빵아래 4 콜라 5 감튀 6 토마토 7 치즈 8 치킨 
            AvailableIngrediants[1].SetActive(true);
            AvailableIngrediants[2].SetActive(true);
            AvailableIngrediants[3].SetActive(true);
            Ingrediants["burger"] = true;

            IsReadyServe = true;
            UI_FoodIcon[0].gameObject.SetActive(false); // 0 빵 1 양상추 2 패티 3 콜라 4 감튀 5 버거 6 토마토 7 치즈 8 치킨
            UI_FoodIcon[1].gameObject.SetActive(false);
            UI_FoodIcon[2].gameObject.SetActive(false);
            UI_FoodIcon[5].gameObject.SetActive(true);
        }

        if (!Ingrediants["burger"])
        {
            if (Ingrediants["bread"])
            {
                AvailableIngrediants[0].SetActive(true); // 빵 윗쪽
                AvailableIngrediants[3].SetActive(true); // 빵 아래쪽
                UI_FoodIcon[0].gameObject.SetActive(true);
            }
            if (Ingrediants["patty"])
            {
                AvailableIngrediants[1].SetActive(true); // 패티
                UI_FoodIcon[2].gameObject.SetActive(true); 
            }
            if (Ingrediants["lettuce"])
            {
                AvailableIngrediants[2].SetActive(true);
                UI_FoodIcon[1].gameObject.SetActive(true);
            }
        }
        if (!Ingrediants["tomatoBurger"])
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
            if (Ingrediants["tomato"])
            {
                AvailableIngrediants[6].SetActive(true); // *****
                UI_FoodIcon[6].gameObject.SetActive(true); // ****
            }
        }
        if (!Ingrediants["cheeseBurger"])
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
            if (Ingrediants["cheese"])
            {
                AvailableIngrediants[7].SetActive(true); // *****
                UI_FoodIcon[7].gameObject.SetActive(true); // ****
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
        if (Ingrediants["chicken"])
        {
            UI_FoodIcon[8].gameObject.SetActive(true);
            AvailableIngrediants[8].SetActive(true);
        }



    }
}
