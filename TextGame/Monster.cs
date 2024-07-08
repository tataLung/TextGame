using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    internal class Monster
    {
        public string name  { get; set;} //怪物名字
        public int hp { get; set;} //怪物血量
        public int maxHp { get; set; } //怪物最大血量
        public float strength { get; set; } //怪物力量
        public int dexterity { get; set; } //怪物敏捷
        public int armorClass { get; set; } //怪物防禦值
        public int experience { get; set; } //怪物經驗值
        public int hpState { get; set; } //怪物狀態 1=滿血2=失血3=死亡
        public int status { get; set; } //怪物狀態 1=正常 2=麻痺 3=中毒 4=渾沌

        public List<int> Item {  get; set; } //掉落物 1.炸彈 2.核彈 3.血包

    }
}
