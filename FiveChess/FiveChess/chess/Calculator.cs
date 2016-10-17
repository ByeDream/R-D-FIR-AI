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
                    if (cb.Data[row][col] != Color.BLACK && cb.Data[row][col] != Color.WHITE)
                    {
                        isFull = false;
                    }
                }
            }
            return isFull;
        }

        #region check if have N count in a line

        public static int hasVerticalCount(byte[][] data, int color, int count, int row, int col, out LineType type)
        {
            int beginRow = row - (count - 1), endRow = row + (count - 1);
            int value_count = 0;
            type = 0;

            for (int r = beginRow; r <= endRow; r++)
            {
                if (r < 0)
                    continue;
                if (r > Side.ROW_ID)
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

        public static int hasHorizontalCount(byte[][] data, int color, int count, int row, int col, out LineType type)
        {
            int beginCol = col - (count - 1), endCol = col + (count - 1);
            int value_count = 0;
            type = 0;

            for (int c = beginCol; c <= endCol; c++)
            {
                if (c < 0)
                    continue;
                if (c > Side.COL_ID)
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
        public static int hasInclinedCount_LT(byte[][] data, int color, int count, int row, int col, out LineType type)
        {
            int beginCol = col - (count - 1), endCol = col + (count - 1);
            int beginRow = row - (count - 1), endRow = row + (count - 1);
            int value_count = 0;
            type = 0;

            for (int r = beginRow, c = beginCol; r <= endRow && c <= endCol; r++, c++)
            {
                if (r < 0 || c < 0)
                    continue;
                if (r > Side.ROW_ID || c > Side.COL_ID)
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
        public static int hasInclinedCount_LB(byte[][] data, int color, int count, int row, int col, out LineType type)
        {
            int beginCol = col - (count - 1), endCol = col + (count - 1);
            int beginRow = row - (count - 1), endRow = row + (count - 1);
            int value_count = 0;
            type = 0;

            for (int r = endRow, c = beginCol; r >= beginRow && c <= endCol; r--, c++)
            {
                if (r < 0 || c < 0)
                    continue;
                if (r > Side.ROW_ID || c > Side.COL_ID)
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

        #region check how many same color chess in a line

        public static int calHorizontalCount(byte[][] data, int color, int row, int col, ref Line line)
        {
            int value_count = (data[row][col] == color) ? 1 : 0;

            line.p1.row = line.p2.row = row;
            line.p1.col = line.p2.col = col;

            int min_col = col - 1;
            while (min_col >= 0 && data[row][min_col] == color)
            {
                line.p1.row = row;
                line.p1.col = min_col;
                value_count++;
                min_col--;
            }
            if (min_col < 0 || data[row][min_col] != Color.NONE)
            {
                line.type = LineType.MinDeadMaxLive;
            }

            int max_col = col + 1;
            while (max_col <= Side.COL_ID && data[row][max_col] == color)
            {
                line.p2.row = row;
                line.p2.col = max_col;
                value_count++;
                max_col++;
            }

            if (max_col > Side.COL_ID || data[row][max_col] != Color.NONE)
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
            Logs.writeln("calHorizontalCount count = " + value_count + "   type = " + line.type, 2);

            line.length = value_count;
            return line.length;
        }

        public static int calVerticalCount(byte[][] data, int color, int row, int col, ref Line line)
        {
            int value_count = (data[row][col] == color) ? 1 : 0;

            line.p1.row = line.p2.row = row;
            line.p1.col = line.p2.col = col;

            int min_row = row - 1;
            while (min_row >= 0 && data[min_row][col] == color)
            {
                line.p1.row = min_row;
                line.p1.col = col;
                min_row--;
                value_count++;
            }
            if (min_row < 0 || data[min_row][col] != Color.NONE)
            {
                line.type = LineType.MinDeadMaxLive;
            }

            int max_row = row + 1;
            while (max_row <= Side.ROW_ID && data[max_row][col] == color)
            {
                line.p2.row = max_row;
                line.p2.col = col;
                max_row++;
                value_count++;
            }

            if (max_row > Side.ROW_ID || data[max_row][col] != Color.NONE)
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

            Logs.writeln("calVerticalCount count = " + value_count + "   type = " + line.type, 2);

            line.length = value_count;
            return line.length;
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
        public static int calInclinedCount_LT(byte[][] data, int color, int row, int col, ref Line line)
        {
            int value_count = (data[row][col] == color) ? 1 : 0;

            line.p1.row = line.p2.row = row;
            line.p1.col = line.p2.col = col;

            int min_row = row - 1;
            int min_col = col - 1;
            while (min_col >= 0 && min_row >= 0 && data[min_row][min_col] == color)
            {
                line.p1.row = min_row;
                line.p1.col = min_col;

                min_row--;
                min_col--;
                value_count++;
            }
            if (min_col < 0 || min_row < 0 || data[min_row][min_col] != Color.NONE)
            {
                line.type = LineType.MinDeadMaxLive;
            }

            int max_row = row + 1;
            int max_col = col + 1;
            while (max_col <= Side.COL_ID && max_row <= Side.ROW_ID && data[max_row][max_col] == color)
            {
                line.p2.row = max_row;
                line.p2.col = max_col;

                max_row++;
                max_col++;
                value_count++;
            }

            if (max_col > Side.COL_ID || max_row > Side.ROW_ID || data[max_row][max_col] != Color.NONE)
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

            Logs.writeln("calInclinedCount_LT count = " + value_count + "   type = " + line.type, 2);

            line.length = value_count;
            return line.length;
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
        public static int calInclinedCount_LB(byte[][] data, int color, int row, int col, ref Line line)
        {
            int value_count = (data[row][col] == color) ? 1 : 0;

            line.p1.row = line.p2.row = row;
            line.p1.col = line.p2.col = col;

            int max_row = row + 1;
            int min_col = col - 1;
            while (min_col >= 0 && max_row <= Side.ROW_ID && data[max_row][min_col] == color)
            {
                line.p1.row = max_row;
                line.p1.col = min_col;

                max_row++;
                min_col--;
                value_count++;
            }
            if (min_col < 0 || max_row > Side.ROW_ID || data[max_row][min_col] != Color.NONE)
            {
                line.type = LineType.MinDeadMaxLive;
            }

            int min_row = row - 1;
            int max_col = col + 1;
            while (max_col <= Side.COL_ID && min_row >= 0 && data[min_row][max_col] == color)
            {
                line.p2.row = min_row;
                line.p2.col = max_col;

                min_row--;
                max_col++;
                value_count++;
            }

            if (max_col > Side.COL_ID || min_row < 0 || data[min_row][max_col] != Color.NONE)
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

            Logs.writeln("calInclinedCount_LB count = " + value_count + "   type = " + line.type, 2);

            line.length = value_count;
            return line.length;
        }
        #endregion


        //计算水平方向一串棋首尾是否还有不连续的棋
        public static int calHorizontalLine(byte[][] data, int color, Line src, ref Line line1, ref Line line2)
        {
            //计算是否有不连续的棋, 至少边界要有两个空位置才有可能
            if (src.p1.col >= 2)
            {
                calHorizontalCount(data, color, src.p1.row, src.p1.col - 2, ref line1);
                Logs.writeln("length = " + line1.length + " type = " + line1.type + "  p1 = (" + line1.p1.col + "," + line1.p1.row + ")," + "  p2 = (" + line1.p2.col + "," + line1.p2.row + "),", 2);
            }

            if (src.p2.col <= Side.COL_ID - 2)
            {
                calHorizontalCount(data, color, src.p2.row, src.p2.col + 2, ref line2);
                Logs.writeln("length = " + line2.length + " type = " + line2.type + "  p1 = (" + line2.p1.col + "," + line2.p1.row + ")," + "  p2 = (" + line2.p2.col + "," + line2.p2.row + "),", 2);
            }
            return src.length + line1.length + line2.length;
        }

        //计算垂直方向一串棋首尾是否还有不连续的棋
        public static int calVerticalLine(byte[][] data, int color, Line src, ref Line line1, ref Line line2)
        {
            //计算是否有不连续的棋, 至少边界要有两个空位置才有可能
            if (src.p1.row >= 2)
            {
                calVerticalCount(data, color, src.p1.row - 2, src.p1.col, ref line1);
                Logs.writeln("length = " + line1.length + " type = " + line1.type + "  p1 = (" + line1.p1.col + "," + line1.p1.row + ")," + "  p2 = (" + line1.p2.col + "," + line1.p2.row + "),", 2);
            }

            if (src.p2.col <= Side.COL_ID - 2)
            {
                calVerticalCount(data, color, src.p2.row + 2, src.p2.col, ref line2);
                Logs.writeln("length = " + line2.length + " type = " + line2.type + "  p1 = (" + line2.p1.col + "," + line2.p1.row + ")," + "  p2 = (" + line2.p2.col + "," + line2.p2.row + "),", 2);
            }
            return src.length + line1.length + line2.length;
        }

        //计算左上到右下方向一串棋首尾是否还有不连续的棋
        public static int calInclined_LT(byte[][] data, int color, Line src, ref Line line1, ref Line line2)
        {
            //计算是否有不连续的棋, 至少边界要有两个空位置才有可能
            if (src.p1.col >= 2 && src.p1.row > 2)
            {
                calInclinedCount_LT(data, color, src.p1.row - 2, src.p1.col - 2, ref line1);
                Logs.writeln("length = " + line1.length + " type = " + line1.type + "  p1 = (" + line1.p1.col + "," + line1.p1.row + ")," + "  p2 = (" + line1.p2.col + "," + line1.p2.row + "),", 2);
            }

            if (src.p2.col <= Side.COL_ID - 2)
            {
                calInclinedCount_LT(data, color, src.p2.row + 2, src.p2.col + 2, ref line2);
                Logs.writeln("length = " + line2.length + " type = " + line2.type + "  p1 = (" + line2.p1.col + "," + line2.p1.row + ")," + "  p2 = (" + line2.p2.col + "," + line2.p2.row + "),", 2);
            }
            return src.length + line1.length + line2.length;
        }

        //计算左下到右上方向一串棋首尾是否还有不连续的棋
        public static int calInclined_LB(byte[][] data, int color, Line src, ref Line line1, ref Line line2)
        {
            //计算是否有不连续的棋, 至少边界要有两个空位置才有可能
            if (src.p1.col >= 2 && src.p1.row <= Side.ROW_ID - 2)
            {
                calInclinedCount_LB(data, color, src.p1.row + 2, src.p1.col - 2, ref line1);
                Logs.writeln("length = " + line1.length + " type = " + line1.type + "  p1 = (" + line1.p1.col + "," + line1.p1.row + ")," + "  p2 = (" + line1.p2.col + "," + line1.p2.row + "),", 4);
            }

            if (src.p2.row >= 2 && src.p2.col <= Side.COL_ID - 2)
            {
                calInclinedCount_LB(data, color, src.p2.row - 2, src.p2.col + 2, ref line2);
                Logs.writeln("length = " + line2.length + " type = " + line2.type + "  p1 = (" + line2.p1.col + "," + line2.p1.row + ")," + "  p2 = (" + line2.p2.col + "," + line2.p2.row + "),", 4);
            }
            return src.length + line1.length + line2.length;
        }


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

        #region 计算增加的权值
        public static byte[][] calIncreaseValue(byte[][] data, int color, int row, int col)
        {
            LineType l_type = LineType.BothLive;

            #region 水平增量
            {
                line_h.clear();
                line_h_min.clear();
                line_h_max.clear();

                calHorizontalCount(data, color, row, col, ref line_h);
                Logs.writeln("length = " + line_h.length + " type = " + l_type + "  p1 = (" + line_h.p1.col + "," + line_h.p1.row + ")," + "  p2 = (" + line_h.p2.col + "," + line_h.p2.row + "),", 2);

                //计算是否有不连续的棋, 至少边界要有两个空位置才有可能
                calHorizontalLine(data, color, line_h, ref line_h_min, ref line_h_max);


            }
            #endregion

            #region 垂直增量
            {
                line_v.clear();
                line_v_min.clear();
                line_v_max.clear();

                calVerticalCount(data, color, row, col, ref line_v);
                Logs.writeln("length = " + line_v.length + " type = " + l_type + "  p1 = (" + line_v.p1.col + "," + line_v.p1.row + ")," + "  p2 = (" + line_v.p2.col + "," + line_v.p2.row + "),", 2);

                //计算是否有不连续的棋, 至少边界要有两个空位置才有可能
                calVerticalLine(data, color, line_v, ref line_v_min, ref line_v_max);
            }
            #endregion

            #region 左上到右下
            {
                line_lt.clear();
                line_lt_min.clear();
                line_lt_max.clear();

                calInclinedCount_LT(data, color, row, col, ref line_lt);
                Logs.writeln("length = " + line_lt.length + " type = " + l_type + "  p1 = (" + line_lt.p1.col + "," + line_lt.p1.row + ")," + "  p2 = (" + line_lt.p2.col + "," + line_lt.p2.row + "),", 2);

                //计算是否有不连续的棋, 至少边界要有两个空位置才有可能
                calInclined_LT(data, color, line_lt, ref line_lt_min, ref line_lt_max);
            }
            #endregion

            #region 坐下到右上
            {
                line_lb.clear();
                line_lb_min.clear();
                line_lb_max.clear();

                calInclinedCount_LB(data, color, row, col, ref line_lb);
                Logs.writeln("length = " + line_lb.length + " type = " + l_type + "  p1 = (" + line_lb.p1.col + "," + line_lb.p1.row + ")," + "  p2 = (" + line_lb.p2.col + "," + line_lb.p2.row + "),", 4);

                //计算是否有不连续的棋, 至少边界要有两个空位置才有可能
                calInclined_LB(data, color, line_lb, ref line_lb_min, ref line_lb_max);
            }
            #endregion

            return null;
        }
        #endregion

    }
}
