using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    class Droper
    {
        public Droper(ChessBoard cb)
        {
            this._cb = cb;

            _canDrop = new int[Side.ROW][];
            _data = new byte[Side.ROW][];
            _valueTable = new byte[Side.ROW][];
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                _canDrop[r] = new int[Side.COL];
                _valueTable[r] = new byte[Side.COL];
                _data[r] = new byte[Side.COL];
            }

            //价值表 赋值
            byte maxValue = Side.COL_ID / 2;
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    int v_row = Math.Abs(maxValue - r);
                    int v_col = Math.Abs(maxValue - c);

                    if((v_row < v_col))
                    {
                        _valueTable[r][c] = (byte)(maxValue - v_col + 1);
                    }
                    else
                    {
                        _valueTable[r][c] = (byte)(maxValue - v_row + 1);
                    }

                    Logs.write( "" + _valueTable[r][c] + ",", Logs.level1);
                }
                Logs.writeln("", Logs.level1);
            }
        }

        #region 计算可以落子的地方

        public int[][] calCanDrop(byte color, int row, int col)
        {
            //复制棋盘
            for(int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    _data[r][c] = _cb.Data[r][c];
                }
            }
            //查看棋盘上是否有空的位置
            Logs.writeln("Can drop table:", Logs.level3);
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    //清空落子表
                    _canDrop[r][c] = _valueTable[r][c];

                    if (_cb.Data[r][c] != Color.NONE || (r == row && c == col))
                    {
                        _canDrop[r][c] = (int)DropType.CANT_NOT;
                        continue;
                    }

                    //Logs.writeln("row = " + r + "col = " + c + ",", 4);

                    int tmp_color = _data[r][c];
                    _data[r][c] = color;
                    int value = Calculator.calIncreaseValue(_data, color, r, c);
                    _data[r][c] = (byte)tmp_color;

                    _canDrop[r][c] += value;

                    if(value > 80)
                        Logs.writeln("row = " + r + "col = " + c + ", cost value =" + value, 4);

                    Logs.write("" + _canDrop[r][c] + ",", Logs.level3);
                }
                Logs.writeln("", Logs.level3);
            }

            return _canDrop;
        }

        private int[][] _canDrop;

        public int[][] GetCanDrop
        {
            get { return _canDrop; }
        }

        #endregion

        private byte[][] _valueTable;

        private ChessBoard _cb;

        private byte[][] _data;
    }
}
