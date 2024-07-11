using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
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
    //1的玩家資料，調炸彈會變成有兩個 x
    internal class Game
    {
        public Player wizard = new Player("法師") { hp = 75, maxHp = 75, strength = 5, dexterity = 3, armorClass = 13, experience = 0, hpState = 1, status = 1 };
        public Player warrior = new Player("戰士") { hp = 100, maxHp = 100, strength = 5, dexterity = 3, armorClass = 15, experience = 0, hpState = 1, status = 1 };
        public Player assassin = new Player("刺客") { hp = 80, maxHp = 80, strength = 5, dexterity = 6, armorClass = 13, experience = 0, hpState = 1, status = 1 };
        public Player cleric = new Player("牧師") { hp = 75, maxHp = 75, strength = 5, dexterity = 3, armorClass = 12, experience = 0, hpState = 1, status = 1 };
        public Player druid = new Player("德魯伊") { hp = 80, maxHp = 80, strength = 5, dexterity = 4, armorClass = 13, experience = 0, hpState = 1, status = 1 };
        public List<Player> players = new List<Player>();
        public List<Monster> monsters = new List<Monster>();
        //public List<Monster> monsters { get; private set; } = new List<Monster>();
        Random random = new Random();
        public int turn = 0;

        /// <summary>
        /// 初始化怪物，加入多隻怪物
        /// </summary>
        public void InitializeMonsters() //之後應該要改成private?
        {
            for (int i = 0; i < 5; i++)
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
            //monsters.Add(new Monster(8)
            //{
            //    //id = 8,
            //    name = "中Boss",
            //    hp = 30,
            //    maxHp = 30,
            //    strength = 20,
            //    dexterity = 6,
            //    armorClass = 13,
            //    experience = 100,
            //    hpState = 1,
            //    status = 1
            //});
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

        public void showAllPlayerClass()
        {
            PrintColorText(wizard.playerClass, ConsoleColor.Green);
            Console.WriteLine($", 血量: {wizard.hp}/{wizard.maxHp}, 力量: {wizard.strength}, 敏捷: {wizard.dexterity}, 防禦: {wizard.armorClass}, 經驗值: {wizard.experience}");
            Console.WriteLine("技能:");
            Console.WriteLine($"Q:{wizard.Attacks['Q'].name},力量:{wizard.Attacks['Q'].strength}");
            Console.WriteLine($"W:{wizard.Attacks['W'].name},力量:{wizard.Attacks['W'].strength}");
            Console.WriteLine($"E:{wizard.Attacks['E'].name},力量:{wizard.Attacks['E'].strength},每三回合可使用一次，敵人陷入麻痺，此回合不可攻擊");
            Console.WriteLine($"R:{wizard.Attacks['R'].name},力量:{wizard.Attacks['W'].strength},每五回合可使用一次，敵人陷入混亂，此回合將隨機攻擊其他怪物");
            Console.WriteLine();
            PrintColorText(warrior.playerClass, ConsoleColor.Green);
            Console.WriteLine($", 血量: {warrior.hp}/{warrior.maxHp}, 力量: {warrior.strength}, 敏捷: {warrior.dexterity}, 防禦: {warrior.armorClass}, 經驗值: {warrior.experience}");
            PrintColorText(assassin.playerClass, ConsoleColor.Green);
            Console.WriteLine($", 血量: {warrior.hp} / {warrior.maxHp} , 力量:  {assassin.strength} , 敏捷:  {assassin.dexterity} , 防禦:  {assassin.armorClass} , 經驗值:  {assassin.experience}");
            PrintColorText(cleric.playerClass, ConsoleColor.Green);
            Console.WriteLine($", 血量: {wizard.hp}/{wizard.maxHp}, 力量: {cleric.strength}, 敏捷: {cleric.dexterity}, 防禦: {cleric.armorClass}, 經驗值: {cleric.experience}");
            PrintColorText(druid.playerClass, ConsoleColor.Green);
            Console.WriteLine($", 血量: {warrior.hp} / {warrior.maxHp} , 力量:  {druid.strength} , 敏捷:  {druid.dexterity} , 防禦:  {druid.armorClass} , 經驗值:  {druid.experience}");
            Console.WriteLine("所有職業B為使用炸彈，固定造成20點傷害，且一定命中");
            Console.WriteLine();
        }

        /// <summary>
        /// 選擇人物及伙伴職業
        /// </summary>
        public void InitializePlayer()
        {
            Console.WriteLine("請先選擇你的職業 0:法師 1:戰士 2.刺客 3.牧師 4.德魯伊");
            while (true)
            {
                char input = Console.ReadKey(true).KeyChar;
                if (AddPlayerToClass(input,false)) break;
            }
            Console.WriteLine("選擇你的夥伴 0:法師 1:戰士 2.刺客 3.牧師 4.德魯伊");
            while (true)
            {
                char input = Console.ReadKey(true).KeyChar;
                if (AddPlayerToClass(input,true)) break;
            }
            Console.WriteLine("現在隊伍裡有");
            PrintColorText($"你({players[0].playerClass})", ConsoleColor.Green);
            Console.WriteLine($", 血量: {players[0].hp} / {players[0].maxHp} , 力量:  {players[0].strength} , 敏捷:  {players[0].dexterity} , 防禦:  {players[0].armorClass} , 經驗值:  {players[0].experience}");
            PrintColorText($"夥伴({players[1].playerClass})", ConsoleColor.Green);
            Console.WriteLine($", 血量: {players[1].hp} / {players[1].maxHp} , 力量:  {players[1].strength} , 敏捷:  {players[1].dexterity} , 防禦:  {players[1].armorClass} , 經驗值:  {players[1].experience}");
        }

        private void SetAttacks()
        {
            wizard.Attacks.Add('Q', new Attack("火球術", 15, turn => true));
            wizard.Attacks.Add('W', new Attack("冰刃術", 15, turn => true));
            wizard.Attacks.Add('E', new Attack("麻痺術", 2, turn => turn % 3 == 0));
            wizard.Attacks.Add('R', new Attack("解離術", 20, turn => turn % 5 == 0));
            wizard.Attacks.Add('B', new Attack("轟炸", 20, turn => wizard.playerBag.Count(item => item == 1) > 0));

            warrior.Attacks.Add('Q', new Attack("揮砍", 15, turn => true));
            warrior.Attacks.Add('W', new Attack("貓一拳", 15, turn => true));
            warrior.Attacks.Add('E', new Attack("掃堂腿", 10, turn => turn % 5 == 0));
            warrior.Attacks.Add('R', new Attack("重擊", 25, turn => turn % 6 == 0));
            warrior.Attacks.Add('B', new Attack("轟炸", 20, turn => warrior.playerBag.Count(item => item == 1) > 0));

            assassin.Attacks.Add('Q', new Attack("穿刺", 15, turn => true));
            assassin.Attacks.Add('W', new Attack("切割", 15, turn => true));
            assassin.Attacks.Add('E', new Attack("掃堂腿", 10, turn => turn % 5 == 0));
            assassin.Attacks.Add('R', new Attack("偷襲", 18, turn => turn % 3 == 0));
            assassin.Attacks.Add('B', new Attack("轟炸", 20, turn => assassin.playerBag.Count(item => item == 1) > 0));
            // 添加其他职业的招式
        }
        public Game()
        {
            SetAttacks();
        }

        public string SetAttackEnter(Player player)
        {
            char attackEnter;

            if (player.isAI)
            {
                // 隨機選擇一個有效的攻擊鍵
                var availableAttacks = player.Attacks.Where(a => a.Value.CanUseAttack(turn) || a.Key == 'B').Select(a => a.Key).ToList();
                attackEnter = availableAttacks[random.Next(availableAttacks.Count)];
            }
            else
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true); // 捕獲按鍵訊息
                attackEnter = keyInfo.KeyChar;
            }

            if (char.IsLetter(attackEnter))
            {
                attackEnter = char.ToUpper(attackEnter);

                if (player.Attacks.ContainsKey(attackEnter))
                {
                    var attack = player.Attacks[attackEnter];
                    if (attack.name == "轟炸" && !player.playerBag.Contains(1))
                    {
                        if (!player.isAI)
                        {
                            Console.WriteLine("包包沒有炸彈!!");
                            return SetAttackEnter(player);
                        }
                        else
                        {
                            // 電腦玩家重新選擇攻擊
                            return SetAttackEnter(player);
                        }
                    }
                    if (attack.CanUseAttack(turn))
                    {
                        //PrintColorText()
                        Console.WriteLine($"使用了{attack.name}");
                        player.strength = attack.strength;
                        return attack.name;
                    }
                    else
                    {
                        if (!player.isAI)
                        {
                            Console.WriteLine($"當前回合不能使用 {attack.name}。");
                        }
                    }
                }
                else
                {
                    if (!player.isAI)
                    {
                        Console.WriteLine("請輸入有效的攻擊鍵");
                    }
                }
                return SetAttackEnter(player);
            }
            else
            {
                if (!player.isAI)
                {
                    Console.WriteLine("\n輸入的不是字母。請再試一次。");
                }
                return SetAttackEnter(player);
            }
        }

        private bool AddPlayerToClass(char input,bool isAI)
        {
            Player player = null;
            switch (input)
            {
                case '0':
                    player = wizard;
                    break;
                case '1':
                    player = warrior;
                    break;
                case '2':
                    player = assassin;
                    break;
                case '3':
                    player = cleric;
                    break;
                case '4':
                    player = druid;
                    break;
                default:
                    Console.WriteLine("無效的輸入項");
                    return false;
            }

            if (player != null && !players.Any(p => p.playerClass == player.playerClass))
            {
                player.isAI = isAI;
                players.Add(player);
                Console.WriteLine($"添加了{player.playerClass}");
                return true; // 成功添加，退出循环
            }
            else if (player != null)
            {
                Console.WriteLine($"{player.playerClass} 已經在隊伍中");
                return false; // 职业已存在，继续循环
            }

            return false;
        }

        /// <summary>
        /// 顯示怪物狀態
        /// </summary>
        public void showMonsterStatus()
        {
            foreach (var monster in monsters)
            {
                ConsoleColor textColor = monster.hpState == 3 ? ConsoleColor.DarkGray : ConsoleColor.White;
                ConsoleColor monsterTextColor = monster.hpState == 3 ? ConsoleColor.DarkRed : ConsoleColor.Red;
                PrintColorText($"編號{monster.id}, ", textColor);
                PrintColorText(monster.name, monsterTextColor);
                PrintColorText($", 血量: {monster.hp}/{monster.maxHp}, 力量: {monster.strength}, 敏捷: {monster.dexterity}, 防禦: {monster.armorClass}, 經驗值: {monster.experience}, 生命狀態: {(monster.hpState == 1 ? "滿血" : monster.hpState == 2 ? "失血" : "死亡")}, 效果狀態: {(monster.status == 1 ? "正常" : monster.status == 2 ? "麻痺" : monster.status == 3 ? "渾沌" : "死亡")}\n", textColor);
                //Console.WriteLine($", 血量: {monster.hp}/{monster.maxHp}, 力量: {monster.strength}, 敏捷: {monster.dexterity}, 防禦: {monster.armorClass}, 經驗值: {monster.experience}, 生命狀態: {(monster.hpState == 1 ? "滿血" : monster.hpState == 2 ? "失血" : "死亡")}, 效果狀態: {(monster.status == 1 ? "正常" : monster.status == 2 ? "麻痺" : monster.status == 3 ? "渾沌" : "死亡")}");
            }
        }
        /// <summary>
        /// 顯示玩家狀態
        /// </summary>
        public void showPlayerClass()
        {
            foreach(var player in players)
            {
                PrintColorText(player.playerClass, ConsoleColor.Green);
                Console.WriteLine($", 血量: {player.hp}/{player.maxHp}, 力量: {player.strength}, 敏捷: {player.dexterity}, 防禦: {player.armorClass}, 經驗值: {player.experience}, 生命狀態: {(player.hpState == 1 ? "滿血" : player.hpState == 2 ? "失血" : "死亡")}, 效果狀態: {(player.status == 1 ? "正常" : player.status == 2 ? "麻痺" : "中毒")}");
                PrintColorText(player.playerClass, ConsoleColor.Green);
                Console.Write("的包包狀態:");
                if (player.playerBag.All(item => item == 0))
                {
                    Console.WriteLine("空的");
                }
                else
                {
                    int bombCount = player.playerBag.Count(item => item == 1);
                    Console.WriteLine($"{bombCount} 個炸彈");
                }
            }
             
            Console.WriteLine();
        }
        /// <summary>
        /// 選擇要攻擊的怪物
        /// </summary>
        /// <param name="monsters"></param>
        /// <returns></returns>
        public Monster SelectMonster(List<Monster> monsters, bool isAI, string playerClass)
        {
            while (true)
            {
                if (!isAI)
                {
                    Console.WriteLine($"選擇怪物 (0-{monsters.Count - 1}):");
                    int index;
                    if (int.TryParse(Console.ReadLine(), out index) && index >= 0 && index < monsters.Count)
                    {
                        Monster monster = monsters[index];
                        if (monster.hpState == 3)
                        {
                            Console.WriteLine("選擇的怪物已死亡!請重新選擇");
                            continue; // 繼續選擇
                        }
                        PrintColorText(playerClass+"(你)", ConsoleColor.Green);
                        Console.WriteLine($"選擇: 編號 {index}, {monster.name} - 血量: {monster.hp}/{monster.maxHp}, 效果狀態: {(monster.status == 1 ? "正常" : monster.status == 2 ? "麻痺" : monster.status == 3 ? "渾沌" : "死亡")}");
                        return monster;
                    }
                    else
                    {
                        Console.WriteLine("無效的怪物編號。請重新輸入。");
                    }
                }
                else // AI选择怪物
                {
                    List<Monster> aliveMonsters = monsters.Where(m => m.hp > 0).ToList();
                    if (aliveMonsters.Count > 0)
                    {
                        // 随机选择一个存活的怪物
                        Random random = new Random();
                        Monster selectedMonster = aliveMonsters[random.Next(aliveMonsters.Count)];
                        PrintColorText(playerClass+"(夥伴)", ConsoleColor.Green);
                        Console.WriteLine($"選擇: 編號 {selectedMonster.id}, {selectedMonster.name} - 血量: {selectedMonster.hp}/{selectedMonster.maxHp}, 效果狀態: {(selectedMonster.status == 1 ? "正常" : selectedMonster.status == 2 ? "麻痺" : selectedMonster.status == 3 ? "渾沌" : "死亡")}");
                        return selectedMonster;
                    }
                    else
                    {
                        Console.WriteLine("所有怪物已死亡。");
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// 玩家攻擊怪物的命中擲骰 (敏捷+骰子 > 防禦 = 命中)
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="dex"></param>
        /// <param name="ac"></param>
        /// <returns>是否命中</returns>
        public bool HitMonster(string turn, Monster monster)
        {
            int attak;
            int hitRoll = rollDice(turn, "命中");
            attak = monster.dexterity + hitRoll;
            Console.ForegroundColor = ConsoleColor.Green;
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
        /// 玩家攻擊怪物的傷害擲骰，若命中執行傷害計算 (力量+骰子/防禦值 * 5 = 傷害)
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="monster"></param>
        /// <param name="wizard"></param>
        /// <returns>傷害值</returns>
        public int PlayerDamage( Monster monster, Player player)
        {
            float damage;
            int damageRoll = rollDice(player.playerClass, "傷害");

            damage = (player.strength + damageRoll) / monster.armorClass * 5;

            PrintColorText(player.playerClass, ConsoleColor.Green);
            Console.WriteLine($"骰中{damageRoll}，傷害值為({player.strength}+{damageRoll})/{monster.armorClass}*5={(int)damage}");
            if (damageRoll == 20)
            {
                damage += 5;
                Console.WriteLine("爆擊加成，最終傷害為" + (int)damage);
            }
            return (int)damage;
        }

        /// <summary>
        /// 判定玩家輸入何種攻擊方式
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
                        if(wizard.playerBag.Count(item => item == 1) == 0)
                        {
                            Console.WriteLine("包包沒有炸彈!!");
                            return SetAttackEnter();
                        }
                        else
                        {
                            return "轟炸";
                        }
                    default:
                        Console.WriteLine("請輸入QWERB其中一鍵");
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
            if (turn.Contains("怪物"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
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
        public void BombFall(Player player)
        {
            int bombHasFall = random.Next(0,2);
            if(bombHasFall == 1 || bombHasFall == 0)
            {
                Console.WriteLine("怪物掉落了一個炸彈");
                //if (player.playerBag.Count < 10 && player.playerBag.FindAll(i => i == 1).Count < player.playerBag.Count)
                if (player.playerBag.Contains(0))
                //if (player.playerBag.Count < 10)
                {
                    int index = player.playerBag.IndexOf(0);
                    player.playerBag[index] = 1;
                    //player.playerBag.Add(1);
                    Console.WriteLine("獲得一個炸彈!!!");
                } else
                {
                    Console.WriteLine("歐歐QQQ包包滿了!炸彈消失了QQQ");
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
        public void ShowMonsterDamageMessage(Monster monster,int damage,Player player)
        {
            if (monster.hp <= 0)
            {
                monster.hp = 0;
                PrintColorText(monster.name, ConsoleColor.Red);
                Console.Write("被");
                //PrintColorText("你", ConsoleColor.Green);
                Console.WriteLine("殺死了!!");
                monster.hpState = 3;
                monster.status = 4;
                player.experience += monster.experience;
                PrintColorText(player.playerClass, ConsoleColor.Green);
                Console.WriteLine($"獲得了{monster.experience}點經驗值!");
                BombFall(player);
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

        /// <summary>
        /// 怪物攻擊玩家的傷害擲骰，若命中執行傷害計算 (力量+骰子/防禦值 * 5 = 傷害)
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="wizard"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 將選擇的怪物復活
        /// </summary>
        /// <param name="monster"></param>
        public void ReviveMonster(Monster monster)
        {
            monster.hp = monster.maxHp;
            monster.hpState = 1;
            monster.status = 1;
            PrintColorText(monster.name,ConsoleColor.Red);
            Console.WriteLine("已復活，血量已滿。");
        }
    }
}
