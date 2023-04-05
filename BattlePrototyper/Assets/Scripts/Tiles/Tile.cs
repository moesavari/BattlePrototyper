using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private BaseTroop currentTroop;
    private GameObject troopObject;
    public bool isOccupied = false;

    public void OccupyTile(BaseTroop troop, Transform transform)
    {
        currentTroop = Instantiate(troop, transform);
        troopObject = currentTroop.gameObject;

        isOccupied = true;
        TileController.Instance.isLocked = false;
    }

    public void RemoveTroop()
    {
        Destroy(troopObject);

        currentTroop = null;
        isOccupied = false;
    }

    public BaseTroop GetTroopInfo()
    {
        return currentTroop;
    }
}
