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
    }

    class Wizard : Character
    {
        public static int CountOfWizards { get; private set; }
        public string NameOfWizard { get; set; }
        public List<Spell> Spells { get; set; }
        public List<Spell> UtilitySpell { get; set; }
        // private int Mana { get; set; }
        private float Experience { get; set; }


        public Wizard(string name, int mana, int health, float experience)
    : base(name, health, mana)
        {
            NameOfWizard = name;
            Mana = mana;
            // Health = health;
            Experience = experience;
            Spells = new List<Spell>(){
                new Spell("Sorcerous Storm", 50, 30),
                new Spell("Lightning Bolt", 40, 25),
                new Spell("Ray of Judgment", 60, 35)
            };
            UtilitySpell = new List<Spell>(){
                new Spell("Heal", 0, 20, 200),
                new Spell("Meditate", 0, 15),
                new Spell("Invisibility", 0, 25)
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

            Console.WriteLine($"{NameOfWizard} casts {spell.Name}!");

            dragon.TakeDamage(spell.Damage);

        }
    }

    class Spell
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int ManaCost { get; set; }
        public int Heal { get; set; }

        public Spell(string name, int damage, int manaCost, int heal = 0)
        {
            Name = name;
            Damage = damage;
            ManaCost = manaCost;
            Heal = heal;
        }
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
        public string NameOfDragon { get; set; }
        public List<Attack> Attacks { get; set; }
        private float Experience { get; set; }

        /** Base: Rufe den Konstruktor der Basisklasse (Character) 
        auf und übergib ihm diese Werte. */ 

        public Dragon(string name, int health, int mana, float experience) : base(name, health, mana)
        {
            NameOfDragon = name;
            Health = health;
            Mana = mana;
            Experience = experience;
            Attacks = new List<Attack>(){
                new Attack("Fire Breath", 50, 30),
                new Attack("Claw Swipe", 40, 0),
                new Attack("Tail Whip", 60, 0),
            };
        }

        public void UseAttack(Attack attack, Wizard wizard)
        {
            Console.WriteLine($"{NameOfDragon} attacks with {attack.Name}!");
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
            Console.WriteLine($"{wizard01.NameOfWizard} has the following spells: {string.Join(", ", wizard01.Spells)}");
        
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Dragon dragon01 = new ("Tiamat", 1000, 300, 700);

            Console.WriteLine($"{dragon01.NameOfDragon} has the following attacks: {string.Join(", ", dragon01.Attacks)}");
        
            do 
            {
                Console.WriteLine("Choose an action: 1. Cast Spell 2. Use Utility Spell 3. Attack Dragon");
                string choice = ReadNonEmptyInput();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Choose a spell to cast:");
                                            

                        for (int i = 0; i < wizard01.Spells.Count; i++)
                        {
                            Console.WriteLine(
                                $"{i + 1}. {wizard01.Spells[i].Name} " +
                                $"(Damage: {wizard01.Spells[i].Damage}, Mana Cost: {wizard01.Spells[i].ManaCost})");
                        }

                        string spellChoice = ReadNonEmptyInput();

                        int spellIndex = int.Parse(spellChoice) - 1;

                        Spell selectedSpell = wizard01.Spells[spellIndex];

                        wizard01.CastSpell(selectedSpell, dragon01);

                        if (dragon01.Health > 0)
                            {
                                dragon01.Attack(wizard01);
                            }

                            break;

                    case "2":
                        Console.WriteLine("Choose a utility spell to use:");
                        for (int i = 0; i < wizard01.UtilitySpell.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {wizard01.UtilitySpell[i].Name} (Heal: {wizard01.UtilitySpell[i].Heal}, Mana Cost: {wizard01.UtilitySpell[i].ManaCost})");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
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
    }
}