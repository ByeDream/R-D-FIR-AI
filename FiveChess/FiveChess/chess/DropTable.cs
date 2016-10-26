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
            _Table = new Position[Side.COL * Side.ROW];
            for (int i = 0; i < _Table.Length; i++)
            {
                _Table[i] = new Position();
            }
        }

        public void clear()
        {
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    _Table[r * Side.COL + c].row = 0;
                    _Table[r * Side.COL + c].col = 0;
                    _Table[r * Side.COL + c].val = 0;
                    //Logs.write("(" + _Table[r * Side.COL + c].row + " " + _Table[r * Side.COL + c].col + ")" + _Table[r * Side.COL + c].val + ",\t\t\t", 4);
                }
                //Logs.writeln("", 4);
            }
        }

        public void sort()
        {
            Position.sort(_Table);
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    //Logs.write("(" + _Table[r * Side.COL + c].row + " " + _Table[r * Side.COL + c].col + ")" + _Table[r *Side.COL + c]. val + ",\t\t\t", 4);
                }
                //Logs.writeln("", 4);
            }
        }

        public void setValue(int row, int col, int value, int color)
        {
            _Table[row * Side.ROW + col].col = col;
            _Table[row * Side.ROW + col].row = row;
            _Table[row * Side.ROW + col].val = value;
            _Table[row * Side.ROW + col].color = color;
        }

        public int Length = Side.COL * Side.ROW;
        private Position[] _Table;

        public Position getValue(int pos)
        {
            return _Table[pos];
        }

        public Position getValue(int row, int col)
        {
            return _Table[row * Side.ROW + col];
        }
    }
}
