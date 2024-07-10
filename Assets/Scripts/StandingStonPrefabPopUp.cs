using UnityEngine;

public class StandingStonPrefabPopUp : MonoBehaviour
{
    StandingStone standingStone;

    public void SetStandingStoneReference(StandingStone stone)
    {
        standingStone = stone;
    }

    public void SwitchIsland()
    {
        standingStone.VeilSwitch();
        CloseMenu();
    }

    public void CloseMenu()
    {
        Destroy(gameObject);
    }
}
