using System.Drawing;
using System.Windows.Forms;
using System;
using Gobang;


namespace FiveChess
{
    partial class ChessForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private GameManager chessMan = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ChessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::FiveChess.Properties.Resources.ChessBG_15_15;
            this.ClientSize = new System.Drawing.Size(535, 535);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.DoubleBuffered = true;
            this.Name = "ChessForm";
            this.Text = "Gobang";

            chessMan = new GameManager();

            this.ResumeLayout(false);

        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            //Logs.writeln("OnPaint");

            Graphics g = e.Graphics;

            chessMan.drawUI(g);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            int row = UI.FormatYToRow(e.Y);
            int col = UI.FormatXToCol(e.X);

            Logs.writeln("OnMouseClick X = " + e.X + "   Y=" + e.Y + "    row=" + row +  "    col=" + col, Logs.level3);

            if(row != -1 && col != -1)
            {
                chessMan.chessboard.setData(row, col, chessMan.Hunman.color);
                this.Refresh();

                chessMan.PlayChess(row, col);
                this.Refresh();
            }
        }

        //protected override void OnMouseCaptureChanged(EventArgs e)
        //{
        //    System.Console.WriteLine("OnMouseCaptureChanged");

        //    System.Console.WriteLine("sizeof(byte) = " + sizeof(byte));

        //    this.Refresh();
        //}

        //protected override void OnMouseDoubleClick(MouseEventArgs e)
        //{ }

        //protected override void OnMouseDown(MouseEventArgs e)
        //{ }

        //protected override void OnMouseEnter(EventArgs e)
        //{ }

        //protected override void OnMouseHover(EventArgs e)
        //{ }

        //protected override void OnMouseLeave(EventArgs e)
        //{ }

        //protected override void OnMouseMove(MouseEventArgs e)
        //{ }

        //protected override void OnMouseUp(MouseEventArgs e)
        //{ }

        //protected override void OnMouseWheel(MouseEventArgs e)
        //{ }
    }
}
