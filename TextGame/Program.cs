namespace TextGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.InitializeMonsters();

            while (true)
            {
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
                        selectedMonster = game.SelectMonster(game.monsters);
                        if(selectedMonster != null)
                        {
                            if(selectedMonster.hp == 0)
                            {
                                Console.WriteLine("你選擇的怪物已死亡!請重新選擇");
                            }
                            else
                            {
                                string attackType = game.SetAttackEnter();
                                if (game.Hit("你", selectedMonster))
                                {
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
                                    if (selectedMonster.hp <= 0)
                                    {
                                        selectedMonster.hp = 0;
                                        game.PrintColorText(selectedMonster.name, ConsoleColor.Red);
                                        Console.Write("被");
                                        game.PrintColorText("你", ConsoleColor.Green);
                                        Console.WriteLine("殺死了!!");
                                        game.wizard.experience += selectedMonster.experience;
                                        game.BombFall(game.wizard);
                                    }
                                    else
                                    {
                                        game.PrintColorText(selectedMonster.name, ConsoleColor.Red);
                                        Console.WriteLine("受到了{0}點傷害，他現在還有{1}點生命值", damage, selectedMonster.hp);
                                    }
                                }
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



        }
    }
}
