using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexpansion
{
    public class Corruption : MonoBehaviour
    {
        public float corruptionLevel;

        public float fillRate;
        public float fillThreshold;

        public bool isSpreading;

        public Cell cell;

        public List<CorruptionOrb> orbs = new List<CorruptionOrb>();



        // Use this for initialization
        void Start()
        {
            isSpreading = true;
            corruptionLevel = 1;
        }

        // Update is called once per frame
        void Update()
        {
        }

        public Cell TickFillCorruption()
        {
            if (isSpreading)
            {
                corruptionLevel += fillRate;
                Vector3 newOrbPos = transform.position + new Vector3(0, 0.15f + (orbs.Count * 0.15f), 0);
                var newOrb = Instantiate(Prefabs.Get<CorruptionOrb>(), newOrbPos, Quaternion.identity);
                orbs.Add(newOrb);

                if (corruptionLevel >= fillThreshold)
                {
                    isSpreading = false;
                    return SpreadCorruptionToNeighbours(cell);
                }
            }
            return null;
        }

        public Cell SpreadCorruptionToNeighbours(Cell parent)
        {
            // Try to spread randomly.
            var newCorruption = parent.Neighbours[Random.Range(0, parent.Neighbours.Count - 1)];
            if (newCorruption.corruption == null)
            {
                newCorruption.MakeCorruptionNode();
                return newCorruption;
            }

            // If random is already corruption.....
            // Find first neighbour not already corrupted -> spread to that.
            foreach (var c in parent.Neighbours)
            {
                if (c.corruption == null)
                {
                    c.MakeCorruptionNode();
                    return c;
                }
            }
            return null;
        }
    }
}
