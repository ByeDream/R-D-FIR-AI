using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gobang
{
    public enum PlayerType
    {
        NONE = 0,
        HUNMAN = 1,
        PC = 2,
    }

    class Player
    {
        public Player(PlayerType type, int color)
        {
            _type = type;
            _color = color;
        }

        private PlayerType _type  = 0;
        public PlayerType type
        {
            get { return _type; }
            set { _type = value; }
        }

        private int _color = 0;
        public int color
        {
            get { return _color; }
            set { _color = value; }
        }
    }
}
