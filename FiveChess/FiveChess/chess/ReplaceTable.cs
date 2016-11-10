using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gobang
{
    class HashItem
    {
        public const int type_value = 1;
        public const int type_low = 2;
        public const int type_upper = 3;

        public HashItem() { }
        public ulong checkSum;
        public int type;            // color of current pawn
        public int PawnSum;         // sum of pawns
        public int value;           // value of current pawn
    }

    class ReplaceTable
    {
        public const uint HASH_ITEM_SIZE = 0xffffff;

        public ReplaceTable()
        {
            //初始化hash棋盘列表，放入随机数用以计算棋盘hash值
            _hashtable32 = new uint[2][][];
            _hashtable64 = new ulong[2][][];
            for (int i = 0; i < 2; i ++)
            {
                _hashtable32[i] = new uint[Side.ROW][];
                _hashtable64[i] = new ulong[Side.ROW][];
                for (int j = 0; j < Side.ROW; j++)
                {
                    _hashtable64[i][j] = new ulong[Side.COL];
                    _hashtable32[i][j] = new uint[Side.COL];
                    for (int k = 0; k < Side.COL; k++)
                    {
                        _hashtable64[i][j][k] = getUInt64();

                        ////////////////////////////

                        _hashtable32[i][j][k] = getUInt32();
                    }
                }
            }

            //初始化置换表数据
            _hashItemTable = new HashItem[2][];
            for(int i = 0; i < 2; i++)
            {
                _hashItemTable[i] = new HashItem[HASH_ITEM_SIZE];
                for(int j = 0; j < HASH_ITEM_SIZE; j++)
                {
                    _hashItemTable[i][j] = new HashItem();
                }
            }
        }

        private uint x = 0;
        private HashItem ht = null;
        public int LookupHashTable(int alpha, int beta, int PawnSum, int TableNo)
        {
            x = (_hashKey32 & HASH_ITEM_SIZE);
            ht = _hashItemTable[TableNo - 1][x];

            if(ht.checkSum == _hashKey64 && ht.PawnSum == PawnSum)
            {
                switch(ht.type)
                {
                    case HashItem.type_value:
                        return ht.value;

                    case HashItem.type_low:
                        if (ht.value >= beta)
                            return ht.value;

                        break;
                    case HashItem.type_upper:
                        if (ht.value <= alpha)
                            return ht.value;

                        break;
                }
            }
            //返回无效值
            return WinState.INVALIDE;
        }

        public void Insert(int pawnSum, int type, int value, int TableNo)
        {
            x = (_hashKey32 & HASH_ITEM_SIZE);
            ht = _hashItemTable[TableNo - 1][x];

            ht.checkSum = _hashKey64;
            ht.PawnSum = pawnSum;
            ht.type = type;
            ht.value = value;
        }

        public void DoStep(int row, int col, int color)
        {
            _hashKey32 ^= _hashtable32[color - 1][row][col];
            _hashKey64 ^= _hashtable64[color - 1][row][col];
        }

        public void UnDoStep(int row, int col, int color)
        {
            _hashKey32 ^= _hashtable32[color - 1][row][col];
            _hashKey64 ^= _hashtable64[color - 1][row][col];
        }

        public void getHashValue64(int[][] data)
        {
            for (int row = 0; row < data.Length; row ++)
            {
                for( int col = 0; col < data.Length; col++)
                {
                    int color = data[row][col];
                    if(color != 0)
                    {
                        _hashKey64 ^= _hashtable64[color - 1] [row] [col];
                    }
                }
            }
        }

        public void getHashValue32(int[][] data)
        {
            for (int row = 0; row < data.Length; row++)
            {
                for (int col = 0; col < data.Length; col++)
                {
                    int color = data[row][col];
                    if (color != 0)
                    {
                        _hashKey32 ^= _hashtable32[color - 1][row][col];
                    }
                }
            }
        }

        public ulong getUInt64()
        {
            ulong tmp64 = 0;
            ulong result = 0;
            for(int i = 0; i < 7; i++)
            {
                tmp64 = (ulong)_random.Next(0xff);
                tmp64 <<= i * 8;
                result |= tmp64;
            }

            return result;
        }

        public uint getUInt32()
        {
            uint tmp32 = 0;
            uint result = 0;

            for (int i = 0; i < 3; i++)
            {
                tmp32 = (uint)_random.Next(0xff);
                tmp32 <<= i * 8;
                result |= tmp32;
            }

            return result;
        }

        private Random _random = new Random();

        private uint _hashKey32;
        private ulong _hashKey64;

        private static HashItem[][] _hashItemTable;

        private ulong[][][] _hashtable64;
        private uint[][][] _hashtable32;
    }
}
