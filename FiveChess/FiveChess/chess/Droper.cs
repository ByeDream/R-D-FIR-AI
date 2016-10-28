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

            //think(ref root, ref st_temp, step);

            return st_temp.selectBestPosition();
            //st_temp.print();
        }

        public void setWinflg(int tempValue, ref int theBestValue, int next_color, ref bool win, ref bool fail)
        {
            if (next_color == Color.BLACK)
            {
                if (tempValue != (int)WinState.WHITE_WIN && tempValue > theBestValue)
                {
                    theBestValue = tempValue;
                }
            }
            else if (next_color == Color.WHITE)
            {
                if (tempValue > theBestValue)
                {
                    theBestValue = tempValue;
                }
            }

            if (tempValue != (int)WinState.BLACK_WIN)
            {
                win = false;
            }
            if (tempValue != (int)WinState.WHITE_WIN)
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

        public int think(ref Position currentPos, ref StepTree st, int depth)
        {
            int row = currentPos.row;
            int col = currentPos.col;
            int color = currentPos.color;

            st.rootNode.depth = depth;

            int tempValue = 0, theBestValue = -0xfffffff;
            int next_color = (color == Color.BLACK) ? Color.WHITE : Color.BLACK;

            bool win = true, fail = true;

            //保存棋子初始值, 并下这一步棋
            Position pStep = new Position(row, col, 0, 0);
            doStep(color, ref pStep);

            //如果胜负已分出，返回胜负值
            WinState state = _rule.checkWinner(_tmpChessTable, row, col);
            if (WinState.GAMING != state)
            {
                setWinflg((int)state, ref theBestValue, next_color, ref win, ref fail);
                setWinflg2(ref currentPos, ref st, ref win, ref fail);

                unDodoStep(pStep);
                return (int)state;
            }

            //如果计算到了最后一步，返回计算值
            if (depth <= 0)
            {
                int stepValue = Calculator.calIncreaseValue(_tmpChessTable, ref currentPos);
                stepValue += _BaseValueTable[row][col];

                setWinflg(stepValue, ref theBestValue, next_color, ref win, ref fail);
                setWinflg2(ref currentPos, ref st, ref win, ref fail);

                unDodoStep(pStep);
                return stepValue;
            }

            //得到可以落子的表
            DropTable canDropTable = new DropTable();
            calCanDrop(_tmpChessTable, next_color, canDropTable);

            //为了效率，取前8个值最大的点, 有一定的风险会找不到最优点
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

                tempValue = think(ref pDrop, ref childTree, depth - 1);

                setWinflg(tempValue, ref theBestValue, next_color, ref win, ref fail);
            }

            setWinflg2(ref currentPos, ref st,ref win,ref fail);

            //Logs.writeln("deep = " + depth + "\trow=" + currentPos.row + "\tcol=" + currentPos.col + "\tcolor=" + currentPos.color + "\tval=" + currentPos.val + "\twin=" + currentPos.win, 4);

            unDodoStep(pStep);
            return theBestValue;
        }

        private void doStep(int color, ref Position pos)
        {
            if (pos.color != 0)
                throw new Exception("Do error step.");
            pos.color = _tmpChessTable[pos.row][pos.col];
            _tmpChessTable[pos.row][pos.col] = color;
        }

        private void unDodoStep(Position pos)
        {
            _tmpChessTable[pos.row][pos.col] = pos.color;
        }

        public void setPCColor(int color)
        {
            _PCColor = color;
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

        public void calCanDrop(int[][] curColorTable, int color, DropTable canDropTable)
        {
            int tmp_color = 0;
            Position dropP = null;
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    //如果已经有棋子存在，不能落子, 设为0值
                    if (curColorTable[r][c] != Color.NONE)
                    {
                        //dropP = new Position(r, c, curColorTable[r][c], 0);
                        //canDropTable.addPosition(ref dropP);
                        //Logs.write("(" + r + " " + c + ")" + canDropTable.getValue(r, c).val + ",\t\t\t", 4);
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
                    //Logs.write("(" + r + " " + c + ")" + canDropTable.getValue(r, c).val + ",\t\t\t", 4);
                }
                //Logs.writeln("", 4);
            }
            //canDropTable.sort();
        }
        #endregion

        //临时棋盘
        private int[][] _tmpChessTable;

        private Rule _rule;

        private int _PCColor = Color.NONE;

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
