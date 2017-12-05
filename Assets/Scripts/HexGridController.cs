using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexpansion
{
    public class HexGridController : MonoBehaviour
    {
        public enum GameState
        {
            Normal,
            PlacingBuilding
        }

        public float CellRaiseDist;
        public float CellRaiseSpeed;
        public float CellFlipSpeed;

        public GameState gameState = GameState.Normal;

        public bool isChoosingBuildingType = false;

        public HexGrid hexGrid;
        public BuildingPlacer buildingPlacer;

        private Cell _selectedTile;
        private List<Cell> _raisedTiles = new List<Cell>();

        public Empire empire;

        void Start()
        {
            empire = new Empire();
            buildingPlacer = new BuildingPlacer();

            hexGrid = FindObjectOfType<HexGrid>();
            hexGrid.Populate();

            var toFlip = hexGrid.AllCells;

            foreach (var cell in toFlip)
            {
                var eT = Random.Range(0, 4);

                if (eT == 0)
                    cell.SetElement<Rock>();
                else if (eT == 1)
                    cell.SetElement<Water>();
                else if (eT == 2)
                    cell.SetElement<Wood>();
                else if (eT == 3)
                    cell.SetElement<Fire>();
            }

            foreach (var cell in toFlip)
                cell.SetState(ClaimState.Hidden);
        }

        void StartGame()
        {
            hexGrid.MidCell.SetState(ClaimState.Visible);
            empire.ClaimCell(hexGrid.MidCell);
            empire.SetCorruptionNode(hexGrid.cells[0][0]);
        }

        void Update()
        {
            // Start game on space
            if (Input.GetKeyDown(KeyCode.Space))
                StartGame();

            if (gameState == GameState.PlacingBuilding)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    gameState = GameState.Normal;
                    buildingPlacer.Reset();
                    LowerRaisedTiles();
                }
                else if (buildingPlacer.HandleBuildingPlacement(this, _selectedTile))
                {
                    gameState = GameState.Normal;
                    empire.SpreadCorruption();
                    LowerRaisedTiles();
                    buildingPlacer.Reset();
                }                    
            }

            if (gameState == GameState.Normal)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    UpdateSelectedTile();

                    if (_selectedTile != null)
                    {
                        // What is the state of the Tile we're interacting with?
                        var claimState = _selectedTile.claimState;

                        if (claimState == ClaimState.Hidden)
                        {
                            // Do nothing -> ERROR?
                        }
                        if (claimState == ClaimState.Visible)
                        {
                            TryToClaimTile();
                        }
                        else if (claimState == ClaimState.Owned)
                        {
                            gameState = GameState.PlacingBuilding;
                        }
                    }
                }
            }            
        }

        private void TryToClaimTile()
        {
            if (_selectedTile == null)
                return;
            if (_selectedTile.claimState == ClaimState.Hidden || _selectedTile.claimState == ClaimState.Owned)
                return;

            // If it's a Visible Tile with a neighbouring Expansion Tower, we can claim it.
            if (empire.CanClaim(_selectedTile))
            {
                empire.ClaimCell(_selectedTile);
                empire.SpreadCorruption();
            }
        }

        // Project a ray from mouse position to see which tile it hit.
        // Raise the neighbours of the tile, if we clicked on one.
        private void UpdateSelectedTile()
        {
            // We're selecting a new tile, lower all previously raised tiles.
            //LowerRaisedTiles();

            RaycastHit rayHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool clickedOnTile = Physics.Raycast(ray, out rayHit, 10000.0f);

            if (clickedOnTile)
            {
                if (rayHit.transform != null)
                    _selectedTile = rayHit.transform.GetComponent<Cell>();                    
            }
        }     

        public void RaiseTile(Cell tile)
        {
            if (_raisedTiles.Contains(tile))
                return;

            tile.GetComponent<Tween>().TweenBy(new Vector3(0, CellRaiseDist, 0), CellRaiseSpeed);
            _raisedTiles.Add(tile);
        }

        public void RaiseTiles(List<Cell> tiles)
        {
            foreach (var t in tiles)
                RaiseTile(t);
        }

        public void LowerTile(Cell tile)
        {
            tile.GetComponent<Tween>().TweenTo(tile.InitialPos, CellRaiseSpeed);
            // tile.transform.position = tile.InitialPos;
            _raisedTiles.Remove(tile);
        }

        public void LowerTiles(List<Cell> tiles)
        {
            foreach (var t in tiles)
                LowerTile(t);
        }

        public void LowerRaisedTiles()
        {
            foreach (var t in _raisedTiles)
                t.GetComponent<Tween>().TweenTo(t.InitialPos, CellRaiseSpeed);
            _raisedTiles.Clear();
        }
    }
}

