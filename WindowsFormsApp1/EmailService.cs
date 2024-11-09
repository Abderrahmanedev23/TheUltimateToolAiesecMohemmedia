using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

public static class EmailService
{
    private const string SMTP_HOST = "smtp.gmail.com";
    private const int SMTP_PORT = 587;
    private const string SENDER_EMAIL = "abderrahmane.mabchour@aiesec.net";
    private const string SENDER_PASSWORD = "hfzx ddwz cdbm kqsc"; // Replace with correct password or App Password

    public static bool SendEmail(string toEmail, string subject, string accessPassword)
    {
        if (string.IsNullOrEmpty(toEmail) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(accessPassword))
        {
            MessageBox.Show("All parameters must be provided.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        string body = GenerateEmailBody(accessPassword);

        using (MailMessage mailMessage = new MailMessage())
        using (SmtpClient smtpClient = new SmtpClient(SMTP_HOST, SMTP_PORT))
        {
            try
            {
                // Configure mail message
                mailMessage.From = new MailAddress(SENDER_EMAIL);
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                // Configure SMTP client
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(SENDER_EMAIL, SENDER_PASSWORD);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Timeout = 30000; // 30 seconds timeout

                // Send email
                smtpClient.Send(mailMessage);
               // MessageBox.Show("Email sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (SmtpFailedRecipientException ex)
            {
                MessageBox.Show($"Failed to deliver email to {ex.FailedRecipient}. Check the recipient's address.",
                                "Email Delivery Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (SmtpException ex)
            {
                MessageBox.Show($"SMTP Error: {ex.Message}\nStatus Code: {ex.StatusCode}",
                              "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }

    private static string GenerateEmailBody(string accessPassword)
    {
        return $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>AIESEC Mohammedia - Access Details</title>
            <style>
                body {{ font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f4f4f4; }}
                .container {{ max-width: 600px; margin: 20px auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }}
                .header {{ text-align: center; padding: 20px 0; background-color: #037ef3; color: white; border-radius: 8px 8px 0 0; }}
                .content {{ padding: 20px; text-align: center; }}
                .button {{ display: inline-block; padding: 12px 24px; background-color: #037ef3; color: white; text-decoration: none; border-radius: 4px; margin: 10px; font-weight: bold; transition: background-color 0.3s ease; }}
                .button:hover {{ background-color: #0262c9; }}
                .password-box {{ background-color: #f8f9fa; padding: 15px; border-radius: 4px; margin: 20px 0; border: 2px dashed #037ef3; }}
                .footer {{ text-align: center; padding: 20px; color: #666; font-size: 12px; border-top: 1px solid #eee; }}
                .logo-container {{ padding: 20px; text-align: center; }}
                .logo {{ max-width: 200px; height: auto; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <div class='logo-container'>
                        <img src='https://aiesec-logos.s3.eu-west-1.amazonaws.com/White-Blue-Logo.png' alt='AIESEC Logo' class='logo'>
                    </div>
                    <h1>Welcome to AIESEC Mohammedia</h1>
                </div>
                <div class='content'>
                    <h2>Your Access Details</h2>
                    <p>Hello AIESECer,</p>
                    <p>Here are your credentials for the Ultimate Tool:</p>
                    <div class='password-box'>
                        <h3>Your Password</h3>
                        <p style='font-size: 24px; font-weight: bold;'>{accessPassword}</p>
                    </div>
                    <p>Please keep these credentials safe and do not share them with anyone.</p>
                    <a href='#' class='button'>Access Platform</a>
                    <a href='#' class='button'>Need Help?</a>
                </div>
                <div class='footer'>
                    <p>AIESEC in Mohammedia</p>
                    <p>This is an automated message. Please do not reply to this email.</p>
                    <p>&copy; {DateTime.Now.Year} AIESEC. All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>";
    }
}
