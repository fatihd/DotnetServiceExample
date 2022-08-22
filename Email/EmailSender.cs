namespace Email
{
    using System;
    using System.Net.Mail;
    using System.ComponentModel;
    using System.Net;
    using Email.Contracts.Commands;

    public class EmailSender
    {
        private static readonly string host = "smtp.ethereal.email";
        private static readonly int port = 587;
        readonly SmtpClient client = new(host, port) { EnableSsl = true };

        static readonly MailAddress from = new(
            address: "annabel.littel86@ethereal.email",
            displayName: "Annabel Littel",
            displayNameEncoding: System.Text.Encoding.UTF8);

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            string? token = e.UserState as string;

            if (e.Cancelled)
                Console.WriteLine("[{0}] Send canceled.", token);

            if (e.Error != null)
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            else
                Console.WriteLine("Message sent.");
        }

        public void Send(SendEmail sendEmail)
        {
            client.Credentials = new NetworkCredential(userName: "annabel.littel86@ethereal.email", password: "7BVjNeugRtnuCxB9W2");

            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            MailMessage message = CreateMessage(sendEmail);

            client.SendAsync(message, userToken: "test message1");
        }

        private static MailMessage CreateMessage(SendEmail sendEmail)
        {
            return new()
            {
                From = from,
                To = { new MailAddress(sendEmail.To) },
                Body = sendEmail.Body,
                BodyEncoding = System.Text.Encoding.UTF8,
                Subject = sendEmail.Subject,
                SubjectEncoding = System.Text.Encoding.UTF8,
            };
        }
    }
}
