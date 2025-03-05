using System;
using System.Net.Mail;
using System.Net;
using UAManagedCore;
using FTOptix.NetLogic;
using FTOptix.Alarm;

public class EmailSender : BaseNetLogic
{
    [ExportMethod]
    public void SendEmail(string replyToAddress, string mailSubject, string mailBody)
    {
        if (string.IsNullOrEmpty(replyToAddress) || mailSubject == null || mailBody == null)
        {
            Log.Error("EmailSender", "Invalid values for one or more parameters.");
            return;
        }

        var fromAddress = new MailAddress("svpwinery@gmail.com", "SVP System Alarm"); // Email Sender
// Commented line below to avoid this variable
        //var toAddress = new MailAddress("7864912979@tmomail.net", "Miguel"); Email Receiver
        // Password for SMTP server authentication if necessary
        const string fromPassword = "ubgd hftu lver vjgy";

        var smtpClient = new SmtpClient
        {
            // Fill the following lines with your SMTP server info
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true, // Set to true if the server requires SSL.
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };

        var message = new MailMessage()
        {
            // Create the message.
            Subject = mailSubject,
            Body = mailBody
        };

        // Specify the sender
        message.From = fromAddress;

        // Recipient emails
        // The MailMessage.To property is a collection of emails, so you can add different recipients using:
        // message.To.Add(new MailAddress(...));
        // message.To.Add(toAddress);
//Add this line in order to directionate the email received trough the optix app
	message.To.Add(replyToAddress);

        // Add reply-to address
        message.ReplyToList.Add(replyToAddress);

        try
        {
            // Send email message
            smtpClient.Send(message);
            Log.Info("Message " + mailSubject + " sent successfully.");
        }
        catch (Exception ex)
        {
            // Insert here actions to be performed in case of an error when sending an email
            Log.Error("Error while sending message " + mailSubject + ", please try again. " + ex.Message);
        }
    }
}

