using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    class BotHandler
    {
        private static int operationCounter = 0;

        private static string tempName;
        private static double tempPrice;
        private static string tempCategoryName;

        private enum OperationCode
        {
            Starting,
            Naming,
            Pricing,
            Categorizing
        }

        public async static Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var type = update.Type switch
            {
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery),
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message),
            };

            try
            {
                await type;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
            
        }

        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private async static Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            Func<bool> checkNumber = () =>
            {
                try
                {
                    var msg = message.Text.TrimStart('/');
                    double.Parse(msg, CultureInfo.InvariantCulture);
                    return true;
                }
                catch
                {
                    return false;
                }
            };

            if (message.Text == "/start") await Start(botClient, message);

            switch (operationCounter)
            {
                case (int)OperationCode.Naming:
                    tempName = message.Text;
                    await BotAnswer(botClient, message, text: "Введите цену", "В начало");
                    operationCounter = (int)OperationCode.Pricing;
                    return;

                case (int)OperationCode.Pricing:
                    if (checkNumber() == false)
                        await BotAnswer(botClient, message, text: "Повторите ввод цены или начните сначала", "В начало");
                    tempPrice = double.Parse(message.Text, CultureInfo.InvariantCulture);
                    await BotAnswer(botClient, message, text: $"Введите название категории или выберите одну из существующих:\n{InfoHelper.ShowAllCategories()}", "В начало");
                    operationCounter = (int)OperationCode.Categorizing;
                    return;

                case (int)OperationCode.Categorizing:
                    if (checkNumber() == true)
                        tempCategoryName = InfoHelper.ChoiceExistingCategory(message.Text);
                    else tempCategoryName = message.Text;
                    PurchaseStorage.AddPurchase(new Purchase(tempName, tempPrice, tempCategoryName));
                    await BotAnswer(botClient, message, text: "Покупка добавлена");
                    await Start(botClient, message);
                    return;
            }
        }

        private async static Task BotAnswer(ITelegramBotClient botClient, Message message, string text = "", params string[] buttons)
        {
            static InlineKeyboardMarkup GetInlineButtons(params string[] buttons)
            {
                var buttonList = new List<InlineKeyboardButton>();
                foreach (var button in buttons)
                {
                    buttonList.Add(new InlineKeyboardButton(button) { CallbackData = button });
                }

                return new InlineKeyboardMarkup(buttonList);
            }

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: text == "" ? "Выберите действие" : text,
                replyMarkup: GetInlineButtons(buttons));
        }

        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id);

            var message = callbackQuery.Message;

            if (callbackQuery.Data == "Добавить покупку")
            {
                await BotAnswer(botClient, message, text: "Введите название покупки");
                operationCounter = (int)OperationCode.Naming;
            }

            if (callbackQuery.Data == "Показать все покупки")
            {
                await BotAnswer(botClient, message, text: InfoHelper.ShowAllPurchases(), "В начало", "Траты по категориям");
            }

            if (callbackQuery.Data == "В начало")
            {
                await Start(botClient, message);
            }

            if (callbackQuery.Data == "Траты по категориям")
            {
                await BotAnswer(botClient, message, text: InfoHelper.ShowSpendsByCategories(), "В начало");
            }
        }

        private static async Task Start(ITelegramBotClient botClient, Message message)
        {
            operationCounter = (int)OperationCode.Starting;
            tempName = "";
            tempPrice = 0;
            tempCategoryName = "";
            
            await BotAnswer(botClient, message, text: "Выберите одно из действий", "Добавить покупку", "Показать все покупки");
        }


    }
}
