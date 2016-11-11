using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gobang
{
    #region 落子表，自定义链表结构

    public class DropPosition
    {
        public DropPosition() { }
        public DropPosition(Position p)
        {
            this.p = p;
        }

        public void reset()
        {
            next = null;
            //prev = null;
            p.reset();
        }

        public void reset(Position p)
        {
            next = null;
            //prev = null;
            this.p.reset(p);
        }

        public Position p = new Position();

        //public DropPosition prev;
        public DropPosition next;
        public int index = 0;
    }

    public class DropTableArry
    {
        public DropTableArry()
        {
            _data = new DropPosition[Side.ROW * Side.COL];
            for (int i = 0; i < Side.ROW * Side.COL; i++)
            {
                _data[i] = new DropPosition();
                _data[i].index = i;
            }
            _head = _data[0];
            //_end = _data[0];
            //DropPositionIdx = new int[Side.ROW * Side.COL];
        }

        public void clear()
        {
            _head = _data[0];
            _head.reset();
            _curIdx = 0;
        }

        private DropPosition add_front = null;
        private DropPosition add_next = null;
        public void addPositionReverse(ref Position pos)
        {
            if (pos.val == 0)
            {
                return;
            }
            if (pos.alone)
            {
                return;
            }
            
            _data[_curIdx].reset(pos);

            add_front = null;
            add_next = _head;

            while (add_next != null && pos.val < add_next.p.val)
            {
                add_front = add_next;
                add_next = add_next.next;
            }

            if(add_front != null)
            {
                add_front.next = _data[_curIdx];
            }
            if( add_next != null && _curIdx != add_next.index)
            {
                _data[_curIdx].next = add_next;
                if (_head == add_next)
                {
                    _head = _data[_curIdx];
                }
            }

            _curIdx++;
        }

        public void addPositionPositive(ref Position pos)
        {
            if (pos.val == 0)
            {
                return;
            }
            if (pos.alone)
            {
                return;
            }

            _data[_curIdx].p.reset(pos);

            add_front = null;
            add_next = _head;

            while (add_next != null && pos.val > add_next.p.val)
            {
                add_front = add_next;
                add_next = add_next.next;
            }

            if (add_front != null)
            {
                add_front.next = _data[_curIdx];
            }
            if (add_next != null && _curIdx != add_next.index)
            {
                _data[_curIdx].next = add_next;
                if (_head == add_next)
                {
                    _head = _data[_curIdx];
                }
            }

            _curIdx++;
        }

        public void print()
        {
            DropPosition p = _head;
            int count = 0;
            while ( p != null )
            {
                Logs.writeln("p.row=" + p.p.row + " \tp.col=" + p.p.col + " \tp.val=" + p.p.val + " \tp.color=" + p.p.color + " \tindex="+p.index, 4);
                p = p.next;
                count++;
            }

            Logs.writeln("length = " + Length + "\tcount=" + count, 4);
        }

        private int _curIdx = 0;
        public int Length
        {
            get { return _curIdx; }
        }

        private DropPosition _head;
        public DropPosition getFront()
        {
            return _head;
        }

        //public DropPosition getEnd()
        //{
        //    return _end;
        //}

        //private DropPosition _end;
        private DropPosition[] _data;

    }

    #endregion

    #region 可以落子的表, 用链表做数据
    public class DropTable
    {
        public DropTable()
        {
            _data = new List<Position>(150);
            //_Table = new Position[Side.COL * Side.ROW];
            //for (int i = 0; i < _Table.Length; i++)
            //{
            //    _Table[i] = new Position();
            //}
        }

        public void clear()
        {
            _data.Clear();
            //for (int r = 0; r <= Side.ROW_ID; r++)
            //{
            //    for (int c = 0; c <= Side.COL_ID; c++)
            //    {
            //        _Table[r * Side.COL + c].row = 0;
            //        _Table[r * Side.COL + c].col = 0;
            //        _Table[r * Side.COL + c].val = 0;
            //        //Logs.write("(" + _Table[r * Side.COL + c].row + " " + _Table[r * Side.COL + c].col + ")" + _Table[r * Side.COL + c].val + ",\t\t\t", 4);
            //    }
            //    //Logs.writeln("", 4);
            //}
        }

        //public void sort()
        //{
        //    Position.sort(_Table);
        //    for (int r = 0; r <= Side.ROW_ID; r++)
        //    {
        //        for (int c = 0; c <= Side.COL_ID; c++)
        //        {
        //            Logs.write("(" + _Table[r * Side.COL + c].row + " " + _Table[r * Side.COL + c].col + ")" + _Table[r * Side.COL + c].val + ",\t\t\t", 4);
        //        }
        //        Logs.writeln("", 4);
        //    }
        //}

        public void addPositionReverse(ref Position pos)
        {
            int low = 0;
            int hight = _data.Count;
            int mid = (low + _data.Count) / 2;
            int current = 0;

            //if (pos.row == 14 && pos.row == 14)
            //{
            //    Logs.write("\n\n", 4);
            //    foreach (var p in _data)
            //    {
            //        Logs.write(p.val + "(" + p.alone +") ", 4);
            //    }
            //}

            if (pos.val == 0)
            {
                return;
            }
            if(pos.alone)
            {
                _data.Add(pos);
                return;
            }
            while (low < hight)
            {
                current = _data.ElementAt(mid).val;
                if (current < pos.val)
                {
                    hight = mid - 1;
                    mid = (low + hight) / 2;
                }
                else if (current > pos.val)
                {
                    low = mid + 1;
                    mid = (low + hight) / 2;
                }
                else
                {
                    break;
                }
            }

            if (mid < _data.Count && _data.ElementAt(mid).val > pos.val)
            {
                _data.Insert(mid + 1, pos);
            }
            else
            {
                _data.Insert(mid, pos);
            }

            //Logs.writeln("Droptable capacity = " + _data.Capacity, 4);
        }

        public void addPositionPositive(ref Position pos)
        {
            int low = 0;
            int hight = _data.Count;
            int mid = (low + _data.Count) / 2;
            int current = 0;

            //if (pos.row == 14 && pos.row == 14)
            //{
            //    Logs.write("\n\n", 4);
            //    foreach (var p in _data)
            //    {
            //        Logs.write(p.val + "(" + p.alone +") ", 4);
            //    }
            //}

            if (pos.val == 0)
            {
                return;
            }
            if (pos.alone)
            {
                _data.Add(pos);
                return;
            }
            while (low < hight)
            {
                current = _data.ElementAt(mid).val;
                if (current > pos.val)
                {
                    hight = mid - 1;
                    mid = (low + hight) / 2;
                }
                else if (current < pos.val)
                {
                    low = mid + 1;
                    mid = (low + hight) / 2;
                }
                else
                {
                    break;
                }
            }

            if (mid < _data.Count && _data.ElementAt(mid).val < pos.val)
            {
                _data.Insert(mid + 1, pos);
            }
            else
            {
                _data.Insert(mid, pos);
            }
        }

        public void print()
        {
            foreach(Position p in _data)
            {
                Logs.writeln("p.row=" + p.row + " p.col=" + p.col + " p.val=" + p.val + " p.color=" + p.color, 4);
            }
        }

        public int Length
        {
            get { return _data.Count; }
        }
        //private Position[] _Table;

        public Position getValue(int pos)
        {
            return _data.ElementAt(pos);
        }

        //public Position getValue(int row, int col)
        //{
        //    return _Table[row * Side.ROW + col];
        //}

        private List<Position> _data;
    }

    #endregion
}
