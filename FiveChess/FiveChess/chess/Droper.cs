using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Chess
{
    public class Droper
    {
        public Droper(Rule rule)
        {
            _rule = rule;

            _tmpChessTable = new int[Side.ROW][];
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                _tmpChessTable[r] = new int[Side.COL];
            }
        }


        StepTree st_temp = null;
        public Position thinkNext(int [][]data, int color, int row, int col, int step)
        {
            copyBoard(data);

            Position root = new Position(row, col, color);
            st_temp = new StepTree(root);

            int value = Calculator.calIncreaseValue(_tmpChessTable, ref root);

            cal_count = 0;
            think(ref root, root.row, root.col, root.color, value, step);
            //think(root.row, root.col, root.color, ref root, ref st_temp, step);

            return st_temp.selectBestPosition();
            //st_temp.print();
        }

        public void setWinflg(int tempValue_t, ref int theBestValue_t, int next_color_t, ref bool win, ref bool fail)
        {
            if (next_color_t == Color.BLACK)
            {
                if (tempValue_t != (int)WinState.WHITE_WIN && tempValue_t > theBestValue_t)
                {
                    theBestValue_t = tempValue_t;
                }
            }
            else if (next_color_t == Color.WHITE)
            {
                if (tempValue_t > theBestValue_t)
                {
                    theBestValue_t = tempValue_t;
                }
            }

            if (tempValue_t != (int)WinState.BLACK_WIN)
            {
                win = false;
            }
            if (tempValue_t != (int)WinState.WHITE_WIN)
            {
                fail = false;
            }
        }

        public void setWinflg2(ref Position currentPos, ref StepTree st, ref bool win ,ref bool fail)
        {
            if (win)
            {
                currentPos.win = (int)WinState.BLACK_WIN;
                if (st.parent != null)
                {
                    st.parent.rootNode.haveWin = 1;
                }
            }

            if (fail)
            {
                currentPos.win = (int)WinState.WHITE_WIN;
                if (st.parent != null)
                {
                    st.parent.rootNode.haveFail = 1;
                }
            }
        }

        public static int cal_count = 0;

        public int think(int row_t, int col_t, int color_t, ref Position currentPos, ref StepTree st, int depth)
        {
            cal_count++;
            //st.rootNode.depth = depth;
            //row_t = currentPos.row;
            //col_t = currentPos.col;
            //color_t = currentPos.color;
            int next_color_t = (color_t == Color.BLACK) ? Color.WHITE : Color.BLACK;
            bool win = true;
            bool fail = true;
            int tempValue_t = 0;
            int theBestValue_t = -0xfffffff;

            //保存棋子初始值, 并下这一步棋
            //doStep(color_t, ref pStep);
            int tmpColor = _tmpChessTable[row_t][col_t];
            _tmpChessTable[row_t][col_t] = color_t;

            //如果胜负已分出，返回胜负值
            int state_t = _rule.checkWinner(_tmpChessTable, row_t, col_t);
            if (WinState.GAMING != state_t)
            {
                setWinflg(state_t, ref theBestValue_t, next_color_t, ref win, ref fail);
                setWinflg2(ref currentPos, ref st, ref win, ref fail);

                _tmpChessTable[row_t][col_t] = tmpColor;
                //unDoStep(pStep);
                return state_t;
            }

            //如果计算到了最后一步，返回计算值
            if (depth <= 0)
            {
                int stepValue_t = 0;
                stepValue_t = Calculator.calIncreaseValue(_tmpChessTable, ref currentPos);
                stepValue_t += _BaseValueTable[row_t][col_t];

                setWinflg(stepValue_t, ref theBestValue_t, next_color_t, ref win, ref fail);
                setWinflg2(ref currentPos, ref st, ref win, ref fail);

                _tmpChessTable[row_t][col_t] = tmpColor;
                //unDoStep(pStep);
                return stepValue_t;
                //return Calculator.calIncreaseValue(_tmpChessTable, ref currentPos) + _BaseValueTable[row_t][col_t];
            }

            //得到可以落子的表
            DropTable canDropTable = new DropTable();
            calCanDrop(_tmpChessTable, next_color_t, canDropTable);

            for (int pos = 0; pos < canDropTable.Length; pos++)
            {
                Position pDrop = canDropTable.getValue(pos);
                if(pDrop.val == 0 || pDrop.alone)
                {
                    break;
                }

                //把这步棋加到树
                StepTree childTree = new StepTree();
                st.addChild(ref pDrop, ref childTree);

                tempValue_t = think(pDrop.row, pDrop.col, pDrop.color, ref pDrop, ref childTree, depth - 1);

                setWinflg(tempValue_t, ref theBestValue_t, next_color_t, ref win, ref fail);
                //if (next_color_t == Color.BLACK)
                //{
                //    if (tempValue_t != (int)WinState.WHITE_WIN && tempValue_t > theBestValue_t)
                //    {
                //        theBestValue_t = tempValue_t;
                //    }
                //}
                //else if (next_color_t == Color.WHITE)
                //{
                //    if (tempValue_t > theBestValue_t)
                //    {
                //        theBestValue_t = tempValue_t;
                //    }
                //}

                //if (tempValue_t != (int)WinState.BLACK_WIN)
                //{
                //    win = false;
                //}
                //if (tempValue_t != (int)WinState.WHITE_WIN)
                //{
                //    fail = false;
                //}

                //if (tempValue_t == WinState.BLACK_WIN || tempValue_t == WinState.WHITE_WIN)
                //{
                //    break;
                //}
            }

            setWinflg2(ref currentPos, ref st, ref win, ref fail);

            //Logs.writeln("deep = " + depth + "\trow=" + currentPos.row + "\tcol=" + currentPos.col + "\tcolor=" + currentPos.color + "\tval=" + currentPos.val + "\twin=" + currentPos.win, 4);

            _tmpChessTable[row_t][col_t] = tmpColor;
            //unDoStep(pStep);

            return theBestValue_t;
        }

        public int think(ref Position currentPos, int row_t, int col_t, int color_t, int value, int depth)
        {
            cal_count++;

            int next_color_t = (color_t == Color.BLACK) ? Color.WHITE : Color.BLACK;
            int tempValue_t = 0;
            int theBestValue_t = -0xfffffff;

            //保存棋子初始值, 并下这一步棋
            int tmpColor = _tmpChessTable[row_t][col_t];
            _tmpChessTable[row_t][col_t] = color_t;

            //如果胜负已分出，返回胜负值
            int state_t = _rule.checkWinner(_tmpChessTable, row_t, col_t);
            if (WinState.GAMING != state_t)
            {
                _tmpChessTable[row_t][col_t] = tmpColor;
                return state_t;
            }

            //如果计算到了最后一步，返回计算值
            if (depth <= 0)
            {
                _tmpChessTable[row_t][col_t] = tmpColor;

                if(color_t == _PCColor)
                {
                    return value + Calculator.calIncreaseValue(_tmpChessTable, ref currentPos) + _BaseValueTable[row_t][col_t];
                }
                else
                {
                    return value - Calculator.calIncreaseValue(_tmpChessTable, ref currentPos) - _BaseValueTable[row_t][col_t];
                }
            }

            //得到可以落子的表
            DropTable canDropTable = new DropTable();
            calCanDrop(_tmpChessTable, next_color_t, canDropTable);

            for (int pos = 0; pos < canDropTable.Length; pos++)
            {
                Position pDrop = canDropTable.getValue(pos);
                if (pDrop.val == 0 || pDrop.alone)
                {
                    break;
                }

                tempValue_t = think(ref pDrop, pDrop.row, pDrop.col, pDrop.color, value, depth - 1);

                if(pDrop.color == _PCColor)
                {
                    if (tempValue_t == WinState.WHITE_WIN || tempValue_t == WinState.BLACK_WIN)
                    {
                        theBestValue_t = tempValue_t;
                    }
                    else if (value - tempValue_t < theBestValue_t)
                    {
                        theBestValue_t = value - tempValue_t;
                    }
                }
                else if(pDrop.color == Color.WHITE)
                {
                    if (tempValue_t == WinState.WHITE_WIN || tempValue_t == WinState.BLACK_WIN)
                    {
                        theBestValue_t = tempValue_t;
                    }
                    else if (value + tempValue_t > theBestValue_t)
                    {
                        theBestValue_t = value + tempValue_t;
                    }
                }

                if (tempValue_t == WinState.BLACK_WIN || tempValue_t == WinState.WHITE_WIN)
                {
                    break;
                }
            }

            //Logs.writeln("deep = " + depth + "\trow=" + currentPos.row + "\tcol=" + currentPos.col + "\tcolor=" + currentPos.color + "\tval=" + currentPos.val + "\twin=" + currentPos.win, 4);

            _tmpChessTable[row_t][col_t] = tmpColor;

            return theBestValue_t;
        }

        private void doStep(int color, ref Position pos)
        {
            if (pos.color != 0)
                throw new Exception("Do error step.");
            pos.color = _tmpChessTable[pos.row][pos.col];
            _tmpChessTable[pos.row][pos.col] = color;
        }

        private void unDoStep(Position pos)
        {
            _tmpChessTable[pos.row][pos.col] = pos.color;
        }

        public void setPCColor(int color)
        {
            _PCColor = color;

            _PLColor = color == Color.BLACK ? Color.WHITE : Color.BLACK;
        }

        public void copyBoard(int [][]data)
        {
            //复制棋盘
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    _tmpChessTable[r][c] = data[r][c];
                }
            }
        }


        #region 计算可以落子的地方

        int tmp_color = 0;
        Position dropP = null;
        public void calCanDrop(int[][] curColorTable, int color, DropTable canDropTable)
        {
            tmp_color = 0;
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    //如果已经有棋子存在，不能落子, 设为0值
                    if (curColorTable[r][c] != Color.NONE)
                    {
                        continue;
                    }

                    //假设落子,计算落子之后, 这一步棋的价值
                    tmp_color = curColorTable[r][c];
                    curColorTable[r][c] = color;

                    dropP = new Position(r, c, color, _BaseValueTable[r][c]);
                    Calculator.calIncreaseValue(curColorTable, ref dropP);

                    //还原走过的棋
                    curColorTable[r][c] = tmp_color;

                    canDropTable.addPosition(ref dropP);
                }
            }
            //canDropTable.print();
        }
        #endregion

        //临时棋盘
        private int[][] _tmpChessTable;

        private Rule _rule;

        private int _PCColor = Color.NONE;
        private int _PLColor = Color.NONE;
        //估值表
        public int[][] _BaseValueTable =
        {
           new int[]{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
           new int[]{ 1,2,2,2,2,2,2,2,2,2,2,2,2,2,1 },
           new int[]{ 1,2,3,3,3,3,3,3,3,3,3,3,3,2,1 },
           new int[]{ 1,2,3,4,4,4,4,4,4,4,4,4,3,2,1 },
           new int[]{ 1,2,3,4,5,5,5,5,5,5,5,4,3,2,1 },
           new int[]{ 1,2,3,4,5,6,6,6,6,6,5,4,3,2,1 },
           new int[]{ 1,2,3,4,5,6,7,7,7,6,5,4,3,2,1 },
           new int[]{ 1,2,3,4,5,6,7,8,7,6,5,4,3,2,1 },
           new int[]{ 1,2,3,4,5,6,7,7,7,6,5,4,3,2,1 },
           new int[]{ 1,2,3,4,5,6,6,6,6,6,5,4,3,2,1 },
           new int[]{ 1,2,3,4,5,5,5,5,5,5,5,4,3,2,1 },
           new int[]{ 1,2,3,4,4,4,4,4,4,4,4,4,3,2,1 },
           new int[]{ 1,2,3,3,3,3,3,3,3,3,3,3,3,2,1 },
           new int[]{ 1,2,2,2,2,2,2,2,2,2,2,2,2,2,1 },
           new int[]{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }
        };

        //价值表 赋值
        //int maxValue = Side.COL_ID / 2;
        //for (int r = 0; r <= Side.ROW_ID; r++)
        //{
        //    for (int c = 0; c <= Side.COL_ID; c++)
        //    {
        //        int v_row = Math.Abs(maxValue - r);
        //        int v_col = Math.Abs(maxValue - c);

        //        if((v_row < v_col))
        //        {
        //            _valueTable[r][c] = (maxValue - v_col + 1);
        //        }
        //        else
        //        {
        //            _valueTable[r][c] = (maxValue - v_row + 1);
        //        }

        //        //Logs.write( "" + _valueTable[r][c] + ",", 4);
        //    }
        //    //Logs.writeln("", 41);
        //}
    }
}
