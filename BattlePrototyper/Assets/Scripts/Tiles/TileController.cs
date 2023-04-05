using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoSingleton<TileController>
{
    public class Targets
    {
        public Tile firstTile;
        public Tile secondTile;
    }

    [Header("Tile Prefab")]
    public GameObject tilePrefab;
    public Transform parentTransform;
    public bool isLocked = false;

    [Header("Grid Controls")]
    public int columns;
    public int rows;
    public float x_Start, z_Start;
    public float x_Space, z_Space;

    private List<GameObject> tiles = new List<GameObject>();
    private Tile selectedTile;
    private Tile selectedTileTarget;

    private bool firstTileSet = false;
    private bool secondTileSet = false;

    private List<Action> actions = new List<Action>();
    private List<Targets> listOfActors = new List<Targets>();
    #region InstanceCache
    private UI_GameHUD _gameHUD;
    public UI_GameHUD gameHUD
    {
        get
        {
            if (_gameHUD == null)
            {
                _gameHUD = UI_GameHUD.Instance;
            }
            return _gameHUD;
        }
    }
    #endregion

    private void Start()
    {
        SpawnTiles();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isLocked)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Tile")
                {
                    Tile clickedTile = hit.collider.gameObject.GetComponent<Tile>();
                    
                    if (!clickedTile.isOccupied)
                    {
                        gameHUD.ShowTroopsPanel(clickedTile);
                        isLocked = true;
                    }
                    else if (!firstTileSet)
                    {
                        gameHUD.ShowTroopInfo(clickedTile.GetTroopInfo());
                        SetDeleteAndUndoButtons(true);

                        selectedTile = clickedTile;
                        firstTileSet = true;
                    }
                    else
                    {
                        gameHUD.ShowTroopTargetInfo(clickedTile.GetTroopInfo());
                        gameHUD.setActionButton.interactable = true;
                        selectedTileTarget = clickedTile;
                        secondTileSet = true;
                    }
                }
            }
        }
    }

    public void SpawnTiles()
    {
        for(int i = 0; i < columns * rows; i++)
        {
            tiles.Add(Instantiate(tilePrefab, parentTransform));
        }

        for(int i = 0; i < tiles.Count; i++)
        {
            tiles[i].name = $"Tile {i}";
            tiles[i].transform.position = new Vector3(x_Start + (x_Space * (i % columns)),
                                                      0,
                                                      z_Start + (-z_Space * (i / columns)));
        }
    }

    public void DeleteTroop()
    {
        if (selectedTileTarget != null)
        {
            selectedTileTarget.RemoveTroop();
            gameHUD.ResetTroopTargetInfo();
            selectedTileTarget = null;
            secondTileSet = false;
            gameHUD.setActionButton.interactable = true;
        }
        else if(selectedTile != null)
        {
            selectedTile.RemoveTroop();
            gameHUD.ResetTroopInfo();
            selectedTile = null;
            firstTileSet = false;

            SetDeleteAndUndoButtons(false);
        }
        else
        {
            Debug.LogError("ERROR: No troop tile found!");
        }
    }

    public void UndoSelection()
    {
        if (secondTileSet)
        {
            gameHUD.ResetTroopTargetInfo();
            selectedTileTarget = null;
            secondTileSet = false;
            gameHUD.setActionButton.interactable = true;
        }
        else if (firstTileSet)
        {
            gameHUD.ResetTroopInfo();
            selectedTile = null;
            firstTileSet = false;

            SetDeleteAndUndoButtons(false);
        }
        else
        {
            Debug.LogError("ERROR: No troop tile found!");
        }
    }

    public void SetDeleteAndUndoButtons(bool val)
    {
        gameHUD.undoSelectButton.interactable = val;
        gameHUD.deleteTroopButton.interactable = val;
    }

    public void DoAttacks()
    {
        for (int i = 0; i < listOfActors.Count; i++)
        {
            listOfActors[i].firstTile.GetTroopInfo().AttackTarget(listOfActors[i].secondTile.GetTroopInfo());
            if (listOfActors[i].secondTile.GetTroopInfo().health <= 0)
            {
                listOfActors[i].secondTile.RemoveTroop();
            }
        }
    }

    public void AddAction()
    {
        Targets targets = new Targets();
        targets.firstTile = selectedTile;
        targets.secondTile = selectedTileTarget;

        listOfActors.Add(targets);

        gameHUD.increaseTickButton.interactable = true;
        gameHUD.setActionButton.interactable = false;
        ResetSelection();
    }

    public void ResetSelection()
    {
        selectedTile = null;
        selectedTileTarget = null;

        firstTileSet = false;
        secondTileSet = false;
    }

    public void IncreaseTick()
    {
        actions.Add(() => DoAttacks());

        actions[0]();

        listOfActors.Clear();

        gameHUD.increaseTickButton.interactable = false;
    }

}
