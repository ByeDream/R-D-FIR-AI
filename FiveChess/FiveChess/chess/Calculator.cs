using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gobang
{
    class Calculator
    {
        public static bool isFullBoard(int [][] data)
        {
            bool isFull = true;
            for (int row = 0; row < data.Length; row++)
            {
                for (int col = 0; col < data[row].Length; col++)
                {
                    if (data[row][col] != Color.BLACK && data[row][col] != Color.WHITE)
                    {
                        isFull = false;
                    }
                }
            }
            return isFull;
        }

        #region check if have N count in a line

        public static int hasVerticalCount(int[][] data, int color, int count, int row, int col)
        {
            int beginRow = row - (count - 1), endRow = row + (count - 1);
            int value_count = 0;

            for (int r = beginRow; r <= endRow; r++)
            {
                if (r < 0)
                    continue;
                if (r > Side.ROW_ID)
                    break;
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

        public static int hasHorizontalCount(int[][] data, int color, int count, int row, int col)
        {
            int beginCol = col - (count - 1), endCol = col + (count - 1);
            int value_count = 0;

            for (int c = beginCol; c <= endCol; c++)
            {
                if (c < 0)
                    continue;
                if (c > Side.COL_ID)
                    break;
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

        public static int hasInclinedCount_LT(int[][] data, int color, int count, int row, int col)
        {
            int beginCol = col - (count - 1), endCol = col + (count - 1);
            int beginRow = row - (count - 1), endRow = row + (count - 1);
            int value_count = 0;

            for (int r = beginRow, c = beginCol; r <= endRow && c <= endCol; r++, c++)
            {
                if (r < 0 || c < 0)
                    continue;
                if (r > Side.ROW_ID || c > Side.COL_ID)
                    break;
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

        public static int hasInclinedCount_LB(int[][] data, int color, int count, int row, int col)
        {
            int beginCol = col - (count - 1), endCol = col + (count - 1);
            int beginRow = row - (count - 1), endRow = row + (count - 1);
            int value_count = 0;

            for (int r = endRow, c = beginCol; r >= beginRow && c <= endCol; r--, c++)
            {
                if (r > Side.ROW_ID || c < 0)
                    continue;
                if (r < 0 || c > Side.COL_ID)
                    break;
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

        #region 计算一个坐标点的一条线上有多少个连续的棋

        private static bool breal_h = false;
        private static int color_h = 0;
        private static int count_h = 0;
        private static int min_col_h = 0;
        private static int max_col_h = 0;
        public static int calHorizontalCount(int[][] data, int row, int col, ref Line line)
        {
            count_h = 1;
            color_h = data[row][col];
            line.color = color_h;
            line.p1.row = line.p2.row = row;
            line.p1.col = line.p2.col = col;

            min_col_h = col - 1;
            while (min_col_h >= 0 && data[row][min_col_h] == color_h)
            {
                line.p1.row = row;
                line.p1.col = min_col_h;
                count_h++;
                min_col_h--;
            }
            if (min_col_h < 0 || data[row][min_col_h] != Color.NONE)
            {
                line.type = LineType.MinDeadMaxLive;
            }
            else if(min_col_h >= 0)
            {
                breal_h = false;
                while (min_col_h >= 0)
                {
                    if(data[row][min_col_h] != Color.NONE && data[row][min_col_h] != color_h)
                    {
                        break;
                    }
                    else if(data[row][min_col_h] == color_h)
                    {
                        breal_h = true;
                    }
                    else if(!breal_h)
                    {
                        line.minSpace++;
                    }
                    line.realSpace++;
                    min_col_h--;
                }
            }

            max_col_h = col + 1;
            while (max_col_h <= Side.COL_ID && data[row][max_col_h] == color_h)
            {
                line.p2.row = row;
                line.p2.col = max_col_h;
                count_h++;
                max_col_h++;
            }

            if (max_col_h > Side.COL_ID || data[row][max_col_h] != Color.NONE)
            {
                if (line.type == LineType.BothLive)
                {
                    line.type = LineType.MinLiveMaxDead;
                }
                else
                {
                    line.type = LineType.BothDead;
                }
            }
            else if (max_col_h <= Side.COL_ID)
            {
                breal_h = false;
                while (max_col_h <= Side.COL_ID)
                {
                    if (data[row][max_col_h] != Color.NONE && data[row][max_col_h] != color_h)
                    {
                        break;
                    }
                    else if (data[row][max_col_h] == color_h)
                    {
                        breal_h = true;
                    }
                    else if(!breal_h)
                    {
                        line.maxSpace++;
                    }
                    line.realSpace++;
                    max_col_h++;
                }
            }

            line.length = count_h;
            line.realSpace += count_h;
            line.space = line.minSpace + line.length + line.maxSpace;

            //if(line.length > 1)
            //{
            //    line.direction = Direction.Horizontal;
            //}
            return line.length;
        }

        private static bool breal_v = false;
        private static int color_v = 0;
        private static int count_v = 0;
        private static int min_row_v = 0;
        private static int max_row_v = 0;
        public static int calVerticalCount(int[][] data, int row, int col, ref Line line)
        {
            count_v = 1;
            color_v = data[row][col];
            line.color = color_v;
            line.p1.row = line.p2.row = row;
            line.p1.col = line.p2.col = col;

            min_row_v = row - 1;
            while (min_row_v >= 0 && data[min_row_v][col] == color_v)
            {
                line.p1.row = min_row_v;
                line.p1.col = col;
                min_row_v--;
                count_v++;
            }
            if (min_row_v < 0 || data[min_row_v][col] != Color.NONE)
            {
                line.type = LineType.MinDeadMaxLive;
            }
            else if (min_row_v >= 0)
            {
                breal_v = false;
                while (min_row_v >= 0)
                {
                    if (data[min_row_v][col] != Color.NONE && data[min_row_v][col] != color_v)
                    {
                        break;
                    }
                    else if (data[min_row_v][col] == color_v)
                    {
                        breal_v = true;
                    }
                    else if (!breal_v)
                    {
                        line.minSpace++;
                    }
                    line.realSpace++;
                    min_row_v--;
                }
            }

            max_row_v = row + 1;
            while (max_row_v <= Side.ROW_ID && data[max_row_v][col] == color_v)
            {
                line.p2.row = max_row_v;
                line.p2.col = col;
                max_row_v++;
                count_v++;
            }

            if (max_row_v > Side.ROW_ID || data[max_row_v][col] != Color.NONE)
            {
                if (line.type == LineType.BothLive)
                {
                    line.type = LineType.MinLiveMaxDead;
                }
                else
                {
                    line.type = LineType.BothDead;
                }
            }
            else if (max_row_v <= Side.ROW_ID)
            {
                breal_v = false;
                while (max_row_v <= Side.COL_ID)
                {
                    if (data[max_row_v][col] != Color.NONE && data[max_row_v][col] != color_v)
                    {
                        break;
                    }
                    else if (data[max_row_v][col] == color_v)
                    {
                        breal_v = true;
                    }
                    else if (!breal_v)
                    {
                        line.maxSpace++;
                    }
                    line.realSpace++;
                    max_row_v++;
                }
            }

            line.length = count_v;
            line.realSpace += count_v;
            line.space = line.minSpace + line.length + line.maxSpace;
            //if (line.length > 1)
            //{
            //    line.direction = Direction.Vertical;
            //}
            return line.length;
        }

        private static bool breal_lt = false;
        private static int color_lt = 0;
        private static int min_row_lt = 0;
        private static int max_row_lt = 0;
        private static int min_col_lt = 0;
        private static int max_col_lt = 0;
        private static int count_lt = 0;

        public static int calInclinedCount_LT(int[][] data, int row, int col, ref Line line)
        {
            count_lt = 1;
            color_lt = data[row][col];
            line.color = color_lt;
            line.p1.row = line.p2.row = row;
            line.p1.col = line.p2.col = col;

            min_row_lt = row - 1;
            min_col_lt = col - 1;
            while (min_col_lt >= 0 && min_row_lt >= 0 && data[min_row_lt][min_col_lt] == color_lt)
            {
                line.p1.row = min_row_lt;
                line.p1.col = min_col_lt;

                min_row_lt--;
                min_col_lt--;
                count_lt++;
            }
            if (min_col_lt < 0 || min_row_lt < 0 || data[min_row_lt][min_col_lt] != Color.NONE)
            {
                line.type = LineType.MinDeadMaxLive;
            }
            else if (min_col_lt >= 0 && min_row_lt >= 0)
            {
                breal_lt = false;
                while (min_col_lt >= 0 && min_row_lt >= 0)
                {
                    if (data[min_row_lt][min_col_lt] != Color.NONE && data[min_row_lt][min_col_lt] != color_lt)
                    {
                        break;
                    }
                    else if (data[min_row_lt][min_col_lt] == color_lt)
                    {
                        breal_lt = true;
                    }
                    else if (!breal_lt)
                    {
                        line.minSpace++;
                    }
                    line.realSpace++;
                    min_row_lt--;
                    min_col_lt--;
                }
            }

            max_row_lt = row + 1;
            max_col_lt = col + 1;
            while (max_col_lt <= Side.COL_ID && max_row_lt <= Side.ROW_ID && data[max_row_lt][max_col_lt] == color_lt)
            {
                line.p2.row = max_row_lt;
                line.p2.col = max_col_lt;

                max_row_lt++;
                max_col_lt++;
                count_lt++;
            }

            if (max_col_lt > Side.COL_ID || max_row_lt > Side.ROW_ID || data[max_row_lt][max_col_lt] != Color.NONE)
            {
                if (line.type == LineType.BothLive)
                {
                    line.type = LineType.MinLiveMaxDead;
                }
                else
                {
                    line.type = LineType.BothDead;
                }
            }
            else if (max_col_lt <= Side.COL_ID && max_row_lt <= Side.ROW_ID)
            {
                breal_lt = false;
                while (max_col_lt <= Side.COL_ID && max_row_lt <= Side.COL_ID)
                {
                    if (data[max_row_lt][max_col_lt] != Color.NONE && data[max_row_lt][max_col_lt] != color_lt)
                    {
                        break;
                    }
                    else if (data[max_row_lt][max_col_lt] == color_lt)
                    {
                        breal_lt = true;
                    }
                    else if (!breal_lt)
                    {
                        line.maxSpace++;
                    }
                    line.realSpace++;
                    max_row_lt++;
                    max_col_lt++;
                }
            }

            //Logs.writeln("calInclinedCount_LT count = " + count_lt + "   type = " + line.type, 2);

            line.length = count_lt;
            line.realSpace += count_lt;
            line.space = line.minSpace + line.length + line.maxSpace;
            //if (line.length > 1)
            //{
            //    line.direction = Direction.LetfTopToRightBottom;
            //}
            return line.length;
        }

        private static bool breal_lb = false;
        private static int color_lb = 0;
        private static int min_row_lb = 0;
        private static int max_row_lb = 0;
        private static int min_col_lb = 0;
        private static int max_col_lb = 0;
        private static int count_lb = 0;

        public static int calInclinedCount_LB(int[][] data, int row, int col, ref Line line)
        {
            count_lb = 1;
            color_lb = data[row][col];
            line.color = color_lb;
            line.p1.row = line.p2.row = row;
            line.p1.col = line.p2.col = col;

            min_col_lb = col - 1;
            max_row_lb = row + 1;
            while (min_col_lb >= 0 && max_row_lb <= Side.ROW_ID && data[max_row_lb][min_col_lb] == color_lb)
            {
                line.p1.row = max_row_lb;
                line.p1.col = min_col_lb;

                max_row_lb++;
                min_col_lb--;
                count_lb++;
            }
            if (min_col_lb < 0 || max_row_lb > Side.ROW_ID || data[max_row_lb][min_col_lb] != Color.NONE)
            {
                line.type = LineType.MinDeadMaxLive;
            }
            else if (min_col_lb >= 0 && max_row_lb <= Side.ROW_ID)
            {
                breal_lb = false;
                while (min_col_lb >= 0 && max_row_lb <= Side.ROW_ID)
                {
                    if (data[max_row_lb][min_col_lb] != Color.NONE && data[max_row_lb][min_col_lb] != color_lb)
                    {
                        break;
                    }
                    else if (data[max_row_lb][min_col_lb] == color_lb)
                    {
                        breal_lb = true;
                    }
                    else if (!breal_lb)
                    {
                        line.minSpace++;
                    }
                    line.realSpace++;
                    max_row_lb++;
                    min_col_lb--;
                }
            }

            min_row_lb = row - 1;
            max_col_lb = col + 1;
            while (max_col_lb <= Side.COL_ID && min_row_lb >= 0 && data[min_row_lb][max_col_lb] == color_lb)
            {
                line.p2.row = min_row_lb;
                line.p2.col = max_col_lb;

                min_row_lb--;
                max_col_lb++;
                count_lb++;
            }

            if (max_col_lb > Side.COL_ID || min_row_lb < 0 || data[min_row_lb][max_col_lb] != Color.NONE)
            {
                if (line.type == LineType.BothLive)
                {
                    line.type = LineType.MinLiveMaxDead;
                }
                else
                {
                    line.type = LineType.BothDead;
                }
            }
            else if (max_col_lb <= Side.COL_ID && min_row_lb >= 0)
            {
                breal_lb = false;
                while (max_col_lb <= Side.COL_ID && min_row_lb >= 0)
                {
                    if (data[min_row_lb][max_col_lb] != Color.NONE && data[min_row_lb][max_col_lb] != color_lb)
                    {
                        break;
                    }
                    else if (data[min_row_lb][max_col_lb] == color_lb)
                    {
                        breal_lb = true;
                    }
                    else if (!breal_lb)
                    {
                        line.maxSpace++;
                    }
                    line.realSpace++;
                    min_row_lb--;
                    max_col_lb++;
                }
            }

            line.length = count_lb;
            line.realSpace += count_lb;
            line.space = line.minSpace + line.length + line.maxSpace;
            //if (line.length > 1)
            //{
            //    line.direction = Direction.LetfBottomToRightTop;
            //}
            return line.length;
        }

        #endregion

        #region 计算一条线上是否有不连续的同颜色的棋 或则 有连续的其他颜色的棋

        private static int tmp_color = 0;

        //计算水平方向一串棋首尾是否还有不连续的棋 或则 有其他颜色的棋
        public static void calHorizontalLine(int[][] data, int color, Line src, ref Line line1, ref Line line2)
        {
            tmp_color = (color == Color.BLACK? Color.WHITE:Color.BLACK);
             if (src.p1.col >= 1 && data[src.p1.row][src.p1.col - 1] == tmp_color)
            {
                calHorizontalCount(data, src.p1.row, src.p1.col - 1, ref line1);
            }
            //计算是否有不连续的棋, 至少边界要有两个空位置才有可能
            else if (src.p1.col >= 2 && data[src.p1.row][src.p1.col - 2] != Color.NONE)
            {
                calHorizontalCount(data, src.p1.row, src.p1.col - 2, ref line1);
            }
            
            if (src.p2.col <= Side.COL_ID - 1 && data[src.p2.row][src.p2.col + 1] == tmp_color)
            {
                calHorizontalCount(data, src.p2.row, src.p2.col + 1, ref line2);
            }
            else if (src.p2.col <= Side.COL_ID - 2 && data[src.p2.row][src.p2.col + 2] != Color.NONE)
            {
                calHorizontalCount(data, src.p2.row, src.p2.col + 2, ref line2);
            }
        }

        //计算垂直方向一串棋首尾是否还有不连续的棋
        public static void calVerticalLine(int[][] data, int color, Line src, ref Line line1, ref Line line2)
        {
            tmp_color = (color == Color.BLACK ? Color.WHITE : Color.BLACK);
            if (src.p1.row >= 1 && data[src.p1.row - 1][src.p1.col] == tmp_color)
            {
                calVerticalCount(data, src.p1.row - 1, src.p1.col, ref line1);
            }
            //计算是否有不连续的棋, 至少边界要有两个空位置才有可能
            else if (src.p1.row >= 2 && data[src.p1.row - 2][src.p1.col] != Color.NONE)
            {
                calVerticalCount(data, src.p1.row - 2, src.p1.col, ref line1);
            }

            if (src.p2.row <= Side.ROW_ID - 1 && data[src.p2.row + 1][src.p2.col] == tmp_color)
            {
                calVerticalCount(data, src.p2.row + 1, src.p2.col, ref line2);
            }
            else if (src.p2.row <= Side.ROW_ID - 2 && data[src.p2.row + 2][src.p2.col] != Color.NONE)
            {
                calVerticalCount(data, src.p2.row + 2, src.p2.col, ref line2);
           }
        }

        //计算左上到右下方向一串棋首尾是否还有不连续的棋
        public static void calInclined_LT(int[][] data, int color, Line src, ref Line line1, ref Line line2)
        {
            tmp_color = (color == Color.BLACK ? Color.WHITE : Color.BLACK);
            if (src.p1.col >= 1 && src.p1.row >= 1 && data[src.p1.row - 1][src.p1.col - 1] == tmp_color)
            {
                calInclinedCount_LT(data, src.p1.row - 1, src.p1.col - 1, ref line1);
            }
            //计算是否有不连续的棋, 至少边界要有两个空位置才有可能
            else if (src.p1.col >= 2 && src.p1.row >= 2 && data[src.p1.row - 2][src.p1.col - 2] != Color.NONE)
            {
                calInclinedCount_LT(data, src.p1.row - 2, src.p1.col - 2, ref line1);
            }

            if (src.p2.col <= Side.COL_ID - 1 && src.p2.row <= Side.ROW_ID - 1 && data[src.p2.row + 1][src.p2.col + 1] == tmp_color)
            {
                calInclinedCount_LT(data, src.p2.row + 1, src.p2.col + 1, ref line2);
            }
            else if (src.p2.col <= Side.COL_ID - 2 && src.p2.row <= Side.ROW_ID - 2 && data[src.p2.row + 2][src.p2.col + 2] != Color.NONE)
            {
                calInclinedCount_LT(data, src.p2.row + 2, src.p2.col + 2, ref line2);
            }
        }

        //计算左下到右上方向一串棋首尾是否还有不连续的棋
        public static void calInclined_LB(int[][] data, int color, Line src, ref Line line1, ref Line line2)
        {
            tmp_color = (color == Color.BLACK ? Color.WHITE : Color.BLACK);

            if (src.p1.col >= 1 && src.p1.row <= Side.ROW_ID - 1 && data[src.p1.row + 1][src.p1.col - 1] == tmp_color)
            {
                calInclinedCount_LB(data, src.p1.row + 1, src.p1.col - 1, ref line1);
            }
            //计算是否有不连续的棋, 至少边界要有两个空位置才有可能
            else if (src.p1.col >= 2 && src.p1.row <= Side.ROW_ID - 2 && data[src.p1.row + 2][src.p1.col - 2] != Color.NONE)
            {
                calInclinedCount_LB(data, src.p1.row + 2, src.p1.col - 2, ref line1);
            }

            if (src.p2.row >= 1 && src.p2.col <= Side.COL_ID - 1 && data[src.p2.row - 1][src.p2.col + 1] == tmp_color)
            {
                calInclinedCount_LB(data, src.p2.row - 1, src.p2.col + 1, ref line2);
            }
            else if (src.p2.row >= 2 && src.p2.col <= Side.COL_ID - 2 && data[src.p2.row - 2][src.p2.col + 2] != Color.NONE)
            {
                calInclinedCount_LB(data, src.p2.row - 2, src.p2.col + 2, ref line2);
            }
        }

        #endregion

        #region 计算一条线上是否有不连续的同颜色的棋

        //计算水平方向一串棋首尾是否还有不连续的棋
        public static void calHorizontalLines(int[][] data, int color, Line src, ref Line front, ref Line end)
        {
            //计算是否有不连续的棋, 至少边界要有两个空位置才有可能
            if (src.p1.col >= 2 && data[src.p1.row][src.p1.col - 2] != Color.NONE)
            {
                calHorizontalCount(data, src.p1.row, src.p1.col - 2, ref front);
            }

            if (src.p2.col <= Side.COL_ID - 2 && data[src.p2.row][src.p2.col + 2] != Color.NONE)
            {
                calHorizontalCount(data, src.p2.row, src.p2.col + 2, ref end);
            }
        }

        //计算垂直方向一串棋首尾是否还有不连续的棋
        public static void calVerticalLines(int[][] data, int color, Line src, ref Line front, ref Line end)
        {
            if (src.p1.row >= 2 && data[src.p1.row - 2][src.p1.col] != Color.NONE)
            {
                calVerticalCount(data, src.p1.row - 2, src.p1.col, ref front);
            }

            if (src.p2.row <= Side.ROW_ID - 2 && data[src.p2.row + 2][src.p2.col] != Color.NONE)
            {
                calVerticalCount(data, src.p2.row + 2, src.p2.col, ref end);
            }
        }

        //计算左上到右下方向一串棋首尾是否还有不连续的棋
        public static void calInclined_LTs(int[][] data, int color, Line src, ref Line front, ref Line end)
        {
            if (src.p1.col >= 2 && src.p1.row >= 2 && data[src.p1.row - 2][src.p1.col - 2] != Color.NONE)
            {
                calInclinedCount_LT(data, src.p1.row - 2, src.p1.col - 2, ref front);
            }
            if (src.p2.col <= Side.COL_ID - 2 && src.p2.row <= Side.ROW_ID - 2 && data[src.p2.row + 2][src.p2.col + 2] != Color.NONE)
            {
                calInclinedCount_LT(data, src.p2.row + 2, src.p2.col + 2, ref end);
            }
        }

        //计算左下到右上方向一串棋首尾是否还有不连续的棋
        public static void calInclined_LBs(int[][] data, int color, Line src, ref Line front, ref Line end)
        {
            if (src.p1.col >= 2 && src.p1.row <= Side.ROW_ID - 2 && data[src.p1.row + 2][src.p1.col - 2] != Color.NONE)
            {
                calInclinedCount_LB(data, src.p1.row + 2, src.p1.col - 2, ref front);
            }
            if (src.p2.row >= 2 && src.p2.col <= Side.COL_ID - 2 && data[src.p2.row - 2][src.p2.col + 2] != Color.NONE)
            {
                calInclinedCount_LB(data, src.p2.row - 2, src.p2.col + 2, ref end);
            }
        }
        #endregion

        #region 计算一串棋的价值
        private static int l_cost1 = 0;
        public static int calCost(int length, LineType type)
        {
            switch (length)
            {
                case 0:
                    return l_cost1 = 0;
                case 1:
                    {
                        switch (type)
                        {
                            case LineType.BothDead:
                                l_cost1 = 0;
                                break;
                            case LineType.BothLive:
                                l_cost1 = Cost.One_1;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                l_cost1 = Cost.One_d1;
                                break;
                        }
                        break;
                    }
                case 2:
                    {
                        switch (type)
                        {
                            case LineType.BothDead:
                                l_cost1 = 0;
                                break;
                            case LineType.BothLive:
                                l_cost1 = Cost.Tow_11;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                l_cost1 = Cost.Tow_d11;
                                break;
                        }
                        break;
                    }
                case 3:
                    {
                        switch (type)
                        {
                            case LineType.BothDead:
                                l_cost1 = 0;
                                break;
                            case LineType.BothLive:
                                l_cost1 = Cost.Three_111;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                l_cost1 = Cost.Three_d111;
                                break;
                        }
                        break;
                    }
                case 4:
                    {
                        switch (type)
                        {
                            case LineType.BothDead:
                                l_cost1 = 0;
                                break;
                            case LineType.BothLive:
                                l_cost1 = Cost.Four_1111;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                l_cost1 = Cost.Four_d1111;
                                break;
                        }
                        break;
                    }
                default:
                    l_cost1 = Cost.Fiv_11111;
                    break;
            }

            return l_cost1;
        }

        private static int l_cost2 = 0;
        public static int calCost(ref Line line)
        {
            l_cost2 = 0;
            switch (line.length)
            {
                case 0:
                    return l_cost2 = 0;
                case 1:
                    {
                        switch (line.type)
                        {
                            case LineType.BothDead:
                                l_cost2 = 0;
                                break;
                            case LineType.BothLive:
                                l_cost2 = Cost.One_1;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                l_cost2 = Cost.One_d1;
                                break;
                        }
                        break;
                    }
                case 2:
                    {
                        switch (line.type)
                        {
                            case LineType.BothDead:
                                l_cost2 = 0;
                                break;
                            case LineType.BothLive:
                                l_cost2 = Cost.Tow_11;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                l_cost2 = Cost.Tow_d11;
                                break;
                        }
                        break;
                    }
                case 3:
                    {
                        switch (line.type)
                        {
                            case LineType.BothDead:
                                l_cost2 = 0;
                                break;
                            case LineType.BothLive:
                                l_cost2 = Cost.Three_111;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                l_cost2 = Cost.Three_d111;
                                break;
                        }
                        break;
                    }
                case 4:
                    {
                        switch (line.type)
                        {
                            case LineType.BothDead:
                                l_cost2 = 0;
                                break;
                            case LineType.BothLive:
                                l_cost2 = Cost.Four_1111;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                l_cost2 = Cost.Four_d1111;
                                break;
                        }
                        break;
                    }
                default:
                    l_cost2 = Cost.Fiv_11111;
                    break;
            }

            line.cost = l_cost2;
            return l_cost2;
        }

        private static int towLineCost = 0;
        public static int calTowLineCost(int[][] data, ref Line front, ref Line end)
        {
            towLineCost = 0;
            switch (front.length)
            {
                case 1:
                    switch (end.length)
                    {
                        case 1:
                            if (front.type == LineType.MinDeadMaxLive || end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Tow_d101;
                            }
                            else
                            {
                                towLineCost = Cost.Tow_101;
                            }
                            break;
                        case 2:
                            if (front.type == LineType.MinDeadMaxLive)
                            {
                                towLineCost = Cost.Three_d1011;
                            }
                            else if (end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Three_d1101;
                            }
                            else
                            {
                                towLineCost = Cost.Three_1011;
                            }
                            break;
                        case 3:
                            if (front.type == LineType.MinDeadMaxLive)
                            {
                                towLineCost = Cost.Four_d10111;
                            }
                            else if (end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Four_d11101;
                            }
                            else
                            {
                                towLineCost = Cost.Four_11101;
                            }
                            break;
                        case 4:
                            if (front.type == LineType.MinDeadMaxLive)
                            {
                                towLineCost = Cost.Fiv_d101111;
                            }
                            else if (end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Fiv_d111101;
                            }
                            else
                            {
                                towLineCost = Cost.Fiv_101111;
                            }
                            break;
                        default:
                            towLineCost = Cost.Fiv_11111;
                            break;
                    }
                    break;
                case 2:
                    switch (end.length)
                    {
                        case 1:
                            if (front.type == LineType.MinDeadMaxLive)
                            {
                                towLineCost = Cost.Three_d1101;
                            }
                            else if (end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Three_d1011;
                            }
                            else
                            {
                                towLineCost = Cost.Three_1011;
                            }
                            break;
                        case 2:
                            if (front.type == LineType.MinDeadMaxLive || end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Four_d11011;
                            }
                            else
                            {
                                towLineCost = Cost.Four_11011;
                            }
                            break;
                        case 3:
                            if (front.type == LineType.MinDeadMaxLive)
                            {
                                towLineCost = Cost.Fiv_d110111;
                            }
                            else if (end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Fiv_d111011;
                            }
                            else
                            {
                                towLineCost = Cost.Four_11101;
                            }
                            break;
                        case 4:
                            if (front.type == LineType.MinDeadMaxLive)
                            {
                                towLineCost = Cost.Six_d1101111;
                            }
                            else if (end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Six_d1101111;
                            }
                            else
                            {
                                towLineCost = Cost.Six_d1111011;
                            }
                            break;
                        default:
                            towLineCost = Cost.Fiv_11111;
                            break;
                    }
                    break;
                case 3:
                    switch (end.length)
                    {
                        case 1:
                            if (front.type == LineType.MinDeadMaxLive)
                            {
                                towLineCost = Cost.Four_d10111;
                            }
                            else if (end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Four_d11101;
                            }
                            else
                            {
                                towLineCost = Cost.Four_11101;
                            }
                            break;
                        case 2:
                            if (front.type == LineType.MinDeadMaxLive)
                            {
                                towLineCost = Cost.Fiv_d111011;
                            }
                            else if (end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Fiv_d110111;
                            }
                            else
                            {
                                towLineCost = Cost.Fiv_110111;
                            }
                            break;
                        case 3:
                            if (front.type == LineType.MinDeadMaxLive || end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Six_d1110111;
                            }
                            else
                            {
                                towLineCost = Cost.Six_1110111;
                            }
                            break;
                        case 4:
                            if (front.type == LineType.MinDeadMaxLive)
                            {
                                towLineCost = Cost.Fiv_d101111;
                            }
                            else if (end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Fiv_d111101;
                            }
                            else
                            {
                                towLineCost = Cost.Seven_11101111;
                            }
                            break;
                        default:
                            towLineCost = Cost.Fiv_11111;
                            break;
                    }
                    break;
                case 4:
                    switch (end.length)
                    {
                        case 1:
                            if (front.type == LineType.MinDeadMaxLive)
                            {
                                towLineCost = Cost.Fiv_d111101;
                            }
                            else if (end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Fiv_d101111;
                            }
                            else
                            {
                                towLineCost = Cost.Fiv_101111;
                            }
                            break;
                        case 2:
                            if (front.type == LineType.MinDeadMaxLive)
                            {
                                towLineCost = Cost.Six_d1111011;
                            }
                            else if (end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Six_d1101111;
                            }
                            else
                            {
                                towLineCost = Cost.Six_1101111;
                            }
                            break;
                        case 3:
                            if (front.type == LineType.MinDeadMaxLive)
                            {
                                towLineCost = Cost.Seven_d11101111;
                            }
                            else if (end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Seven_d11110111;
                            }
                            else
                            {
                                towLineCost = Cost.Seven_11101111;
                            }
                            break;
                        case 4:
                            if (front.type == LineType.MinDeadMaxLive || end.type == LineType.MinLiveMaxDead)
                            {
                                towLineCost = Cost.Eight_d111101111;
                            }
                            else
                            {
                                towLineCost = Cost.Eight_111101111;
                            }
                            break;
                        default:
                            towLineCost = Cost.Fiv_11111;
                            break;
                    }
                    break;
                default:
                    towLineCost = Cost.Fiv_11111;
                    break;
            }
            return towLineCost;
        }

        private static int threeLineCost = 0;
        public static int calTowLineCost(int[][] data, ref Line front, ref Line mid, ref Line end)
        {
            if (mid.length + front.length >= 4 && mid.length + end.length >= 4)
            {
                return 3000;
            }


            if(front.length > end.length)
            {
                return calTowLineCost(data, ref front, ref end) + calCost(ref end);
            }
            else
            {
                return calTowLineCost(data, ref mid, ref end) + calCost(ref front);
            }
        }

        #endregion

        #region 计算落子之后一串棋增加的价值
        private static int cal_in_cost = 0;
        public static int calInCreaseCost(Line line, Position p)
        {
            cal_in_cost = 0;
            switch (line.type)
            {
                case LineType.BothDead: //小头（左上方）大头(右下方)都被堵死
                    switch (line.length)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            cal_in_cost = 0;
                            break;
                        default:
                            cal_in_cost = Cost.Fiv_11111;
                            break;
                    }
                    break;
                case LineType.MinDeadMaxLive: //小头（左上方）堵死，大头(右下方)活着
                case LineType.MinLiveMaxDead: //小头（左上方）活着，大头(右下方)堵死
                    switch (line.length)
                    {
                        case 1:
                            cal_in_cost = Cost.One_d1;
                            break;
                        case 2:
                            cal_in_cost = Cost.One_add_1_a;
                            break;
                        case 3:
                            cal_in_cost = Cost.Tow_add_1_a;
                            break;
                        case 4:
                            cal_in_cost = Cost.Three_add_1;
                            break;
                        default:
                            cal_in_cost = Cost.Fiv_11111;
                            break;
                    }
                    break;
                case LineType.BothLive: //大头小头都活着
                    switch (line.length)
                    {
                        case 1:
                            cal_in_cost = Cost.One_1;
                            break;
                        case 2:
                            cal_in_cost = Cost.One_add_1_b;
                            break;
                        case 3:
                            cal_in_cost = Cost.Two_add_1_b;
                            break;
                        case 4:
                            cal_in_cost = Cost.Tree_add_1_a;
                            break;
                        default:
                            cal_in_cost = Cost.Fiv_11111;
                            break;
                    }
                    break;
                default:
                    cal_in_cost = 0;
                    break;
            }
            return cal_in_cost;
        }

        private static int cal_in_cost1 = 0;
        public static int calInCreasePrevCost(int[][] data, ref Line min, ref Line mid, ref Position p)
        {
            cal_in_cost1 = 0;

            if (min.color == mid.color)
            {
                if (p.row == mid.p1.row && p.col == mid.p1.col)
                {
                    switch(min.length)
                    {
                        case 1:
                            cal_in_cost1 += Cost.One_1 - 5;
                            break;
                        case 2:
                            cal_in_cost1 += Cost.Tow_11 - 5;
                            break;
                        case 3:
                            cal_in_cost1 += Cost.Three_111 - 5;
                            break;
                        default:
                            cal_in_cost1 += Cost.Four_1111 - 5;
                            break;
                    }
                }

                if (mid.realSpace == 5)
                {
                    cal_in_cost1 += calInCreaseCost(mid, p);
                    cal_in_cost1 = cal_in_cost1 / 2;
                }
                else if (mid.realSpace > 5)
                {
                    cal_in_cost1 += calInCreaseCost(mid, p);
                }
                else
                {
                    cal_in_cost1 = 0;
                }
            }
            else
            {
                if (mid.realSpace == 5)
                {
                    cal_in_cost1 += calInCreaseCost(mid, p) / 2;
                }
                else if (mid.realSpace > 5)
                {
                    cal_in_cost1 += calInCreaseCost(mid, p);
                }

                if (p.row == mid.p1.row && p.col == mid.p1.col)
                {
                    if ((mid.length == 1 ? mid.space + min.realSpace - min.maxSpace : 1 + min.realSpace) >= 5
                        && (min.maxSpace == 0 || min.realSpace < 5))
                    {
                        switch (min.length)
                        {
                            case 1:
                                cal_in_cost1 += Cost.One_1 + 5;
                                break;
                            case 2:
                                cal_in_cost1 += Cost.Tow_11 + 5;
                                break;
                            case 3:
                                cal_in_cost1 += Cost.Three_111 + 5;
                                break;
                            default:
                                cal_in_cost1 += Cost.Four_1111 + 5;
                                break;

                        }
                    }
                    //else if (min.realSpace == 5 && min.length < 3)
                    //{
                    //    cal_in_cost1 += 300 * min.length / 2 + 10;
                    //}
                }
            }

            return cal_in_cost1;
        }

        private static int cal_in_cost2 = 0;
        public static int calInCreaseBehindCost(int[][] data, ref Line mid, ref Line max, ref Position p)
        {
            cal_in_cost2 = 0;
            if (mid.color == max.color)
            {
                if (p.row == mid.p2.row && p.col == mid.p2.col)
                {
                    switch (max.length)
                    {
                        case 1:
                            cal_in_cost1 += Cost.One_1 - 5;
                            break;
                        case 2:
                            cal_in_cost1 += Cost.Tow_11 - 5;
                            break;
                        case 3:
                            cal_in_cost1 += Cost.Three_111 - 5;
                            break;
                        case 4:
                            cal_in_cost1 += Cost.Four_1111 - 5;
                            break;
                    }
                }
                //else
                //{
                //    cal_in_cost2 += 150 * max.length - 20;
                //}
                if (mid.realSpace == 5)
                {
                    cal_in_cost2 += calInCreaseCost(mid, p);
                    cal_in_cost2 = cal_in_cost2 / 2;
                }
                else if (mid.realSpace > 5)
                {
                    cal_in_cost2 += calInCreaseCost(mid, p);
                }
                else
                {
                    cal_in_cost2 = 0;
                }
            }
            else
            {
                if (mid.realSpace == 5)
                {
                    cal_in_cost2 += calInCreaseCost(mid, p) / 2;
                }
                else if (mid.realSpace > 5)
                {
                    cal_in_cost2 += calInCreaseCost(mid, p);
                }

                if (p.row == mid.p2.row && p.col == mid.p2.col)
                {
                    //被压倒版边max.realSpace == 4 并且 max.length == 1并且mid.space = 3 会有个问题，可以忽略
                    if ((mid.length == 1 ? max.realSpace + mid.space - max.minSpace : 1 + max.realSpace) >= 5
                        && (max.minSpace == 0 || max.realSpace < 5))
                    {
                        switch (max.length)
                        {
                            case 1:
                                cal_in_cost1 += Cost.One_1 + 5;
                                break;
                            case 2:
                                cal_in_cost1 += Cost.Tow_11 + 5;
                                break;
                            case 3:
                                cal_in_cost1 += Cost.Three_111 + 5;
                                break;
                            default:
                                cal_in_cost2 += Cost.Four_d1111;
                                break;
                        }
                    }
                    //else if (max.realSpace == 5)
                    //{
                    //    cal_in_cost2 += 300 * max.length / 2 + 10;
                    //}
                }
            }

            return cal_in_cost2;
        }

        private static int cal_in_cost3 = 0;
        public static int calInCreaseThreeCost(int[][] data, ref Line min, ref Line mid, ref Line max, ref Position p)
        {
            cal_in_cost3 = 0;
            if ( mid.color == min.color && mid.color == max.color)
            {
                if (mid.length >= 3)
                {
                    return Cost.Fiv_1011101;
                }
                //if (p.row == mid.p1.row && p.col == mid.p1.col && p.row == mid.p2.row && p.col == mid.p2.col)
                //{
                cal_in_cost3 += 300 * max.length - 10;
                    cal_in_cost3 += 300 * min.length - 10;
                //}
                //else if (p.row == mid.p2.row && p.col == mid.p2.col)
                //{
                //    cal_in_cost3 += 300 * max.length;
                //}
                //else
                //{
                //    cal_in_cost3 += 300 * min.length;
                //}

                if (mid.realSpace == 5)
                {
                    cal_in_cost3 += calInCreaseCost(mid, p);
                    cal_in_cost3 = cal_in_cost3 / 2;
                }
                else if (mid.realSpace > 5)
                {
                    cal_in_cost3 += calInCreaseCost(mid, p);
                }
                else
                {
                    cal_in_cost3 = 0;
                }

            }
            else if(mid.color == min.color && mid.color != max.color)
            {
                if (p.row == mid.p1.row && p.col == mid.p1.col)
                {
                    switch (max.length)
                    {
                        case 1:
                            cal_in_cost1 += Cost.One_1 - 5;
                            break;
                        case 2:
                            cal_in_cost1 += Cost.Tow_11 - 5;
                            break;
                        case 3:
                            cal_in_cost1 += Cost.Three_111 - 5;
                            break;
                        default:
                            cal_in_cost2 += Cost.Four_1111;
                            break;
                    }
                }
                if (mid.realSpace == 5)
                {
                    cal_in_cost3 += calInCreaseCost(mid, p);
                    cal_in_cost3 = cal_in_cost3 / 2;
                }
                else if (mid.realSpace > 5)
                {
                    cal_in_cost3 += calInCreaseCost(mid, p);
                }

                if (p.row == mid.p2.row && p.col == mid.p2.col)
                {
                    if ((mid.length == 1 ? max.realSpace + mid.space - max.minSpace : 1 + max.realSpace) >= 5
                        && (max.minSpace == 0 || max.realSpace < 5))
                    {
                        switch (min.length)
                        {
                            case 1:
                                cal_in_cost1 += Cost.One_1 + 5;
                                break;
                            case 2:
                                cal_in_cost1 += Cost.Tow_11 + 5;
                                break;
                            case 3:
                                cal_in_cost1 += Cost.Three_111 + 5;
                                break;
                            default:
                                cal_in_cost1 += Cost.Four_1111 + 5;
                                break;

                        }
                    }
                    //else if (max.realSpace == 5)
                    //{
                    //    cal_in_cost3 += 300 * max.length / 2 + 10;
                    //}
                }
                //else
                //{
                //    cal_in_cost3 += 150 * min.length;
                //}
            }
            else if (mid.color != min.color && mid.color == max.color)
            {
                if (p.row == mid.p2.row && p.col == mid.p2.col)
                {
                    switch (min.length)
                    {
                        case 1:
                            cal_in_cost1 += Cost.One_1 - 5;
                            break;
                        case 2:
                            cal_in_cost1 += Cost.Tow_11 - 5;
                            break;
                        case 3:
                            cal_in_cost1 += Cost.Three_111 - 5;
                            break;
                        default:
                            cal_in_cost1 += Cost.Four_1111 - 5;
                            break;

                    }
                }
                if (mid.realSpace == 5)
                {
                    cal_in_cost3 += calInCreaseCost(mid, p);
                    cal_in_cost3 = cal_in_cost3 / 2;
                }
                else if (mid.realSpace > 5)
                {
                    cal_in_cost3 += calInCreaseCost(mid, p);
                }

                if (p.row == mid.p1.row && p.col == mid.p1.col)
                {
                    if ((mid.length == 1 ? mid.space + min.realSpace - min.maxSpace: 1 + min.realSpace) >= 5
                        && (min.maxSpace == 0 || min.realSpace < 5))
                    {
                        switch (min.length)
                        {
                            case 1:
                                cal_in_cost1 += Cost.One_1 + 5;
                                break;
                            case 2:
                                cal_in_cost1 += Cost.Tow_11 + 5;
                                break;
                            case 3:
                                cal_in_cost1 += Cost.Three_111 + 5;
                                break;
                            default:
                                cal_in_cost1 += Cost.Four_1111 + 5;
                                break;

                        }
                    }
                    //else if (min.realSpace == 5)
                    //{
                    //    cal_in_cost3 += 300 * min.length / 2 + 10;
                    //}
                }
                //else
                //{
                //    cal_in_cost3 += 150 * min.length;
                //}
            }
            else if (mid.color != min.color && mid.color != max.color)
            {
                if (mid.realSpace == 5)
                {
                    cal_in_cost3 += calInCreaseCost(mid, p) / 2;
                }
                else if (mid.realSpace > 5)
                {
                    cal_in_cost3 += calInCreaseCost(mid, p);
                }

                if (mid.length == 1 && min.maxSpace == 0 && max.minSpace == 0 && min.length + max.length >= 4)
                {
                    return Cost.Four_skip;
                }

                if (p.row == mid.p1.row && p.col == mid.p1.col)
                {
                    if ((mid.length == 1 ? (min.realSpace + mid.length + max.realSpace) : (1 + min.realSpace)) >= 5
                        && (min.maxSpace == 0 || min.realSpace < 5))
                    {
                        switch (min.length)
                        {
                            case 1:
                                cal_in_cost1 += Cost.One_1 + 5;
                                break;
                            case 2:
                                cal_in_cost1 += Cost.Tow_11 + 5;
                                break;
                            case 3:
                                cal_in_cost1 += Cost.Three_111 + 5;
                                break;
                            default:
                                cal_in_cost1 += Cost.Four_1111 + 5;
                                break;

                        }
                    }
                    //else if (min.realSpace == 5)
                    //{
                    //    cal_in_cost3 += 300 * min.length / 2 + 10;
                    //}
                }
                if (p.row == mid.p2.row && p.col == mid.p2.col)
                {
                    if ( (mid.length == 1? (min.realSpace + mid.length + max.realSpace): (1 + max.realSpace)) >= 5
                        && (max.minSpace == 0 || max.realSpace < 5))
                    {
                        switch (min.length)
                        {
                            case 1:
                                cal_in_cost1 += Cost.One_1 + 5;
                                break;
                            case 2:
                                cal_in_cost1 += Cost.Tow_11 + 5;
                                break;
                            case 3:
                                cal_in_cost1 += Cost.Three_111 + 5;
                                break;
                            default:
                                cal_in_cost1 += Cost.Four_1111 + 5;
                                break;

                        }
                    }
                    //else if (max.realSpace == 5)
                    //{
                    //    cal_in_cost3 += 300 * max.length/ 2 + 10;
                    //}
                }
            }

            return cal_in_cost3;
        }

        #endregion

        #region 计算落子之后一串棋减少的权值

        public static int calDreaseCost(ref Line line)
        {
            int cost = 0;
            switch (line.length)
            {
                case 0:
                    return cost = 0;
                case 1:
                    {
                        switch (line.type)
                        {
                            case LineType.BothDead:
                                cost = (Cost.One_d1);
                                break;
                            case LineType.BothLive:
                                //throw new Exception("This step found no effect.");
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = (Cost.One_1 - Cost.One_d1);
                                break;
                        }
                        break;
                    }
                case 2:
                    {
                        switch (line.type)
                        {
                            case LineType.BothDead:
                                cost = (Cost.Tow_d11);
                                break;
                            case LineType.BothLive:
                                //throw new Exception("This step found no effect.");
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = (Cost.Tow_11 - Cost.Tow_d11);
                                break;
                        }
                        break;
                    }
                case 3:
                    {
                        switch (line.type)
                        {
                            case LineType.BothDead:
                                cost = (Cost.Three_d111);
                                break;
                            case LineType.BothLive:
                                //throw new Exception("This step found no effect.");
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = (Cost.Three_111 - Cost.Three_d111);
                                break;
                        }
                        break;
                    }
                case 4:
                    {
                        switch (line.type)
                        {
                            case LineType.BothDead:
                                cost = (Cost.Four_d1111);
                                break;
                            case LineType.BothLive:
                                //throw new Exception("This step found no effect.");
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = (Cost.Four_1111 - Cost.Four_d1111);
                                break;
                        }
                        break;
                    }
                default:
                    cost = 0;
                    break;
            }

            return cost;
        }

        #endregion

        #region 计算落子后一串不连续的棋原本的价值
        public static int calUnSequentialLineCostBase(int[][] data, ref Line min, ref Line mid, ref Line max, Position p)
        {
            if (min.length == 0 && max.length == 0)
            {
                if (mid.realSpace < 5)
                {
                    return 0;
                }
                if (mid.realSpace == 5)
                {
                    return calCost(mid.length, mid.type) / 2;
                }
                return calCost(mid.length, mid.type);
            }

            if (min.length != 0 && max.length == 0)
            {
                return calTowLineCost(data, ref min, ref mid);
            }

            if (min.length == 0 && max.length != 0)
            {
                return calTowLineCost(data, ref mid, ref max);
            }

            return calInCreaseThreeCost(data, ref min, ref mid, ref max, ref p);
        }
        #endregion

        #region 计算落子后一串不连续的棋增加的价值
        public static int calUnSequentialLineIncreaseCost(int[][] data, ref Line min, ref Line mid, ref Line max, Position p)
        {
            if(min.length == 0 && max.length == 0)
            {
                if (mid.realSpace < 5)
                {
                    return 0;
                }
                if (mid.realSpace == 5)
                {
                    return calInCreaseCost(mid, p) / 2;
                }
                return calInCreaseCost(mid, p);
            }

            if( min.length != 0 && max.length == 0)
            {
                return calInCreasePrevCost(data, ref min, ref mid, ref p);
            }

            if (min.length == 0 && max.length != 0)
            {
                return calInCreaseBehindCost(data, ref mid, ref max, ref p);
            }

            return calInCreaseThreeCost(data, ref min, ref mid, ref max, ref p);
        }
        #endregion

        #region 计算落子之后权值的变动

        private static Line line_h = new Line();
        private static Line line_h_min = new Line();
        private static Line line_h_max = new Line();

        private static Line line_v = new Line();
        private static Line line_v_min = new Line();
        private static Line line_v_max = new Line();

        private static Line line_lt = new Line();
        private static Line line_lt_min = new Line();
        private static Line line_lt_max = new Line();

        private static Line line_lb = new Line();
        private static Line line_lb_min = new Line();
        private static Line line_lb_max = new Line();

        private static int color = 0;
        private static int row = 0;
        private static int col = 0;
        private static int cost_v = 0;
        private static int cost_h = 0;
        private static int cost_lt = 0;
        private static int cost_lb = 0;
        public static int calStepValue(int[][] data, ref Position p)
        {
            color = p.color;
            row = p.row;
            col = p.col;
            //是否是独子
            #region 水平增量
            {
                line_h.clear();
                line_h_min.clear();
                line_h_max.clear();

                calHorizontalCount(data, row, col, ref line_h);

                //计算是否有不连续的棋
                calHorizontalLine(data, color, line_h, ref line_h_min, ref line_h_max);

                cost_h = calUnSequentialLineIncreaseCost(data, ref line_h_min, ref line_h, ref line_h_max, p);
            }
            #endregion

            #region 垂直增量
            {
                line_v.clear();
                line_v_min.clear();
                line_v_max.clear();

                calVerticalCount(data, row, col, ref line_v);

                //计算是否有不连续的棋
                calVerticalLine(data, color, line_v, ref line_v_min, ref line_v_max);

                cost_v = calUnSequentialLineIncreaseCost(data, ref line_v_min, ref line_v, ref line_v_max, p);
            }
            #endregion

            #region 左上到右下
            {
                line_lt.clear();
                line_lt_min.clear();
                line_lt_max.clear();

                calInclinedCount_LT(data, row, col, ref line_lt);

                //计算是否有不连续的棋
                calInclined_LT(data, color, line_lt, ref line_lt_min, ref line_lt_max);

                cost_lt = calUnSequentialLineIncreaseCost(data, ref line_lt_min, ref line_lt, ref line_lt_max, p);
            }
            #endregion

            #region 坐下到右上
            {
                line_lb.clear();
                line_lb_min.clear();
                line_lb_max.clear();

                calInclinedCount_LB(data, row, col, ref line_lb);

                //计算是否有不连续的棋
                calInclined_LB(data, color, line_lb, ref line_lb_min, ref line_lb_max);

                cost_lb = calUnSequentialLineIncreaseCost(data, ref line_lb_min, ref line_lb, ref line_lb_max, p);
            }
            #endregion

            p.val += cost_h + cost_v + cost_lt + cost_lb;

            if(line_v.length == 1 && line_h.length == 1 && line_lb.length == 1 && line_lt.length == 1
                && line_h_max.length == 0 && line_h_min.length == 0 && line_v_max.length == 0 && line_v_min.length == 0
                && line_lt_max.length == 0 && line_lt_min.length == 0 && line_lb_max.length == 0 && line_lb_min.length == 0)
            {
                //这里强行把值设为0，防止排序的时候出错
                p.val = 0;
                p.alone = true;
            }

            //Logs.writeln("cost=" + p.val, 4);
            return p.val;
        }
        #endregion

        //计算相加的权值
        public static int calValue(int[][] data, ref Position p)
        {
            color = p.color;
            row = p.row;
            col = p.col;
            //是否是独子
            #region 水平增量
            {
                line_h.clear();
                line_h_min.clear();
                line_h_max.clear();

                calHorizontalCount(data, row, col, ref line_h);

                //计算是否有不连续的棋
                calHorizontalLines(data, color, line_h, ref line_h_min, ref line_h_max);

                cost_h = calUnSequentialLineCostBase(data, ref line_h_min, ref line_h, ref line_h_max, p);
            }
            #endregion

            #region 垂直增量
            {
                line_v.clear();
                line_v_min.clear();
                line_v_max.clear();

                calVerticalCount(data, row, col, ref line_v);

                ////计算是否有不连续的棋
                //calVerticalLine(data, color, line_v, ref line_v_min, ref line_v_max);

                //cost_v = calUnSequentialLineCost(data, ref line_v_min, ref line_v, ref line_v_max, p);
            }
            #endregion

            #region 左上到右下
            {
                line_lt.clear();
                line_lt_min.clear();
                line_lt_max.clear();

                calInclinedCount_LT(data, row, col, ref line_lt);

                ////计算是否有不连续的棋
                //calInclined_LT(data, color, line_lt, ref line_lt_min, ref line_lt_max);

                //cost_lt = calUnSequentialLineCost(data, ref line_lt_min, ref line_lt, ref line_lt_max, p);
            }
            #endregion

            #region 坐下到右上
            {
                line_lb.clear();
                line_lb_min.clear();
                line_lb_max.clear();

                calInclinedCount_LB(data, row, col, ref line_lb);

                ////计算是否有不连续的棋
                //calInclined_LB(data, color, line_lb, ref line_lb_min, ref line_lb_max);

                //cost_lb = calUnSequentialLineCost(data, ref line_lb_min, ref line_lb, ref line_lb_max, p);
            }
            #endregion

            //p.val += cost_h + cost_v + cost_lt + cost_lb;

            //if (line_v.length == 1 && line_h.length == 1 && line_lb.length == 1 && line_lt.length == 1
            //    && line_h_max.length == 0 && line_h_min.length == 0 && line_v_max.length == 0 && line_v_min.length == 0
            //    && line_lt_max.length == 0 && line_lt_min.length == 0 && line_lb_max.length == 0 && line_lb_min.length == 0)
            //{
            //    //这里强行把值设为0，防止排序的时候出错
            //    p.val = 0;
            //    p.alone = true;
            //}

            //Logs.writeln("cost=" + p.val, 4);
            return p.val;
        }

    }
}
