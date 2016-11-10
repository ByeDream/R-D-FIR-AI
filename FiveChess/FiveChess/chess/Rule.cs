using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gobang
{
    public class Rule
    {
        public Rule() { }

        #region winner check

        public int checkWinner( Pawns cb)
        {
            //TODO
            return WinState.GAMING;
        }

        public int checkWinner(int[][] data, Position p)
        {
            return checkWinner(data, p.color, p.row, p.col);
        }

        private int line_count = 0;
        private int state = 0;
        //检测胜负，知道最后一步落子的情况下,只需要横竖斜4个方向是否有连续的5个子
        public int checkWinner(int [][]data, int color, int row, int col)
        {
            state = WinState.GAMING;
            line_count = 0;
            //如果棋盘摆满了，先假设是和局
            //if (Calculator.isFullBoard(data))
            //{
            //    check = WinState.DRAW;
            //}

            //horizontal
            if (line_count < Con.WIN_COUNT)
            {
                line_count = Calculator.hasHorizontalCount(data, color, Con.WIN_COUNT, row, col);
                //count = Calculator.hasHorizontalCount(cb.Data, chess_value, Com.WIN_COUNT, row, col, out type);
            }

            //vertical
            if (line_count < Con.WIN_COUNT)
            {
                line_count = Calculator.hasVerticalCount(data, color, Con.WIN_COUNT, row, col);
                //count = Calculator.hasVerticalCount(cb.Data, chess_value, Com.WIN_COUNT, row, col, out type);
            }

            //inclined
            if (line_count < Con.WIN_COUNT)
            {
                line_count = Calculator.hasInclinedCount_LT(data, color, Con.WIN_COUNT, row, col);
            }

            //inclined
            if (line_count < Con.WIN_COUNT)
            {
                line_count = Calculator.hasInclinedCount_LB(data, color, Con.WIN_COUNT, row, col);
            }

            if (line_count >= Con.WIN_COUNT)
            {
                switch(color)
                {
                    case Color.BLACK:
                        //Logs.writeln("\n===================\n\nBLACK WINS!!!\n\n===================\n",Logs.level4);
                        state = WinState.BLACK_WIN;
                        break;
                    case Color.WHITE:
                        //Logs.writeln("\n===================\n\nWHITE WINS!!!\n\n===================\n", Logs.level4);
                        state = WinState.WHITE_WIN;
                        break;
                    default:
                        break;
                }       
            }
            //Logs.writeln("Winer check = " + check);
            return state;
        }

        #endregion
    }
}
