using GalaSoft.MvvmLight.Messaging;
using System;

namespace MembershipDemoMVVM
{
    public static class Debug
    {
        public static bool debugOnFlag;

        public static void Write(params string[] msg)
        {
            if (debugOnFlag)
            {
                foreach (string var in msg)
                    Console.Write($"{var} ");
                Console.WriteLine();
            }
        }

        public static void Popup(string msg)
        {
            if (debugOnFlag)
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(msg));
        }
    }
}
