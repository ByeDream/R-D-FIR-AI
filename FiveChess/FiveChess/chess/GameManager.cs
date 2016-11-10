﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace Gobang
{
    class GameManager
    {
        public GameManager()
        {
            _ui = new UI();
            _chessboard = new Pawns();
            _rule = new Rule();

            _Hunman = new Player(PlayerType.HUNMAN, Color.WHITE);
            _PC = new Player(PlayerType.PC, Color.BLACK);
            currentPlayer = _Hunman;

            _droper = new Searcher(_rule);
            _droper.setPCColor(_PC.color);

            _ui.chessboard = _chessboard;
        }

        #region ChessBoard

        private Pawns _chessboard = null;

        public Pawns chessboard
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
        private Searcher _droper;
        public Searcher droper
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
            int state = rule.checkWinner(_chessboard.Data, CurrentPlayer.color, row, col);

            switch (state)
            {
                case WinState.BLACK_WIN:
                    Logs.writeln("\n===================\n\nBLACK WIN!!!\n\n===================\n", Logs.level4);
                    return;
                case WinState.WHITE_WIN:
                    Logs.writeln("\n===================\n\nWHITE WIN!!!\n\n===================\n", Logs.level4);
                    return;
                default:
                    break;
            }

            Position p = _droper.think(_chessboard.Data, CurrentPlayer.color, row, col, 5);

            _chessboard.setData(p.row, p.col, p.color);

            state = rule.checkWinner(_chessboard.Data, p.color, p.row, p.col);

            switch (state)
            {
                case WinState.BLACK_WIN:
                    Logs.writeln("\n===================\n\nBLACK WIN!!!\n\n===================\n", Logs.level4);
                    return;
                case WinState.WHITE_WIN:
                    Logs.writeln("\n===================\n\nWHITE WIN!!!\n\n===================\n", Logs.level4);
                    return;
                default:
                    break;
            }
            
            //swapTurn();
        }
    }
}
