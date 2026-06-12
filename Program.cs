using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Mage_v_Drag
{

        class Character
    {
        public string Name { get; }
        public int Health { get; protected set; }
        public int Mana { get; set; }

        public Character(string name, int health, int mana)
        {
            Name = name; 
            Health = health;
            Mana = mana;
        }


        public void TakeDamage(int damage)
        {
            Health -= damage;
            Console.WriteLine($"{Name} takes {damage} damage! Remaining health: {Health}");
        }

          public void HealCharacter(int heal)
        {
            Health += heal;
            Console.WriteLine($"{Name} heals {heal} health! Remaining health: {Health}");
        }
    }

    class Wizard : Character
    {
        public static int CountOfWizards { get; private set; }
        public List<Spell> Spells { get; set; }
        public List<Spell> UtilitySpells { get; set; }
        // private int Mana { get; set; }
        private float Experience { get; set; }


        public Wizard(string name, int mana, int health, float experience)
    : base(name, health, mana)
        {
            Experience = experience;
            Spells = new List<Spell>(){
                new Spell("Sorcerous Storm", 30) {EffectDragon = (w, d) => d.TakeDamage(170)},
                new Spell("Lightning Bolt", 25) {EffectDragon = (w, d) => d.TakeDamage(120)},
                new Spell("Ray of Judgment", 35) {EffectDragon = (w, d) => d.TakeDamage(230)},
                // new Spell("Sorcerous Storm", 30, (w, d) => d.TakeDamage(170)),
                // new Spell("Lightning Bolt", 25, (w, d) => d.TakeDamage(120)),
                // new Spell("Ray of Judgment", 35, (w, d) => d.TakeDamage(230))
            };
            UtilitySpells = new List<Spell>(){
                new Spell("Heal", 20) {EffectSelf = (w) => w.HealCharacter(200)},
                new Spell("Meditate", 25) {EffectSelf = (w) => w.RegenerateMana(130)},
                new Spell("Invisibility", 35) {EffectSelf = (w) => Console.WriteLine($"{w.Name} is now invisible!")},
                // new Spell("Heal", 20, (w) => w.HealCharacter(200)),
                // new Spell("Meditate", 0, (w) => w.RegenerateMana(130)),
                // new Spell("Invisibility", 25, (w) => Console.WriteLine($"{w.Name} is now invisible!"))
            };
            CountOfWizards++;
        }

 

        public void CastSpell(Spell spell, Dragon dragon)
        {
            if (Mana < spell.ManaCost)
            {
                Console.WriteLine("Not enough mana!");
                return;
            }

            Mana -= spell.ManaCost;
            Console.WriteLine($"{Name} casts {spell.Name}!");

            spell.EffectDragon?.Invoke(this, dragon);
        }

        public void CastUtilSpell(Spell spell, Wizard target) 
        {
            if (Mana < spell.ManaCost)
            {
                Console.WriteLine("Not enough mana!");
                return;
            }

            Mana -= spell.ManaCost;
            Console.WriteLine($"{Name} casts {spell.Name}!");

            // Sicherer Aufruf mittels ?.Invoke
            spell.EffectSelf?.Invoke(target);
        }

        public void RegenerateMana(int amount)
        {
            int maxMana = 400;



            int spaceLeft = maxMana - Mana;
            int addedAmount = Math.Min(amount, spaceLeft);

            Mana += addedAmount;
            
            Console.WriteLine($"{Name} regeneriert {addedAmount} Mana! Aktuelles Mana: {Mana}");
        }
    }

    class Spell
    {
        public string Name { get; set; }
        // public int Damage { get; set; }
        public int ManaCost { get; set; }
        // public int Heal { get; set; }
        // public int ManaRegen { get; set; }
        public Action<Wizard, Dragon>? EffectDragon { get; set; }
        public Action<Wizard>? EffectSelf {get; set;}

        public Spell(string name, int manaCost)
                {
                    Name = name;
                    ManaCost = manaCost;
                }

        // public Spell(string name, int manaCost, Action<Wizard, Dragon> effect)
        // {
        //     Name = name;
        //     ManaCost = manaCost;
        //     EffectDragon = effect;
        // }

        // public Spell(string name, int manaCost, Action<Wizard> effect)
        // {
        //     Name = name;
        //     ManaCost = manaCost;
        //     EffectSelf = effect; 
        // }

    }

    class Attack
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int ManaCost { get; set; }

        public Attack(string name, int damage, int manaCost)
        {
            Name = name;
            Damage = damage;
            ManaCost = manaCost;
        }
    }

    class Dragon : Character
    {
        public List<Attack> Attacks { get; set; }
        private float Experience { get; set; }

        /** Base: Rufe den Konstruktor der Basisklasse (Character) 
        auf und übergib ihm diese Werte. */ 

        public Dragon(string name, int health, int mana, float experience) : base(name, health, mana)
        {
            Experience = experience;
            Attacks = new List<Attack>(){
                new Attack("Fire Breath", 50, 30),
                new Attack("Claw Swipe", 40, 0),
                new Attack("Tail Whip", 60, 0),
            };
        }

        public void UseAttack(Attack attack, Wizard wizard)
        {
            Console.WriteLine($"{Name} attacks with {attack.Name}!");
            wizard.TakeDamage(attack.Damage);
        }

        public void Attack(Wizard wizard)
        {
            if (Attacks.Count > 0)
            {
                Random numberGen = new ();
                int randomDragonAbility = numberGen.Next(0, Attacks.Count);
                UseAttack(Attacks[randomDragonAbility], wizard);
            }
        }
    }




    class Program
    {
        static void Main(string[] args) 
        {
            /* Parameter: Name, Health, Mana, Exp. */
            Console.ForegroundColor = ConsoleColor.Magenta;
            Wizard wizard01 = new ("Aerith", 120, 400, 2000);

            Console.WriteLine($"Total Wizards Created: {Wizard.CountOfWizards}");
            
            IEnumerable<string> spellNames = wizard01.Spells.Select(s => s.Name);
            Console.WriteLine($"{wizard01.Name} has the following spells: {string.Join(", ", spellNames)}");
        
            Console.ForegroundColor = ConsoleColor.Magenta;
            Dragon dragon01 = new ("Tiamat", 1000, 300, 700);

            IEnumerable<string> abilityNames = dragon01.Attacks.Select(s => s.Name);
            Console.WriteLine($"{dragon01.Name} has the following attacks: {string.Join(", ", abilityNames)}");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
        
            do 
            {
                Console.WriteLine("Choose an action: 1. Cast Spell 2. Use Utility Spell 3. Attack Dragon");
            Console.ForegroundColor = ConsoleColor.Red;

                string choice = ReadNonEmptyInput();
            Console.ForegroundColor = ConsoleColor.DarkCyan;

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Choose a spell to cast:");
                        DisplaySpells(wizard01.Spells);

                        Console.ForegroundColor = ConsoleColor.Red;
                        string spellChoice = ReadNonEmptyInput();
                        Console.ForegroundColor = ConsoleColor.DarkCyan;

                        int spellIndex = int.Parse(spellChoice) - 1;
                        
                        if (spellIndex < 0 || spellIndex >= wizard01.Spells.Count)
                        {
                            Console.WriteLine("Invalid spell choice!\n");
                            continue; // Bricht ab und startet wieder beim Hauptmenü
                        }

                        Spell selectedSpell = wizard01.Spells[spellIndex];

                     
                        if (wizard01.Mana < selectedSpell.ManaCost)
                        {
                            Console.WriteLine($"Not enough mana! (Need {selectedSpell.ManaCost}, have {wizard01.Mana})\n");
                            continue; 
                        }

                        
                        wizard01.CastSpell(selectedSpell, dragon01);
                        
                        if (dragon01.Health > 0)
                        {
                            dragon01.Attack(wizard01);
                        }
                        break;

                    case "2":
                        Console.WriteLine("Choose a utility spell to use:");
                        DisplaySpells(wizard01.UtilitySpells);

                        string utilSpellChoice = ReadNonEmptyInput();
                        int utilSpellIndex = int.Parse(utilSpellChoice) - 1;

                        // Sicherheitscheck für den Index
                        if (utilSpellIndex < 0 || utilSpellIndex >= wizard01.UtilitySpells.Count)
                        {
                            Console.WriteLine("Invalid spell choice!\n");
                            continue; // Bricht ab und startet wieder beim Hauptmenü
                        }

                        Spell selectedUtilSpell = wizard01.UtilitySpells[utilSpellIndex];

                        // Prüfung auf Mana
                        if (wizard01.Mana < selectedUtilSpell.ManaCost) 
                        {
                            Console.WriteLine($"Not enough mana! (Need {selectedUtilSpell.ManaCost}, have {wizard01.Mana})\n");
                            continue; // Bricht ab und startet wieder beim Hauptmenü
                        }

                        // Wenn genug Mana da ist: Zaubern und Drache greift an
                        wizard01.CastUtilSpell(selectedUtilSpell, wizard01);
                        
                        if (dragon01.Health > 0)
                        {
                            dragon01.Attack(wizard01);
                        }
                        break;
                }
                
                
            } while (dragon01.Health > 0 && wizard01.Health > 0);
        
        }
        /* GATE KEEPER MECHANISM*/
        static string ReadNonEmptyInput()
        {
            while (true)
            {
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                Console.WriteLine("Please enter a valid value.");
            }
        }

        static void DisplaySpells(List<Spell> spells)
        {
            for (int i = 0; i < spells.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {spells[i].Name} (Mana: {spells[i].ManaCost})");
            }
        }
    }
}