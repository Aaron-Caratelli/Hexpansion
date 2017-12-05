using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexpansion
{
    public class BuildingPlacer
    {
        public enum PlacingState
        {
            SelectBuildingType
        }

        public PlacingState state = PlacingState.SelectBuildingType;

        public Cell buildingCell;
        public HexMenu hexMenu;

        public void Reset()
        {
            buildingCell = null;
            state = PlacingState.SelectBuildingType;

            if (hexMenu != null)
                hexMenu.Clear();
        }

        public bool HandleBuildingPlacement(HexGridController hex, Cell buildOn)
        {
            // If starting selection with a new cell, populate Hex Menu
            if (buildingCell != buildOn)
            {
                Reset();
                buildingCell = buildOn;
                var toCamera = Vector3.Normalize(Camera.main.transform.position - buildingCell.transform.position);
                hexMenu = Object.Instantiate(Prefabs.Get<HexMenu>(), buildOn.transform.position, Quaternion.identity);

                hexMenu.StartAndSpread();
                hex.RaiseTile(buildingCell);
            }

            if (state == PlacingState.SelectBuildingType)
            {
                var selection = GetBuildingSelection();

                if (selection != null)
                {
                    buildingCell.AddBuilding(selection.GetType());
                    hexMenu.Clear();
                    return true;
                }
            }

            return false;
        }

        private Building GetBuildingSelection ()
        {
            // If Mouse Down and Ray cast from Mouse
            if (!Input.GetMouseButtonDown(0))
                return null;

            RaycastHit rayHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out rayHit, 10000.0f))
            {
                var selection = rayHit.transform.GetComponent<Cell>();

                if (hexMenu.cells.Contains(selection))
                    return selection.building;
            }
            return null;
        }
    }
}

