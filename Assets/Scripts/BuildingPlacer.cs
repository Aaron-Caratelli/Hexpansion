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
                hexMenu.Populate();
                hex.RaiseTile(buildingCell);
            }


            if (state == PlacingState.SelectBuildingType)
            {
                if (!ValidBuildingPlacementKeyPressed())
                    return false;

                if (Input.GetKeyDown(KeyCode.Alpha1))
                    buildingCell.AddBuilding<ExpansionTower>();
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                    buildingCell.AddBuilding<Cleanser>();
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                    buildingCell.AddBuilding<Enricher>();
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                    buildingCell.AddBuilding<Foundation>();
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                    buildingCell.AddBuilding<Spire>();

                hexMenu.Clear();
                return true;
            }
            return false;
        }

        private bool ValidBuildingPlacementKeyPressed()
        {
            return Input.GetKeyDown(KeyCode.Alpha1)
                || Input.GetKeyDown(KeyCode.Alpha2)
                || Input.GetKeyDown(KeyCode.Alpha3)
                || Input.GetKeyDown(KeyCode.Alpha4)
                || Input.GetKeyDown(KeyCode.Alpha5);
        }
    }
}

