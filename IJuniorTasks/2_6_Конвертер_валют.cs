using System;

namespace IJuniorTasks
{
	internal class Program
	{
		static void Main(string[] args)
		{
            const string CommandRubToUsd = "1";
            const string CommandUsdToRub = "2";
            const string CommandRubToEur = "3";
            const string CommandEurToRub = "4";
            const string CommandUsdToEur = "5";
            const string CommandEurToUsd = "6";
            const string CommandExit = "7";

			var balanceRub = 100f;
			var balanceUsd = 10f;
			var balanceEur = 5f;

			var convertRubToUsd = 90f;
			var convertUsdToRub = 0.013f;

			var convertRubToEur = 110f;
			var convertEurToRub = 0.009f;

			var convertUsdToEur = 0.9f;
			var convertEurToUsd = 1.1f;

			var isContinue = true;

			while (isContinue)
			{
				Console.WriteLine($"Ваш баланс:\n" +
								  $"Rub: {balanceRub}\n" +
								  $"Usd: {balanceUsd}\n" +
								  $"Eur: {balanceEur}\n" +
								  $"Введите команду:");
				Console.WriteLine($"{CommandRubToUsd}. Перевести Rub в Usd");
				Console.WriteLine($"{CommandUsdToRub}. Перевести Usd в Rub");

				Console.WriteLine($"{CommandRubToEur}. Перевести Rub в Eur");
				Console.WriteLine($"{CommandEurToRub}. Перевести Eur в Rub");

				Console.WriteLine($"{CommandUsdToEur}. Перевести Usd в Eur");
				Console.WriteLine($"{CommandEurToUsd}. Перевести Eur в Usd");

				Console.WriteLine($"{CommandExit}. Выход");

				var command = Console.ReadLine();
				Console.WriteLine("Введите количество денег для перевода: ");
				var moneyToConvert = Convert.ToInt32(Console.ReadLine());

				switch (command)
				{
					case CommandRubToUsd:
						if (balanceRub < moneyToConvert)
						{
							Console.WriteLine("Недостаточно денег для перевода: ");
							break;
						}

						var usdFromRubToConvert = moneyToConvert / convertRubToUsd;
						balanceRub -= moneyToConvert;
						balanceUsd += usdFromRubToConvert;
						Console.WriteLine($"Вы обменяли {moneyToConvert} RUB на {usdFromRubToConvert} USD ");
						break;

					case CommandUsdToRub:
						if (balanceUsd < moneyToConvert)
						{
							Console.WriteLine("Недостаточно денег для перевода: ");
							break;
						}

						var rubFromUsdToConvert = moneyToConvert / convertUsdToRub;
						balanceUsd -= moneyToConvert;
						balanceRub += rubFromUsdToConvert;
						Console.WriteLine($"Вы обменяли {moneyToConvert} USD на {rubFromUsdToConvert} RUB ");
						break;

					case CommandRubToEur:
						if (balanceRub < moneyToConvert)
						{
							Console.WriteLine("Недостаточно денег для перевода: ");
							break;
						}

						var eurFromRubToConvert = moneyToConvert / convertRubToEur;
						balanceRub -= moneyToConvert;
						balanceEur += eurFromRubToConvert;
						Console.WriteLine($"Вы обменяли {moneyToConvert} RUB на {eurFromRubToConvert} EUR ");
						break;

					case CommandEurToRub:
						if (balanceEur < moneyToConvert)
						{
							Console.WriteLine("Недостаточно денег для перевода: ");
							break;
						}

						var rubFromEurToConvert = moneyToConvert / convertEurToRub;
						balanceEur -= moneyToConvert;
						balanceRub += rubFromEurToConvert;
						Console.WriteLine($"Вы обменяли {moneyToConvert} EUR на {rubFromEurToConvert} RUB ");
						break;

                    case CommandUsdToEur:
						if (balanceUsd < moneyToConvert)
						{
							Console.WriteLine("Недостаточно денег для перевода: ");
							break;
						}

						var eurFromUsdToConvert = moneyToConvert / convertUsdToEur;
						balanceUsd -= moneyToConvert;
						balanceEur += eurFromUsdToConvert;
						Console.WriteLine($"Вы обменяли {moneyToConvert} USD на {eurFromUsdToConvert} EUR ");
						break;

					case CommandEurToUsd:
						if (balanceEur < moneyToConvert)
						{
							Console.WriteLine("Недостаточно денег для перевода: ");
							break;
						}

						var usdFromEurToConvert = moneyToConvert / convertEurToUsd;
						balanceEur -= moneyToConvert;
						balanceUsd += usdFromEurToConvert;
						Console.WriteLine($"Вы обменяли {moneyToConvert} EUR на {usdFromEurToConvert} RUB ");
						break;

					case CommandExit:
						isContinue = false;
						break;

					default:
						Console.WriteLine($"Неизвестная команда");
						break;
				}
				Console.WriteLine("Приходите еще!\n");
			}
		}
	}
}