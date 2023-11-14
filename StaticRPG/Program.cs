using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

public static class Game
{

    static Random rnd = new Random();

    static bool Quitflag = false;
    static bool TurnState = true; //true means the player can act

    static string pAction = "temporary";
    static string lastAction = "temporary";

    static int pHealth = 100;
    static int pHealthDefault = 100;
    static int pShield = 50;
    static int pShieldDefault = 50;
    static int Lives = 3;
    static int HealPots = 2;
    static int HealBonus = 0; //temp
    static int HealTotal = 20; //temp

    static int pDamageMin = 1; //temp
    static int pDamageMax = 10; //temp
    static int pDamage = 5; //temp
    static int pDurabilityMin = 1; //temp
    static int pDurabilityMax = 10; //temp
    static int pDurability = 0; //temp
    static int pShieldChip = 1; //temp

    static int dplHealth = 50; //temp
    static int dplHealthMax = 100;
    static int dplHealthMin = 1;
    static int dplShield = 50; //temp
    static int dplShieldMax = 100;
    static int dplShieldMin = 0;
    static int dplShieldChip = 1; //temp

    static int exp = 0; //tally
    static int expGiven = 0; //temp
    static int lvl = 1;
    static int expRollMin = 1; //temp
    static int expRollMax = 10; //temp
    static int expTarget = (lvl + 1) * 10;


    static int eHealth = 50; //temp
    static int eHealthMax = 100; //temp
    static int eHealthMin = 10; //temp
    static int eDamageMax = 20; //temp
    static int eDamageMin = 1; //temp
    static int eDamage = 5; //temp
    static int BlockDamage = 1; //temp
    static int DamageBlocked = 1; //temp, used to tell player damage avoided

    static int eKilled = 0; //tally

    static int pToolsIndex = rnd.Next(0, 99);
    static string pTool = "[???]"; //temp

    static int eTypeIndex = rnd.Next(0, 100);
    static string eType = "MissingNo"; //temp

    //==================================================================
    static void pToolFinder()
    {
        pToolsIndex = rnd.Next(0, 99);

        if (pToolsIndex <= 9)
        {
            pTool = "SQUEAKY HAMMER";
        }
        else if (pToolsIndex <= 34 && pToolsIndex > 9)
        {
            pTool = "STICK";
        }
        else if (pToolsIndex <= 84 && pToolsIndex > 34)
        {
            pTool = "DAGGER";
        }
        else if (pToolsIndex <= 99 && pToolsIndex > 84)
        {
            pTool = "GIANT'S KNIFE";
        }
    }
    static void pToolStats()
    {
        if (pTool == "SQUEAKY HAMMER")
        {
            pDamageMin = 1;
            pDamageMax = 2;
            pDurabilityMin = 1;
            pDurabilityMax = 60;
        }

        else if (pTool == "STICK")
        {
            pDamageMin = 1;
            pDamageMax = 3;
            pDurabilityMin = 2;
            pDurabilityMax = 5;
        }

        else if (pTool == "DAGGER")
        {
            pDamageMin = 3;
            pDamageMax = 6;
            pDurabilityMin = 5;
            pDurabilityMax = 7;
        }

        else if (pTool == "GIANT'S KNIFE")
        {
            pDamageMin = 6;
            pDamageMax = 10;
            pDurabilityMin = 3;
            pDurabilityMax = 4;
        }
        pDamage = rnd.Next(pDamageMin, pDamageMax + 1);

        pDurability = rnd.Next(pDurabilityMin, pDurabilityMax + 1);
    }

