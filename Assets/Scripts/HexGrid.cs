using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Hexpansion
{
    public class HexGrid : MonoBehaviour, IEnumerable<Cell>
    {
        public List<List<Cell>> cells;

        public List<Cell> AllCells
        {
            get
            {
                var result = new List<Cell>();

                foreach (var col in cells)
                    result.AddRange(col);

                return result;
            }
        }

        public int NumCols;
        public int NumRows;
        public float HexSize;

        public float HexWidth
        {
            get { return HexSize * 0.866f; }
        }

        public float HexHeight
        {
            get { return HexSize * 0.75f; }
        }

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

        public void Populate()
        {
            cells = new List<List<Cell>>();

            for (int i = 0; i < NumCols; ++i)
                AddColumn();
            for (int i = 0; i < NumRows; ++i)
                AddRow();

            for (int col = 0; col < NumCols; ++col)
            {
                for (int row = 0; row < NumRows; ++row)
                {
                    bool isOffsetRow = row % 2 == 1;
                    CalculateCellNeighbours(col, row, isOffsetRow);
                }
            }
        }

        private void CalculateCellNeighbours(int col, int row, bool isOffsetRow)
        {
            var c = cells[col][row];
            var neighbours = new Dictionary<HexDir, Cell>();

            if (!isOffsetRow)
            {
                neighbours.Add(HexDir.UpLeft,       ByIndex(col - 1, row - 1));
                neighbours.Add(HexDir.UpRight,      ByIndex(col, row - 1));
                neighbours.Add(HexDir.Left,         ByIndex(col - 1, row));
                neighbours.Add(HexDir.Right,        ByIndex(col + 1, row));
                neighbours.Add(HexDir.DownLeft,     ByIndex(col - 1, row + 1));
                neighbours.Add(HexDir.DownRight,    ByIndex(col, row + 1));
            }
            else
            {
                neighbours.Add(HexDir.UpLeft,       ByIndex(col, row - 1));
                neighbours.Add(HexDir.UpRight,      ByIndex(col + 1, row - 1));
                neighbours.Add(HexDir.Left,         ByIndex(col - 1, row));
                neighbours.Add(HexDir.Right,        ByIndex(col + 1, row));
                neighbours.Add(HexDir.DownLeft,     ByIndex(col, row + 1));
                neighbours.Add(HexDir.DownRight,    ByIndex(col + 1, row + 1));
            }

            c.neighbours = neighbours;
        }

        public Cell MidCell
        {
            get
            {
                if (Cols == 0 || Rows == 0)
                    return null;

                int midCol = System.Convert.ToInt32(Cols / 2);
                int midRow = System.Convert.ToInt32(Rows / 2);

                return cells[midCol][midRow];
            }
        }

        public Cell ByIndex(int col, int row)
        {
            if (col < 0 || col > Cols - 1 || row < 0 || row > Rows - 1)
                return null;
            return cells[col][row];
        }

        public void AddColumn()
        {            
            var newCol = new List<Cell>();

            for (int row = 0; row < Rows; ++row)
            {
                float x = transform.position.x + Cols * HexWidth;
                float y = transform.position.y;
                float z = transform.position.z + row * HexHeight;

                if (row % 2 == 1) // Odd rows are offset 
                    x += (HexWidth / 2);

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

                if (NextRowIsOffset)
                    x += (HexWidth / 2);

                cells[col].Add(AddCellAt(new Vector3(x, y, z)));
            }
        }

        private Cell AddCellAt(Vector3 pos)
        {
            var newCell = Instantiate(Prefabs.Get<Cell>(), pos, Prefabs.Get<Cell>().transform.rotation);
            newCell.transform.parent = transform;
            return newCell;
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            for (int col = 0; col < Cols; ++col)
            {
                for (int row = 0; row < Rows; ++row)
                    yield return cells[col][row];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

}
