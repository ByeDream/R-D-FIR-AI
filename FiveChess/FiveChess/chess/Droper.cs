using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    class Droper
    {
        public Droper(Rule rule)
        {
            _rule = rule;

            _canDrop = new int[Side.ROW][];
            _data = new int[Side.ROW][];
            _valueTable = new int[Side.ROW][];
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                _canDrop[r] = new int[Side.COL];
                _valueTable[r] = new int[Side.COL];
                _data[r] = new int[Side.COL];
            }

            //价值表 赋值
            int maxValue = Side.COL_ID / 2;
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    int v_row = Math.Abs(maxValue - r);
                    int v_col = Math.Abs(maxValue - c);

                    if((v_row < v_col))
                    {
                        _valueTable[r][c] = (maxValue - v_col + 1);
                    }
                    else
                    {
                        _valueTable[r][c] = (maxValue - v_row + 1);
                    }

                    Logs.write( "" + _valueTable[r][c] + ",", Logs.level1);
                }
                Logs.writeln("", Logs.level1);
            }
        }


        private int think_value = 0;
        public int think(int color, int row, int col, int deep)
        {
            if(deep < 0)
            {
                return think_value;
            }

            calCanDrop(color, row, col);

            int tmp_value1 = 0, tmp_value2 = 0;
            for(int r = 0; r < _canDrop.Length; r ++)
            {
                for(int c = 0; c < _canDrop[r].Length; c++)
                {
                    if (color == 0)
                    {
                        tmp_value1 += think(_canDrop[r][c], row, col, deep - 1);
                    }
                    else
                    {
                        tmp_value1 -= think(_canDrop[r][c], row, col, deep - 1);
                    }
                    if(tmp_value1 > tmp_value2)
                    {
                        tmp_value2 = tmp_value1;
                        //TODO 保存
                    }
                }
            }

            WinState state = _rule.checkWinner(_data, row, col);
            if (WinState.GAMING != state)
            {
                return (int)state;
            }

            return think_value;
        }

        public void copyBoard(ChessBoard cb)
        {
            //复制棋盘
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    _data[r][c] = cb.Data[r][c];
                }
            }
        }

        #region 计算可以落子的地方

        public int[][] calCanDrop(int color, int row, int col)
        {
            //查看棋盘上是否有空的位置
            //Logs.writeln("Can drop table:", Logs.level3);
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    //清空落子表
                    _canDrop[r][c] = _valueTable[r][c];

                    if (_data[r][c] != Color.NONE || (r == row && c == col))
                    {
                        _canDrop[r][c] = (int)DropType.CANT_NOT;
                        continue;
                    }

                    int tmp_color = _data[r][c];
                    _data[r][c] = color;
                    int value = Calculator.calIncreaseValue(_data, color, r, c);
                    _data[r][c] = tmp_color;

                    _canDrop[r][c] += value;

                    if(value > 80)
                    {
                        Logs.writeln( "col = " + c + "row = " + r + ", Increase value =" + value, 2);
                    }

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

        //估值表
        private int[][] _valueTable;

        //计算出来的步数表
        private int[][] _step;

        //临时棋盘
        private int[][] _data;

        private Rule _rule;
    }
}
