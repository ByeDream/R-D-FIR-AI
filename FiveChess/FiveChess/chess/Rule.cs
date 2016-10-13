using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    class Rule
    {
        //胜负未分
        public const int GAMING = 0;
        //白胜
        public const int WHITE_WIN = 1;
        //黑胜
        public const int BLACK_WIN = 2;
        //和局
        public const int DRAW = 3;

        public Rule() { }

        public bool isFullBoard(ChessBoard cb)
        {
            bool isFull = true;
            for( int row = 0; row < cb.Data.Length; row ++)
            {
                for (int col = 0; col < cb.Data[row].Length; col++)
                {
                    if(cb.Data[row][col] != ChessBoard.CHESS_BLACK && cb.Data[row][col] != ChessBoard.CHESS_WHITE)
                    {
                        isFull = false;
                    }
                }
            }
            return isFull;
        }

        public int checkWinner( ChessBoard cb)
        {
            //TODO
            return GAMING;
        }

        public int checkWinner(ChessBoard cb, Position p)
        {
            return checkWinner(cb, p.row, p.col);
        }

        //检测胜负，知道最后一步落子的情况下,只需要横竖斜4个方向是否有连续的5个子
        public int checkWinner(ChessBoard cb, int row, int col)
        {
            int check = GAMING;
            byte chess_value = cb.Data[row][col];

            //记录是否有5次相同的棋
            int count = 0;

            int col0 = col, col1 = col - 4, col2 = col + 4;
            int row0 = row, row1 = row - 4, row2 = row + 4;

            //col1 = col1 < 0 ? 0 : col1;
            //col2 = col2 > 14 ? 14 : col2;

            //row1 = row1 < 0 ? 0 : row1;
            //row2 = row2 > 14 ? 14 : row2;

            //如果棋盘摆满了，先假设是和局
            if (isFullBoard(cb))
            {
                chess_value = DRAW;
            }

            //horizontal
            if(count <5)
            {
                count = checkHorizontalFive(cb, col1, col2, row, chess_value);
            }

            //vertical
            if (count < 5)
            {
                count = checkVerticalFive(cb, row1, row2, col, chess_value);
            }

            //inclined
            if (count < 5)
            {
                count = checkInclinedFive1(cb, col1, col2, row1, row2, chess_value);
            }

            //inclined
            if (count < 5)
            {
                count = checkInclinedFive2(cb, col1, col2, row1, row2, chess_value);
            }

            if (count >= 5)
            {
                switch(chess_value)
                {
                    case ChessBoard.CHESS_BLACK:
                        Logs.writeln("\n===================\n\nBLACK WINS!!!\n\n===================\n",Logs.level4);
                        check = BLACK_WIN;
                        break;
                    case ChessBoard.CHESS_WHITE:
                        Logs.writeln("\n===================\n\nWHITE WINS!!!\n\n===================\n", Logs.level4);
                        check = WHITE_WIN;
                        break;
                    default:
                        break;
                }       
            }
            Logs.writeln("Winer check = " + check);
            return check;
        }

        public static int checkVerticalFive(ChessBoard cb, int beginRow, int endRow, int baseCol, int chessValue)
        {
            int count = 0;
            for (int r = beginRow; r <= endRow; r++)
            {
                if (r < 0)
                    continue;
                if (r > 14)
                    break;
                Logs.writeln(" row = " + r);
                if (chessValue == cb.Data[r][baseCol])
                {
                    if (++count >= 5)
                    {
                        break;
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return count;
        }

        public static int checkHorizontalFive(ChessBoard cb, int beginCol, int endCol, int baseRow, int chessValue)
        {
            int count = 0;
            for (int c = beginCol; c <= endCol; c++)
            {
                if (c < 0)
                    continue;
                if (c > 14)
                    break;
                Logs.writeln(" col = " + c);
                if (chessValue == cb.Data[baseRow][c])
                {
                    if (++count >= 5)
                    {
                        break;
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return count;
        }

        public static int checkInclinedFive1(ChessBoard cb, int beginCol, int endCol, int beginRow, int endRow, int chessValue)
        {
            int count = 0;
            for (int r = beginRow, c = beginCol; r <= endRow && c <= endCol; r++, c++)
            {
                if (r < 0 || c < 0)
                    continue;
                if (r > 14 || c > 14)
                    break;
                Logs.writeln("col = " + c + " row = " + r);
                if (chessValue == cb.Data[r][c])
                {
                    if (++count >= 5)
                    {
                        break;
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return count;
        }

        public static int checkInclinedFive2(ChessBoard cb, int beginCol, int endCol, int beginRow, int endRow, int chessValue)
        {
            int count = 0;
            for (int r = endRow, c = beginCol; r >= beginRow && c <= endCol; r--, c++)
            {
                if (r < 0 || c < 0)
                    continue;
                if (r > 14 || c > 14)
                    break;
                Logs.writeln("col = " + c + " row = " + r);
                if (chessValue == cb.Data[r][c])
                {
                    if (++count >= 5)
                    {
                        break;
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return count;
        }
    }
}
