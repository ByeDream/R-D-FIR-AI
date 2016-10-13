using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    class ChessManager
    {
        public ChessManager()
        {
            _ui = new UI();
            _chessboard = new ChessBoard();
            _rule = new Rule();


            _ui.chessboard = _chessboard;
        }

        /// <summary>
        /// /
        /// </summary>
        private ChessBoard _chessboard = null;

        public ChessBoard chessboard
        {
            get { return _chessboard; }
            set { _chessboard = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        private UI _ui = null;

        public UI ui
        {
            get { return _ui; }
            set { _ui = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private Rule _rule = null;

        public Rule rule
        {
            get { return _rule; }
            set { _rule = value; }
        }
    }
}
