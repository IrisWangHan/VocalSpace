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

    // 寄送重設密碼連結
    public async Task<bool> SendPasswordResetEmailAsync(string recipientEmail, string resetLink)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("聲維宇宙", senderEmail));
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
