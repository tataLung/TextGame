using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TextGame
{
    public abstract class Skill
    {
        public string name { get; set; } //招式名稱
        public int strength { get; set; } //招式基礎值
        public Func<int, bool> CanUse { get; set; }

        public Skill(string name, int strength, Func<int, bool> canUse)
        {
            this.name = name;
            this.strength = strength;
            CanUse = canUse;
        }

        public bool CanUseSkill(int turn)
        {
            return CanUse(turn);
        }
        public abstract void UseSkill();
        //public abstract void ShowSkill();
    }
    // 攻擊技能類別
    public class Attack : Skill
    {
        public Attack(string name, int strength, Func<int, bool> canUse)
            : base(name, strength, canUse)
        {
        }

        public override void UseSkill()
        {
            Console.WriteLine($"使用攻擊技能: {name}, 造成 {strength} 點傷害.");
        }

        //public override void ShowSkill()
        //{
        //    Console.WriteLine($"Q:{wizard.Attacks['Q'].name},力量:{wizard.Attacks['Q'].strength}");

        //}
    }

    // 補血技能類別
    public class Heal : Skill
    {
        public Heal(string name, int strength, Func<int, bool> canUse)
            : base(name, strength, canUse)
        {
        }

        public override void UseSkill()
        {
            Console.WriteLine($"使用補血技能: {name}, 回復 {strength} 點生命.");
        }
    }
}
