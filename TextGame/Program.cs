namespace TextGame
{
    internal class Program
    {
        static void Main(string[] args)
        {

            
        //10隻怪物擁有血量(mHp)、最大血量(mMaxHp)、經驗值(mExp)、50 % 機率掉落炸彈(編號1，不堆疊，對目標造成100傷害)、目前狀態(mState, 0🡺滿血 1🡺失血 2🡺死亡)
        //玩家擁有力量(pSt)、經驗值(pExp)、包包(pBag 數量上限 10)。
        //當玩家按下’esc’ 後通過”再次確認”機制後結束遊戲
        //當玩家按下’1’ 時顯示玩家狀態(力、經驗、包包每格內容)
        //當玩家按下’2’ 進入選怪(確認後顯示該怪物資訊)
        //當按下’3’時，對選擇怪物造成傷害(力量值)

        //        如果選擇怪物此時死亡🡺玩家經驗值與包包物品依怪物設定增加
        //        如果沒有死亡🡺顯示攻擊扣傷等字串
        //當按下’4’時使用包包內炸彈對選擇怪物造成傷害(後續處理同上)
        //當按下’5’ 且選擇怪物於死亡狀態下 🡺 怪物滿血復活 🡺 並顯示適合的訊息
        //依據各自遊戲數值區間經驗，自行宣告酷炫屬性名稱並給定屬性值
        //設計包包、選怪的容錯處理
        //機率可暫用每打兩隻掉一個取代

            //怪物屬性
            //mHp：血量。
            //mMaxHp：最大血量。
            //mExp：經驗值。
            //mState：目前狀態（0：滿血，1：失血(血量)，2：死亡，3:狀態(麻痺..)）。
            //50 % 機率掉落炸彈（編號1，不堆疊，對目標造成100傷害）。
            //25%機率掉落核彈(可一次攻擊兩隻怪物，造成150傷害)這個功能作業3再加

            //玩家屬性
            //職業 第四個作業增加玩家可以一次控制多個角色(可能可以一開始隨機排列玩家與怪物攻擊的回合，像博得那樣)
            //血量
            //最大血量
            //pSt：力量。
            //pExp：經驗值。
            //mState：目前狀態（0：滿血，1：失血(血量)，2：死亡，3:狀態(麻痺..)）。
            //pBag：包包（數量上限10）
            int monsterHp = 60; //怪物血量
            float monsterSt = 20; //怪物力量
            int monsterDex = 2; //怪物敏捷
            int monsterAc = 12; //怪物防禦值
            int monsterExp = 10; //怪物經驗值
            bool isParalysis = false; //是否為麻痺狀態

            int playerHp = 75; //玩家血量
            float playerSt = 15; //玩家力量
            int playerDex = 3; //玩家敏捷
            int playerAc = 13; //玩家防禦值
            int playerExp = 18; //玩家經驗值
            int turn = 0;
            int q = 0, w = 0, e = 0, r = 0; //計算最後按了幾次qwer

            Random random = new Random();
            //敏捷+骰子 > 防禦 = 命中
            //力量+骰子/防禦值 * 5 = 傷害
            //骰到20的話極端成功(爆擊)，傷害加5
            //骰到1極端失敗
            //勇者是法師，Q=火球術 15點傷害 W=冰刃術 15點傷害 E=麻痺術 2點傷害，怪物此回合不能攻擊 R=解離術 20點傷害，每五回合可用一次
            Game game = new Game();
            game.InitializeMonsters();
            game.showMonsterStatus();
            game.showPlayerClass();
            Console.WriteLine("勇者!!現在你遇到了一隻怪物!!! 請按Q/W/E/R來進行攻擊!!!\n");
            Console.WriteLine("Q為火球術基礎值為15\nW為冰刃術基礎值為15\nE為麻痺術基礎值為2，此回合敵人不可攻擊，每三回合可用一次\nR為解離術基礎值為20，每五回合可用一次");
            PrintColorText("怪物", ConsoleColor.Red);
            Console.WriteLine("- 血量:60 力量:22 敏捷:2 防禦:12 經驗值:10");
            PrintColorText("你", ConsoleColor.Green);
            Console.WriteLine("-   血量:75 力量:QWER 敏捷:3 防禦:13 經驗值:18");
            Console.WriteLine("攻擊時將進行命中擲骰，骰子為1-20，敏捷+骰子>對方防禦則命中，反之未命中");
            Console.WriteLine("命中後將進行傷害擲骰，骰子為1-20，傷害值計算為 (力量+骰子)/對方防禦值*5(玩家力量根據QWER更改，例:輸入Q為15，輸入E為2) \n傷害擲骰骰中20為爆擊，爆擊加成為總傷害+5");
            Console.WriteLine("----------------------------------------------\n");
            //while (monsterHp > 0 && playerHp > 0)
            //{
            //    turn++;
            //    Console.WriteLine("\n第{0}回合開始----------\n", turn);
            //    string attackType = SetAttackEnter();
            //    PrintColorText("你", ConsoleColor.Green);
            //    Console.WriteLine($"使用了{attackType}");

            //    //玩家攻擊
            //    if (Hit("你", playerDex, monsterAc))
            //    {
            //        if (attackType == "麻痺術")
            //        {
            //            isParalysis = true;
            //        }
            //        int damage = Damage("你", playerSt, monsterAc, attackType);
            //        monsterHp -= damage;
            //        if (monsterHp <= 0)
            //        {
            //            monsterHp = 0;
            //            PrintColorText("怪物", ConsoleColor.Red);
            //            Console.Write("被");
            //            PrintColorText("你", ConsoleColor.Green);
            //            Console.WriteLine("殺死了!!");
            //            playerExp += monsterExp;
            //        }
            //        else
            //        {
            //            PrintColorText("怪物", ConsoleColor.Red);
            //            Console.WriteLine("受到了{0}點傷害，他現在還有{1}點生命值", damage, monsterHp);
            //        }

            //    }
            //    else
            //    {
            //        Console.Write("噢!");
            //        PrintColorText("你", ConsoleColor.Green);
            //        Console.WriteLine("未命中怪物!!! 他現在還有{0}點生命值", monsterHp);
            //    }
            //    //怪物攻擊
            //    if (monsterHp > 0)
            //    {
            //        if (!isParalysis)
            //        {
            //            if (Hit("怪物", monsterDex, playerAc))
            //            {
            //                int damage = Damage("怪物", monsterSt, playerAc, "");
            //                playerHp -= damage;
            //                if (playerHp <= 0)
            //                {
            //                    playerHp = 0;
            //                    PrintColorText("你", ConsoleColor.Green);
            //                    Console.WriteLine("被怪物殺了!!");
            //                }
            //                else
            //                {
            //                    PrintColorText("你", ConsoleColor.Green);
            //                    Console.Write("受到了{0}點傷害，", damage);
            //                    Console.Write("你");
            //                    Console.WriteLine("現在還有{0}點生命值", playerHp);
            //                }

            //            }
            //            else
            //            {
            //                Console.Write("噢!");
            //                PrintColorText("怪物", ConsoleColor.Red);
            //                Console.Write("未命中");
            //                PrintColorText("你", ConsoleColor.Green);
            //                Console.WriteLine("!!! ");
            //                PrintColorText("你", ConsoleColor.Green);
            //                Console.WriteLine("現在還有{0}點生命值", playerHp);
            //            }
            //        }
            //        else
            //        {
            //            PrintColorText("怪物", ConsoleColor.Red);
            //            Console.WriteLine("被麻痺了，此回合不可攻擊!");
            //            isParalysis = false;
            //        }
            //    }

            //}

            if (monsterHp <= 0)
            {
                Console.WriteLine("\n遊戲結束-------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("恭喜你獲得了{0}點經驗值，現在你有{1}點經驗值了!!!", monsterExp, playerExp);
            }
            else if (playerHp <= 0)
            {
                Console.WriteLine("\n遊戲結束-------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("遊戲結束!!!你被怪物殺死了，再去練等吧!!");
            }
            Console.ResetColor();
            Console.WriteLine($"你總共使用了{q}次火球術(Q)，{w}次冰刃術(W)，{e}次麻痺術(E)，{r}次解離術(R)!");



            //命中擲骰，是否命中 (敏捷+骰子 > 防禦 = 命中)
            bool Hit(string turn, int dex, int ac)
            {
                int attak;
                int hitRoll = rollDice(turn, "命中");
                attak = dex + hitRoll;
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
                if (attak >= ac)
                {
                    Console.WriteLine("骰中{0}，命中值為{1}+{2}={3}", hitRoll, dex, hitRoll, attak);
                }
                else
                {
                    Console.WriteLine("骰中{0}，命中值為{1}+{2}={3}，對方防禦為{4}，未命中", hitRoll, dex, hitRoll, attak, ac);
                }

                return attak >= ac;
            }


            //傷害擲骰，若命中執行傷害計算 (力量+骰子/防禦值 * 5 = 傷害)
            int Damage(string turn, float st, int ac, string HitEnter)
            {
                float damage;
                int damageRoll = rollDice(turn, "傷害");

                damage = (st + damageRoll) / ac * 5;

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
                //if (turn == "你")
                //{
                //    Console.Write($"使用了{HitEnter}，");
                //}
                Console.WriteLine($"骰中{damageRoll}，傷害值為({st}+{damageRoll})/{ac}*5={(int)damage}");
                if (damageRoll == 20)
                {
                    damage += 5;
                    Console.WriteLine("爆擊加成，最終傷害為" + (int)damage);
                }
                return (int)damage;
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

            //更改文字顏色
            void PrintColorText(string text, ConsoleColor color)
            {
                Console.ForegroundColor = color;
                Console.Write(text);
                Console.ResetColor();
            }

            //判定玩家輸入何種法術
            string SetAttackEnter()
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true); //捕獲按鍵訊息
                char attackEnter = keyInfo.KeyChar;
                if (char.IsLetter(attackEnter))
                {
                    //attackEnter.ToString().ToUpper();
                    //if (attackEnter == 'Q')
                    //{
                    //    playerSt = 15;
                    //    return "火球術";
                    //}
                    switch (attackEnter.ToString().ToUpper())
                    {
                        case "Q":
                            q++;
                            playerSt = 15;
                            return "火球術";
                        case "W":
                            w++;
                            playerSt = 15;
                            return "冰刃術";
                        case "E":
                            if (turn % 3 == 0)
                            {
                                e++;
                                playerSt = 2;
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
                                r++;
                                playerSt = 20;
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
        }
    }
}
