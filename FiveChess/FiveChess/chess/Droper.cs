using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Chess
{
    public class DropTable
    {
        public DropTable()
        {
            _Table = new Position[Side.COL * Side.ROW];
            for(int i = 0; i < _Table.Length; i++)
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

        public void setValue(int row, int col, int value)
        {
            _Table[row * Side.ROW + col].col = col;
            _Table[row * Side.ROW + col].row = row;
            _Table[row * Side.ROW + col].val = value;
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

    public class Droper
    {
        public Droper(Rule rule)
        {
            _rule = rule;

            _data = new int[Side.ROW][];
            _valueTable = new int[Side.ROW][];
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
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


        private Stack<Position> steps = new Stack<Position>();


        public void prepareThink()
        {
            steps.Clear();
        }

        public void printSteps()
        {
            Logs.writeln("\n\n\n");
            foreach (Position p in steps)
            {
                //Logs.writeln("row = " + p.row + ", col = " + p.col + " color = " + p.val, 4);
            }
        }

        public int think(int color, int row, int col, int deep)
        {
            if(deep <= 0)
            {
                return Calculator.calIncreaseValue(_data, color, row, col); ;
            }

            WinState state = _rule.checkWinner(_data, row, col);
            if (WinState.GAMING != state)
            {
                return (int)state;
            }

            DropTable dropTable = new DropTable();
            calCanDrop(color, row, col, dropTable);

            Position target = new Position();
            int value, bestValue = 0;
            int next_color = 0;
            next_color = (color == Color.BLACK) ? Color.WHITE : Color.BLACK;

            //steps.Push(new Position(row, col, color));
            //Logs.writeln("000  push row = " + row + ", col = " + col + " color = " + color, 4);

            for (int pos = 0; pos < dropTable.Length; pos++)
            {
                Position p = dropTable.getValue(pos);
                if(p.val == 0)
                {
                    break;
                }
                else if(p.val < 100)
                {
                    break;
                }

                value = think(next_color, p.row, p.col, deep - 1);
                if(value > bestValue)
                {
                    bestValue = value;

                    target.col = p.col;
                    target.row = p.row;
                    target.val = p.val;
                    target.color = next_color;
                }
                //else
                //{
                //    Position p1 = steps.Pop();
                //    Logs.writeln("333 pop row = " + p1.row + ", col = " + p1.col + "color = " + next_color, 4);
                //}
            }
            steps.Push(target);
            Logs.writeln("222 push row = " + target.row + ", col = " + target.col + "color = " + next_color, 4);

            return bestValue;
        }

        public void setPCColor(int color)
        {
            _PCColor = color;
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

        private Position tmp_pos = new Position();
        public void setChessValue(int row ,int col, int value)
        {
            tmp_pos.row = row;
            tmp_pos.col = col;
            tmp_pos.val = _data[row][col];

            _data[row][col] = value;
        }

        public void revertChessValue()
        {
            _data[tmp_pos.row][tmp_pos.col] = tmp_pos.val;

            tmp_pos.row = 0;
            tmp_pos.col = 0;
            tmp_pos.val = 0;
        }

        #region 计算可以落子的地方

        public void calCanDrop(int color, int row, int col, DropTable dropTable)
        {
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    if (_data[r][c] != Color.NONE || (r == row && c == col))
                    {
                        dropTable.setValue(r, c, 0);
                        //Logs.write("(" + r + " " + c + ")" + dropTable.getValue(r, c) + ",\t\t\t", 4);
                        continue;
                    }
                    int total_value = 0, step_value = 0;

                    //用基本的位置数值  清空数值表
                    total_value += _valueTable[r][c];

                    //计算落子之后, 这一步棋的价值
                    setChessValue(r, c, color);
                    step_value = Calculator.calIncreaseValue(_data, color, r, c);
                    total_value += step_value;

                    revertChessValue();

                    if (total_value > 0)
                        dropTable.setValue(r, c, total_value);

                    //Logs.write("(" + r +" " + c +")" + dropTable.getValue(r, c) + ",\t\t\t", 4);
                }
                //Logs.writeln("", 4);
            }

            dropTable.sort();
        }
        #endregion

        //估值表
        private static int[][] _valueTable;

        //计算出来的步数表
        private int[][] _step;

        //临时棋盘
        private int[][] _data;

        private Rule _rule;

        private int _PCColor = Color.NONE;
        //private Tree<Position> tree;
    }
}
