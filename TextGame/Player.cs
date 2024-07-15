using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TextGame
{
    internal class Player
    {
        public string playerClass { get; private set; } //玩家職業
        public int hp { get; set; } //玩家血量
        public int maxHp { get; set; } //玩家最大血量
        public float strength { get; set; } //玩家力量
        public int dexterity { get; set; } //玩家敏捷
        public int armorClass { get; set; } //玩家防禦值
        public int experience { get; set; } //玩家經驗值
        public Utility.hpStatusType hpStatus { get; set; } //怪物狀態 1=滿血2=失血3=死亡
        public Utility.effectStatusType effStatus { get; set; } //怪物狀態 1=正常 2=麻痺 3=渾沌 4=死亡
        public List<int> playerBag { get; set; } //玩家包包 0.空 1.炸彈 2.核彈 3.血包 數量上限10
        public Dictionary<char, Skill> skill { get; set; } = new Dictionary<char, Skill>(); //玩家攻擊招式
        public bool isAI { get; set; }

        public Player(string playerClass) 
        {
            this.playerClass = playerClass;
            //playerBag = new List<int>(10);
            playerBag = new List<int>(new int[10]);
            //與playerBag = new List<int>(10);的差別是使用 PlayerBag = new List<int>(10); 时，你得到的是一个容量为 10 但实际上为空的列表，适用于需要动态添加元素的场景。
            //使用 PlayerBag = new List<int>(new int[10]); 时，你得到的是一个已经包含 10 个元素（默认值为 0）的列表，适用于需要初始化固定数量元素的场景。

        }

    }

    internal class PlayerManager(dataManager data)
    {
        //private dataManager data;

        public void ShowPlayerInfo(Player player,string QText, string WText, string EText, string RText)
        {
            Utility.PrintColorText(player.playerClass, ConsoleColor.Green);
            Console.WriteLine($", 血量: {player.hp}/{player.maxHp}, 力量: {player.strength}, 敏捷: {player.dexterity}, 防禦: {player.armorClass}, 經驗值: {player.experience}");
            Console.WriteLine("技能:");
            Console.WriteLine($"Q: {player.skill['Q'].name}, 力量: {player.skill['Q'].strength}" + QText);
            Console.WriteLine($"W: {player.skill['W'].name}, 力量: {player.skill['W'].strength}" + WText);
            Console.WriteLine($"E: {player.skill['E'].name}, 力量: {player.skill['E'].strength}" + EText);
            Console.WriteLine($"R: {player.skill['R'].name}, 力量: {player.skill['R'].strength}" + RText);
            Console.WriteLine();
        }
        /// <summary>
        /// 顯示職業及其技能(應該要用繼承重寫)
        /// </summary>
        public void showAllPlayerClass()
        {
            //ShowPlayerInfo(data.wizard, "", "", " ,每三回合可使用一次，敵人陷入麻痺，此回合不可攻擊", " ,每五回合可使用一次，敵人陷入混亂，此回合將隨機攻擊其他怪物");
            //ShowPlayerInfo(data.warrior, "", "", " ,每五回合可使用一次，敵人陷入麻痺，此回合不可攻擊", " ,每六回合可使用一次，敵人陷入混亂，此回合將隨機攻擊其他怪物");
            //ShowPlayerInfo(data.assassin, "", "", " ,每兩回合可使用一次，敵人陷入麻痺，此回合不可攻擊", " ,每三回合可使用一次");
            //ShowPlayerInfo(data.cleric, "", "", " ,每兩回合可使用一次，幫自己補5滴血", " ,每五回合可使用一次，全體補10滴血");
            //ShowPlayerInfo(data.druid, "", "", " ,每兩回合可使用一次，幫自己補5滴血", " ,每六回合可使用一次，敵人陷入麻痺，此回合不可攻擊");
            ShowPlayerInfo(data.wizard, "", "", " ,每三回合可使用一次", " ,每五回合可使用一次");
            ShowPlayerInfo(data.warrior, "", "", " ,每五回合可使用一次", " ,每六回合可使用一次");
            ShowPlayerInfo(data.assassin, "", "", " ,每兩回合可使用一次", " ,每三回合可使用一次");
            ShowPlayerInfo(data.cleric, "", "", " ,每兩回合可使用一次，幫自己補5滴血", " ,每五回合可使用一次，全體補10滴血");
            ShowPlayerInfo(data.druid, "", "", " ,每兩回合可使用一次，幫自己補5滴血", " ,每六回合可使用一次");
            Console.WriteLine("所有職業B為使用炸彈，固定造成20點傷害，且一定命中");
        }

        /// <summary>
        /// 選擇人物及伙伴職業(之後應該要搬到player)
        /// </summary>
        public void InitializePlayer()
        {
            Console.WriteLine("請先選擇你的職業 0:法師 1:戰士 2.刺客 3.牧師 4.德魯伊");
            while (true)
            {
                char input = Console.ReadKey(true).KeyChar;
                if (AddPlayerToClass(input, false)) break;
            }
            Console.WriteLine("選擇你的夥伴 0:法師 1:戰士 2.刺客 3.牧師 4.德魯伊");
            while (true)
            {
                char input = Console.ReadKey(true).KeyChar;
                if (AddPlayerToClass(input, true)) break;
            }
            Console.WriteLine("現在隊伍裡有");
            Utility.PrintColorText($"你({data.players[0].playerClass})", ConsoleColor.Green);
            Console.WriteLine($", 血量: {data.players[0].hp} / {data.players[0].maxHp} , 力量:  {data.players[0].strength} , 敏捷:  {data.players[0].dexterity} , 防禦:  {data.players[0].armorClass} , 經驗值:  {data.players[0].experience}");
            Utility.PrintColorText($"夥伴({data.players[1].playerClass})", ConsoleColor.Green);
            Console.WriteLine($", 血量: {data.players[1].hp} / {data.players[1].maxHp} , 力量:  {data.players[1].strength} , 敏捷:  {data.players[1].dexterity} , 防禦:  {data.players[1].armorClass} , 經驗值:  {data.players[1].experience}");
        }

        /// <summary>
        /// 將玩家選擇的職業添加到list中
        /// </summary>
        /// <returns></returns>
        private bool AddPlayerToClass(char input, bool isAI)
        {
            Player player = null;
            switch (input)
            {
                case '0':
                    player = data.wizard;
                    break;
                case '1':
                    player = data.warrior;
                    break;
                case '2':
                    player = data.assassin;
                    break;
                case '3':
                    player = data.cleric;
                    break;
                case '4':
                    player = data.druid;
                    break;
                default:
                    Console.WriteLine("無效的輸入項");
                    return false;
            }

            if (player != null && !data.players.Any(p => p.playerClass == player.playerClass))
            {
                player.isAI = isAI;
                data.players.Add(player);
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
    }
}
