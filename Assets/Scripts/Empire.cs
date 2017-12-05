using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexpansion
{
    public class Empire
    {
        public List<Cell> ownedCells = new List<Cell>();

        public List<Cell> corruptCells = new List<Cell>();

        // A cell can be claimed if a neighbouring cell has an Expansion Tower we own.
        public bool CanClaim(Cell toClaim)
        {
            if (ownedCells.Contains(toClaim))
                return false;

            foreach (var c in toClaim.GetNeighbours(3))
            {
                if (c.building is ExpansionTower)
                    return true;
            }
            return false;
        }

        // Assume this is a "Visible" cell, now becoming "Owned".
        public void ClaimCell(Cell toClaim)
        {
            toClaim.SetState(ClaimState.Owned);
            ownedCells.Add(toClaim);
        }

        public void SetCorruptionNode(Cell toCorrupt)
        {
            toCorrupt.MakeCorruptionNode();
            corruptCells.Add(toCorrupt);
        }

        public void SpreadCorruption()
        {
            Corruption corruption;
            var newCorruptionCells = new List<Cell>();
            foreach (var c in corruptCells)
            {
                corruption = c.corruption;

                if (corruption.isSpreading)
                {
                    var newCorruption = corruption.TickFillCorruption();

                    if (newCorruption != null)
                        newCorruptionCells.Add(newCorruption);
                }                    
            }

            AddNonDuplicateCorruptionCells(newCorruptionCells);
        }

        private void AddNonDuplicateCorruptionCells(List<Cell> cells)
        {
            foreach (var c in cells)
            {
                if (!corruptCells.Contains(c))
                    corruptCells.Add(c);
            }
        }
    }
}

