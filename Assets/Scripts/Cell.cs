using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
    
    public enum ClaimState
    {
        Hidden,
        Visible,
        Owned
    }

    public class Cell : MonoBehaviour
    {
        public ClaimState claimState = ClaimState.Hidden;
        private Vector3 _initialPos;

        public Vector3 InitialPos
        {
            get { return _initialPos; }
        }

        public Dictionary<HexDir, Cell> neighbours = new Dictionary<HexDir, Cell>();

        public List<Cell> Neighbours
        {
            get
            {
                return neighbours.Values.Where(n => n != null).ToList();
            }
        }

        public Cell Neighbour(HexDir dir)
        {
            return neighbours[dir];
        }

        private void Start()
        {
            _initialPos = transform.position;
        }
    }
}