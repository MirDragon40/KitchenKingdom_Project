using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Option : MonoBehaviour
{
    public void OnContinueButton() 
    {
        GameManager.Instance.OptionUl.gameObject.SetActive(false);
    }
    public void OnResumeButton() 
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    public void OnControlsButton() 
    {
        GameManager.Instance.ControlImage.gameObject.SetActive(true);
    }
    public void OnQuitButton() 
    {
        Debug.Log("게임종료 버튼 클릭");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }

}
