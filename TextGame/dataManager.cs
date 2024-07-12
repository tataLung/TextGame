using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    internal class dataManager
    {
        public Player wizard = new Player("法師") { hp = 75, maxHp = 75, strength = 5, dexterity = 3, armorClass = 13, experience = 0, hpState = 1, status = 1 };
        public Player warrior = new Player("戰士") { hp = 100, maxHp = 100, strength = 5, dexterity = 3, armorClass = 15, experience = 0, hpState = 1, status = 1 };
        public Player assassin = new Player("刺客") { hp = 80, maxHp = 80, strength = 5, dexterity = 6, armorClass = 13, experience = 0, hpState = 1, status = 1 };
        public Player cleric = new Player("牧師") { hp = 75, maxHp = 75, strength = 5, dexterity = 3, armorClass = 12, experience = 0, hpState = 1, status = 1 };
        public Player druid = new Player("德魯伊") { hp = 80, maxHp = 80, strength = 5, dexterity = 4, armorClass = 13, experience = 0, hpState = 1, status = 1 };
        public List<Player> players = new List<Player>();
        public List<Monster> monsters = new List<Monster>();
    }
}