    static void eTypeFinder()
    {
        eTypeIndex = rnd.Next(0, 100);

        if (eTypeIndex <= 24)
        {
            eType = "SLIME";
        }
        else if (eTypeIndex <= 49 && eTypeIndex > 24)
        {
            eType = "SKELETON";
        }
        else if (eTypeIndex <= 74 && eTypeIndex > 49)
        {
            eType = "GOBLIN";
        }
        else if (eTypeIndex <= 99 && eTypeIndex > 74)
        {
            eType = "ZOMBIE";
        }
        else if (eTypeIndex == 100)
        {
            eType = "EXODIA";
        }
    }
    static void eStats()
    {
        if (eType == "SLIME")
        {
            eHealthMin = 1;
            eHealthMax = 5;
            eDamageMin = 1 + lvl * 2;
            eDamageMax = 3 + lvl * 2;

            expRollMin = 1;
            expRollMax = 2;
        }

        else if (eType == "SKELETON")
        {
            eHealthMin = 3 + lvl;
            eHealthMax = 6 + lvl;
            eDamageMin = 2;
            eDamageMax = 5;

            expRollMin = 5;
            expRollMax = 6;
        }

        else if (eType == "GOBLIN")
        {
            eHealthMin = 3 + lvl;
            eHealthMax = 10 + lvl;
            eDamageMin = 2;
            eDamageMax = 6;

            expRollMin = 6;
            expRollMax = 10;
        }

        else if (eType == "ZOMBIE")
        {
            eHealthMin = 5 + lvl;
            eHealthMax = 20 + lvl;
            eDamageMin = 4;
            eDamageMax = 12;

            expRollMin = 12;
            expRollMax = 20;
        }

        else if (eType == "EXODIA")
        {
            Console.WriteLine("YOU ENCOUNTER EXODIA THE FORBIDDEN ONE!!!");
            Console.ReadKey(true);
            Console.WriteLine("You feel your health draining!");
            Console.ReadKey(true);
            for (int i = 0; i < pHealth;)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                pHealth -= 1;
                Console.WriteLine(pHealth);
                Thread.Sleep(25); //makes the system wait 25ms before going again
            }
            Quitflag = true; //ends the loop early
            Console.ForegroundColor = ConsoleColor.Green;
        }

