using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    internal class Player
    {
        public string playerClass { get; private set; } //玩家職業
        public int hp { get; set; } //玩家血量
        public int maxHp { get; set; } //玩家最大血量
        public float strength { get; set; } //玩家力量
        public int dexterity { get; set; } //玩家敏捷
        public int armorClass { get; set; } //玩家防禦值
        public int experience { get; set; } //玩家經驗值
        public int hpState { get; set; } //玩家狀態 1=滿血2=失血3=死亡
        public int status { get; set; } //玩家狀態 1=正常 2=麻痺 3=中毒
        public List<int> playerBag { get; set; } //玩家包包 0.空 1.炸彈 2.核彈 3.血包 數量上限10
        public Dictionary<char, Skill> Attacks { get; set; } = new Dictionary<char, Skill>(); //玩家攻擊招式
        public bool isAI { get; set; }

        public Player(string playerClass) 
        {
            this.playerClass = playerClass;
            //playerBag = new List<int>(10);
            playerBag = new List<int>(new int[10]);
            //與playerBag = new List<int>(10);的差別是使用 PlayerBag = new List<int>(10); 时，你得到的是一个容量为 10 但实际上为空的列表，适用于需要动态添加元素的场景。
            //使用 PlayerBag = new List<int>(new int[10]); 时，你得到的是一个已经包含 10 个元素（默认值为 0）的列表，适用于需要初始化固定数量元素的场景。

        }

        public virtual string GetAttack(char attackKey, int turn)
        {
            return "";
        }
    }
}
