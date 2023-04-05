using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedTroopElement : MonoBehaviour
{
    public TextMeshProUGUI troopInfo;

    public void SetupTroopInfo(BaseTroop troop)
    {
        troopInfo.text = $"Name: {troop.troopName}\n" +
                         $"Health: {troop.health}\n" +
                         $"Attack: {troop.attack}\n" +
                         $"Range: {troop.range}";
    }

    public void ResetTroopInfo()
    {
        troopInfo.text = $"Name: \n" +
                         $"Health: \n" +
                         $"Attack: \n" +
                         $"Range: ";
    }
}
