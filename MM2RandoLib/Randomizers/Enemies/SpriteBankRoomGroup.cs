using System;
using System.Collections.Generic;
using MM2Randomizer.Enums;

namespace MM2Randomizer.Randomizers.Enemies
{
    public class SpriteBankRoomGroup
    {
        public EStageID Stage { get; set; }

        /// <summary>
        /// Room numbers in the stage that this sprite bank applies to. Will always be sorted from least to greatest.
        /// </summary>
        public List<Room> Rooms { get; set; }
        public Int32 PatternAddressStart { get; set; }
        public List<EnemyType> NewEnemyTypes { get; set; }

        public Boolean IsSpriteRestricted { get; set; }
        public List<Int32> SpriteBankRowsRestriction { get; set; }
        public List<Byte> PatternTableAddressesRestriction { get; set; }

        public SpriteBankRoomGroup (EStageID stage, Int32 patternAddressStart, Int32[] roomNums)
            : this(stage, patternAddressStart, roomNums, null, null, false)
        {
        }

        public SpriteBankRoomGroup (EStageID stage, Int32 patternAddressStart, Int32[] roomNums, Int32[] spriteBankRowsRestriction, Byte[] patternTableAddressesRestriction/*, params EnemyInstance[] enemyInstances*/)
            : this(stage, patternAddressStart, roomNums, spriteBankRowsRestriction, patternTableAddressesRestriction, true)
        {
        }

        private SpriteBankRoomGroup(EStageID stage, Int32 patternAddressStart, Int32[] roomNums, Int32[]? spriteBankRowsRestriction, Byte[]? patternTableAddressesRestriction, Boolean isSpriteRestricted)
        {
            this.Stage = stage;
            this.PatternAddressStart = patternAddressStart;

            this.Rooms = new List<Room>();
            for (Int32 i = 0; i < roomNums.Length; i++)
            {
                this.Rooms.Add(new Room(roomNums[i]));
            }

            //EnemyInstances = new List<EnemyInstance>();
            this.NewEnemyTypes = new List<EnemyType>();
            this.IsSpriteRestricted = isSpriteRestricted;

            if (null != spriteBankRowsRestriction)
            {
                this.SpriteBankRowsRestriction = new List<Int32>(spriteBankRowsRestriction);
            }
            else
            {
                this.SpriteBankRowsRestriction = new List<Int32>();
            }

            if (null != patternTableAddressesRestriction)
            {
                this.PatternTableAddressesRestriction = new List<Byte>(patternTableAddressesRestriction);
            }
            else
            {
                this.PatternTableAddressesRestriction = new List<Byte>();
            }
        }

        public Boolean ContainsRoom(Int32 roomNum)
        {
            foreach (Room room in this.Rooms)
            {
                if (room.RoomNum == roomNum)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
