using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    internal class Game
    {
        Player wizard = new Player { playerClass = "法師", hp = 75, maxHp = 75, strength = 15, dexterity = 3, armorClass = 13, experience = 18, hpState = 1, status = 1 };
        List<Player> players = new List<Player>();
        List<Monster> monsters = new List<Monster>();

        //public void InitializePlayer()
        //{
        //    players.Add(wizard);
        //}

        public void InitializeMonsters() //之後應該要改成private?
        {
            for (int i = 0; i < 8; i++)
            {
                monsters.Add(new Monster
                {
                    name = "小怪物" + (i + 1).ToString(),
                    hp = 20,
                    maxHp = 20,
                    strength = 10,
                    dexterity = 5,
                    armorClass = 3,
                    experience = 20,
                    hpState = 1,
                    status = 1
                });
            }
            monsters.Add(new Monster
            {
                name = "中Boss",
                hp = 30,
                maxHp = 30,
                strength = 50,
                dexterity = 20,
                armorClass = 10,
                experience = 100,
                hpState = 1,
                status = 1
            });
            monsters.Add(new Monster
            {
                name = "最終Boss",
                hp = 50,
                maxHp = 50,
                strength = 80,
                dexterity = 30,
                armorClass = 15,
                experience = 200,
                hpState = 1,
                status = 1
            });

        }

        public void showMonsterStatus()
        {
            foreach (var monster in monsters)
            {
                Console.WriteLine($"{monster.name}, 血量: {monster.hp}/{monster.maxHp}, 力量: {monster.strength}, 敏捷: {monster.dexterity}, 防禦: {monster.armorClass}, 經驗值: {monster.experience}, 生命狀態: {monster.hpState}, 效果狀態: {monster.status}");
            }
        }
        public void showPlayerClass()
        {
            Console.WriteLine($"{wizard.playerClass}, 血量: {wizard.hp}/{wizard.maxHp}, 力量: {wizard.strength}, 敏捷: {wizard.dexterity}, 防禦: {wizard.armorClass}, 經驗值: {wizard.experience}, 生命狀態: {wizard.hpState}, 效果狀態: {wizard.status}");
            Console.Write("包包狀態:");
            for (int i = 0; i < wizard.playerBag.Count; i++)
            {
                Console.Write(wizard.playerBag[i]);
            }
            Console.WriteLine();
        }
    }
}
