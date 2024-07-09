using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace TextGame
{
    //血使用炸彈功能
    //玩家回合結束後怪物打回來
    //怪物獲得狀態後的回饋
    //我死了怪物會繼續鞭屍，要修一下
    internal class Game
    {
        public Player wizard = new Player { playerClass = "法師", hp = 75, maxHp = 75, strength = 5, dexterity = 3, armorClass = 13, experience = 18, hpState = 1, status = 1 };
        List<Player> players = new List<Player>();
        public List<Monster> monsters = new List<Monster>();
        //public List<Monster> monsters { get; private set; } = new List<Monster>();
        Random random = new Random();
        public int turn = 0;
        //public void InitializePlayer()
        //{
        //    players.Add(wizard);
        //}

        public void InitializeMonsters() //之後應該要改成private?
        {
            for (int i = 0; i < 3; i++)
            {
                monsters.Add(new Monster(i)
                {
                    //id = i,
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
            monsters.Add(new Monster(8)
            {
                //id = 8,
                name = "中Boss",
                hp = 30,
                maxHp = 30,
                strength = 20,
                dexterity = 6,
                armorClass = 13,
                experience = 100,
                hpState = 1,
                status = 1
            });
            //monsters.Add(new Monster
            //{
            //    id = 9,
            //    name = "最終Boss",
            //    hp = 50,
            //    maxHp = 50,
            //    strength = 30,
            //    dexterity = 6,
            //    armorClass = 13,
            //    experience = 200,
            //    hpState = 1,
            //    status = 1
            //});

        }

        /// <summary>
        /// 顯示怪物狀態
        /// </summary>
        public void showMonsterStatus()
        {
            foreach (var monster in monsters)
            {
                Console.Write($"編號{monster.id}, ");
                PrintColorText(monster.name,ConsoleColor.Red);
                Console.WriteLine($", 血量: {monster.hp}/{monster.maxHp}, 力量: {monster.strength}, 敏捷: {monster.dexterity}, 防禦: {monster.armorClass}, 經驗值: {monster.experience}, 生命狀態: {(monster.hpState == 1 ? "滿血" : monster.hpState == 2 ? "失血" : "死亡")}, 效果狀態: {(monster.status == 1 ? "正常" : monster.status == 2 ? "麻痺" : monster.status == 3 ? "渾沌" : "死亡")}");
            }
        }
        /// <summary>
        /// 顯示玩家狀態
        /// </summary>
        public void showPlayerClass()
        {
            PrintColorText(wizard.playerClass, ConsoleColor.Green);
            Console.WriteLine($", 血量: {wizard.hp}/{wizard.maxHp}, 力量: {wizard.strength}, 敏捷: {wizard.dexterity}, 防禦: {wizard.armorClass}, 經驗值: {wizard.experience}, 生命狀態: {(wizard.hpState == 1 ? "滿血" : wizard.hpState == 2 ? "失血" : "死亡")}, 效果狀態: {(wizard.status == 1 ? "正常" : wizard.status == 2 ? "麻痺" : "中毒")}");
            Console.Write("包包狀態:");
            if (wizard.playerBag.Count == 0)
            {
                Console.WriteLine("空的");
            }
            for (int i = 0; i < wizard.playerBag.Count; i++)
            {
                Console.Write(wizard.playerBag[i]);
                Console.WriteLine("個炸彈");
            }
            Console.WriteLine();
        }
        /// <summary>
        /// 選擇要攻擊的怪物
        /// </summary>
        /// <param name="monsters"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 判定玩家輸入何種法術
        /// </summary>
        /// <returns></returns>
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
                    case "B":
                        //使用炸彈一定命中
                        
                        return "轟炸";
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

        /// <summary>
        /// 執行擲骰子
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="HitOrDamage"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 擊敗怪物後是否掉落炸彈
        /// </summary>
        /// <param name="wizard"></param>
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
                Console.WriteLine("怪物沒有掉落任何物品QQQQ");
            }
        }

        /// <summary>
        ///使用炸彈，若包包有炸彈，移除一顆炸彈並設力量為20
        /// </summary>
        /// <param name="wizard"></param>
        /// <returns></returns>
        public bool UseBomb(Player wizard)
        {
            if (wizard.playerBag.Remove(1))
            {
                wizard.strength = 20;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更改文字顏色
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public void PrintColorText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        /// <summary>
        /// 顯示怪物受到傷害後失血或死亡狀態
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="damage"></param>
        public void ShowMonsterDamageMessage(Monster monster,int damage)
        {
            if (monster.hp <= 0)
            {
                monster.hp = 0;
                PrintColorText(monster.name, ConsoleColor.Red);
                Console.Write("被");
                PrintColorText("你", ConsoleColor.Green);
                Console.WriteLine("殺死了!!");
                monster.hpState = 3;
                monster.status = 4;
                wizard.experience += monster.experience;
                BombFall(wizard);
            }
            else
            {
                monster.hpState = 2;
                PrintColorText(monster.name, ConsoleColor.Red);
                Console.WriteLine("受到了{0}點傷害，他現在還有{1}點生命值", damage, monster.hp);
            }
        }

        /// <summary>
        /// 依照怪物狀態判定怪物動作(1.正常 2.麻痺 3.渾沌 4.死亡)
        /// </summary>
        public void ExecuteMonsterActions()
        {
            foreach (var monster in monsters)
            {
                if (monster.status == 1)
                {
                    // 怪物正常攻击玩家
                    if(HitPlayer(monster, wizard))
                    {
                        int damage = MonsterDamage(monster, wizard);
                        wizard.hp -= damage;
                    }
                    HitPlayer(monster,wizard);
                }
                else if (monster.status == 2)
                {
                    // 怪物麻痹，不能攻击
                    PrintColorText(monster.name, ConsoleColor.Red);
                    Console.WriteLine("被麻痺了，此回合不能攻擊!");
                }
                else if (monster.status == 3)
                {
                    // 怪物渾沌，随机攻击其他怪物
                    RandomAttack(monsters,monster);
                }

                // 回合结束后，将怪物状态重置为正常状态
                monster.status = 1;
            }
        }

        /// <summary>
        /// 怪物是否全數死亡
        /// </summary>
        /// <returns></returns>
        public bool AreAllMonstersDead()
        {
            return monsters.All(monster => monster.hpState == 3);
        }

        public int MonsterDamage( Monster monster, Player wizard)
        {
            float damage;
            int damageRoll = rollDice(monster.name, "傷害");

            damage = (monster.strength + damageRoll) / wizard.armorClass * 5;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(monster.name);
            Console.ResetColor();
            Console.WriteLine($"骰中{damageRoll}，傷害值為({wizard.strength}+{damageRoll})/{monster.armorClass}*5={(int)damage}");
            if (damageRoll == 20)
            {
                damage += 5;
                Console.WriteLine("爆擊加成，最終傷害為" + (int)damage);
            }
            return (int)damage;
        }

        /// <summary>
        /// 怪物攻擊玩家的命中擲骰檢定(敏捷+骰子 > 防禦 = 命中)
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="wizard"></param>
        /// <returns></returns>
        public bool HitPlayer(Monster monster, Player wizard)
        {
            int attak;
            int hitRoll = rollDice(monster.name, "命中");
            attak = wizard.dexterity + hitRoll;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(monster.name);
            Console.ResetColor();
            if (attak >= wizard.armorClass)
            {
                Console.WriteLine("骰中{0}，命中值為{1}+{2}={3}", hitRoll, wizard.dexterity, hitRoll, attak);
            }
            else
            {
                Console.WriteLine("骰中{0}，命中值為{1}+{2}={3}，對方防禦為{4}，未命中", hitRoll, wizard.dexterity, hitRoll, attak, wizard.armorClass);
            }

            return attak >= wizard.armorClass;
        }

        /// <summary>
        /// 怪物陷入渾沌狀態時，隨機攻擊一隻怪物，並造成5點傷害。
        /// </summary>
        /// <param name="monsters"></param>
        /// <param name="monster"></param>
        public void RandomAttack(List<Monster> monsters,Monster monster)
        {
            Random random = new Random();
            int targetIndex = random.Next(monsters.Count);
            Monster target = monsters[targetIndex];
            PrintColorText(monster.name, ConsoleColor.Red);
            Console.Write("陷入了渾沌狀態並且攻擊了");
            PrintColorText(target.name, ConsoleColor.Red);
            Console.WriteLine("造成了5點傷害");
            target.hp -= 5;
        }
    }
}
