using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;
using VocalSpace.Models;

public class EmailService
{
    private readonly EmailSettings _emailSettings;
    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task<bool> SendVerificationCodeAsync(string recipientEmail, string verificationCode)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("聲維宇宙", _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = "VocalSpace - 註冊驗證碼";

            message.Body = new TextPart("plain")
            {
                Text = $"您的驗證碼是: {verificationCode}，請在 5 分鐘內輸入。"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
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

    // 寄送重設密碼連結
    public async Task<bool> SendPasswordResetEmailAsync(string recipientEmail, string resetLink)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("聲維宇宙", _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = "密碼重設通知";

            message.Body = new TextPart("html")
            {
                Text = $@"
                    <p>親愛的用戶，</p>
                    <p>我們收到您重設密碼的請求。</p>
                    <p>請點擊以下連結來重設您的密碼：</p>
                    <p><a href='{resetLink}' style='color: blue;'>重設密碼</a></p>
                    <p>如果您沒有請求重設密碼，請忽略此郵件。</p>
                    <p>此連結將在 3 分鐘後失效。</p>
                    <br>
                    <p>VocalSpace 團隊敬上</p>
                "            
            };
            Console.WriteLine("收件人: "+recipientEmail);
            Console.WriteLine("reset連結: "+resetLink);

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
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
    public async Task<bool> SendNotificationAsync(string recipientEmail, string subject, string emailContent)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("聲維宇宙", _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = "VocalSpace - "+subject;

            message.Body = new TextPart("plain")
            {
                Text = $"{emailContent}"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
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
    //  寄送更改信箱連結
    public async Task<bool> SendChangeEmailAsync(string NewEmail, string resetLink)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("聲維宇宙", _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("", NewEmail));
            message.Subject = "更改信箱通知";
            message.Body = new TextPart("html")
            {
                Text = $@"
                    <p>親愛的用戶，</p>
                    <p>我們收到您更改信箱的請求。</p>
                    <p>請點擊以下連結來更改您的信箱：</p>
                    <p><a href='{resetLink}' style='color: blue;'>更改信箱</a></p>
                    <p>如果您沒有請求更改信箱，請忽略此郵件。</p>
                    <p>此連結將在 3 分鐘後失效。</p>
                    <br>
                    <p>VocalSpace 團隊敬上</p>
                "
            };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"更改信箱寄信失敗: {ex.Message}");
            return false;
        }
    }
}
