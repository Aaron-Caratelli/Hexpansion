using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexpansion
{
    public class ExpansionTower : Building
    {
        // Turn cells over in 3 radius.
        // Claim and remove bramble on cells in 2 radius
        public override void OnAddedToTile(Cell cell)
        {
            foreach (var neighbour in cell.GetNeighbours(3))
            {
                if (neighbour == cell) continue;

                if (neighbour.claimState == ClaimState.Hidden)
                    neighbour.SetState(ClaimState.Visible);
            }

            foreach (var neighbour in cell.GetNeighbours(2))
            {
                if (neighbour == cell) continue;
                neighbour.claimState = ClaimState.Owned;
                neighbour.RemoveBramble();
            }
        }
    }
}

