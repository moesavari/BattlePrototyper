using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameHUD : MonoSingleton<UI_GameHUD>
{
    [Header("Troop Panel")]
    [SerializeField] private TroopButtonElement troopButtonRef;
    [SerializeField] private TroopTuning troopTuning;
    [SerializeField] private GameObject troopPanel;
    public RectTransform parentTransform;

    [Header("Troop Info Panel")]
    [SerializeField] private SelectedTroopElement troopInfoPanel;
    [SerializeField] private SelectedTroopElement troopTargetInfoPanel;

    [Header("Troop Button Elements")]
    public Button increaseTickButton;
    public Button setActionButton;
    public Button undoSelectButton;
    public Button deleteTroopButton;

    private List<TroopButtonElement> troopButtonElements = new List<TroopButtonElement>();
    private Tile currentTile;

    #region InstanceCache
    private TileController _tileCon;
    public TileController tileCon
    {
        get
        {
            if (_tileCon == null)
            {
                _tileCon = TileController.Instance;
            }
            return _tileCon;
        }
    }
    #endregion

    private void Start()
    {
        troopPanel.SetActive(false);
    }

    public void ShowTroopsPanel(Tile tile)
    {
        troopPanel.SetActive(true);
        currentTile = tile;
        SetupButtons();
    }

    public void SetupButtons()
    {
        for (int i = 0; i < troopButtonElements.Count; i++)
        {
            troopButtonElements[i].gameObject.SetActive(false);
            troopButtonElements[i].button.onClick.RemoveAllListeners();
        }

        for (int i = troopButtonElements.Count; i < troopTuning.troopList.Count; i++)
        {
            troopButtonElements.Add(Instantiate(troopButtonRef, parentTransform));
        }

        for (int i = 0; i < troopButtonElements.Count; i++)
        {
            int index = i;
            troopButtonElements[i].SetupButton(troopTuning.troopList[index].GetComponent<BaseTroop>());
            troopButtonElements[i].button.onClick.AddListener(() => Button_OnClick(troopButtonElements[index].troopType));
            troopButtonElements[i].gameObject.SetActive(true);
        }
    }

    public void Button_OnClick(BaseTroop troop)
    {
        troopPanel.SetActive(false);
        currentTile.OccupyTile(troop, currentTile.transform);
    }

    public void Button_DeleteTroop()
    {
        tileCon.DeleteTroop();
    }

    public void Button_UndoSelection()
    {
        tileCon.UndoSelection();
    }

    public void Button_SetAction()
    {
        ResetTroopInfo();
        ResetTroopTargetInfo();
        tileCon.AddAction();
    }

    public void Button_IncreaseTick()
    {
        tileCon.IncreaseTick();
    }

    public void ShowTroopInfo(BaseTroop troop)
    {
        troopInfoPanel.SetupTroopInfo(troop);
    }

    public void ResetTroopInfo()
    {
        troopInfoPanel.ResetTroopInfo();
    }

    public void ShowTroopTargetInfo(BaseTroop troop)
    {
        troopTargetInfoPanel.SetupTroopInfo(troop);
    }

    public void ResetTroopTargetInfo()
    {
        troopTargetInfoPanel.ResetTroopInfo();
    }
}
