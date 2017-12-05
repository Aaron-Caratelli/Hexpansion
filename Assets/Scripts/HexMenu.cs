using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Hexpansion
{
    public class HexMenu : MonoBehaviour
    {
        public List<Cell> cells = new List<Cell>();

        public void StartAndSpread()
        {
            var cellPos = transform.position + new Vector3(0, 1, 0);

            var allBuildingTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                   from type in assembly.GetTypes()
                                   where type.IsSubclassOf(typeof(Building))
                                   select type).ToList();


            for (int i = 0; i < allBuildingTypes.Count; ++i)
            {
                cells.Add(Instantiate(Prefabs.Get<Cell>(), cellPos, Prefabs.Get<Cell>().transform.rotation));
                cells[i].RemoveBramble();
                cells[i].SetElement<Wood>();
                cells[i].AddBuilding(allBuildingTypes[i]);
            }

            for (int i = 0; i < allBuildingTypes.Count; ++i)
            {
                float angle = (i * 60) * Mathf.Deg2Rad;

                var tweenDirection = new Vector3((float)Mathf.Sin(angle), 0, -(float)Mathf.Cos(angle));
                cells[i].GetComponent<Tween>().TweenBy(tweenDirection * 2, 0.5f);
            }
        }

        public void Clear()
        {
            foreach (var cell in cells)
                Destroy(cell.gameObject);
            cells.Clear();
        }
    }
}

