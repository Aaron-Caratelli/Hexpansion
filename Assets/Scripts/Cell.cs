using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Hexpansion
{
    public enum HexDir
    {
        UpLeft,
        UpRight,
        Right,
        DownRight,
        DownLeft,
        Left
    }

    public class Cell : MonoBehaviour
    {
        public Dictionary<HexDir, Cell> neighbours = new Dictionary<HexDir, Cell>();

        public List<Cell> Neighbours
        {
            get { return new List<Cell>(neighbours.Values); }
        }

        public Cell Neighbour(HexDir dir)
        {
            return neighbours[dir];
        } 

        public ControlStates State = ControlStates.Locked;
        public CellTypes Type = CellTypes.Grass;

        public string Info = "This cell is boring.";        //Temporary, will display better systems 

        public void Interact(int Button)
        {
            if (State == ControlStates.Available)
            {
                State = ControlStates.Owned;
                UpdateGraphics();
            }

            if (State == ControlStates.Owned)
            {
                Debug.Log(Info);
            }
        }

        private void UpdateGraphics()
        {
            switch (State)
            {
                case ControlStates.Owned:
                    GetComponent<Image>().color = Color.green;
                    break;
                case ControlStates.Available:
                    GetComponent<Image>().color = Color.yellow;
                    break;
                case ControlStates.Locked:
                    GetComponent<Image>().color = Color.red;
                    break;
            }
        }
    }

    public enum ControlStates
    {
        Owned,
        Available,
        Locked,
        Hidden
    }

    public enum CellTypes
    {
        City,
        Town,
        Road,
        Grass,
        Forest,
        Library,
        Crypt,
        VoidPortal

    }
}