using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Tgbot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine("=== Telegram Bot 22.8.1 ===");

            // Введите токен бота
            Console.Write("Введите токен бота: ");
            var token = "8547332304:AAE8kswaaFdyeK5JTIRSqDzKsZZ8OVHR8i8";

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Токен не может быть пустым!");
                return;
            }

            var botService = new AdvancedTelegramBot(token);
            await botService.StartAsync();

            Console.ReadKey();

        }
        public class AdvancedTelegramBot
        {
            private readonly TelegramBotClient _bot;
            private User _botInfo;

            public AdvancedTelegramBot(string token)
            {
                _bot = new TelegramBotClient(token);
            }

            public async Task StartAsync()
            {
                try
                {
                    // Получаем информацию о боте
                    _botInfo = await _bot.GetMe();
                    Console.WriteLine($"🤖 Бот: {_botInfo.FirstName} (@{_botInfo.Username})");
                    Console.WriteLine($"🆔 ID бота: {_botInfo.Id}");
                    Console.WriteLine($"📚 Библиотека: Telegram.Bot 22.8.1");

                    // Настройки получения обновлений
                    var receiverOptions = new ReceiverOptions
                    {
                        AllowedUpdates = new[]
                        {
                            UpdateType.Message,
                            UpdateType.CallbackQuery,
                            UpdateType.MyChatMember
                        },
                        DropPendingUpdates = true,
                    };

                    using var cts = new CancellationTokenSource();

                    // Начинаем получать обновления
                    _bot.StartReceiving(
                        updateHandler: HandleUpdateAsync,
                        errorHandler: HandleErrorAsync,
                        receiverOptions: receiverOptions,
                        cancellationToken: cts.Token
                    );

                    Console.WriteLine("\n✅ Бот запущен и слушает сообщения...");
                    Console.WriteLine("Нажмите любую клавишу для остановки");

                    // Ждем нажатия клавиши для остановки
                    Console.ReadKey();
                    cts.Cancel();

                    Console.WriteLine("\n🛑 Бот остановлен");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Ошибка при запуске бота: {ex.Message}");
                }
            }

            private async Task HandleUpdateAsync(
                ITelegramBotClient botClient,
                Update update,
                CancellationToken ct)
            {
                try
                {
                    switch (update.Type)
                    {
                        case UpdateType.Message:
                            await HandleMessageAsync(update.Message, ct);
                            break;

                        case UpdateType.MyChatMember:
                            await HandleMyChatMemberAsync(update.MyChatMember, ct);
                            break;

                        case UpdateType.CallbackQuery:
                            await HandleCallbackQueryAsync(update.CallbackQuery, ct);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка обработки update: {ex.Message}");
                }
            }

            private async Task HandleMessageAsync(Message message, CancellationToken ct)
            {
                if (message == null) return;

                var chat = message.Chat;
                var user = message.From;

                Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] " +
                                 $"Чат: {chat.Id} ({chat.Type}), " +
                                 $"Пользователь: {user?.FirstName} ({user?.Id}), " +
                                 $"Текст: {message.Text?.Substring(0, Math.Min(50, message.Text.Length))}...");

                // Обработка текстовых сообщений
                if (message.Text != null)
                {
                    await HandleTextMessage(message, ct);
                    return;
                }

                // Обработка других типов сообщений
                if (message.Photo != null)
                {
                    await _bot.SendMessage(
                        chat.Id,
                        "📸 Спасибо за фото!",
                        cancellationToken: ct
                    );
                }
                else if (message.Sticker != null)
                {
                    await _bot.SendMessage(
                        chat.Id,
                        "😀 Классный стикер!",
                        cancellationToken: ct
                    );
                }
            }

            private async Task HandleTextMessage(Message message, CancellationToken ct)
            {
                var chatId = message.Chat.Id;
                var text = message.Text.ToLower();

                switch (text)
                {
                    case "/start":
                        await SendWelcomeMessage(chatId, message.From, ct);
                        break;

                    case "/info":
                        await SendBotInfo(chatId, ct);
                        break;

                    case "/ping":
                        await _bot.SendMessage(
                            chatId,
                            "🏓 Pong!",
                            cancellationToken: ct
                        );
                        break;

                    case "/keyboard":
                        await SendTestKeyboard(chatId, ct);
                        break;

                    default:
                        // Эхо-ответ
                        await _bot.SendMessage(
                            chatId,
                            $"Вы сказали: {message.Text}",

                            replyParameters: new ReplyParameters
                            {
                                MessageId = message.MessageId
                            },
                            cancellationToken: ct
                        );
                        break;
                }
            }

            private async Task SendWelcomeMessage(long chatId, User user, CancellationToken ct)
            {
                var welcomeText = $"👋 Привет, {user.FirstName}!\n\n" +
                                 $"Я бот {_botInfo.FirstName} (@{_botInfo.Username})\n" +
                                 $"Мой ID: {_botInfo.Id}\n\n" +
                                 "Доступные команды:\n" +
                                 "/info - информация о боте\n" +
                                 "/ping - проверка связи\n" +
                                 "/keyboard - тест клавиатуры\n\n" +
                                 $"Твой ID: {user.Id}";

                await _bot.SendMessage(
                    chatId,
                    welcomeText,
                    cancellationToken: ct
                );
            }

            private async Task SendBotInfo(long chatId, CancellationToken ct)
            {
                var info = $"🤖 *Информация о боте*\n\n" +
                          $"• Имя: {_botInfo.FirstName}\n" +
                          $"• Фамилия: {_botInfo.LastName ?? "—"}\n" +
                          $"• Username: @{_botInfo.Username}\n" +
                          $"• ID: `{_botInfo.Id}`\n" +
                          $"• Может читать групповые сообщения: {_botInfo.CanReadAllGroupMessages}\n" +
                          $"• Поддерживает инлайн: {_botInfo.SupportsInlineQueries}";

                await _bot.SendMessage(
                    chatId,
                    info,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
            }

            private async Task SendTestKeyboard(long chatId, CancellationToken ct)
            {
                var keyboard = new ReplyKeyboardMarkup(new[]
                {
                new[]
                {
                    new KeyboardButton("Кнопка 1"),
                    new KeyboardButton("Кнопка 2")
                },
                new[]
                {
                    new KeyboardButton("Отправить контакт") { RequestContact = true },
                    new KeyboardButton("Отправить местоположение") { RequestLocation = true }
                }
            })
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };

                await _bot.SendMessage(
                    chatId,
                    "Выберите действие:",
                    replyMarkup: keyboard,
                    cancellationToken: ct
                );
            }

            private async Task HandleMyChatMemberAsync(ChatMemberUpdated chatMember, CancellationToken ct)
            {
                Console.WriteLine($"Статус бота изменился: {chatMember.NewChatMember.Status}");

                if (chatMember.NewChatMember.Status == ChatMemberStatus.Kicked)
                {
                    Console.WriteLine($"Бот был заблокирован в чате {chatMember.Chat.Id}");
                }
                else if (chatMember.NewChatMember.Status == ChatMemberStatus.Member)
                {
                    Console.WriteLine($"Бот добавлен в чат {chatMember.Chat.Title}");
                }
            }

            private async Task HandleCallbackQueryAsync(CallbackQuery callback, CancellationToken ct)
            {
                //await _bot.AnswerCallbackQueryAsync(
                //    callback.Id,
                //    $"Вы нажали: {callback.Data}",
                //    cancellationToken: ct
                //);

                await _bot.SendMessage(
                    callback.Message.Chat.Id,
                    $"Callback data: {callback.Data}",
                    cancellationToken: ct
                );
            }

            private Task HandleErrorAsync(
                ITelegramBotClient botClient,
                Exception exception,
                CancellationToken ct)
            {
                Console.WriteLine($"❌ Ошибка: {exception.Message}");
                return Task.CompletedTask;
            }
        }
    }
}
