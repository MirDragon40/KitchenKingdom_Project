using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalResultSceneManager : MonoBehaviour
{
    public GameObject StarFill1;
    public GameObject StarFill2;
    public GameObject StarFill3;
    public GameObject StarFill4;
    public GameObject StarFill5;

    private void Awake()
    {
        StarFill1.SetActive(false);
        StarFill2.SetActive(false);
        StarFill3.SetActive(false);
        StarFill4.SetActive(false);
        StarFill5.SetActive(false);
    }

    private void Start()
    {
        DisplayStars(GameManager.Instance.TotalScore);
    }

    public void DisplayStars(int score)
    {
        int starCount = 0;

        if (score > 0 && score <= 150)
            starCount = 1;
        else if (score > 150 && score <= 250)
            starCount = 2;
        else if (score > 250 && score <= 350)
            starCount = 3;
        else if (score > 350 && score <= 450)
            starCount = 4;
        else if (score > 450)
            starCount = 5;

        StartCoroutine(ActivateStars(starCount));
    }

    private IEnumerator ActivateStars(int count)
    {
        GameObject[] stars = { StarFill1, StarFill2, StarFill3, StarFill4, StarFill5 };

        for (int i = 0; i < count; i++)
        {
            stars[i].SetActive(true);
            Vector3 originalScale = stars[i].transform.localScale;
            stars[i].transform.localScale = Vector3.zero;

            stars[i].transform.DOScale(originalScale * 1.2f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                stars[i].transform.DOScale(originalScale, 0.5f).SetEase(Ease.InBounce);
            });

            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene("EndingScene");
    }
}
