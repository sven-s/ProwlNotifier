using System;
using System.Collections.Generic;

using Mono.Options;

using Prowl;

namespace ProwlNotifier
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var show_help = false;
            var apikey = "";
            var applicationName = "Default";
            var eventName = "Default";
            var show_timestamp = false;

            var p = new OptionSet
                    {
                        {
                            "a|apikey=", "the {APIKEY} for Prowl.", v =>
                            {
                                if (v == null)
                                {
                                    throw new OptionException("Missing api key for option -a", "-a");
                                }
                                apikey = v;
                            }
                        },
                        { "n|name=", "the {NAME} of the application that sends the notification.", v => applicationName = v },
                        { "e|event=", "the {EVENT} of the notification.", v => eventName = (v ?? "Default") },
                        { "d|datetime=", "shows a timestamp with the message.", v => show_timestamp = v != null },
                        { "h|help", "show this message and exit", v => show_help = v != null },
                    };

            List<string> extra;
            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("prowlNotifier: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try 'prowlNotifier --help' for more information.");
                return;
            }

            if (show_help || string.IsNullOrEmpty(apikey))
            {
                ShowHelp(p);
                return;
            }

            // Create and send the notification.
            var message = string.Join(" ", extra.ToArray());
            if (string.IsNullOrEmpty(message))
            {
                message = "Test";
            }

            if (show_timestamp) message = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " + message;

            var notification = new ProwlNotification { Description = message, Event = eventName, Priority = ProwlNotificationPriority.Normal };

            var client = new ProwlClient(new ProwlClientConfiguration { ApiKeychain = apikey, ApplicationName = applicationName });
            client.PostNotification(notification);
        }

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: prowlNotifier [OPTIONS] + message");
            Console.WriteLine("Send a notfication via Prowl");
            Console.WriteLine("If no message is specified, 'Test' is used.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}