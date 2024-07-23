using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeasonUI : MonoBehaviour
{
    [SerializeField] Image seasonIcon;
    [SerializeField] TextMeshProUGUI seasonText;

    [Space(5)]
    [Header("Season Icons")]
    [SerializeField] Sprite winterIcon;
    [SerializeField] Sprite springIcon;
    [SerializeField] Sprite summerIcon;
    [SerializeField] Sprite autumnIcon;

    private void Start()
    {
        UpdateSeasonDisplay();
        TimeSystem.AddMonthlyEvent(this, UpdateSeasonDisplay);
    }

    public void UpdateSeasonDisplay()
    {
        Season currentSeason = TimeSystem.GetCurrentSeason();

        switch (currentSeason)
        {
            case Season.Winter:
                seasonIcon.sprite = winterIcon;
                seasonText.text = "Winter";
                break;
            case Season.Spring:
                seasonIcon.sprite = springIcon;
                seasonText.text = "Spring";
                break;
            case Season.Summer:
                seasonIcon.sprite = summerIcon;
                seasonText.text = "Summer";
                break;
            case Season.Autumn:
                seasonIcon.sprite = autumnIcon;
                seasonText.text = "Autumn";
                break;
        }
    }
}
