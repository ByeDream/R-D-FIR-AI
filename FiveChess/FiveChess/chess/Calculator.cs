using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
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

        public static int hasVerticalCount(int[][] data, int color, int count, int row, int col, out LineType type)
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

        public static int hasHorizontalCount(int[][] data, int color, int count, int row, int col, out LineType type)
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
        public static int hasInclinedCount_LT(int[][] data, int color, int count, int row, int col, out LineType type)
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
        public static int hasInclinedCount_LB(int[][] data, int color, int count, int row, int col, out LineType type)
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

        #region 计算一个坐标点的一条线上有多少个连续的棋

        public static int calHorizontalCount(int[][] data, int row, int col, ref Line line)
        {
            int color = data[row][col];
            if(color == Color.NONE)
            {
                return 0;
            }

            int value_count = 1;
            line.color = color;
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
            else if(min_col > 0)
            {
                while(min_col >=0 && data[row][min_col] == Color.NONE)
                {
                    line.minSpace++;
                    min_col--;
                }
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
            else if (max_col < Side.COL_ID)
            {
                while (max_col <= Side.COL_ID && data[row][max_col] == Color.NONE)
                {
                    line.maxSpace++;
                    max_col++;
                }
            }
            Logs.writeln("calHorizontalCount count = " + value_count + "   type = " + line.type, 2);

            line.length = value_count;
            if(line.length > 1)
            {
                line.direction = Direction.Horizontal;
            }
            calCost(ref line);
            return line.length;
        }

        public static int calVerticalCount(int[][] data, int row, int col, ref Line line)
        {
            int color = data[row][col];
            if (color == Color.NONE)
            {
                return 0;
            }

            int value_count = 1;
            line.color = color;
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
            else if (min_row > 0)
            {
                while (min_row >= 0 && data[min_row][col] == Color.NONE)
                {
                    line.minSpace++;
                    min_row--;
                }
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
            else if (max_row < Side.ROW_ID)
            {
                while (max_row <= Side.COL_ID && data[max_row][col] == Color.NONE)
                {
                    line.maxSpace++;
                    max_row++;
                }
            }

            line.length = value_count;
            if (line.length > 1)
            {
                line.direction = Direction.Vertical;
            }
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
        public static int calInclinedCount_LT(int[][] data, int row, int col, ref Line line)
        {
            int color = data[row][col];
            if (color == Color.NONE)
            {
                return 0;
            }

            int value_count = 1;

            line.color = color;
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
            else if (min_col > 0 && min_row > 0)
            {
                while (min_col >= 0 && min_row >= 0 && data[min_row][min_col] == Color.NONE)
                {
                    line.minSpace++;
                    min_row--;
                    min_col--;
                }
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
            else if (max_col < Side.COL_ID && max_row < Side.ROW_ID)
            {
                while (max_col <= Side.COL_ID && max_row <= Side.COL_ID && data[max_row][max_col] == Color.NONE)
                {
                    line.maxSpace++;
                    max_row++;
                    max_col++;
                }
            }

            Logs.writeln("calInclinedCount_LT count = " + value_count + "   type = " + line.type, 2);

            line.length = value_count;
            if (line.length > 1)
            {
                line.direction = Direction.LetfTopToRightBottom;
            }
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
        public static int calInclinedCount_LB(int[][] data, int row, int col, ref Line line)
        {
            int color = data[row][col];
            if (color == Color.NONE)
            {
                return 0;
            }
            int value_count = 1;

            line.color = color;
            line.p1.row = line.p2.row = row;
            line.p1.col = line.p2.col = col;

            int min_col = col - 1;
            int max_row = row + 1;
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
            else if (min_col > 0 && max_row < Side.ROW_ID)
            {
                while (min_col >= 0 && max_row <= Side.ROW_ID && data[max_row][min_col] == Color.NONE)
                {
                    line.minSpace++;
                    max_row++;
                    min_col--;
                }
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
            else if (max_col < Side.COL_ID && min_row > 0)
            {
                while (max_col <= Side.COL_ID && min_row >= 0 && data[min_row][max_col] == Color.NONE)
                {
                    line.maxSpace++;
                    min_row--;
                    max_col++;
                }
            }

            line.length = value_count;
            if (line.length > 1)
            {
                line.direction = Direction.LetfBottomToRightTop;
            }
            calCost(ref line);
            return line.length;
        }

        #endregion

        #region 计算一条线上不连续的棋 或则 有其他颜色的棋

        //计算水平方向一串棋首尾是否还有不连续的棋 或则 有其他颜色的棋
        public static int calHorizontalLine(int[][] data, int color, Line src, ref Line line1, ref Line line2)
        {
            int tmp_color = Color.BLACK;
            if(color == Color.BLACK)
            {
                tmp_color = Color.WHITE;
            }
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
            return src.length + line1.length + line2.length;
        }

        //计算垂直方向一串棋首尾是否还有不连续的棋
        public static int calVerticalLine(int[][] data, int color, Line src, ref Line line1, ref Line line2)
        {
            int tmp_color = Color.BLACK;
            if (color == Color.BLACK)
            {
                tmp_color = Color.WHITE;
            }
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
            return src.length + line1.length + line2.length;
        }

        //计算左上到右下方向一串棋首尾是否还有不连续的棋
        public static int calInclined_LT(int[][] data, int color, Line src, ref Line line1, ref Line line2)
        {
            int tmp_color = Color.BLACK;
            if (color == Color.BLACK)
            {
                tmp_color = Color.WHITE;
            }
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
            return src.length + line1.length + line2.length;
        }

        //计算左下到右上方向一串棋首尾是否还有不连续的棋
        public static int calInclined_LB(int[][] data, int color, Line src, ref Line line1, ref Line line2)
        {
            int tmp_color = Color.BLACK;
            if (color == Color.BLACK)
            {
                tmp_color = Color.WHITE;
            }

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
            return src.length + line1.length + line2.length;
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
                                cost = Cost.One_1_2;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = Cost.One_1_1;
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
                                cost = Cost.Tow_2_2;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = Cost.Tow_2_1;
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
                                cost = Cost.Three_3_2a;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = Cost.Three_3_1a;
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
                                cost = Cost.Four_4_2a;
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = Cost.Four_4_1a;
                                break;
                        }
                        break;
                    }
                default:
                    cost = Cost.Five;
                    break;
            }

            line.cost = cost;
            return cost;
        }

        #endregion

        #region 计算落子之后一串棋增加的价值
        public static int calInCreaseCost(Line line, Position p)
        {
            int cost = 0;
            switch (line.type)
            {
                case LineType.BothDead: //小头（左上方）大头(右下方)都被堵死
                    cost = 0;
                    break;
                case LineType.MinDeadMaxLive: //小头（左上方）堵死，大头(右下方)活着
                    if (line.p1.row == p.row && line.p1.col == p.col)
                    {
                        switch (line.length)
                        {
                            case 1:
                                cost = Cost.One_1_1;
                                break;
                            case 2:
                                cost = Cost.One_add_1;
                                break;
                            case 3:
                                cost = Cost.Tow_add_1;
                                break;
                            case 4:
                                cost = Cost.Tree_add_1;
                                break;
                        }
                    }
                    else
                    {
                        switch (line.length)
                        {
                            case 1:
                                cost = Cost.One_1_1;
                                break;
                            case 2:
                                cost = Cost.One_add_1_a;
                                break;
                            case 3:
                                cost = Cost.Tow_add_1;
                                break;
                            case 4:
                                cost = Cost.Tree_add_1;
                                break;
                        }
                    }
                    break;
                case LineType.MinLiveMaxDead: //小头（左上方）活着，大头(右下方)堵死
                    if (line.p2.row == p.row && line.p2.col == p.col)
                    {
                        switch (line.length)
                        {
                            case 1:
                                cost = Cost.One_1_1;
                                break;
                            case 2:
                                cost = Cost.One_add_1;
                                break;
                            case 3:
                                cost = Cost.Tow_add_1;
                                break;
                            case 4:
                                cost = Cost.Tree_add_1;
                                break;
                        }
                    }
                    else
                    {
                        switch (line.length)
                        {
                            case 1:
                                cost = Cost.One_1_1;
                                break;
                            case 2:
                                cost = Cost.One_add_1_a;
                                break;
                            case 3:
                                cost = Cost.Tow_add_1;
                                break;
                            case 4:
                                cost = Cost.Tree_add_1;
                                break;
                        }
                    }
                    break;
                case LineType.BothLive: //大头小头都活着
                    switch (line.length)
                    {
                        case 1:
                            cost = Cost.One_1_2;
                            break;
                        case 2:
                            cost = Cost.One_add_1_b;
                            break;
                        case 3:
                            cost = Cost.Two_add_1_b;
                            break;
                        case 4:
                            cost = Cost.Tree_add_1_b;
                            break;
                    }
                    break;
                default:
                    cost = 0;
                    break;
            }
            return cost;
        }
        #endregion

        #region 计算落子之后一串棋减少的权值

        public static int calDreaseCost(Line line)
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
                                cost = (Cost.One_1_1);
                                break;
                            case LineType.BothLive:
                                //throw new Exception("This step found no effect.");
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = (Cost.One_1_2 - Cost.One_1_1);
                                break;
                        }
                        break;
                    }
                case 2:
                    {
                        switch (line.type)
                        {
                            case LineType.BothDead:
                                cost = (Cost.Tow_2_1);
                                break;
                            case LineType.BothLive:
                                //throw new Exception("This step found no effect.");
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = (Cost.Tow_2_2 - Cost.Tow_2_1);
                                break;
                        }
                        break;
                    }
                case 3:
                    {
                        switch (line.type)
                        {
                            case LineType.BothDead:
                                cost = (Cost.Three_3_1a);
                                break;
                            case LineType.BothLive:
                                //throw new Exception("This step found no effect.");
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = (Cost.Three_3_2a - Cost.Three_3_1a);
                                break;
                        }
                        break;
                    }
                case 4:
                    {
                        switch (line.type)
                        {
                            case LineType.BothDead:
                                cost = (Cost.Four_4_1a);
                                break;
                            case LineType.BothLive:
                                //throw new Exception("This step found no effect.");
                                break;
                            case LineType.MinDeadMaxLive:
                            case LineType.MinLiveMaxDead:
                                cost = (Cost.Four_4_2a - Cost.Four_4_1a);
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

        #region 计算落子后一串不连续的棋增加的价值
        public static int calUnSequentialLineCost(int[][] data, ref Line begin, ref Line mid, ref Line end, Position p)
        {
            int cost;
            if(begin.length > 0 && end.length > 0)
            {
                if (begin.color == mid.color && end.color == mid.color)
                {
                    //空间不足以5连
                    int len = begin.length + end.length + mid.length;
                    int space = 2 + begin.minSpace + end.maxSpace;
                    switch(len)
                    {
                        case 3:
                            switch(space)
                            {
                                case 2:
                                case 3:
                                    cost = Cost.Three_111_0a;
                                    break;
                                case 4:
                                    cost = Cost.Three_111_1a;
                                    break;
                                default:
                                    cost = Cost.Three_111_2a;
                                    break;
                            }
                            break;
                        case 4:
                            switch (space)
                            {
                                case 2:
                                    cost = Cost.Four_211_0a;
                                    break;
                                case 3:
                                    if (mid.length == 2 || (begin.length == 2 && begin.type == LineType.BothLive) || (end.length == 2 && end.type == LineType.BothLive))
                                        cost = Cost.Four_211_2a;
                                    else
                                        cost = Cost.Four_211_1a;
                                    break;
                                default:
                                    cost = Cost.Four_211_12a;
                                    break;
                            }
                            break;
                        case 5:
                            switch (space)
                            {
                                case 2:
                                    if (mid.length == 1)
                                    {
                                        if (begin.length == 2)
                                            cost = Cost.Fiv_212_0a;
                                        if (begin.length == 3 || begin.length == 1)
                                            cost = 1;
                                    }
                                    else if (mid.length == 2)
                                    {
                                        cost = Cost.Fiv_221_0a;
                                    }
                                    else if (mid.length == 3)
                                    {
                                        cost = Cost.Fiv_131_0a;
                                    }
                                    break;
                                case 3:
                                    if (mid.length == 1)
                                    {
                                        if (begin.length == 2)
                                            cost = Cost.Fiv_212_0a;
                                        if (begin.length == 3 || begin.length == 1)
                                            cost = 1;
                                    }
                                    else if (mid.length == 2)
                                    {
                                        cost = Cost.Fiv_221_0a;
                                    }
                                    else if (mid.length == 3)
                                    {
                                        cost = Cost.Fiv_131_0a;
                                    }
                                    break;
                                default:
                                    cost = Cost.Four_211_12a;
                                    break;
                            }
                            break;
                    }
                }
            }
            else if(begin.length > 0)
            {

            }
            else if(end.length > 0)
            {

            }
            return 0;
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

        public static int calIncreaseValue(int[][] data, ref Position p)
        {
            int color = p.color;
            int row = p.row;
            int col = p.col;
            int cost = 0;
            //是否是独子
            #region 水平增量
            {
                int cost_v = 0;
                line_h.clear();
                line_h_min.clear();
                line_h_max.clear();

                calHorizontalCount(data, row, col, ref line_h);
                cost = calInCreaseCost(line_h, p);

                //计算是否有不连续的棋
                calHorizontalLine(data, color, line_h, ref line_h_min, ref line_h_max);

                calUnSequentialLineCost(data, ref line_h_min, ref line_h, ref line_h_max, p);

                if (line_h_min.color == color)
                {

                    cost += line_h_min.cost;
                }
                else
                {
                    cost += 2 * calDreaseCost(line_h_min); ;
                }

                if (line_h_max.color == color)
                {
                    cost += line_h_max.cost;
                }
                else
                {
                    cost += 2 * calDreaseCost(line_h_max);
                }
                cost_v = cost;
                cost = 0;
            }
            #endregion

            #region 垂直增量
            {
                line_v.clear();
                line_v_min.clear();
                line_v_max.clear();

                calVerticalCount(data, row, col, ref line_v);
                cost = calInCreaseCost(line_h, p);

                //计算是否有不连续的棋
                calVerticalLine(data, color, line_v, ref line_v_min, ref line_v_max);

                cost += line_v.cost;
                if (line_v_min.color == color)
                {
                    cost += line_v_min.cost;
                }
                else
                {
                    cost += 2 * calDreaseCost(line_v_min);
                }
                if (line_v_max.color == color)
                {
                    cost += line_v_max.cost;
                }
                {
                    cost += 2 * calDreaseCost(line_v_max);
                }
                cost = 0;
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

                cost += line_lt.cost;
                if (line_lt_min.color == color)
                {
                    cost += line_lt_min.cost;
                }
                else
                {
                    cost += 2 * calDreaseCost(line_lt_min);
                }
                if (line_lt_max.color == color)
                {
                    cost += line_lt_max.cost;
                }
                {
                    cost += 2 * calDreaseCost(line_lt_max);
                }

                cost = 0;
            }
            #endregion

            #region 坐下到右上
            {
                line_lb.clear();
                line_lb_min.clear();
                line_lb_max.clear();

                calInclinedCount_LB(data, row, col, ref line_lb);
                Logs.writeln("length = " + line_lb.length + " type = " + line_lb.type + "  p1 = (" + line_lb.p1.col + "," + line_lb.p1.row + ")," + "  p2 = (" + line_lb.p2.col + "," + line_lb.p2.row + "),", 2);

                //计算是否有不连续的棋
                calInclined_LB(data, color, line_lb, ref line_lb_min, ref line_lb_max);

                cost += line_lb.cost;
                if (line_lb_min.color == color)
                {
                    cost += line_lb_min.cost;
                }
                else
                {
                    cost += 2 * calDreaseCost(line_lb_min);
                }
                if (line_lb_max.color == color)
                {
                    cost += line_lb_max.cost;
                }
                {
                    cost += 2 * calDreaseCost(line_lb_max);
                }

                cost = 0;
            }
            #endregion

            p.val += cost;

            if(line_v.length == 1 && line_h.length == 1 && line_lb.length == 1 && line_lt.length == 1
                && line_h_max.length == 0 && line_h_min.length == 0 && line_v_max.length == 0 && line_v_min.length == 0
                && line_lt_max.length == 0 && line_lt_min.length == 0 && line_lb_max.length == 0 && line_lb_min.length == 0)
            {
                //这里强行把值设为0，防止排序的时候出错
                p.val = 0;
                p.alone = true;
            }

            return cost;
        }
        #endregion

     }
}
