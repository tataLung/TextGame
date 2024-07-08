using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    internal class Monster
    {
        public string monsterName  { get; set;} //怪物名字
        public int monsterHp  { get; set;} //怪物血量
        public int monsterMaxHp { get; set; } //怪物最大血量
        public float monsterSt { get; set; } //怪物力量
        public int monsterDex { get; set; } //怪物敏捷
        public int monsterAc { get; set; } //怪物防禦值
        public int monsterExp { get; set; } //怪物經驗值
        public int monsterHpState { get; set; } //怪物狀態 1=滿血2=失血3=死亡
        public int monsterState { get; set; } //怪物狀態 1=正常 2=麻痺 3=中毒

    }
}
