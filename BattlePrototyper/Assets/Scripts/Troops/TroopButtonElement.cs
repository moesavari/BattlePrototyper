using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TroopButtonElement : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI nameText;
    public BaseTroop troopType;

    public void SetupButton(BaseTroop troop)
    {
        nameText.text = troop.troopName;
        troopType = troop;
    }
}
