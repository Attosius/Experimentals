using System;

namespace IJuniorTasks
{
    internal class БойСБоссом
    {
        static void Main29(string[] args)
        {
            const string CommandUsualAttack = "1";
            const string CommandFireBall = "2";
            const string CommandExplosion = "3";
            const string CommandHealing = "4";

            const int PlayerMaxHealth = 50;
            const int PlayerMaxMana = 20;
            const int PlayerMinAttack = 1;
            const int PlayerMaxAttack = 5;
            const int PlayerFireBallAttack = 10;
            const int PlayerFireBallManaCost = 5;
            const int PlayerExplosionAttack = 15;
            const int PlayerHealing = 10;
            const int PlayerHealingMaxCount = 4;

            const int BossMinAttack = 2;
            const int BossMaxAttack = 3;

            var random = new Random();

            int bossHealth = 100;
            int playerHealth = PlayerMaxHealth;
            int playerMana = PlayerMaxMana;
            bool isFireBallUsed = false;
            int playerHealingCount = 0;

            int step = 1;

            while (playerHealth > 0 && bossHealth > 0)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"Раунд: {step}\n" +
                                  $"Здоровье героя: {playerHealth}, мана {playerMana}\n" +
                                  $"Здоровье босса: {bossHealth}\n\n" +
                                  $"Введите команду:");
                Console.WriteLine($"{CommandUsualAttack}. Обычная атака, урон: {PlayerMinAttack}-{PlayerMaxAttack}");
                Console.WriteLine($"{CommandFireBall}. Огненный шар, урон: {PlayerFireBallAttack}, мана {PlayerFireBallManaCost}");
                Console.WriteLine($"{CommandExplosion}. Взрыв, урон: {PlayerExplosionAttack}");
                Console.WriteLine($"{CommandHealing}. Лечение, восстанавливает жизни: {PlayerHealing}, осталось лечений: {PlayerHealingMaxCount - playerHealingCount}");
                Console.WriteLine($"Введите команду: ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                var command = Console.ReadLine();

                switch (command)
                {
                    case CommandUsualAttack:
                        var usualAttack = random.Next(PlayerMinAttack, PlayerMaxAttack + 1);
                        bossHealth -= usualAttack;

                        if (usualAttack == PlayerMaxAttack)
                        {
                            Console.WriteLine($"Критический удар!");
                        }

                        Console.WriteLine($"Вы бьете обычной атакой на {usualAttack} урона.");
                        break;

                    case CommandFireBall:
                        if (playerMana < PlayerFireBallManaCost)
                        {
                            Console.WriteLine($"У вас недостаточно маны на огненный шар!");
                            break;
                        }
                        bossHealth -= PlayerFireBallAttack;
                        playerMana -= PlayerFireBallManaCost;
                        isFireBallUsed = true;
                        Console.WriteLine($"Вы бьете огненным шаром на {PlayerFireBallAttack} урона.");
                        break;

                    case CommandExplosion:
                        if (isFireBallUsed == false)
                        {
                            Console.WriteLine($"Взрыв можно использовать только после огненного шара!");
                            break;
                        }
                        isFireBallUsed = false;
                        bossHealth -= PlayerExplosionAttack;
                        Console.WriteLine($"Вы бьете взрывом на {PlayerExplosionAttack} урона.");
                        break;

                    case CommandHealing:
                        if (playerHealingCount >= PlayerHealingMaxCount)
                        {
                            Console.WriteLine($"Лечений больше не осталось!");
                            break;
                        }

                        playerHealingCount++;
                        playerHealth += PlayerHealing;
                        playerMana += PlayerHealing;

                        if (playerHealth > PlayerMaxHealth)
                        {
                            playerHealth = PlayerMaxHealth;
                        }

                        if (playerMana > PlayerMaxHealth)
                        {
                            playerMana = PlayerMaxMana;
                        }

                        Console.WriteLine($"Вы вылечились на {PlayerHealing}");
                        break;

                    default:
                        Console.WriteLine($"Неизвестная команда");
                        break;
                }

                var bossAttack = random.Next(BossMinAttack, BossMaxAttack + 1);
                playerHealth -= bossAttack;
                Console.WriteLine($"Босс ударил на {bossAttack} урона.\n");

                step++;
            }

            Console.Write($"Бой окончен!\nЗдоровье игрока: {playerHealth}, здоровье босса: {bossHealth}. ");
            if (playerHealth <= 0 && bossHealth <= 0)
            {
                Console.WriteLine("Ничья!");
            }
            else if (playerHealth <= 0)
            {
                Console.WriteLine("Вы проиграли!");
            }
            else if (bossHealth <= 0)
            {
                Console.WriteLine("Вы выиграли!");
            }
        }
    }
}