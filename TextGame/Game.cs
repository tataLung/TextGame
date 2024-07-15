using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace TextGame
{
    //我死了怪物會繼續鞭屍，要修一下
    internal class Game
    {
        public static dataManager data;
        public static PlayerManager playerManager;
        public static MonsterManager monsterManager;
        Random random = new Random();
        public int turn = 0;
        static Game()
        {
            data = new dataManager();
            playerManager = new PlayerManager(data);
            monsterManager = new MonsterManager(data);
        }

        /// <summary>
        /// 執行遊戲
        /// </summary>
        public void RunGame()
        {
            while (!AreAllMonstersDead() && !AreAllPlayersDead())
            {
                Console.WriteLine("輸入操作 (0: 結束遊戲, 1: 顯示玩家狀態, 2:顯示怪物狀態 3:進入此回合戰鬥)");
                char input = Console.ReadKey(true).KeyChar;
                Monster selectedMonster = null;
                switch (input)
                {
                    case '0':
                        Console.WriteLine("確定要結束遊戲嗎？(y/n)");
                        if (Console.ReadKey(true).KeyChar == 'y' || Console.ReadKey(true).KeyChar == 'Y')
                        {
                            Console.WriteLine("遊戲結束");
                            return;
                        }
                        break;
                    case '1':
                        showPlayerClass();
                        break;
                    case '2':
                        monsterManager.showMonsterStatus();
                        break;
                    case '3':
                        Console.Clear();
                        //玩家攻擊
                        turn++;
                        Console.WriteLine("\n第{0}回合開始-------------------------↓\n", turn);
                        foreach (var player in data.players)
                        {
                            // 玩家攻击阶段
                            bool isAI = player.isAI; // 假设 Player 类有一个 IsAI 属性来表示是否是 AI
                            selectedMonster = SelectMonster(data.monsters, isAI, player.playerClass);
                            if (selectedMonster != null)
                            {
                                Skill.skillType attackType = SetAttackEnter(player);
                                if (attackType == Skill.skillType.bomb)
                                {
                                    if (UseBomb(player))
                                    {
                                        Console.WriteLine("咻~~~~~~崩幾勒!");
                                        selectedMonster.hp -= 20;
                                        ShowMonsterDamageMessage(selectedMonster, 20, player);
                                    }
                                    else
                                    {
                                        Console.WriteLine("包包沒有炸彈!");
                                    }
                                }
                                else if (attackType == Skill.skillType.heal)
                                {
                                    Heal(player);
                                }
                                else if(attackType == Skill.skillType.healGroup)
                                {
                                    HealGroup(data.players,player);
                                }
                                else
                                {
                                    if (HitMonster(player, selectedMonster))
                                    {
                                        switch (attackType)
                                        {
                                            case Skill.skillType.paralysis:
                                                selectedMonster.effStatus = Utility.effectStatusType.paralysis;
                                                break;
                                            case Skill.skillType.chaos:
                                                selectedMonster.effStatus = Utility.effectStatusType.chaos;
                                                break;
                                            default:
                                                selectedMonster.effStatus = Utility.effectStatusType.normal;
                                                break;
                                        }
                                        int damage = PlayerDamage(selectedMonster, player);
                                        selectedMonster.hp -= damage;
                                        ShowMonsterDamageMessage(selectedMonster, damage, player);
                                    }
                                }

                                //Console.WriteLine("\n第{0}回合結束-------------------------↑\n", game.turn);
                                //}
                            }
                            else if(!AreAllMonstersDead() && !AreAllPlayersDead())
                            {
                                Console.WriteLine("請先選擇一個怪物");
                            }
                            if (player == data.players.Last())
                            {
                                Console.WriteLine($"\n第{turn}回合結束-------------------------↑\n");
                            }
                        }

                        break;
                    //之後應該會拿掉復活怪物
                    //case '4':
                    //    selectedMonster = game.SelectMonster(game.monsters);
                    //    if (selectedMonster != null && selectedMonster.hpState == 3)
                    //        game.ReviveMonster(selectedMonster);
                    //    else
                    //        Console.WriteLine("請選擇一個死亡的怪物。");
                    //    break;
                    default:
                        Console.WriteLine("無效的輸入項");
                        break;
                }
            }
            //Console.WriteLine(game.AreAllMonstersDead());
            if (AreAllMonstersDead())
            {
                Console.WriteLine("\n遊戲結束-------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"謝謝你勇者，你打倒了所有怪物，我們感謝你:)))");
                foreach (var player in data.players)
                {
                    Console.WriteLine($"{player.playerClass}有 {player.experience}點經驗值!");
                }
                Console.ResetColor();
            }
            else if (data.wizard.hp <= 0)
            {
                Console.WriteLine("\n遊戲結束-------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("遊戲結束!!!你被怪物殺死了，再去練等吧!!");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 玩家選擇使用的技能
        /// </summary>
        /// <returns>技能類別</returns>
        public Skill.skillType SetAttackEnter(Player player)
        {
            char attackEnter;

            if (player.isAI)
            {
                // 隨機選擇一個有效的攻擊鍵
                var availableAttacks = player.skill.Where(a => a.Value.CanUseSkill(turn) || a.Key == 'B').Select(a => a.Key).ToList();
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

                if (player.skill.ContainsKey(attackEnter))
                {
                    var attack = player.skill[attackEnter];
                    if (attack.type == Skill.skillType.bomb && !player.playerBag.Contains(1))
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
                    if (attack.CanUseSkill(turn))
                    {
                        //PrintColorText()
                        Console.WriteLine($"使用了{attack.name}");
                        player.strength = attack.strength;
                        return attack.type;
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


        /// <summary>
        /// 顯示玩家狀態
        /// </summary>
        public void showPlayerClass()
        {
            foreach(var player in data.players)
            {
                Utility.PrintColorText(player.playerClass, ConsoleColor.Green);
                Console.WriteLine($", 血量: {player.hp}/{player.maxHp}, 力量: {player.strength}, 敏捷: {player.dexterity}, 防禦: {player.armorClass}, 經驗值: {player.experience}, 生命狀態: {(player.hpStatus == Utility.hpStatusType.fullHp ? "滿血" : player.hpStatus == Utility.hpStatusType.lossHp ? "失血" : "死亡")}, 效果狀態: {(player.effStatus == Utility.effectStatusType.normal ? "正常" : player.effStatus == Utility.effectStatusType.paralysis ? "麻痺" : "中毒")}");
                Utility.PrintColorText(player.playerClass, ConsoleColor.Green);
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
                        if (monster.hpStatus == Utility.hpStatusType.dead)
                        {
                            Console.WriteLine("選擇的怪物已死亡!請重新選擇");
                            continue; // 繼續選擇
                        }
                        Utility.PrintColorText(playerClass+"(你)", ConsoleColor.Green);
                        Console.WriteLine($"選擇: 編號 {index}, {monster.name} - 血量: {monster.hp}/{monster.maxHp}, 效果狀態: {(monster.effStatus == Utility.effectStatusType.normal ? "正常" : monster.effStatus == Utility.effectStatusType.paralysis ? "麻痺" : monster.effStatus == Utility.effectStatusType.chaos ? "渾沌" : "死亡")}");
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
                        Utility.PrintColorText(playerClass+"(夥伴)", ConsoleColor.Green);
                        Console.WriteLine($"選擇: 編號 {selectedMonster.id}, {selectedMonster.name} - 血量: {selectedMonster.hp}/{selectedMonster.maxHp}, 效果狀態: {(selectedMonster.effStatus == Utility.effectStatusType.normal ? "正常" : selectedMonster.effStatus == Utility.effectStatusType.paralysis ? "麻痺" : selectedMonster.effStatus == Utility.effectStatusType.chaos ? "渾沌" : "死亡")}");
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
        /// 補血技能(適用於施技能者自行補血)
        /// </summary>
        public void Heal(Player player)
        {
            player.hp = (int)Math.Min(player.hp + player.strength, player.maxHp);
        }

        /// <summary>
        /// 補血技能(適用於全隊補血)
        /// </summary>
        public void HealGroup(List<Player> players, Player player)
        {
            float healAmount = 0;
            if(player.skill['R'] is Heal heal)
            {
                healAmount = player.strength;
            }
            foreach (Player p in players)
            {
                p.hp = (int)Math.Min(p.hp + healAmount, p.maxHp);
            }
        }

        /// <summary>
        /// 玩家攻擊怪物的命中擲骰 (敏捷+骰子 > 防禦 = 命中)
        /// </summary>
        /// <returns>是否命中</returns>
        public bool HitMonster(Player player, Monster monster)
        {
            int attak;
            int hitRoll = rollDice(player.playerClass, "命中");
            attak = player.dexterity + hitRoll;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(player.playerClass);
            Console.ResetColor();
            if (attak >= monster.armorClass)
            {
                Console.WriteLine("骰中{0}，命中值為{1}+{2}={3}", hitRoll, player.dexterity, hitRoll, attak);
            }
            else
            {
                Console.WriteLine("骰中{0}，命中值為{1}+{2}={3}，對方防禦為{4}，未命中", hitRoll, player.dexterity, hitRoll, attak, monster.armorClass);
            }

            return attak >= monster.armorClass;
        }

        /// <summary>
        /// 玩家攻擊怪物的傷害擲骰，若命中執行傷害計算 (力量+骰子/防禦值 * 5 = 傷害)
        /// </summary>
        /// <returns>傷害值</returns>
        public int PlayerDamage( Monster monster, Player player)
        {
            float damage;
            int damageRoll = rollDice(player.playerClass, "傷害");

            damage = (player.strength + damageRoll) / monster.armorClass * 5;

            Utility.PrintColorText(player.playerClass, ConsoleColor.Green);
            Console.WriteLine($"骰中{damageRoll}，傷害值為({player.strength}+{damageRoll})/{monster.armorClass}*5={(int)damage}");
            if (damageRoll == 20)
            {
                damage += 5;
                Console.WriteLine("爆擊加成，最終傷害為" + (int)damage);
            }
            return (int)damage;
        }

        /// <summary>
        /// 執行擲骰子
        /// </summary>
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
        public void BombFall(Player player)
        {
            int bombHasFall = random.Next(0,2);
            if(bombHasFall == 1)
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
        public bool UseBomb(Player player)
        {
            if (player.playerBag.Remove(1))
            {
                player.strength = 20;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 顯示怪物受到傷害後失血或死亡狀態
        /// </summary>
        public void ShowMonsterDamageMessage(Monster monster,int damage,Player player)
        {
            if (monster.hp <= 0)
            {
                monster.hp = 0;
                Utility.PrintColorText(monster.name, ConsoleColor.Red);
                Console.Write("被");
                Console.WriteLine("殺死了!!");
                monster.hpStatus = Utility.hpStatusType.dead;
                monster.effStatus = Utility.effectStatusType.dead;
                player.experience += monster.experience;
                Utility.PrintColorText(player.playerClass, ConsoleColor.Green);
                Console.WriteLine($"獲得了{monster.experience}點經驗值!");
                BombFall(player);
            }
            else
            {
                monster.hpStatus = Utility.hpStatusType.lossHp;
                Utility.PrintColorText(monster.name, ConsoleColor.Red);
                Console.WriteLine("受到了{0}點傷害，他現在還有{1}點生命值", damage, monster.hp);
            }
        }

        /// <summary>
        /// 怪物是否全數死亡
        /// </summary>
        public bool AreAllMonstersDead()
        {
            return data.monsters.All(monster => monster.hpStatus == Utility.hpStatusType.dead);
        }

        /// <summary>
        /// 玩家是否全數死亡
        /// </summary>
        public bool AreAllPlayersDead()
        {
            return data.players.All(player => player.hpStatus == Utility.hpStatusType.dead);
        }

        //這裡是怪物如果會攻擊回來的程式，目前還沒將進去這個功能
        #region
        /// <summary>
        /// 依照怪物狀態判定怪物動作(1.正常 2.麻痺 3.渾沌 4.死亡)
        /// </summary>
        public void ExecuteMonsterActions()
        {
            foreach (var monster in data.monsters)
            {
                if (monster.effStatus == Utility.effectStatusType.normal)
                {
                    // 怪物正常攻击玩家
                    if(HitPlayer(monster, data.wizard))
                    {
                        int damage = MonsterDamage(monster, data.wizard);
                        data.wizard.hp -= damage;
                    }
                    HitPlayer(monster, data.wizard);
                }
                else if (monster.effStatus == Utility.effectStatusType.paralysis)
                {
                    // 怪物麻痹，不能攻击
                    Utility.PrintColorText(monster.name, ConsoleColor.Red);
                    Console.WriteLine("被麻痺了，此回合不能攻擊!");
                }
                else if (monster.effStatus == Utility.effectStatusType.chaos)
                {
                    // 怪物渾沌，随机攻击其他怪物
                    RandomAttack(data.monsters,monster);
                }

                // 回合结束后，将怪物状态重置为正常状态
                monster.effStatus = Utility.effectStatusType.normal;
            }
        }

        /// <summary>
        /// 怪物攻擊玩家的傷害擲骰，若命中執行傷害計算 (力量+骰子/防禦值 * 5 = 傷害)
        /// </summary>
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
        public void RandomAttack(List<Monster> monsters,Monster monster)
        {
            Random random = new Random();
            int targetIndex = random.Next(monsters.Count);
            Monster target = monsters[targetIndex];
            Utility.PrintColorText(monster.name, ConsoleColor.Red);
            Console.Write("陷入了渾沌狀態並且攻擊了");
            Utility.PrintColorText(target.name, ConsoleColor.Red);
            Console.WriteLine("造成了5點傷害");
            target.hp -= 5;
        }

        /// <summary>
        /// 將選擇的怪物復活
        /// </summary>
        public void ReviveMonster(Monster monster)
        {
            monster.hp = monster.maxHp;
            monster.hpStatus = Utility.hpStatusType.fullHp;
            monster.effStatus = Utility.effectStatusType.normal;
            Utility.PrintColorText(monster.name,ConsoleColor.Red);
            Console.WriteLine("已復活，血量已滿。");
        }
        #endregion
    }

    public static class Utility
    {
        /// <summary>
        /// 更改文字顏色
        /// </summary>
        public static void PrintColorText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        /// <summary>
        /// hp狀態
        /// </summary>
        public enum hpStatusType
        {
            fullHp,
            lossHp,
            dead
        }
        /// <summary>
        /// 效果狀態
        /// </summary>
        public enum effectStatusType
        {
            normal,
            paralysis,
            chaos,
            dead
        }
    }
}
