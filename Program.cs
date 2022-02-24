using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TelegramBot
{
    class Program
    {
        private static string token = "{Your token}";
        private static ITelegramBotClient botClient = new TelegramBotClient(token);

        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions { AllowedUpdates = { } };

            botClient.StartReceiving(
                BotHandler.HandleUpdateAsync,
                BotHandler.HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            Console.ReadLine();

            cts.Cancel();
        } 
    }
}

