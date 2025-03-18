using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string smtpServer = "smtp.gmail.com";
    private readonly int smtpPort = 587; // Gmail 使用 587 (TLS) 或 465 (SSL)
    private readonly string senderEmail = "su3cl32000@gmail.com"; // 你的 Gmail
    private readonly string senderPassword = "uhknxryminnvaibf"; // Gmail 應用程式密碼

    public async Task<bool> SendVerificationCodeAsync(string recipientEmail, string verificationCode)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("聲維宇宙", senderEmail));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = "VocalSpace - 註冊驗證碼";

            message.Body = new TextPart("plain")
            {
                Text = $"您的驗證碼是: {verificationCode}，請在 5 分鐘內輸入。"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"寄信失敗: {ex.Message}");
            return false;
        }
    }

    // 待fix測試
    public async Task<bool> SendPasswordResetEmailAsync(string recipientEmail, string resetLink)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("聲維宇宙", senderEmail));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = "密碼重設通知";

            message.Body = new TextPart("plain")
            {
                Text = $"請點擊以下連結重設密碼: {resetLink}"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"寄信失敗: {ex.Message}");
            return false;
        }
    }
}
