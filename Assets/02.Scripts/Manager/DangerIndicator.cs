using UnityEngine;

public class DangerIndicator : MonoBehaviour
{
    public SpriteRenderer danger;
    private Sprite dangerSprite;

    public void ShowDangerIndicator(Sprite dangerSprite)
    {
        danger.sprite = dangerSprite;
        danger.enabled = true;
    }

    public void HideDangerIndicator()
    {
        danger.enabled = false;
    }

    internal void SetDangerSprite(Sprite sprite)
    {
        dangerSprite = sprite;
    }

    // 새로운 메서드 추가
    public void ShowDangerInRange(bool show)
    {
        danger.enabled = show;
    }
}