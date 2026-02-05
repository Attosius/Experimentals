using System;


namespace Tgbot
{
    using Telegram.Bot;
    using Telegram.Bot.Polling;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    public class TelegramBotService
    {
        private readonly TelegramBotClient _botClient;
        private readonly CancellationTokenSource _cts;
        private User _botUser;

        public TelegramBotService(string token)
        {
            _botClient = new TelegramBotClient(token);
            _cts = new CancellationTokenSource();
        }

        public async Task StartBotAsync()
        {
            try
            {
                // Правильный способ получить информацию о боте в версии 22.8.1
                _botUser = await _botClient.GetMe();
                Console.WriteLine($"Бот запущен: @{_botUser.Username} (ID: {_botUser.Id})");

                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>(),
                    DropPendingUpdates = true
                };

                // Запуск получения обновлений
                _botClient.StartReceiving(
                    updateHandler: HandleUpdateAsync,
                    errorHandler: HandlePollingErrorAsync,
                    receiverOptions: receiverOptions,
                    cancellationToken: _cts.Token
                );

                Console.WriteLine("Бот начал работу. Нажмите Ctrl+C для остановки...");

                // Бесконечное ожидание
                await Task.Delay(Timeout.Infinite, _cts.Token);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Бот остановлен");
            }
        }

        private async Task HandleUpdateAsync(
            ITelegramBotClient botClient,
            Update update,
            CancellationToken cancellationToken)
        {
            // Обработка только сообщений
            if (update.Message is not { } message)
                return;

            var chatId = message.Chat.Id;
            var userId = message.From?.Id;

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Сообщение от {userId}: {message.Text}");

            // Обработка команд
            switch (message.Text?.ToLower())
            {
                case "/start":
                    await SendStartMessage(chatId, cancellationToken);
                    break;

                case "/me":
                    await SendBotInfo(chatId, cancellationToken);
                    break;

                case "/help":
                    await SendHelpMessage(chatId, cancellationToken);
                    break;

                case "/chatid":
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"ID этого чата: `{chatId}`",
                        parseMode: ParseMode.Markdown,
                        cancellationToken: cancellationToken
                    );
                    break;
            }
        }

        private async Task SendStartMessage(long chatId, CancellationToken cancellationToken)
        {
            var text = $"👋 Привет! Я бот @{_botUser.Username}\n\n" +
                      "Доступные команды:\n" +
                      "/me - информация о боте\n" +
                      "/chatid - узнать ID чата\n" +
                      "/help - помощь";

            await _botClient.SendMessage(
                chatId: chatId,
                text: text,
                cancellationToken: cancellationToken
            );
        }

        private async Task SendBotInfo(long chatId, CancellationToken cancellationToken)
        {
            var text = $"🤖 Информация о боте:\n" +
                      $"ID: {_botUser.Id}\n" +
                      $"Username: @{_botUser.Username}\n" +
                      $"Имя: {_botUser.FirstName}\n" +
                      $"Фамилия: {_botUser.LastName ?? "не указана"}\n" +
                      $"Поддерживает инлайн: {_botUser.CanJoinGroups}";

            await _botClient.SendMessage(
                chatId: chatId,
                text: text,
                cancellationToken: cancellationToken
            );
        }

        private async Task SendHelpMessage(long chatId, CancellationToken cancellationToken)
        {
            await _botClient.SendMessage(
                chatId: chatId,
                text: "ℹ️ Это тестовый бот на C# с использованием Telegram.Bot версии 22.8.1",
                cancellationToken: cancellationToken
            );
        }

        private Task HandlePollingErrorAsync(
            ITelegramBotClient botClient,
            Exception exception,
            CancellationToken cancellationToken)
        {
            Console.WriteLine($"Ошибка: {exception.Message}");
            return Task.CompletedTask;
        }

        public void StopBot()
        {
            _cts.Cancel();
        }
    }
}
