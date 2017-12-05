using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexpansion
{
    public class HexGridController : MonoBehaviour
    {
        public float CellRaiseDist;
        public float CellRaiseSpeed;
        public float CellFlipSpeed;

        public bool isChoosingBuildingType = false;

        public HexGrid hexGrid;

        private Cell _selectedTile;
        private List<Cell> _raisedTiles = new List<Cell>();

        public Empire empire;

        void Start()
        {
            empire = new Empire();

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

            if (isChoosingBuildingType)
                HandleBuildingPlacement();

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
                        isChoosingBuildingType = true;
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
            LowerRaisedTiles();

            RaycastHit rayHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool clickedOnTile = Physics.Raycast(ray, out rayHit, 10000.0f);

            if (clickedOnTile)
            {
                if (rayHit.transform != null)
                    _selectedTile = rayHit.transform.GetComponent<Cell>();                    
            }
        }

        private void HandleBuildingPlacement()
        {
            if (_selectedTile == null)
                return;
            if (_selectedTile.building != null)
                return;
            if (!ValidBuildingPlacementKeyPressed())
                return;

            empire.SpreadCorruption();
            isChoosingBuildingType = false;

            if (Input.GetKeyDown(KeyCode.Alpha1))
                _selectedTile.AddBuilding<ExpansionTower>();
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                _selectedTile.AddBuilding<Cleanser>();
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                _selectedTile.AddBuilding<Enricher>();
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                _selectedTile.AddBuilding<Foundation>();
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                _selectedTile.AddBuilding<Spire>();
        }

        private bool ValidBuildingPlacementKeyPressed()
        {
            return Input.GetKeyDown(KeyCode.Alpha1)
                || Input.GetKeyDown(KeyCode.Alpha2)
                || Input.GetKeyDown(KeyCode.Alpha3)
                || Input.GetKeyDown(KeyCode.Alpha4)
                || Input.GetKeyDown(KeyCode.Alpha5);
        }

        private void RaiseTile(Cell tile)
        {
            if (tile.transform.position != tile.InitialPos)
                return;

            tile.GetComponent<Tween>().TweenBy(new Vector3(0, CellRaiseDist, 0), CellRaiseSpeed);
            //var toCamera = tile.transform.position - Camera.main.transform.position;
            //var translation = Vector3.Normalize(toCamera) * CellRaiseDist;
            //tile.transform.Translate(translation);


            //tile.transform.Translate(new Vector3(0, CellRaiseDist, 0));
            _raisedTiles.Add(tile);
        }

        private void RaiseTiles(List<Cell> tiles)
        {
            foreach (var t in tiles)
                RaiseTile(t);
        }

        private void LowerTile(Cell tile)
        {
            tile.GetComponent<Tween>().TweenTo(tile.InitialPos, CellRaiseSpeed);
            // tile.transform.position = tile.InitialPos;
            _raisedTiles.Remove(tile);
        }

        private void LowerTiles(List<Cell> tiles)
        {
            foreach (var t in tiles)
                LowerTile(t);
        }

        private void LowerRaisedTiles()
        {
            foreach (var t in _raisedTiles)
                t.GetComponent<Tween>().TweenTo(t.InitialPos, CellRaiseSpeed);
            _raisedTiles.Clear();
        }
    }
}

