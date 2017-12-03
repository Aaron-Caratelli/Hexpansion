using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexpansion
{
    public class HexGridController : MonoBehaviour
    {
        public float CellRaiseDist;
        public float CellRaiseSpeed;

        public HexGrid hexGrid;

        private Cell _selectedTile;
        private List<Cell> _raisedTiles = new List<Cell>();

        void Start()
        {
            hexGrid = FindObjectOfType<HexGrid>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                HandleTileSelection();
        }

        // Project a ray from mouse position to see which tile it hit.
        // Raise the neighbours of the tile, if we clicked on one.
        private void HandleTileSelection()
        {
            // We're selecting a new tile, lower all previously raised tiles.
            LowerRaisedTiles();

            RaycastHit rayHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool clickedOnTile = Physics.Raycast(ray, out rayHit, 10000.0f);

            if (clickedOnTile)
            {
                if (rayHit.transform != null)
                {
                    _selectedTile = rayHit.transform.GetComponent<Cell>();

                    // Prevent duplicate Tweening
                    if (_selectedTile != null && !_raisedTiles.Contains(_selectedTile))
                        RaiseTile(_selectedTile);
                }
            }
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

