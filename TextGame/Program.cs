using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace TextGame
{
    internal class Program
    {
        //尚未加入怪物攻擊回來的功能
        //怪物掉落物目前僅會掉落炸彈
        static void Main(string[] args)
        {
            Game game = new Game();
            Game.monsterManager.InitializeMonsters(5);
            Console.WriteLine("勇者，就在剛才，你被異世界傳送器，也就是卡車撞了，請選擇你的職業以及一位同伴!");
            Game.playerManager.showAllPlayerClass();
            Game.playerManager.InitializePlayer();
            Console.WriteLine("隨機按一個鍵進入遊戲!");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("勇者!!現在你遇到了一群怪物!!! 請按3後進入戰鬥，選擇攻擊怪物對象按ENTER，並輸入Q/W/E/R/B來進行攻擊!!!\n");
            Game.monsterManager.showMonsterStatus();
            Console.WriteLine("攻擊時將進行命中擲骰，骰子為1-20，敏捷+骰子>對方防禦則命中，反之未命中");
            Console.WriteLine("命中後將進行傷害擲骰，骰子為1-20，傷害值計算為 (力量+骰子)/對方防禦值*5(玩家力量根據QWER更改，例:輸入Q為15，輸入E為2) \n傷害擲骰骰中20為爆擊，爆擊加成為總傷害+5");
            Console.WriteLine("----------------------------------------------\n");
            game.RunGame(); 
        }
    }
}
