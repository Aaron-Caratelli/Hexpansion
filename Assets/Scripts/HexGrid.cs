using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Hexpansion
{
    public class HexGrid : MonoBehaviour
    {
        public List<List<Cell>> cells;

        public int NumCols;
        public int NumRows;
        public int HexSize;

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

        // List count after adding determines offset
        // Even -> Next Row = Not Offset
        // Odd -> Next Row = Offset
        private bool NextRowIsOffset
        {
            get
            {
                if (cells.Count == 0)
                    return false;
                return cells.Last().Count % 2 == 1; // Odd
            }
        }

        // Use this for initialization
        void Start()
        {
            cells = new List<List<Cell>>();
            HexSize = 1;

            for (int i = 0; i < 10; ++i)
            {
                AddColumn();
                AddRow();
            }
        }

        public void AddColumn()
        {            
            var newCol = new List<Cell>();

            for (int row = 0; row < Rows; ++row)
            {
                float x = transform.position.x + Cols * HexSize;
                float y = transform.position.y;
                float z = transform.position.z + row * HexSize;

                if (row % 2 == 1) // Odd rows are offset 
                    x += (HexSize / 2);

                newCol.Add(AddCellAt(new Vector3(x, y, z)));
            }

            cells.Add(newCol);
        }

        public void AddRow()
        {
            for (int col = 0; col < Cols; ++col)
            {
                float x = transform.position.x + col * HexSize;
                float y = transform.position.y;
                float z = transform.position.z + Rows * HexSize;

                if (NextRowIsOffset)
                    x += (HexSize / 2);

                cells[col].Add(AddCellAt(new Vector3(x, y, z)));
            }
        }

        private Cell AddCellAt(Vector3 pos)
        {
            var newCell = Instantiate(Prefabs.Get<Cell>(), pos, Prefabs.Get<Cell>().transform.rotation);
            newCell.transform.parent = transform;
            return newCell;
        }
    }

}
