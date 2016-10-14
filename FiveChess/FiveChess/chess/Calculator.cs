using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    class Calculator
    {
        public static bool isFullBoard(ChessBoard cb)
        {
            bool isFull = true;
            for (int row = 0; row < cb.Data.Length; row++)
            {
                for (int col = 0; col < cb.Data[row].Length; col++)
                {
                    if (cb.Data[row][col] != DEF.CHESS_BLACK && cb.Data[row][col] != DEF.CHESS_WHITE)
                    {
                        isFull = false;
                    }
                }
            }
            return isFull;
        }

        #region check if have N count in a line

        public static int hasVerticalCount(byte[][] data, int color, int count, int row, int col, out chessCountType type)
        {
            int beginRow = row - (count - 1), endRow = row + (count - 1);
            int value_count = 0;
            type = 0;

            //int tmp_row = beginRow;
            //while (tmp_row >= 0)
            //{
            //    if (data[tmp_row][col] == color)
            //    {
            //        tmp_row--;
            //    }
            //    else if(data[tmp_row][col] == DEF.CHESS_NONE)
            //    {
            //        type = chessCountType.BothLive;
            //    }
            //}

            for (int r = beginRow; r <= endRow; r++)
            {
                if (r < 0)
                    continue;
                if (r > DEF.MAX_ROW_ID)
                    break;
                Logs.writeln(" row = " + r);
                if (color == data[r][col])
                {
                    value_count++;
                }
                else
                {
                    if (value_count >= count)
                    {
                        return value_count;
                    }
                    else
                    {
                        value_count = 0;
                    }
                }
            }
            return value_count;
        }

        public static int hasHorizontalCount(byte[][] data, int color, int count, int row, int col, out chessCountType type)
        {
            int beginCol = col - (count - 1), endCol = col + (count - 1);
            int value_count = 0;
            type = 0;

            for (int c = beginCol; c <= endCol; c++)
            {
                if (c < 0)
                    continue;
                if (c > DEF.MAX_COL_ID)
                    break;
                Logs.writeln(" col = " + c);
                if (color == data[row][c])
                {
                    value_count++;
                }
                else
                {
                    if (value_count >= count)
                    {
                        return value_count;
                    }
                    else
                    {
                        value_count = 0;
                    }
                }
            }
            return value_count;
        }

        /// <summary>
        /// Check line count from left top to right bottom
        /// </summary>
        /// <param name="data"></param>
        /// <param name="color"></param>
        /// <param name="count"></param>
        /// <param name="beginRow"></param>
        /// <param name="endRow"></param>
        /// <param name="beginCol"></param>
        /// <param name="endCol"></param>
        /// <param name="targetCol"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int hasInclinedCount_LT(byte[][] data, int color, int count, int row, int col, out chessCountType type)
        {
            int beginCol = col - (count - 1), endCol = col + (count - 1);
            int beginRow = row - (count - 1), endRow = row + (count - 1);
            int value_count = 0;
            type = 0;

            for (int r = beginRow, c = beginCol; r <= endRow && c <= endCol; r++, c++)
            {
                if (r < 0 || c < 0)
                    continue;
                if (r > DEF.MAX_ROW_ID || c > DEF.MAX_COL_ID)
                    break;
                Logs.writeln("col = " + c + " row = " + r);
                if (color == data[r][c])
                {
                    value_count++;
                }
                else
                {
                    if (value_count >= count)
                    {
                        return value_count;
                    }
                    else
                    {
                        value_count = 0;
                    }
                }
            }
            return value_count;
        }

        /// <summary>
        /// Check line count from left bottom to right top
        /// </summary>
        /// <param name="data"></param>
        /// <param name="color"></param>
        /// <param name="count"></param>
        /// <param name="beginRow"></param>
        /// <param name="endRow"></param>
        /// <param name="beginCol"></param>
        /// <param name="endCol"></param>
        /// <param name="targetCol"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int hasInclinedCount_LB(byte[][] data, int color, int count, int row, int col, out chessCountType type)
        {
            int beginCol = col - (count - 1), endCol = col + (count - 1);
            int beginRow = row - (count - 1), endRow = row + (count - 1);
            int value_count = 0;
            type = 0;

            for (int r = endRow, c = beginCol; r >= beginRow && c <= endCol; r--, c++)
            {
                if (r < 0 || c < 0)
                    continue;
                if (r > DEF.MAX_ROW_ID || c > DEF.MAX_COL_ID)
                    break;
                Logs.writeln("col = " + c + " row = " + r);
                if (color == data[r][c])
                {
                    value_count++;
                }
                else
                {
                    if (value_count >= count)
                    {
                        return value_count;
                    }
                    else
                    {
                        value_count = 0;
                    }
                }
            }
            return value_count;
        }

        #endregion


        public static void FormatCountTypeHead(byte color, ref chessCountType type)
        {
            switch (color)
            {
                case DEF.CHESS_NONE:
                    type = chessCountType.BothLive;
                    break;
                default:
                    type = chessCountType.MinDeadMaxLive;
                    break;
            }
        }

        public static void FormatCountTypeEnd(byte color, ref chessCountType type)
        {
            switch (color)
            {
                case DEF.CHESS_NONE:
                    if (type == chessCountType.BothLive)
                    {
                        type = chessCountType.MinLiveMaxDead;
                    }
                    else if (type == chessCountType.MinDeadMaxLive)
                    {
                        type = chessCountType.BothDead;
                    }
                    break;
                default:
                    type = chessCountType.MinDeadMaxLive;
                    break;
            }
        }

        #region check how many same color chess in a line

        public static int calVerticalCount(byte[][] data, int color, int row, int col, out chessCountType type)
        {
            type = chessCountType.BothLive;

            int value_count = 0;
            bool cal_min = false, cal_max = false;

            int min_row = row;
            while (data[min_row][col] == color && min_row > 0)
            {
                min_row--;
                value_count++;
                cal_min = true;
            }
            if(data[min_row][col] != DEF.CHESS_NONE || min_row == 0)
            {
                type = chessCountType.MinDeadMaxLive;
            }

            int max_row = row;
            while (data[max_row][col] == color && max_row < DEF.MAX_ROW_ID)
            {
                max_row++;
                value_count++;
                cal_max = true;
            }

            if (max_row == DEF.MAX_ROW_ID || data[max_row][col] != DEF.CHESS_NONE)
            {
                if(type == chessCountType.BothLive)
                {
                    type = chessCountType.MinLiveMaxDead;
                }
                else
                {
                    type = chessCountType.BothDead;
                }
            }

            //检测是否多算了一次
            value_count -= (cal_max && cal_min) ? 1 : 0;
            Logs.writeln("count = " + value_count + "   type = " + type);
            return value_count;
        }

        public static int calHorizontalCount(byte[][] data, int color, int row, int col, out chessCountType type)
        {
            type = chessCountType.BothLive;

            int value_count = 0;
            bool cal_min = false, cal_max = false;

            int min_col = col;
            while (data[row][min_col] == color && min_col > 0)
            {
                min_col--;
                value_count++;
                cal_min = true;
            }
            if (data[row][min_col] != DEF.CHESS_NONE || min_col == 0)
            {
                type = chessCountType.MinDeadMaxLive;
            }

            int max_col = col;
            while (data[row][max_col] == color && max_col < DEF.MAX_COL_ID)
            {
                max_col++;
                value_count++;
                cal_max = true;
            }

            if (max_col == DEF.MAX_COL_ID || data[row][max_col] != DEF.CHESS_NONE)
            {
                if (type == chessCountType.BothLive)
                {
                    type = chessCountType.MinLiveMaxDead;
                }
                else
                {
                    type = chessCountType.BothDead;
                }
            }

            //检测是否多算了一次
            value_count -= (cal_max && cal_min) ? 1 : 0;
            Logs.writeln("count = " + value_count + "   type = " + type, 4);
            return value_count;
        }

        /// <summary>
        /// Check line count from left top to right bottom
        /// </summary>
        /// <param name="data"></param>
        /// <param name="color"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int calInclinedCount_LT(byte[][] data, int color, int row, int col, out chessCountType type)
        {
            int value_count = 0;
            type = 0;


            return value_count;
        }

        /// <summary>
        /// Check line count from left top to right bottom
        /// </summary>
        /// <param name="data"></param>
        /// <param name="color"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int calInclinedCount_LB(byte[][] data, int color, int row, int col, out chessCountType type)
        {
            int value_count = 0;
            type = 0;


            return value_count;
        }
        #endregion
    }
}
