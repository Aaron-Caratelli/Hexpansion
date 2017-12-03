using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexpansion
{
    public class Empire
    {
        public List<Cell> ownedCells = new List<Cell>();

        // A cell can be claimed if it borders any cell the Empire owns.
        // Ask each cell if it's neighbours contain the cell.
        // If one of them does, the cell can be claimed.
        public bool CanClaim(Cell toClaim)
        {
            foreach (var c in ownedCells)
            {
                if (c.Neighbours.Contains(toClaim))
                    return true;
            }
            return false;
        }
    }
}

