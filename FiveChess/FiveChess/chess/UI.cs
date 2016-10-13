using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FiveChess;

namespace Chess
{
    class UI
    {
        private const int img_offset = 22;
        private const int grid_width = 35;
        private const int chess_width = 14;

        private const int click_offset_min = 12;
        private const int click_offset_max = 23;

        public UI()
        {
            white_chess = FiveChess.Properties.Resources.white_chess;
            black_chess = FiveChess.Properties.Resources.black_chess;
        }

        public void drawChess(Graphics g)
        {
            for( int row = 0; row < _chb.Data.Length; row++)
            {
                for (int col = 0; col < _chb.Data[row].Length; col++)
                {
                    if( ChessBoard.CHESS_WHITE == _chb.Data[row][col] )
                    {
                        g.DrawImage(white_chess, new Point(col * grid_width + img_offset - chess_width, row * grid_width + img_offset - chess_width));
                    }
                    else if(ChessBoard.CHESS_BLACK == _chb.Data[row][col] )
                    {
                        g.DrawImage(black_chess, new Point(col * grid_width + img_offset - chess_width, row * grid_width + img_offset - chess_width));
                    }
                }
            }
        }


        /// class members
        //////////////////////////////////

        private Image white_chess = null;
        private Image black_chess = null;

        private  ChessBoard _chb  = null;
        public ChessBoard chessboard
        {
            set { _chb = value; }
        }

        //////////////////////////////////


        /// global functions
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static int FormatYToRow(int Y)
        {
            int tmpY = Y;
            int row = -1;

            tmpY = (tmpY - img_offset) % grid_width;

            Logs.writeln("tmpY = " + tmpY);

            if (tmpY < click_offset_min)
            {
                if (tmpY > -click_offset_min)
                {
                    row = (Y - img_offset) / grid_width;
                }
                Logs.writeln("click_offset_min  row = " + row);
            }
            else if (tmpY > click_offset_max)
            {
                row = (Y - img_offset + click_offset_min) / grid_width;
                Logs.writeln("click_offset_max  row = " + row);
            }

            if (row > click_offset_min)
            {
                row = click_offset_min;
            }

            return row;
        }

        public static int FormatXToCol(int X)
        {
            int tmpX = X;
            int col = -1;

            tmpX = (tmpX - img_offset) % grid_width;

            Logs.writeln("tmpX = " + tmpX);

            if (tmpX < click_offset_min)
            {
                if (tmpX > -click_offset_min)
                {
                    col = (X - img_offset) / grid_width;
                }
                Logs.writeln("click_offset_min  col = " + col);
            }
            else if (tmpX > click_offset_max)
            {
                col = (X - img_offset + click_offset_min) / grid_width;
                Logs.writeln("click_offset_max  col = " + col);
            }

            if (col > click_offset_min)
            {
                col = click_offset_min;
            }

            return col;
        }
    }
}
