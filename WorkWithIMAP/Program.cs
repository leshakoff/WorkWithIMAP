using EAGetMail;
using System;

namespace GetGmailMes
{
    /// <summary>
    /// Простое консольное приложение, 
    /// которое отметит все письма в твоём заброшенном 
    /// гмэйловском почтовом ящике прочитанными ( ͡° ͜ʖ ͡°)
    /// </summary>
    class Program
    {
        // дока для EAGetMail - (https://www.emailarchitect.net/eagetmail)


        // линки для того, чтобы читающему код было легче жить (или не легче, но интереснее):
        //  (https://ru.wikipedia.org/wiki/IMAP), 
        //  (https://ru.wikipedia.org/wiki/SMTP)

        static void Main(string[] args)
        {
            // у gmail-а сервер входящей почты это - imap.gmail.com, работающий на порту 993 
            // (https://support.google.com/mail/answer/7126229?hl=ru)
            // поэтому в конструктор мы передаём сервер ^ , адрес своей электронной почты и пароль.
            // в качестве пароля я использовала пароль приложения (https://support.google.com/accounts/answer/185833?hl=ru)

            MailServer oServer = new MailServer("imap.gmail.com", "youraddress@gmail.com", "yourpassword", ServerProtocol.Imap4);
            oServer.SSLConnection = true;
            oServer.Port = 993;


            // в конструктор клиента кладём строку с лицензионным ключом
            // (https://www.emailarchitect.net/eagetmail/sdk/html/mailclient_constructor.htm)
            MailClient oClient = new MailClient("TryIt");

            // указываем, что нам нужны только непрочитанные письма
            oClient.GetMailInfosParam.GetMailInfosOptions = GetMailInfosOptionType.NewOnly;
            // и коннектимся
            oClient.Connect(oServer);

            // создаём массив с информацией о письмах
            MailInfo[] infos = oClient.GetMailInfos();

            // выводим на консоль кол-во непрочитанных
            Console.WriteLine("Total {0} email(s)\r\n", infos.Length);

            // и проходимся циклом по массиву
            for (int i = 0; i < infos.Length; i++)
            {
                MailInfo info = infos[i];

                // получаем письмо с IMAP-сервера
                Mail oMail = oClient.GetMail(info);
                Console.WriteLine(" _ __ ________________ MAIL ________________  __ _ \r\n" +
                    $"From: {oMail.From.ToString()};\r\n" +
                    $"Subject: {oMail.Subject};\r\n");

                // здесь и происходит магия, в MarkAsRead. 
                oClient.MarkAsRead(info, true);
                //oClient.Delete(info);

            }
        }
    }
}
