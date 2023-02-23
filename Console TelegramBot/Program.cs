using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bots.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace TelegramBotExperiments
{

    class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient("5769981946:AAF9xvo5VH2Cttpm2qU2ScatyDt3mm1uiBI");
        
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            // Некоторые действия
            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {

                var message = update.Message;
                var use = message?.Chat.Username ?? message?.Chat.FirstName;
                Console.WriteLine(use, "|",message.Text);
                if (message.Text != null)
                {
                    if (message.Text.ToLower() == "/game")
                    {
                        
                        var st = await botClient.SendDiceAsync(message.Chat.Id);
                        int sd = st.Dice.Value;

                        if (sd == 1)
                        {
                        await botClient.SendTextMessageAsync(message.Chat, "О поздравляю, ты выйграл!!!");

                        }
                        //await botClient.SendTextMessageAsync(message.Chat, $"d");
                       

                    }
                    if (message.Text.ToLower() == "/save")
                    {
                        string[] lisp = { CountryCodeToFlag("ac") , CountryCodeToFlag("af")};
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"{lisp[0]+lisp[1]}" );
                      
                        return;
                    }
                    if (message.Text.ToLower() == "/start")
                    {

                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Добро пожаловать на борт, добрый  {message.Chat.FirstName}!", replyToMessageId: message.MessageId);
                        return;
                    }
                }
            }
            }
        static string CountryCodeToFlag(string country)
        {
         return string.Concat(country.ToUpper().Select(x => char.ConvertFromUtf32(x + 0x1F1A5)));
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
                ThrowPendingUpdates = true

            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            ) ;
            Console.ReadLine();
        }
    }
}