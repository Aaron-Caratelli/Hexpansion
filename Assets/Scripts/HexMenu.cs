using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hexpansion
{
    public class HexMenu : MonoBehaviour, IEnumerable<HexMenuCell>
    {
        public List<List<HexMenuCell>> cells = new List<List<HexMenuCell>>();

        public int Cols
        {
            get { return cells.Count; }
        }

        public int Rows
        {
            get
            {
                if (cells.Count == 0)
                    return 0;
                return cells.Last().Count;
            }
        }

        private bool NextColIsOffset
        {
            get
            {
                if (cells.Count == 0)
                    return false;
                return cells.Count % 2 == 1;
            }
        }

        public float HexSize = 2.4f;

        public float HexWidth
        {
            get { return HexSize * 0.75f; }
        }

        public float HexHeight
        {
            get { return HexSize * 0.866f; }
        }

        public void Populate()
        {
            Clear();
            AddColumn();
            AddColumn();
            AddColumn();
            AddRow();
            AddRow();
            AddRow();

            foreach (var cell in this)
                cell.transform.parent = transform;
            transform.rotation = Quaternion.Euler(-20, 0, 0);
        }

        public void Clear()
        {
            foreach (var hex in this)
                Object.Destroy(hex.gameObject);
            cells.Clear();
        }

        public void AddColumn()
        {
            var newCol = new List<HexMenuCell>();

            for (int row = 0; row < Rows; ++row)
            {
                float x = transform.position.x + Cols * HexWidth;
                float y = transform.position.y;
                float z = transform.position.z + row * HexHeight;

                if (NextColIsOffset)
                    z += (HexHeight / 2);

                newCol.Add(AddCellAt(new Vector3(x, y, z)));
            }

            cells.Add(newCol);
        }

        public void AddRow()
        {
            for (int col = 0; col < Cols; ++col)
            {
                float x = transform.position.x + col * HexWidth;
                float y = transform.position.y;
                float z = transform.position.z + Rows * HexHeight;

                if (col % 2 == 1)
                    z += (HexHeight / 2);

                cells[col].Add(AddCellAt(new Vector3(x, y, z)));
            }
        }

        private HexMenuCell AddCellAt(Vector3 pos)
        {
            var newCell = Object.Instantiate(Prefabs.Get<HexMenuCell>(), pos, Quaternion.Euler(90, 0, 0));

            var offset = Vector3.zero;
            //offset.x = (HexSize * 1.5f) * -1;
            //offset.y = 0.2f;
            //offset.z = 0f;
            offset.x = (HexSize * 1.5f) * -1;
            offset.y = 0.5f;
            offset.z = 0;

            newCell.transform.Translate(offset);

            return newCell;
        }

        public IEnumerator<HexMenuCell> GetEnumerator()
        {
            for (int col = 0; col < Cols; ++col)
            {
                for (int row = 0; row < Rows; ++row)
                    yield return cells[col][row];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

