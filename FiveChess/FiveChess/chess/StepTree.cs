using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gobang
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
            _bestPosition.reset();
            _priority = 0;
            Logs.writeln("count = " + Searcher._cal_count, 4);
            foreach (var node in _tree)
            {
                if(_priority <= 5 && node.Value.rootNode.total == (int)WinState.BLACK_WIN )
                {
                    if(_bestPosition.val != (int)WinState.BLACK_WIN)
                    {
                        _bestPosition.reset(node.Value.rootNode);
                    }
                    else if (node.Value.rootNode.val > _bestPosition.val)
                    {
                        _bestPosition.reset(node.Value.rootNode);
                    }
                    _priority = 5;
                    Logs.writeln("_priority=" + _priority + "\tval=" + _bestPosition.val + "\tdeep = " + _bestPosition.depth + "\trow=" + _bestPosition.row + "\tcol=" + _bestPosition.col + "\tcolor=" + _bestPosition.color + "\twin=" + _bestPosition.total, 4);
                }
                else if (_priority <= 4 && node.Value.rootNode.haveFail != 1)
                {
                    if(_priority == 4)
                    {
                        if (node.Value.rootNode.val > _bestPosition.val)
                        {
                            _bestPosition.reset(node.Value.rootNode);
                        }
                    }
                    else
                    {
                        _bestPosition.reset(node.Value.rootNode);
                    }
                     _priority = 4;
                    Logs.writeln("_priority=" + _priority + "\tval=" + _bestPosition.val + "\tdeep = " + _bestPosition.depth + "\trow=" + _bestPosition.row + "\tcol=" + _bestPosition.col + "\tcolor=" + _bestPosition.color + "\twin=" + _bestPosition.total, 4);
                }
                else if (_priority <= 3 && node.Value.rootNode.haveWin == 1)
                {
                    if(_bestPosition.haveWin != 1)
                    {
                        _bestPosition.reset(node.Value.rootNode);
                    }
                    else if (node.Value.rootNode.val > _bestPosition.val)
                    {
                        _bestPosition.reset(node.Value.rootNode);
                    }
                     _priority = 3;
                    Logs.writeln("_priority=" + _priority + "\tval=" + _bestPosition.val + "\tdeep = " + _bestPosition.depth + "\trow=" + _bestPosition.row + "\tcol=" + _bestPosition.col + "\tcolor=" + _bestPosition.color + "\twin=" + _bestPosition.total, 4);
                }
                else if(_priority <= 1)
                {
                    //选择最长路径
                    //if (node.Value.rootNode.val > _bestPosition.val)
                    {
                        _bestPosition.reset(node.Value.rootNode);
                        _priority = 1;
                        Logs.writeln("_priority=" + _priority + "\tval=" + _bestPosition.val + "\tdeep = " + _bestPosition.depth + "\trow=" + _bestPosition.row + "\tcol=" + _bestPosition.col + "\tcolor=" + _bestPosition.color + "\twin=" + _bestPosition.total, 4);
                    }
                }
            }
            return _bestPosition;
        }

        public void print()
        {
            Logs.writeln("deep = " + _position.depth + "\trow=" + _position.row + "\tcol=" + _position.col + "\tcolor=" + _position.color + "\twin=" + _position.total, 4);// "\tval=" + _position.val +
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
