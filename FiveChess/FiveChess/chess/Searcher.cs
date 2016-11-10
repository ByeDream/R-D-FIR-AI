using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Gobang
{
    public class Searcher
    {
        public Searcher(Rule rule)
        {
            _rule = rule;
            _replaceTable = new ReplaceTable();
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
            _pawnSum++;

            _curPawnSum = _pawnSum;
            cal_count = 0;
            cur_max_depth = depth;

            //极大极小值算法
            //minMax(ref root, root.row, root.col, root.color, 0, depth);

            DateTime dt_begin = DateTime.Now;

            int a = DateTime.Now.Millisecond;
            //alphabeta算法
            alphaBeta(ref root, root.row, root.col, root.color, 0, -0xfffffff, 0xfffffff, depth);

            DateTime dt_end = DateTime.Now;

            Logs.writeln("time used:" + Convert.ToInt64((dt_end.ToUniversalTime() - dt_begin.ToUniversalTime()).TotalMilliseconds), 4);

            Logs.writeln("count :" + Searcher.cal_count, 4);

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
        public int minMax(ref Position currentPos, int row, int col, int color, int current_value, int depth)
        {
            cal_count++;

            int next_color = (color == Color.BLACK ? Color.WHITE : Color.BLACK);
            int tempValue = 0;
            int theBestValue = (next_color == _PCColor ? -0xfffffff : 0xfffffff);

            //保存棋子初始值, 并下这一步棋
            int tmpColor = _tmpChessTable[row][col];
            _tmpChessTable[row][col] = color;

            //如果胜负已分出，返回胜负值
            int state = _rule.checkWinner(_tmpChessTable, color, row, col);
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

                    tempValue = minMax(ref pDrop, pDrop.row, pDrop.col, pDrop.color, current_value + pDrop.val, depth - 1);
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

                    tempValue = minMax(ref pDrop, pDrop.row, pDrop.col, pDrop.color, current_value - pDrop.val, depth - 1);
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


        private int _curPawnSum = 0;
        private long time = 0;
        //public int cur_max_step = 0;
        //public static int cal_count = 0;
        public int alphaBeta(ref Position currentPos, int row, int col, int color, int current_value, int alpha, int beta, int depth)
        {
            cal_count++;

            #region 初始化

            int next_color = (color == Color.BLACK ? Color.WHITE : Color.BLACK);
            int tempValue = 0;
            int theBestValue = (next_color == _PCColor ? -0xfffffff : 0xfffffff);

            #endregion

            #region  保存棋子初始值, 并下这一步棋

            int tmpColor = _tmpChessTable[row][col];
            _tmpChessTable[row][col] = color;
            _curPawnSum++;

            #endregion

            #region  如果胜负已分出，返回胜负值

            int state = _rule.checkWinner(_tmpChessTable, color, row, col);
            if (WinState.GAMING != state)
            {
                currentPos.total = (color == Color.BLACK ? WinState.BLACK_WIN : WinState.WHITE_WIN);

                _tmpChessTable[row][col] = tmpColor;
                _curPawnSum--;

                return currentPos.total;
            }

            #endregion

            #region  如果计算到了最后一步，返回计算值

            int total = _replaceTable.LookupHashTable(alpha, beta, _curPawnSum, color);
            if(total != WinState.INVALIDE)
            {
                return total;
            }

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

                _replaceTable.Insert(_pawnSum, HashItem.type_value, currentPos.total, _tmpChessTable[row][col]);

                //还原棋盘
                _tmpChessTable[row][col] = tmpColor;
                _curPawnSum--;

                return currentPos.total;
            }

            #endregion

            #region  得到可以落子的表

            DropTable canDropTable = new DropTable();
            calCanDrop(_tmpChessTable, next_color, canDropTable, Order.REVERSE);

            #endregion

            if (next_color == Color.BLACK)
            {
                for (int pos = 0; pos < canDropTable.Length; pos++)
                {
                    Position pDrop = canDropTable.getValue(pos);

                    tempValue = alphaBeta(ref pDrop, pDrop.row, pDrop.col, pDrop.color, current_value + pDrop.val, alpha, beta, depth - 1);

                    if (alpha < tempValue)
                    {
                        alpha = tempValue;
                        if (depth == cur_max_depth)
                        {
                            _bestPosition = pDrop;
                        }
                    }
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                _tmpChessTable[row][col] = tmpColor;
                _curPawnSum--;
                return alpha;
            }
            else
            {
                for (int pos = 0; pos < canDropTable.Length; pos++)
                {
                    Position pDrop = canDropTable.getValue(pos);

                    tempValue = alphaBeta(ref pDrop, pDrop.row, pDrop.col, pDrop.color, current_value - pDrop.val, alpha, beta, depth - 1);

                    if (beta > tempValue)
                    {
                        beta = tempValue;
                    }
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                _tmpChessTable[row][col] = tmpColor;
                _curPawnSum--;
                return beta;
            }
        }

        #endregion

        public void setPCColor(int color)
        {
            _PCColor = color;
            _PLColor = (_PCColor == Color.BLACK ?Color.WHITE : Color.BLACK); 
        }

        public void copyBoard(int [][]data)
        {
            _pawnSum = 0;
            //复制棋盘
            for (int r = 0; r <= Side.ROW_ID; r++)
            {
                for (int c = 0; c <= Side.COL_ID; c++)
                {
                    _tmpChessTable[r][c] = data[r][c];

                    if (data[r][c] != 0)
                    {
                        _pawnSum++;
                    }
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


        private int _pawnSum = 0;

        //临时棋盘
        private int[][] _tmpChessTable;

        private ReplaceTable _replaceTable;

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
