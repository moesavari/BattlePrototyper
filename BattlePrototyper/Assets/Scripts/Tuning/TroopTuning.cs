using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TroopTuning", menuName = "Tuning/TroopTuning", order = 1)]
public class TroopTuning : ScriptableObject
{
    public List<GameObject> troopList;
}
