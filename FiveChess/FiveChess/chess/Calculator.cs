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


        #region 计算一串棋的价值

        public static int calCost(ref Line line)
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
                                cost = 0;
                                break;
                            case LineType.BothLive:
                                cost = (int)Cost.One_1_2;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = (int)Cost.One_1_1;
                                break;
                        }
                        break;
                    }
                case 2:
                    {
                        switch (line.type)
                        {
                            case LineType.BothDead:
                                cost = 0;
                                break;
                            case LineType.BothLive:
                                cost = (int)Cost.Tow_2_2;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = (int)Cost.Tow_2_1;
                                break;
                        }
                        break;
                    }
                case 3:
                    {
                        switch (line.type)
                        {
                            case LineType.BothDead:
                                cost = 0;
                                break;
                            case LineType.BothLive:
                                cost = (int)Cost.Three_3_2;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = (int)Cost.Three_3_1;
                                break;
                        }
                        break;
                    }
                case 4:
                    {
                        switch (line.type)
                        {
                            case LineType.BothDead:
                                cost = 0;
                                break;
                            case LineType.BothLive:
                                cost = (int)Cost.Four_4_2;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = (int)Cost.Four_4_1;
                                break;
                        }
                        break;
                    }
                default:
                    cost = (int)Cost.Five;
                    break;
            }

            line.cost = cost;
            return cost;
        }

        #endregion

        #region 计算一条线上的棋的价值
        public static int calLinesCost(int row, int col, ref Line line_mid, ref Line line_min, ref Line line_max)
        {
            if(line_min.type == LineType.MinDeadMaxLive)
            { 
}
            return 0;
        }
        #endregion

        #region 计算一个坐标点的一条线上有多少个连续的棋

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
            calCost(ref line);
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
            calCost(ref line);
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
            calCost(ref line);
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
            calCost(ref line);
            return line.length;
        }

        #endregion


        #region 计算一条线上不连续的棋

        //计算水平方向一串棋首尾是否还有不连续的棋
        public static int calHorizontalLine(byte[][] data, int color, Line src, ref Line line1, ref Line line2)
        {
            //计算是否有不连续的棋, 至少边界要有两个空位置才有可能
            if (src.p1.col >= 2 && data[src.p1.row][src.p1.col - 2] == color)
            {
                calHorizontalCount(data, color, src.p1.row, src.p1.col - 2, ref line1);
                Logs.writeln("length = " + line1.length + " type = " + line1.type + "  p1 = (" + line1.p1.col + "," + line1.p1.row + ")," + "  p2 = (" + line1.p2.col + "," + line1.p2.row + "),", 2);
            }

            if (src.p2.col <= Side.COL_ID - 2 && data[src.p1.row][src.p1.col + 2] == color)
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
            if (src.p1.row >= 2 && data[src.p1.row - 2][src.p1.col] == color)
            {
                calVerticalCount(data, color, src.p1.row - 2, src.p1.col, ref line1);
                Logs.writeln("length = " + line1.length + " type = " + line1.type + "  p1 = (" + line1.p1.col + "," + line1.p1.row + ")," + "  p2 = (" + line1.p2.col + "," + line1.p2.row + "),", 2);
            }

            if (src.p2.row <= Side.ROW_ID - 2 && data[src.p1.row + 2][src.p1.col] == color)
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
            if (src.p1.col >= 2 && src.p1.row >= 2 && data[src.p1.row - 2][src.p1.col - 2] == color)
            {
                calInclinedCount_LT(data, color, src.p1.row - 2, src.p1.col - 2, ref line1);
                Logs.writeln("length = " + line1.length + " type = " + line1.type + "  p1 = (" + line1.p1.col + "," + line1.p1.row + ")," + "  p2 = (" + line1.p2.col + "," + line1.p2.row + "),", 2);
            }

            if (src.p2.col <= Side.COL_ID - 2 && src.p2.row <= Side.ROW_ID - 2 && data[src.p1.row + 2][src.p1.col + 2] == color)
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
            if (src.p1.col >= 2 && src.p1.row <= Side.ROW_ID - 2 && data[src.p1.row + 2][src.p1.col - 2] == color)
            {
                calInclinedCount_LB(data, color, src.p1.row + 2, src.p1.col - 2, ref line1);
                Logs.writeln("length = " + line1.length + " type = " + line1.type + "  p1 = (" + line1.p1.col + "," + line1.p1.row + ")," + "  p2 = (" + line1.p2.col + "," + line1.p2.row + "),", 2);
            }

            if (src.p2.row >= 2 && src.p2.col <= Side.COL_ID - 2 && data[src.p1.row - 2][src.p1.col + 2] == color)
            {
                calInclinedCount_LB(data, color, src.p2.row - 2, src.p2.col + 2, ref line2);
                Logs.writeln("length = " + line2.length + " type = " + line2.type + "  p1 = (" + line2.p1.col + "," + line2.p1.row + ")," + "  p2 = (" + line2.p2.col + "," + line2.p2.row + "),", 2);
            }
            return src.length + line1.length + line2.length;
        }

        #endregion


        #region 计算增加的权值

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

        public static int calIncreaseValue(byte[][] data, int color, int row, int col)
        {
            int cost = 0;
            #region 水平增量
            {
                line_h.clear();
                line_h_min.clear();
                line_h_max.clear();

                calHorizontalCount(data, color, row, col, ref line_h);

                //计算是否有不连续的棋
                calHorizontalLine(data, color, line_h, ref line_h_min, ref line_h_max);

                //if(line_h.cost > 0)
                {
                    Logs.writeln("length = " + line_h.length + " type = " + line_h.type + " cost = " + line_h.cost + "  p1 = (" + line_h.p1.col + "," + line_h.p1.row + ")," + "  p2 = (" + line_h.p2.col + "," + line_h.p2.row + "),", 3);
                    Logs.writeln("length = " + line_h_min.length + " type = " + line_h_min.type + " cost = " + line_h_min.cost + "  p1 = (" + line_h_min.p1.col + "," + line_h_min.p1.row + ")," + "  p2 = (" + line_h_min.p2.col + "," + line_h_min.p2.row + "),", 3);
                    Logs.writeln("length = " + line_h_max.length + " type = " + line_h_max.type + " cost = " + line_h_max.cost + "  p1 = (" + line_h_max.p1.col + "," + line_h_max.p1.row + ")," + "  p2 = (" + line_h_max.p2.col + "," + line_h_max.p2.row + "),", 3);

                    cost += line_h.cost;
                    cost += line_h_min.cost;
                    cost += line_h_max.cost;
                }
            }
            #endregion

            #region 垂直增量
            {
                line_v.clear();
                line_v_min.clear();
                line_v_max.clear();

                calVerticalCount(data, color, row, col, ref line_v);
                Logs.writeln("length = " + line_v.length + " type = " + line_v.type + "  p1 = (" + line_v.p1.col + "," + line_v.p1.row + ")," + "  p2 = (" + line_v.p2.col + "," + line_v.p2.row + "),", 2);

                //计算是否有不连续的棋
                calVerticalLine(data, color, line_v, ref line_v_min, ref line_v_max);

                cost += line_v.cost;
                cost += line_v_min.cost;
                cost += line_v_max.cost;
            }
            #endregion

            #region 左上到右下
            {
                line_lt.clear();
                line_lt_min.clear();
                line_lt_max.clear();

                calInclinedCount_LT(data, color, row, col, ref line_lt);
                Logs.writeln("length = " + line_lt.length + " type = " + line_lt.type + "  p1 = (" + line_lt.p1.col + "," + line_lt.p1.row + ")," + "  p2 = (" + line_lt.p2.col + "," + line_lt.p2.row + "),", 2);

                //计算是否有不连续的棋
                calInclined_LT(data, color, line_lt, ref line_lt_min, ref line_lt_max);

                cost += line_lt.cost;
                cost += line_lt_min.cost;
                cost += line_lt_max.cost;
            }
            #endregion

            #region 坐下到右上
            {
                line_lb.clear();
                line_lb_min.clear();
                line_lb_max.clear();

                calInclinedCount_LB(data, color, row, col, ref line_lb);
                Logs.writeln("length = " + line_lb.length + " type = " + line_lb.type + "  p1 = (" + line_lb.p1.col + "," + line_lb.p1.row + ")," + "  p2 = (" + line_lb.p2.col + "," + line_lb.p2.row + "),", 2);

                //计算是否有不连续的棋
                calInclined_LB(data, color, line_lb, ref line_lb_min, ref line_lb_max);

                cost += line_lb.cost;
                cost += line_lb_min.cost;
                cost += line_lb_max.cost;
            }
            #endregion

            return cost;
        }
        #endregion

    }
}
