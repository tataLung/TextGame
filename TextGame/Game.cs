using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    internal class Game
    {
        Player wizard = new Player { playerClass = "法師", playerHp = 75, playerMaxHp = 75, playerSt = 15, playerDex = 3, playerAc = 13, playerExp = 18, playerHpState = 1, playerState = 1 };
        List<Monster> monsters = new List<Monster>();

        public void InitializeMonsters() //之後應該要改成private?
        {
            for (int i = 0; i < 8; i++)
            {
                monsters.Add(new Monster
                {
                    monsterName = "小怪物" + (i + 1).ToString(),
                    monsterHp = 100,
                    monsterMaxHp = 100,
                    monsterSt = 10,
                    monsterDex = 5,
                    monsterAc = 3,
                    monsterExp = 20,
                    monsterHpState = 1,
                    monsterState = 1
                });
            }
            monsters.Add(new Monster
            {
                monsterName = "中Boss",
                monsterHp = 300,
                monsterMaxHp = 300,
                monsterSt = 50,
                monsterDex = 20,
                monsterAc = 10,
                monsterExp = 100,
                monsterHpState = 1,
                monsterState = 1
            });
            monsters.Add(new Monster
            {
                monsterName = "最終Boss",
                monsterHp = 500,
                monsterMaxHp = 500,
                monsterSt = 80,
                monsterDex = 30,
                monsterAc = 15,
                monsterExp = 200,
                monsterHpState = 1,
                monsterState = 1
            });

        }

        public void showMonsterStatus()
        {
            foreach (var monster in monsters)
            {
                Console.WriteLine($"{monster.monsterName}, 血量: {monster.monsterHp}/{monster.monsterMaxHp}, 力量: {monster.monsterSt}, 敏捷: {monster.monsterDex}, 防禦: {monster.monsterAc}, 經驗值: {monster.monsterExp}, 生命狀態: {monster.monsterHpState}, 效果狀態: {monster.monsterState}");
            }
        }
    }
}
