using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    public enum WinState
    {
        //胜负未分
        GAMING = 0,
        //白胜
        WHITE_WIN = 1,
        //黑胜
        BLACK_WIN = 2,
        //和局
        DRAW = 3,
    }

    class Rule
    {
        public Rule() { }

        #region winner check

        public WinState checkWinner( ChessBoard cb)
        {
            //TODO
            return WinState.GAMING;
        }

        public WinState checkWinner(ChessBoard cb, Position p)
        {
            return checkWinner(cb, p.row, p.col);
        }

        //检测胜负，知道最后一步落子的情况下,只需要横竖斜4个方向是否有连续的5个子
        public WinState checkWinner(ChessBoard cb, int row, int col)
        {
            WinState check = WinState.GAMING;
            byte chess_value = cb.Data[row][col];
            chessCountType type;


            //记录是否有5次相同的棋
            int count = 0;

            //如果棋盘摆满了，先假设是和局
            if (Calculator.isFullBoard(cb))
            {
                check = WinState.DRAW;
            }

            Calculator.calVerticalCount(cb.Data, chess_value, row, col, out type);
            Calculator.calHorizontalCount(cb.Data, chess_value, row, col, out type);
            //horizontal
            if (count < DEF.WIN_COUNT)
            {
                count = Calculator.hasHorizontalCount(cb.Data, chess_value, DEF.WIN_COUNT, row, col, out type);
            }

            //vertical
            if (count < DEF.WIN_COUNT)
            {
                count = Calculator.hasVerticalCount(cb.Data, chess_value, DEF.WIN_COUNT, row, col, out type);
            }

            //inclined
            if (count < DEF.WIN_COUNT)
            {
                count = Calculator.hasInclinedCount_LT(cb.Data, chess_value, DEF.WIN_COUNT, row, col, out type);
            }

            //inclined
            if (count < DEF.WIN_COUNT)
            {
                count = Calculator.hasInclinedCount_LB(cb.Data, chess_value, DEF.WIN_COUNT, row, col, out type);
            }

            if (count >= DEF.WIN_COUNT)
            {
                switch(chess_value)
                {
                    case DEF.CHESS_BLACK:
                        Logs.writeln("\n===================\n\nBLACK WINS!!!\n\n===================\n",Logs.level4);
                        check = WinState.BLACK_WIN;
                        break;
                    case DEF.CHESS_WHITE:
                        Logs.writeln("\n===================\n\nWHITE WINS!!!\n\n===================\n", Logs.level4);
                        check = WinState.WHITE_WIN;
                        break;
                    default:
                        break;
                }       
            }
            Logs.writeln("Winer check = " + check);
            return check;
        }

        #endregion
    }
}
