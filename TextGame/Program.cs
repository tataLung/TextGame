using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace TextGame
{
    internal class Program
    {
        //尚未加入怪物攻擊回來的功能
        //尚未加入人物選擇加入夥伴
        //怪物掉落物目前僅會掉落炸彈
        //在顯示資訊時，把已死亡的怪物及角色便灰色
        static void Main(string[] args)
        {
            Game game = new Game();
            game.InitializeMonsters();
            Console.WriteLine("勇者，就在剛才，你被異世界傳送器，也就是卡車撞了，請選擇你的職業以及一位同伴!");
            game.showAllPlayerClass();
            game.InitializePlayer();
            Console.WriteLine("隨機按一個鍵進入遊戲!");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("勇者!!現在你遇到了一群怪物!!! 請按3後進入戰鬥，選擇攻擊怪物對象按ENTER，並輸入Q/W/E/R/B來進行攻擊!!!\n");
            //Console.WriteLine("Q為火球術基礎值為15\nW為冰刃術基礎值為15\nE為麻痺術基礎值為2，此回合敵人不可攻擊，每三回合可用一次\nR為解離術基礎值為20，每五回合可用一次\nB為使用炸彈，固定造成20點傷害");
            game.showMonsterStatus();
            game.showPlayerClass();
            Console.WriteLine("攻擊時將進行命中擲骰，骰子為1-20，敏捷+骰子>對方防禦則命中，反之未命中");
            Console.WriteLine("命中後將進行傷害擲骰，骰子為1-20，傷害值計算為 (力量+骰子)/對方防禦值*5(玩家力量根據QWER更改，例:輸入Q為15，輸入E為2) \n傷害擲骰骰中20為爆擊，爆擊加成為總傷害+5");
            Console.WriteLine("----------------------------------------------\n");
            while (!game.AreAllMonstersDead() && game.wizard.hp >0)
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
                        game.showPlayerClass();
                        break;
                    case '2':
                        game.showMonsterStatus();
                        break;
                    case '3':
                        Console.Clear();
                        //玩家攻擊
                        game.turn++;
                        Console.WriteLine("\n第{0}回合開始-------------------------↓\n", game.turn);
                        foreach (var player in game.players)
                        {
                            // 玩家攻击阶段
                            bool isAI = player.isAI; // 假设 Player 类有一个 IsAI 属性来表示是否是 AI
                            selectedMonster = game.SelectMonster(game.monsters, isAI, player.playerClass);
                            if (selectedMonster != null)
                            {
                                //if (selectedMonster.hpState == 3)
                                //{
                                //    Console.WriteLine("你選擇的怪物已死亡!請重新選擇");
                                //}
                                //else
                                //{
                                    //game.turn++;
                                    //Console.WriteLine("\n第{0}回合開始-------------------------↓\n", game.turn);
                                    string attackType = game.SetAttackEnter(player);
                                    if (attackType == "轟炸")
                                    {
                                        if (game.UseBomb(player))
                                        {
                                            Console.WriteLine("咻~~~~~~崩幾勒!");
                                            selectedMonster.hp -= 20;
                                            game.ShowMonsterDamageMessage(selectedMonster, 20,player);
                                        }
                                        else
                                        {
                                            Console.WriteLine("包包沒有炸彈!");
                                        }
                                    }
                                    else
                                    {
                                        if (game.HitMonster(player.playerClass, selectedMonster))
                                        {
                                            if (attackType == "麻痺術" || attackType == "掃堂腿")
                                            {
                                                selectedMonster.status = 2;
                                            }
                                            else if (attackType == "解離術")
                                            {
                                                selectedMonster.status = 3;
                                            }
                                            else
                                            {
                                                selectedMonster.status = 1;
                                            }
                                            int damage = game.PlayerDamage(selectedMonster, player);
                                            selectedMonster.hp -= damage;
                                            game.ShowMonsterDamageMessage(selectedMonster, damage,player);
                                        }
                                    }

                                    //Console.WriteLine("\n第{0}回合結束-------------------------↑\n", game.turn);
                                //}
                            }
                            else
                            {
                                Console.WriteLine("請先選擇一個怪物");
                            }
                            if (player == game.players.Last())
                            {
                                //game.turn++;
                                Console.WriteLine($"\n第{game.turn}回合結束-------------------------↑\n");
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
            if (game.AreAllMonstersDead())
            {
                Console.WriteLine("\n遊戲結束-------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"謝謝你勇者，你打倒了所有怪物，我們感謝你:))) 你現在有{game.wizard.experience}點經驗值!!");
                Console.ResetColor();
            }
            else if (game.wizard.hp <= 0)
            {
                Console.WriteLine("\n遊戲結束-------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("遊戲結束!!!你被怪物殺死了，再去練等吧!!");
                Console.ResetColor();
            }


        }
    }
}
