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

            _droper = new Droper(_rule);
            _droper.setPCColor(_PC.color);

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
            int color = CurrentPlayer.color;

            _chessboard.setData(row, col, color);

            Position p = _droper.thinkNext(_chessboard.Data, color, row, col, 3);

            int state = rule.checkWinner(_chessboard.Data, row, col);

            switch (state)
            {
                case WinState.BLACK_WIN:
                    Logs.writeln("\n===================\n\nBLACK WINS!!!\n\n===================\n", Logs.level4);
                    return;
                case WinState.WHITE_WIN:
                    Logs.writeln("\n===================\n\nWHITE WINS!!!\n\n===================\n", Logs.level4);
                    return;
                default:
                    break;
            }

            _chessboard.setData(p.row, p.col, p.color);

            state = rule.checkWinner(_chessboard.Data, p.row, p.col);

            switch (state)
            {
                case WinState.BLACK_WIN:
                    Logs.writeln("\n===================\n\nBLACK WINS!!!\n\n===================\n", Logs.level4);
                    return;
                case WinState.WHITE_WIN:
                    Logs.writeln("\n===================\n\nWHITE WINS!!!\n\n===================\n", Logs.level4);
                    return;
                default:
                    break;
            }
            
            //swapTurn();
        }
    }
}
