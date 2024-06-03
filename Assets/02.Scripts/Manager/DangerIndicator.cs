using UnityEngine;

public class DangerIndicator : MonoBehaviour
{
    public SpriteRenderer danger;

    // 표시창을 보여주는 메서드
    public void ShowDangerIndicator(Sprite dangerSprite)
    {
        danger.sprite = dangerSprite; 
        danger.enabled = true; 
    }

    // 표시창을 숨기는 메서드
    public void HideDangerIndicator()
    {
        danger.enabled = false; 
    }
}