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

            _canDrop = new byte[Side.ROW][];
            _valueTable = new byte[Side.ROW][];
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                _canDrop[r] = new byte[Side.COL];
                _valueTable[r] = new byte[Side.ROW];
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

        public byte[][] calCanDrop(byte color)
        {
            //清空落子表
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    _canDrop[r][c] = _valueTable[r][c];
                }
            }

            //查看棋盘上是否有空的位置
            Logs.writeln("Can drop table:", Logs.level3);
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    if(_cb.Data[r][c] != 0)
                    {
                        _canDrop[r][c] = 0;
                    }
                    Logs.write("" + _canDrop[r][c] + ",", Logs.level3);
                }
                Logs.writeln("", Logs.level3);
            }

            return _canDrop;
        }

        private byte[][] _canDrop;

        public byte[][] GetCanDrop
        {
            get { return _canDrop; }
        }

        #endregion

        private byte[][] _valueTable;

        private ChessBoard _cb;
    }
}
