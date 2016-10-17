using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace Chess
{
    class ChessManager
    {
        public ChessManager()
        {
            _ui = new UI();
            _chessboard = new ChessBoard();
            _rule = new Rule();

            _Hunman = new Player(PlayerType.HUNMAN, Color.WHITE);
            _PC = new Player(PlayerType.PC, Color.BLACK);
            currentPlayer = _Hunman;

            _droper = new Droper(_chessboard);

            _ui.chessboard = _chessboard;
        }

        #region ChessBoard

        private ChessBoard _chessboard = null;

        public ChessBoard chessboard
        {
            get { return _chessboard; }
            set { _chessboard = value; }
        }
        #endregion

        #region UI

        private UI _ui = null;

        public UI ui
        {
            get { return _ui; }
            set { _ui = value; }
        }

        #endregion

        #region Rule

        private Rule _rule = null;

        public Rule rule
        {
            get { return _rule; }
            set { _rule = value; }
        }

        #endregion

        #region Player turn status

        //玩家
        private Player _Hunman = null;
        private Player _PC = null;

        public Player Hunman { get { return _Hunman; } }
        public Player PC { get { return _PC; } }

        //当前的玩家
        private Player currentPlayer = null;

        public Player CurrentPlayer
        {
            get { return currentPlayer; }
            //set { currentPlayer = value; }
        }

        public Player swapTurn()
        {
            if (currentPlayer.type == PlayerType.HUNMAN)
            {
                currentPlayer = _PC;
            }
            else
            {
                currentPlayer = _Hunman;
            }
            return currentPlayer;
        }

        #endregion

        #region 
        private Droper _droper;
        public Droper droper
        {
            get { return droper; }
        }
        
        #endregion


        public void drawUI(Graphics g)
        {
            _ui.drawChess(g);
        }

        public void PlayChess(int row, int col)
        {
            byte color = CurrentPlayer.color;

            _chessboard.setData(row, col, color);

            _droper.calCanDrop(color);

            rule.checkWinner(_chessboard, row, col);

            swapTurn();
        }
    }
}
