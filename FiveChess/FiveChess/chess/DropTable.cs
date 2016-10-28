using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    public class DropTable
    {
        public DropTable()
        {
            _data = new List<Position>();
            //_Table = new Position[Side.COL * Side.ROW];
            //for (int i = 0; i < _Table.Length; i++)
            //{
            //    _Table[i] = new Position();
            //}
        }

        public void clear()
        {
            _data.Clear();
            //for (int r = 0; r <= Side.ROW_ID; r++)
            //{
            //    for (int c = 0; c <= Side.COL_ID; c++)
            //    {
            //        _Table[r * Side.COL + c].row = 0;
            //        _Table[r * Side.COL + c].col = 0;
            //        _Table[r * Side.COL + c].val = 0;
            //        //Logs.write("(" + _Table[r * Side.COL + c].row + " " + _Table[r * Side.COL + c].col + ")" + _Table[r * Side.COL + c].val + ",\t\t\t", 4);
            //    }
            //    //Logs.writeln("", 4);
            //}
        }

        //public void sort()
        //{
        //    Position.sort(_Table);
        //    for (int r = 0; r <= Side.ROW_ID; r++)
        //    {
        //        for (int c = 0; c <= Side.COL_ID; c++)
        //        {
        //            Logs.write("(" + _Table[r * Side.COL + c].row + " " + _Table[r * Side.COL + c].col + ")" + _Table[r * Side.COL + c].val + ",\t\t\t", 4);
        //        }
        //        Logs.writeln("", 4);
        //    }
        //}

        public void addPosition(ref Position pos)
        {
            int low = 0;
            int hight = _data.Count;
            int mid = (low + _data.Count) / 2;
            int current = 0;

            //if (pos.row == 14 && pos.row == 14)
            //{
            //    Logs.write("\n\n", 4);
            //    foreach (var p in _data)
            //    {
            //        Logs.write(p.val + "(" + p.alone +") ", 4);
            //    }
            //}

            if (pos.val == 0)
            {
                return;
            }
            if(pos.alone)
            {
                _data.Add(pos);
                return;
            }
            while (low < hight)
            {
                current = _data.ElementAt(mid).val;
                if (current < pos.val)
                {
                    hight = mid - 1;
                    mid = (low + hight) / 2;
                }
                else if (current > pos.val)
                {
                    low = mid + 1;
                    mid = (low + hight) / 2;
                }
                else
                {
                    break;
                }
            }

            if (mid < _data.Count && _data.ElementAt(mid).val > pos.val)
            {
                _data.Insert(mid + 1, pos);
            }
            else
            {
                _data.Insert(mid, pos);
            }
        }

        public int Length
        {
            get { return _data.Count; }
        }
        //private Position[] _Table;

        public Position getValue(int pos)
        {
            return _data.ElementAt(pos);
        }

        //public Position getValue(int row, int col)
        //{
        //    return _Table[row * Side.ROW + col];
        //}

        private List<Position> _data;
    }
}
