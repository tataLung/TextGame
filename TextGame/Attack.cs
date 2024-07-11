using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    internal class Attack
    {
        public string name { get; set; } //招式名稱
        public int strength { get; set; } //招式基礎值
        public Func<int, bool> CanUse { get; set; }

        public Attack(string name, int strength, Func<int, bool> canUse)
        {
            this.name = name;
            this.strength = strength;
            CanUse = canUse;
        }

        public bool CanUseAttack(int turn)
        {
            return CanUse(turn);
        }
    }
}
