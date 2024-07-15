using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static TextGame.Monster;

namespace TextGame
{
    internal class Monster
    {
        Game game;
        public int id {  get; private set; } //怪物編號
        public string name  { get; set;} //怪物名字
        public int hp { get; set;} //怪物血量
        public int maxHp { get; set; } //怪物最大血量
        public float strength { get; set; } //怪物力量
        public int dexterity { get; set; } //怪物敏捷
        public int armorClass { get; set; } //怪物防禦值
        public int experience { get; set; } //怪物經驗值

        public Utility.hpStatusType hpStatus { get; set; } //怪物狀態 1=滿血2=失血3=死亡
        public Utility.effectStatusType effStatus { get; set; } //怪物狀態 1=正常 2=麻痺 3=渾沌 4=死亡

        public List<int> Item {  get; set; } //掉落物 1.炸彈 2.核彈 3.血包


        public Monster(int id)
        {
            this.id = id;
            //this.name = name + id.ToString();
        }
    }
    internal class MonsterManager(dataManager data)
    {
        //dataManager data;
        /// <summary>
        /// 顯示怪物狀態
        /// </summary>
        public void showMonsterStatus()
        {
            foreach (var monster in data.monsters)
            {
                ConsoleColor textColor = monster.hpStatus == Utility.hpStatusType.dead ? ConsoleColor.DarkGray : ConsoleColor.White;
                ConsoleColor monsterTextColor = monster.hpStatus == Utility.hpStatusType.dead ? ConsoleColor.DarkRed : ConsoleColor.Red;
                Utility.PrintColorText($"編號{monster.id}, ", textColor);
                Utility.PrintColorText(monster.name, monsterTextColor);
                Utility.PrintColorText($", 血量: {monster.hp}/{monster.maxHp}, 力量: {monster.strength}, 敏捷: {monster.dexterity}, 防禦: {monster.armorClass}, 經驗值: {monster.experience}, " +
                    $"生命狀態: {(monster.hpStatus == Utility.hpStatusType.fullHp ? "滿血" : monster.hpStatus == Utility.hpStatusType.lossHp ? "失血" : "死亡")}, " +
                    $"效果狀態: {(monster.effStatus == Utility.effectStatusType.normal ? "正常" : monster.effStatus == Utility.effectStatusType.paralysis ? "麻痺" : monster.effStatus == Utility.effectStatusType.chaos ? "渾沌" : "死亡")}\n", textColor);
            }
        }

        /// <summary>
        /// 初始化怪物，加入多隻怪物
        /// </summary>
        public void InitializeMonsters(int size)
        {
            size = (size <= 0) ? 1 : size;
            for (int i = 0; i < size; i++)
            {
                data.monsters.Add(new Monster(i)
                {
                    //id = i,
                    name = "小怪物" + i.ToString(),
                    hp = 20,
                    maxHp = 20,
                    strength = 10,
                    dexterity = 5,
                    armorClass = 12,
                    experience = 20,
                    hpStatus = Utility.hpStatusType.fullHp,
                    effStatus = Utility.effectStatusType.normal
                });
            }
        }
    }
}
