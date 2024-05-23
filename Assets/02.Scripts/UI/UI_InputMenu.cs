using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_InputMenu : MonoBehaviour
{
    [HideInInspector]
    public TMP_InputField InputField;
    public UI_GatheringMenu GatheringMenu;
    //public UnityEvent OnSubmit;
    public CinemachineVirtualCamera NameInputCamera;


    private void Awake()
    {
        if (InputField == null)
        {
            InputField = GetComponentInChildren<TMP_InputField>();
        }
        InputField.onSubmit.AddListener(HandleSubmit);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (InputField.isFocused)
            {
                InputField.onSubmit.Invoke(InputField.text);

            }
        }
    }

    private void HandleSubmit(string input)
    {
        PhotonNetwork.NickName = input;
        NameInputCamera.Priority = 1;
        GatheringMenu.gameObject.SetActive(true);
        this.gameObject.SetActive(false);

    }
}
