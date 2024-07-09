using static System.Net.Mime.MediaTypeNames;

namespace TextGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.InitializeMonsters();
            Console.WriteLine("勇者!!現在你遇到了一群怪物!!! 請按Q/W/E/R來進行攻擊!!!\n");
            Console.WriteLine("Q為火球術基礎值為15\nW為冰刃術基礎值為15\nE為麻痺術基礎值為2，此回合敵人不可攻擊，每三回合可用一次\nR為解離術基礎值為20，每五回合可用一次");
            game.showMonsterStatus();
            game.showPlayerClass();
            Console.WriteLine("攻擊時將進行命中擲骰，骰子為1-20，敏捷+骰子>對方防禦則命中，反之未命中");
            Console.WriteLine("命中後將進行傷害擲骰，骰子為1-20，傷害值計算為 (力量+骰子)/對方防禦值*5(玩家力量根據QWER更改，例:輸入Q為15，輸入E為2) \n傷害擲骰骰中20為爆擊，爆擊加成為總傷害+5");
            Console.WriteLine("----------------------------------------------\n");
            while (!game.AreAllMonstersDead() && game.wizard.hp >0)
            {
                Console.WriteLine(game.AreAllMonstersDead());
                Console.WriteLine("輸入操作 (0: 結束遊戲, 1: 顯示玩家狀態, 2:顯示怪物狀態 3:進入此回合戰鬥 )");
                char input = Console.ReadKey(true).KeyChar;
                Monster selectedMonster = null;
                switch (input)
                {
                    case '0':
                        Console.WriteLine("確定要結束遊戲嗎？(y/n)");
                        if (Console.ReadKey(true).KeyChar == 'y')
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
                        //玩家攻擊
                        selectedMonster = game.SelectMonster(game.monsters);
                        if(selectedMonster != null)
                        {
                            if(selectedMonster.hpState == 3)
                            {
                                Console.WriteLine("你選擇的怪物已死亡!請重新選擇");
                            }
                            else
                            {
                                //加回合
                                game.turn++;
                                Console.WriteLine("\n第{0}回合開始-------------------------\n", game.turn);
                                string attackType = game.SetAttackEnter();
                                if(attackType == "轟炸")
                                {
                                    if (game.UseBomb(game.wizard))
                                    {
                                        selectedMonster.hp -= 20;
                                        game.ShowMonsterDamageMessage(selectedMonster, 20);
                                    }
                                    else
                                    {
                                        Console.WriteLine("包包沒有炸彈!");
                                    }
                                    
                                }
                                else
                                {
                                    if (game.Hit("你", selectedMonster))
                                    {
                                        //selectedMonster.hpState = 1;
                                        if (attackType == "麻痺術")
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
                                        int damage = game.Damage("你", selectedMonster, game.wizard);
                                        selectedMonster.hp -= damage;
                                        game.ShowMonsterDamageMessage(selectedMonster, damage);
                                    }
                                }

                                //怪物攻擊
                                //if (!game.AreAllMonstersDead())
                                //{
                                //    game.ExecuteMonsterActions();

                                //}


                            }

                        }
                        else
                            Console.WriteLine("請先選擇一個怪物");
                        break;
                    default:
                        Console.WriteLine("無效的輸入項");
                        break;
                }
            }
            Console.WriteLine("dfdfd"+game.AreAllMonstersDead());
            if (game.AreAllMonstersDead())
            {
                Console.WriteLine("\n遊戲結束-------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"謝謝你勇者，你打倒了所有怪物，我們感謝你:))) 你現在有{game.wizard.experience}");
            }
            else if (game.wizard.hp <= 0)
            {
                Console.WriteLine("\n遊戲結束-------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("遊戲結束!!!你被怪物殺死了，再去練等吧!!");
            }


        }
    }
}
