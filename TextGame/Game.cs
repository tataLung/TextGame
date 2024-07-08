using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TextGame
{
    //血使用炸彈功能
    //玩家回合結束後怪物打回來
    //怪物獲得狀態後的回饋
    internal class Game
    {
        public Player wizard = new Player { playerClass = "法師", hp = 75, maxHp = 75, strength = 15, dexterity = 3, armorClass = 13, experience = 18, hpState = 1, status = 1 };
        List<Player> players = new List<Player>();
        public List<Monster> monsters = new List<Monster>();
        Random random = new Random();
        int turn = 0;
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
                    id = i,
                    name = "小怪物" + i.ToString(),
                    hp = 20,
                    maxHp = 20,
                    strength = 10,
                    dexterity = 5,
                    armorClass = 12,
                    experience = 20,
                    hpState = 1,
                    status = 1
                });
            }
            monsters.Add(new Monster
            {
                id = 8,
                name = "中Boss",
                hp = 30,
                maxHp = 30,
                strength = 50,
                dexterity = 20,
                armorClass = 15,
                experience = 100,
                hpState = 1,
                status = 1
            });
            monsters.Add(new Monster
            {
                id = 9,
                name = "最終Boss",
                hp = 50,
                maxHp = 50,
                strength = 80,
                dexterity = 30,
                armorClass = 20,
                experience = 200,
                hpState = 1,
                status = 1
            });

        }

        /// <summary>
        /// 顯示怪物狀態
        /// </summary>
        public void showMonsterStatus()
        {
            foreach (var monster in monsters)
            {
                Console.WriteLine($"編號{monster.id}, {monster.name}, 血量: {monster.hp}/{monster.maxHp}, 力量: {monster.strength}, 敏捷: {monster.dexterity}, 防禦: {monster.armorClass}, 經驗值: {monster.experience}, 生命狀態: {monster.hpState}, 效果狀態: {monster.status}");
            }
        }
        /// <summary>
        /// 顯示玩家狀態
        /// </summary>
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

         public Monster SelectMonster(List<Monster> monsters)
        {
            Console.WriteLine("選擇怪物 (0-9):");
            int index;
            if (int.TryParse(Console.ReadLine(), out index) && index >= 0 && index < monsters.Count)
            {
                Monster monster = monsters[index];
                Console.WriteLine($"編號 {index}, {monster.name} - 血量: {monster.hp}/{monster.maxHp}, 效果狀態: {monster.status}");
                return monster;
            }
            else
            {
                Console.WriteLine("無效的怪物編號。");
                return null;
            }
        }

        /// <summary>
        /// 命中擲骰 (敏捷+骰子 > 防禦 = 命中)
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="dex"></param>
        /// <param name="ac"></param>
        /// <returns>是否命中</returns>
        public bool Hit(string turn, Monster monster)
        {
            int attak;
            int hitRoll = rollDice(turn, "命中");
            attak = monster.dexterity + hitRoll;
            if (turn == "你")
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write(turn);
            Console.ResetColor();
            if (attak >= monster.armorClass)
            {
                Console.WriteLine("骰中{0}，命中值為{1}+{2}={3}", hitRoll, monster.dexterity, hitRoll, attak);
            }
            else
            {
                Console.WriteLine("骰中{0}，命中值為{1}+{2}={3}，對方防禦為{4}，未命中", hitRoll, monster.dexterity, hitRoll, attak, monster.armorClass);
            }

            return attak >= monster.armorClass;
        }

        /// <summary>
        /// 傷害擲骰，若命中執行傷害計算 (力量+骰子/防禦值 * 5 = 傷害)
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="monster"></param>
        /// <param name="wizard"></param>
        /// <returns>傷害值</returns>
        public int Damage(string turn, Monster monster, Player wizard)
        {
            float damage;
            int damageRoll = rollDice(turn, "傷害");

            damage = (wizard.strength + damageRoll) / monster.armorClass * 5;

            if (turn == "你")
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write(turn);
            Console.ResetColor();
            Console.WriteLine($"骰中{damageRoll}，傷害值為({wizard.strength}+{damageRoll})/{monster.armorClass}*5={(int)damage}");
            if (damageRoll == 20)
            {
                damage += 5;
                Console.WriteLine("爆擊加成，最終傷害為" + (int)damage);
            }
            return (int)damage;
        }

        //判定玩家輸入何種法術
        public string SetAttackEnter()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true); //捕獲按鍵訊息
            char attackEnter = keyInfo.KeyChar;
            if (char.IsLetter(attackEnter))
            {
                switch (attackEnter.ToString().ToUpper())
                {
                    case "Q":
                        wizard.strength = 15;
                        return "火球術";
                    case "W":
                        wizard.strength = 15;
                        return "冰刃術";
                    case "E":
                        if (turn % 3 == 0)
                        {
                            wizard.strength = 2;
                            return "麻痺術";
                        }
                        else
                        {
                            Console.WriteLine("當前回合不能使用麻痺術。");
                            return SetAttackEnter();
                        }
                    case "R":
                        if (turn % 5 == 0)
                        {
                            wizard.strength = 20;
                            return "解離術";
                        }
                        else
                        {
                            Console.WriteLine("當前回合不能使用解離術。");
                            return SetAttackEnter();
                        }
                    default:
                        Console.WriteLine("請輸入QWER其中一鍵");
                        return SetAttackEnter();
                }
            }
            else
            {
                Console.WriteLine("\n輸入的不是字母。請再試一次。");
                return SetAttackEnter();
            }
        }



        //執行擲骰子
        int rollDice(string turn, string HitOrDamage)
        {
            if (turn == "你")
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write(turn);
            Console.ResetColor();
            Console.Write("的{0}骰子點數:", HitOrDamage);
            int displayDuration = 500; // 顯示隨機數字總時間
            int interval = 50; // 每個隨機數字的間隔顯示時間

            //這個變量用來存儲計算出的未來時間點
            DateTime endTime = DateTime.Now.AddMilliseconds(displayDuration); //AddMilliseconds(displayDuration)這個方法用來在當前時間的基礎上添加一個指定的時間段
            int randomNumber = 1;
            int originalX = Console.CursorLeft;
            int originalY = Console.CursorTop;
            while (DateTime.Now < endTime)
            {
                randomNumber = random.Next(1, 21);
                // 清除當前數字（將光標移動到數字的位置並清空該行）
                Console.SetCursorPosition(originalX, originalY);
                Console.Write("   "); // 清除數字占用的字符位置
                                      // 顯示新的隨機數字
                Console.SetCursorPosition(originalX, originalY);
                Console.Write(randomNumber);
                Thread.Sleep(interval);
            }

            // 顯示最終數字
            Console.SetCursorPosition(originalX, originalY);
            Console.Write("   ");
            Console.SetCursorPosition(originalX, originalY);
            Console.WriteLine(randomNumber);

            return randomNumber;
            //return 20;
        }

        public void BombFall(Player wizard)
        {
            int bombHasFall = random.Next(0,2);
            if(bombHasFall == 1)
            {
                Console.WriteLine("怪物掉落了一個炸彈");
                //if (wizard.playerBag.Count < 10 && wizard.playerBag.FindAll(i => i == 1).Count < wizard.playerBag.Count)
                if (wizard.playerBag.Count < 10)
                    {
                    wizard.playerBag.Add(1);
                    Console.WriteLine("你獲得一個炸彈!!!");
                } else
                {
                    Console.WriteLine("你的包包滿了!炸彈消失了QQQ");
                }
            }
            else
            {
                Console.WriteLine("哈哈非洲");
            }
        }

        //更改文字顏色
        public void PrintColorText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}
