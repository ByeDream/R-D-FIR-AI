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


        //private StepTree st_temp = null;
        private Position _bestPosition = null;
        public Position think(int [][]data, int color, int row, int col, int depth)
        {
            copyBoard(data);

            Position root = new Position(row, col, color);
            //st_temp = new StepTree(root);

            int value = Calculator.calIncreaseValue(_tmpChessTable, ref root);

            cal_count = 0;
            cur_max_depth = depth;

            //极大极小值算法
            //think(ref root, root.row, root.col, root.color, 0, depth);

            //alphabeta算法
            alphaBeta(ref root, root.row, root.col, root.color, 0, -0xfffffff, 0xfffffff, depth);

            Logs.writeln("count = " + Droper.cal_count, 5);
            //st_temp.print();
            return _bestPosition;
        }

        private Stack<Position> _steps = new Stack<Position>();
        public Stack<Position> steps
        {
            get { return _steps; }
        }

        #region 极大极小值算法

        public int cur_max_depth = 0;
        public static int cal_count = 0;
        public int think(ref Position currentPos, int row, int col, int color, int current_value, int depth)
        {
            cal_count++;

            int next_color = (color == Color.BLACK ? Color.WHITE : Color.BLACK);
            int tempValue = 0;
            int theBestValue = (next_color == _PCColor ? -0xfffffff : 0xfffffff);

            //保存棋子初始值, 并下这一步棋
            int tmpColor = _tmpChessTable[row][col];
            _tmpChessTable[row][col] = color;

            //如果胜负已分出，返回胜负值
            int state = _rule.checkWinner(_tmpChessTable, row, col);
            if (WinState.GAMING != state)
            {
                currentPos.total = (color == Color.BLACK ? WinState.BLACK_WIN : WinState.WHITE_WIN);

                _tmpChessTable[row][col] = tmpColor;
                return currentPos.total;
            }

            //如果计算到了最后一步，返回计算值
            if (depth <= 0)
            {
                //unDoStep(pStep);
                //_steps.Pop();
                Calculator.calIncreaseValue(_tmpChessTable, ref currentPos);
                if (color == Color.BLACK)
                {
                    currentPos.total = current_value + currentPos.val + _BaseValueTable[row][col];
                }
                else
                {
                    currentPos.total = current_value - currentPos.val - _BaseValueTable[row][col];
                }
                _tmpChessTable[row][col] = tmpColor;
                return currentPos.total;
            }

            //得到可以落子的表
            DropTable canDropTable = new DropTable();
            calCanDrop(_tmpChessTable, next_color, canDropTable, Order.REVERSE);

            if (next_color == Color.BLACK)
            {
                for (int pos = 0; pos < canDropTable.Length; pos++)
                {
                    Position pDrop = canDropTable.getValue(pos);

                    tempValue = think(ref pDrop, pDrop.row, pDrop.col, pDrop.color, current_value + pDrop.val, depth - 1);
                    if (theBestValue < tempValue)
                    {
                        theBestValue = tempValue;
                        if (depth == cur_max_depth)
                        {
                            _bestPosition = pDrop;
                        }
                    }
                    if (tempValue == WinState.BLACK_WIN)
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int pos = 0; pos < canDropTable.Length; pos++)
                {
                    Position pDrop = canDropTable.getValue(pos);

                    tempValue = think(ref pDrop, pDrop.row, pDrop.col, pDrop.color, current_value - pDrop.val, depth - 1);
                    if (theBestValue > tempValue)
                    {
                        theBestValue = tempValue;
                    }
                    if (tempValue == WinState.WHITE_WIN)
                    {
                        break;
                    }
                }
            }

            //Logs.writeln("deep = " + depth + "\trow=" + currentPos.row + "\tcol=" + currentPos.col + "\tcolor=" + currentPos.color + "\tval=" + currentPos.val + "\twin=" + currentPos.win, 4);

            _tmpChessTable[row][col] = tmpColor;

            return theBestValue;
        }

        #endregion

        #region alphabeta 剪枝算法

        //public int cur_max_step = 0;
        //public static int cal_count = 0;
        public int alphaBeta(ref Position currentPos, int row, int col, int color, int current_value, int alhpa, int beta, int depth)
        {
            #region 初始化

            cal_count++;
            int next_color = (color == Color.BLACK ? Color.WHITE : Color.BLACK);
            int tempValue = 0;
            int theBestValue = (next_color == _PCColor ? -0xfffffff : 0xfffffff);

            #endregion

            #region  保存棋子初始值, 并下这一步棋

            int tmpColor = _tmpChessTable[row][col];
            _tmpChessTable[row][col] = color;

            #endregion

            #region  如果胜负已分出，返回胜负值

            int state = _rule.checkWinner(_tmpChessTable, row, col);
            if (WinState.GAMING != state)
            {
                currentPos.total = (color == Color.BLACK ? WinState.BLACK_WIN : WinState.WHITE_WIN);
                _tmpChessTable[row][col] = tmpColor;
                return currentPos.total;
            }

            #endregion

            #region  如果计算到了最后一步，返回计算值

            if (depth <= 0)
            {
                //unDoStep(pStep);
                //_steps.Pop();
                Calculator.calIncreaseValue(_tmpChessTable, ref currentPos);
                if (color == Color.BLACK)
                {
                    currentPos.total = current_value + currentPos.val + _BaseValueTable[row][col];
                }
                else
                {
                    currentPos.total = current_value - currentPos.val - _BaseValueTable[row][col];
                }
                _tmpChessTable[row][col] = tmpColor;
                return currentPos.total;
            }

            #endregion

            #region  得到可以落子的表

            DropTable canDropTable = new DropTable();
            calCanDrop(_tmpChessTable, next_color, canDropTable, Order.REVERSE);

            #endregion

            if(next_color == Color.BLACK)
            {
                for (int pos = 0; pos < canDropTable.Length; pos++)
                {
                    Position pDrop = canDropTable.getValue(pos);

                    tempValue = alphaBeta(ref pDrop, pDrop.row, pDrop.col, pDrop.color, current_value + pDrop.val, alhpa, beta, depth - 1);

                    if (alhpa < tempValue)
                    {
                        alhpa = tempValue;
                        if (depth == cur_max_depth)
                        {
                            _bestPosition = pDrop;
                        }
                    }
                    if (alhpa >= beta)
                    {
                        break;
                    }
                }
                _tmpChessTable[row][col] = tmpColor;
                return alhpa;
            }
            else
            {
                for (int pos = 0; pos < canDropTable.Length; pos++)
                {
                    Position pDrop = canDropTable.getValue(pos);

                    tempValue = alphaBeta(ref pDrop, pDrop.row, pDrop.col, pDrop.color, current_value - pDrop.val, alhpa, beta, depth - 1);

                    if (beta > tempValue)
                    {
                        beta = tempValue;
                    }
                    if (beta <= alhpa)
                    {
                        break;
                    }
                }
                _tmpChessTable[row][col] = tmpColor;
                return beta;
            }
        }

        #endregion

        #region 主递归调用2

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

        public void setWinflg2(ref Position currentPos, ref StepTree st, ref bool win, ref bool fail)
        {
            if (win)
            {
                currentPos.total = (int)WinState.BLACK_WIN;
                if (st.parent != null)
                {
                    st.parent.rootNode.haveWin = 1;
                }
            }

            if (fail)
            {
                currentPos.total = (int)WinState.WHITE_WIN;
                if (st.parent != null)
                {
                    st.parent.rootNode.haveFail = 1;
                }
            }
        }

        private int row_t = 0;
        private int col_t = 0;
        private int color_t = 0;
        private int next_color = 0;
        private int state = 0;
        private int stepValue_t = 0;

        public int think(ref Position currentPos, ref StepTree st, int depth)
        {
            cal_count++;
            //st.rootNode.depth = depth;
            row_t = currentPos.row;
            col_t = currentPos.col;
            color_t = currentPos.color;
            next_color = (color_t == Color.BLACK) ? Color.WHITE : Color.BLACK;

            bool win = true;
            bool fail = true;
            int tempValue_t = 0;
            int theBestValue_t = -0xfffffff;

            //保存棋子初始值, 并下这一步棋
            //doStep(color_t, ref pStep);
            int tmpColor = _tmpChessTable[row_t][col_t];
            _tmpChessTable[row_t][col_t] = color_t;

            //如果胜负已分出，返回胜负值
            state = _rule.checkWinner(_tmpChessTable, row_t, col_t);
            if (WinState.GAMING != state)
            {
                setWinflg(state, ref theBestValue_t, next_color, ref win, ref fail);
                setWinflg2(ref currentPos, ref st, ref win, ref fail);

                _tmpChessTable[row_t][col_t] = tmpColor;
                //unDoStep(pStep);
                return state;
            }

            //如果计算到了最后一步，返回计算值
            if (depth <= 0)
            {
                stepValue_t = Calculator.calIncreaseValue(_tmpChessTable, ref currentPos);
                stepValue_t += _BaseValueTable[row_t][col_t];

                setWinflg(stepValue_t, ref theBestValue_t, next_color, ref win, ref fail);
                setWinflg2(ref currentPos, ref st, ref win, ref fail);

                _tmpChessTable[row_t][col_t] = tmpColor;
                //unDoStep(pStep);
                return stepValue_t;
            }

            //得到可以落子的表
            DropTable canDropTable = new DropTable();
            calCanDrop(_tmpChessTable, next_color, canDropTable);

            for (int pos = 0; pos < canDropTable.Length; pos++)
            {
                Position pDrop = canDropTable.getValue(pos);
                if (pDrop.val == 0 || pDrop.alone)
                {
                    break;
                }

                //把这步棋加到树
                StepTree childTree = new StepTree();
                st.addChild(ref pDrop, ref childTree);

                tempValue_t = think(ref pDrop, ref childTree, depth - 1);

                setWinflg(tempValue_t, ref theBestValue_t, next_color, ref win, ref fail);

                if (tempValue_t == WinState.BLACK_WIN || tempValue_t == WinState.WHITE_WIN)
                {
                    break;
                }
            }

            setWinflg2(ref currentPos, ref st, ref win, ref fail);

            //Logs.writeln("deep = " + depth + "\trow=" + currentPos.row + "\tcol=" + currentPos.col + "\tcolor=" + currentPos.color + "\tval=" + currentPos.val + "\twin=" + currentPos.win, 4);

            _tmpChessTable[row_t][col_t] = tmpColor;
            //unDoStep(pStep);

            return theBestValue_t;
        }

        #endregion

        public void setPCColor(int color)
        {
            _PCColor = color;
            _PLColor = (_PCColor == Color.BLACK ?Color.WHITE : Color.BLACK); 
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
        public void calCanDrop(int[][] curColorTable, int color, DropTable canDropTable, int order = Order.REVERSE)
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

                    if (order == Order.REVERSE)
                    {
                        canDropTable.addPositionReverse(ref dropP);
                    }
                    else
                    {
                        canDropTable.addPositionPositive(ref dropP);
                    }
                }
            }
            //if (color == Color.BLACK)
            //    canDropTable.print();
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
