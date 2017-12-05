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

        public Element element;
        public Corruption corruption;

        public void MakeCorruptionNode()
        {
            // Don't duplicate corruption.
            if (this.corruption != null)
                return;
            
            var corruption = Instantiate(Prefabs.Get<Corruption>(), transform.position + new Vector3(0, 0.65f, 0), Quaternion.identity);
            corruption.cell = this;
            this.corruption = corruption;
        }

        public void SetElement<T>() where T : Element
        {
            element = Instantiate(Prefabs.Get<T>(), transform.position, Quaternion.identity, transform);
        }

        public Vector3 InitialPos
        {
            get { return _initialPos; }
        }

        public Building building;

        public void AddBuilding<T>() where T : Building
        {
            if (building != null)
                DestroyImmediate(building.gameObject);
            building = Instantiate(Prefabs.Get<T>(), transform.position, Quaternion.identity, transform);
            building.OnAddedToTile(this);
        }

        public Dictionary<HexDir, Cell> neighbours = new Dictionary<HexDir, Cell>();

        public List<Cell> Neighbours
        {
            get
            {
                return neighbours.Values.Where(n => n != null).ToList();
            }
        }

        public List<Cell> GetNeighbours(int range)
        {
            var result = Neighbours;

            for (int i = 1; i < range; ++i)
            {
                var temp = new List<Cell>();

                foreach (var c in result)
                {
                    foreach (var neighbour in c.Neighbours)
                    {
                        if (!result.Contains(neighbour))
                            temp.Add(neighbour);
                    }
                }

                result.AddRange(temp);
            }

            return result;
        }

        public Cell Neighbour(HexDir dir)
        {
            return neighbours[dir];
        }

        public void SetState(ClaimState newState)
        {
            if (newState == ClaimState.Hidden)
            {
                MakeBramble();
                GetComponent<Tween>().Flip(10);
            }
            else if (newState == ClaimState.Visible)
            {
                GetComponent<Tween>().Flip(10);
            }
            else if (newState == ClaimState.Owned)
            {
                RemoveBramble();
            }                

            this.claimState = newState;
        }

        public void Start()
        {
            _initialPos = transform.position;
            //SetState(ClaimState.Hidden);
        }

        public void Update()
        {
            transform.position = _initialPos;

        }

        public void MakeBramble()
        {
            // Turn off mesh renderers
            foreach (var renderer in BrambleRenderers)
                renderer.enabled = true;
        }

        public void RemoveBramble()
        {
            foreach (var renderer in BrambleRenderers)
                renderer.enabled = false;
        }

        private List<MeshRenderer> BrambleRenderers
        {
            get
            {
                return transform.Find("Bramble")
                .GetComponentsInChildren<MeshRenderer>()
                .ToList();
            }
        }
    }
}