        eHealth = rnd.Next(eHealthMin, eHealthMax + 1);
        eDamage = rnd.Next(eDamageMin, eDamageMax + 1);
        expGiven = rnd.Next(expRollMin, expRollMax);
    }

    static void HUD()
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("~~~~~~~~~~~~");
        Console.WriteLine("Player Health: " + pHealth);
        if (pHealth >= 75)
        {
            Console.WriteLine("Player Health is GREAT");
        }
        else if (pHealth >= 50 && pHealth < 75)
        {
            Console.WriteLine("Player Health is FINE");
        }
        else if (pHealth >= 25 && pHealth < 50)
        {
            Console.WriteLine("Player Health is LOW");
        }
        else if (pHealth >= 1 && pHealth < 25)
        {
            Console.WriteLine("Player Health is IN DANGER");
        }
        Console.WriteLine("Current weapon: " + pTool);
        Console.WriteLine("Shield Durability: " + pShield);
        Console.WriteLine("Health Potions: " + HealPots);
        Console.WriteLine("Lives: " + Lives);
        Console.WriteLine("~~~~~~~~~~~~");
        Console.WriteLine("Level: " + lvl);
        Console.WriteLine("EXP: " + exp);
        Console.WriteLine("~~~~~~~~~~~~");
        Console.ForegroundColor = ConsoleColor.White;
    }
    static void DamageTaken()
    {
        pShield -= eDamage;
        if (pShield < 0)
        {
            pHealth += pShield;
            pShield = 0;
        }
    }
    static void pDeathCheck()
    {
        if (pHealth <= 0 && pShield == 0)
        {
            if (Lives >= 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("PLAYER HAS DIED!");
                Console.ReadKey(true);
                Console.WriteLine("RESPAWNING");
                Console.ReadKey(true);
                Console.WriteLine(".");
                Console.ReadKey(true);
                Console.WriteLine(".");
                Console.ReadKey(true);
                Console.WriteLine(".");
                Console.ReadKey(true);
                Console.WriteLine(".");
                Console.ReadKey(true);
                Console.WriteLine(".");
                Console.ReadKey(true);
                Console.ForegroundColor = ConsoleColor.White;
                pHealth = pHealthDefault;
                pShield = pShieldDefault;
                Lives -= 1;
                if (HealPots == 0)
                {
                    HealPots = 1;
                }
            }
            else
            {
                Quitflag = true;
            }
        }
    }
    static void lvlUp()
    {
        if (exp >= expTarget)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("|LEVEL UP!|");
            exp -= expTarget;
            lvl += 1;
            expTarget = (lvl + 1) * 10;
            Console.WriteLine("Player is now level " + lvl);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    static void pActAttack()
    {
        Console.WriteLine("Player deals " + pDamage + " damage to the " + eType);
        eHealth -= pDamage;
        pDurability -= 1;

        TurnState = false;
    }
    static void pActHeal()
    {
        HealBonus = ((lvl + 1) / 2) * 3; //just random numbers, don't ask.
        if (HealPots > 0 && pHealth < pHealthDefault)
        {
            Console.WriteLine("You decide to heal for this turn");
            if ((pHealth + 20 + HealBonus) > pHealthDefault)
            {
                HealTotal = (pHealthDefault - pHealth);
            }
            else
            {
                HealTotal = 20 + HealBonus;
            }
            pHealth += HealTotal;

            Console.WriteLine("You regenerated " + HealTotal + " health!");
            Console.ReadKey(true);

            HealPots -= 1;

            TurnState = false;
        }

        else if (HealPots == 0)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("You're out of potions!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        else if (pHealth == pHealthDefault)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("You're at MAX health");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    static void pActBlock()
    {
        if (pShield > 0)
        {

            BlockDamage = eDamage - lvl;

            if (BlockDamage <= 0)
            {
                BlockDamage = 1;
            }

            if (lvl <= eDamage)
            {
                pShieldChip = lvl;
            }
            else
            {
                pShieldChip = lvl - eDamage;
            }

            pShield -= BlockDamage;
            DamageBlocked = eDamage - BlockDamage;

            if (pShield < 0)
            {
                pHealth += pShield;
                pShield = 0;
            }

            eHealth -= pShieldChip;

            TurnState = false;
        }

        else
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Your SHIELD is broken!");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    //==================================================================

    static void Main()
    {
        //==================================================================//
        //GAME START//
        //==================================================================//
        pToolFinder();
        pToolStats();
        eTypeFinder();
        eStats();


        if (eType != "EXODIA") //this is here bc the whole exodia process is self-contained in the eStats method
        {
            Console.WriteLine("Player encounters a " + eType + "!");
        }

        while (Quitflag == false)
        {
            Console.WriteLine("The " + eType + " has " + eHealth + " health.");
            HUD();

            while (TurnState == true)
            {
                Console.WriteLine("Choose an action");
                pAction = Console.ReadLine();
                pAction = pAction.ToUpper();

                if (pAction == "ATTACK")
                {
                    pActAttack();
                }

                else if (pAction == "HEAL")
                {
                    pActHeal();
                }

                else if (pAction == "BLOCK")
                {
                    pActBlock();

                    if (eHealth < 0)
                    {
                        Console.WriteLine("You manage to block " + DamageBlocked + " damage!");
                        Console.WriteLine("Still recieve " + BlockDamage + " damage!");
                        Console.WriteLine("The " + eType + " recieves " + pShieldChip + " recoil damage!");
                    }

                }
                else if (pAction == "QUIT")
                {
                    TurnState = false;
                    Quitflag = true;
                }
                else
                {
                    Console.WriteLine("I don't understand, please try again");
                }

            }
            lastAction = pAction;
            if (eHealth > 0)
            {
                if (lastAction == "ATTACK")
                {
                    Console.WriteLine("Enemy Survives!");
                    Console.WriteLine("Retaliates with " + eDamage + " Damage!");
                    DamageTaken();
                    pDeathCheck();
                }
                else if (lastAction == "HEAL")
                {
                    Console.WriteLine("Enemy takes advantage to strike with " + eDamage + " Damage!");
                    DamageTaken();
                    pDeathCheck();
                }
                else if (lastAction == "BLOCK")
                {
                    Console.WriteLine("You manage to block " + DamageBlocked + " damage!");
                    Console.WriteLine("Still recieve " + BlockDamage + " damage!");
                    Console.WriteLine("The " + eType + " recieves " + pShieldChip + " recoil damage!");

                    pDeathCheck();
                }
            }
            else
            {
                if (pDurability < 1)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Your " + pTool + " breaks!");
                    pToolFinder();
                    pToolStats();
                    Console.WriteLine("You find a " + pTool + "!");
                    Console.WriteLine("Your new " + pTool + " has " + pDurability + " durability!");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine("ENEMY IS SLAIN!");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("You gained " + expGiven + " EXP");
                exp += expGiven;
                lvlUp();
                Console.ForegroundColor = ConsoleColor.White;
                eKilled += 1;
                Console.ReadKey(true);
                eTypeFinder();
                eStats();
                Console.WriteLine("");
                if (eType != "EXODIA")
                {
                    Console.WriteLine("Player encounters a " + eType + "!");
                }
            }
            TurnState = true;
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("|PLAYER IS DEFEATED|");
        Console.WriteLine("YOU KILLED " + eKilled + " ENEMIES");
        Console.WriteLine("YOU MADE IT TO LEVEL " + lvl);
        Console.ForegroundColor = ConsoleColor.White;
    }
}