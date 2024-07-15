using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TextGame.Skill;

namespace TextGame
{
    internal class dataManager
    {
        public Player wizard = new Player("法師") { hp = 75, maxHp = 75, strength = 5, dexterity = 3, armorClass = 13, experience = 0, hpState = 1, status = 1 };
        public Player warrior = new Player("戰士") { hp = 100, maxHp = 100, strength = 5, dexterity = 3, armorClass = 15, experience = 0, hpState = 1, status = 1 };
        public Player assassin = new Player("刺客") { hp = 80, maxHp = 80, strength = 5, dexterity = 6, armorClass = 13, experience = 0, hpState = 1, status = 1 };
        public Player cleric = new Player("牧師") { hp = 75, maxHp = 75, strength = 5, dexterity = 3, armorClass = 12, experience = 0, hpState = 1, status = 1 };
        public Player druid = new Player("德魯伊") { hp = 80, maxHp = 80, strength = 5, dexterity = 4, armorClass = 13, experience = 0, hpState = 1, status = 1 };
        public List<Player> players = new List<Player>();
        public List<Monster> monsters = new List<Monster>();

        public dataManager()
        {
            SetAttacks();
        }

        /// <summary>
        /// 加入角色技能
        /// </summary>
        public void SetAttacks()
        {
            wizard.skill.Add('Q', new Attack("火球術", 15, skillType.normal, turn => true));
            wizard.skill.Add('W', new Attack("冰刃術", 15, skillType.normal, turn => true));
            wizard.skill.Add('E', new Attack("麻痺術", 5, skillType.paralysis, turn => turn % 3 == 0));
            wizard.skill.Add('R', new Attack("解離術", 20, skillType.chaos, turn => turn % 5 == 0));
            wizard.skill.Add('B', new Attack("轟炸", 20, skillType.bomb, turn => wizard.playerBag.Count(item => item == 1) > 0));

            warrior.skill.Add('Q', new Attack("揮砍", 15, skillType.normal, turn => true));
            warrior.skill.Add('W', new Attack("貓一拳", 15, skillType.normal, turn => true));
            warrior.skill.Add('E', new Attack("掃堂腿", 10, skillType.paralysis, turn => turn % 5 == 0));
            warrior.skill.Add('R', new Attack("重擊", 25, skillType.chaos, turn => turn % 6 == 0));
            warrior.skill.Add('B', new Attack("轟炸", 20, skillType.bomb, turn => warrior.playerBag.Count(item => item == 1) > 0));

            assassin.skill.Add('Q', new Attack("穿刺", 15, skillType.normal, turn => true));
            assassin.skill.Add('W', new Attack("切割", 15, skillType.normal, turn => true));
            assassin.skill.Add('E', new Attack("偷襲", 3, skillType.paralysis, turn => turn % 2 == 0));
            assassin.skill.Add('R', new Attack("飛刀", 18, skillType.normal, turn => turn % 3 == 0));
            assassin.skill.Add('B', new Attack("轟炸", 20, skillType.bomb, turn => assassin.playerBag.Count(item => item == 1) > 0));

            cleric.skill.Add('Q', new Attack("聖光術", 15, skillType.normal, turn => true));
            cleric.skill.Add('W', new Attack("光球術", 15, skillType.normal, turn => true));
            cleric.skill.Add('E', new Heal("補血", 5, skillType.heal, turn => turn % 2 == 0));
            cleric.skill.Add('R', new Heal("全體補血", 10, skillType.healGroup, turn => turn % 5 == 0));
            cleric.skill.Add('B', new Attack("轟炸", 20, skillType.bomb, turn => cleric.playerBag.Count(item => item == 1) > 0));

            druid.skill.Add('Q', new Attack("狼人嚎叫", 15, skillType.normal, turn => true));
            druid.skill.Add('W', new Attack("貓掌揮拳", 15, skillType.normal, turn => true));
            druid.skill.Add('E', new Heal("補血", 5, skillType.normal, turn => turn % 2 == 0));
            druid.skill.Add('R', new Attack("豬突猛進", 25, skillType.normal, turn => turn % 6 == 0));
            druid.skill.Add('B', new Attack("轟炸", 20, skillType.bomb, turn => druid.playerBag.Count(item => item == 1) > 0));
        }
    }
}
