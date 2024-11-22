using DG.Tweening;
using UnityEngine;
public class UIButtonAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform[] stars;
    [SerializeField] private int[] scoreThresholds = { 200, 500, 1000 };
    [SerializeField] private int playerScore = 0;
    [SerializeField] private Color unlockedColor = new Color(245 / 255f, 213 / 255f, 112 / 255f);

    void Start()
    {
        AnimateStarsBasedOnScore();
    }

    void AnimateStarsBasedOnScore()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (playerScore >= scoreThresholds[i])
            {
                UnlockStar(stars[i]);
            }
        }
    }

    void UnlockStar(RectTransform star)
    {
        var starImage = star.GetComponent<UnityEngine.UI.Image>();
        if (starImage != null)
        {
            starImage.color = unlockedColor;
        }

        star.DOScale(new Vector3(1.2f, 1.2f, 1.0f), 0.8f)
            .SetEase(Ease.OutElastic)
            .SetLoops(-1, LoopType.Yoyo);
    }
}