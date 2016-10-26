using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    public class StepTree
    {
        public StepTree(Position p) { _position = p; }
        public StepTree(){ _position = new Position(); }

        public void addChild(Position p)
        {
            _childList.Add(p);
            if (!_tree.ContainsKey(p))
            {
                _tree.Add(p, new StepTree(p));
            }
        }

        public void addChild(ref Position p, ref StepTree st)
        {
            _childList.Add(p);
            if (!_tree.ContainsKey(p))
            {
                st._position = p;
                st._parent = this;
                _tree.Add(p, st);
            }
        }

        private int _priority = 0;
        private Position _bestPosition = new Position();
        public Position selectBestPosition()
        {
            _priority = 0;
            foreach (var node in _tree)
            {
                if(_priority < 3 && node.Value.rootNode.win == (int)WinState.BLACK_WIN )
                {
                    _bestPosition.row = node.Value.rootNode.row;
                    _bestPosition.col = node.Value.rootNode.col;
                    _bestPosition.val = node.Value.rootNode.val;
                    _bestPosition.win = node.Value.rootNode.win;
                    _bestPosition.color = node.Value.rootNode.color;
                    _bestPosition.depth = node.Value.rootNode.depth;
                    _priority = 3;
                }
                else if (_priority < 2 && node.Value.rootNode.win != (int)WinState.WHITE_WIN)
                {
                    _bestPosition.row = node.Value.rootNode.row;
                    _bestPosition.col = node.Value.rootNode.col;
                    _bestPosition.val = node.Value.rootNode.val;
                    _bestPosition.win = node.Value.rootNode.win;
                    _bestPosition.color = node.Value.rootNode.color;
                    _bestPosition.depth = node.Value.rootNode.depth;
                    _priority = 2;
                }
                else if(_priority < 1 && node.Value != null )
                {
                    _bestPosition.row = node.Value.rootNode.row;
                    _bestPosition.col = node.Value.rootNode.col;
                    _bestPosition.val = node.Value.rootNode.val;
                    _bestPosition.win = node.Value.rootNode.win;
                    _bestPosition.color = node.Value.rootNode.color;
                    _bestPosition.depth = node.Value.rootNode.depth;
                    _priority = 1;
                }
            }
            Logs.writeln("val=" + _bestPosition.val + "\tdeep = " + _bestPosition.depth + "\trow=" + _bestPosition.row + "\tcol=" + _bestPosition.col + "\tcolor=" + _bestPosition.color + "\twin=" + _bestPosition.win, 4);
            return _bestPosition;
        }

        public void print()
        {
            Logs.writeln("deep = " + _position.depth + "\trow=" + _position.row + "\tcol=" + _position.col + "\tcolor=" + _position.color + "\twin=" + _position.win, 4);// "\tval=" + _position.val +
            foreach (var node in _tree)
            {
                node.Value.print();
            }
        }

        public void SetBase(Position pos)
        {
            _position = pos;
        }

        public Position rootNode
        {
            get { return _position; }
            set { _position = value; }
        }

        //子节点表
        private List<Position> _childList = new List<Position>();
        public List<Position> keys
        {
            get { return _childList; }
        }

        //子节点的节点树
        private Dictionary<Position, StepTree> _tree = new Dictionary<Position, StepTree>(); 
        public Dictionary<Position, StepTree> rootTree
        {
            get { return _tree; }
        }

        //父节点的节点树
        private StepTree _parent = null;
        public StepTree parent
        {
            get { return _parent; }
        }

        //当前的节点值
        private Position _position;
    }
}
