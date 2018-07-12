using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Text; // StringBuilder

public class SendMailHelper 
{
	static SmtpClient smtpClient = null;
	
	static readonly int SmtpClientTimeout = 5000;
	static readonly int EnclosureMBLimit = 10;
	
	public static void TestCode()
	{
		SendMailHelper.SetMailSender("smildnet@126.com", "111", "LihaojieT");
			
		List<string> toAddList = new List<string>();
		toAddList.Add("seaclientfeedback@gmail.com");
		SendMailHelper.SendMail("TestMailSubject", "TestMailContent", "smildnet@126.com", toAddList);
	}
	
	// QQ smtp is smtp.qq.com, and sina smtp is smtp.sina.cn
	// SMTP default port is 25
	static void SetSmtpClient(string host, int port)
	{
		if (null == smtpClient)
			smtpClient = new SmtpClient(host, port);
		
        smtpClient.Host = host;
        smtpClient.Port = port;
        smtpClient.Timeout = SmtpClientTimeout;
		
		smtpClient.EnableSsl = false;
		// if (host.Contains("smtp.gmail.com"))
		// 	smtpClient.EnableSsl = true;
		smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
		
		smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
	}
	
	public static void SetMailSender(string mailAddress, string mailPwd, string displayName)
	{
		MailAddress fromAddress = new MailAddress(mailAddress, displayName);
		
		string host = "";
		int port = 25;
		if (mailAddress.Contains("@qq.com"))
		{
			host = "smtp.qq.com";
			port = 25;
		}
		else if (mailAddress.Contains("@sina.cn"))
		{
			host = "smtp.sina.cn";
			port = 25;
		}
		else if (mailAddress.Contains("@163.com"))
		{
			host = "smtp.163.com";
			port = 25;
		}
		else if (mailAddress.Contains("@126.com"))
		{
			host = "smtp.126.com";
			port = 25;
		}
		else if (mailAddress.Contains("@gmail.com"))
		{
			host = "smtp.gmail.com";
			port = 25;
		}
		
		SetSmtpClient(host, port);
		
		// // Credentials
		// CredentialCache credentials = new CredentialCache();
		// credentials.Add(host, port, "login", new NetworkCredential(mailAddress, mailPwd));
		// smtpClient.Credentials = credentials;
		smtpClient.Credentials = (ICredentialsByHost)(new NetworkCredential(mailAddress, mailPwd));
	}
	
	public static void SendMail(string subject, string sendMsg, string fromAddress, List<string> toAddressList, 
		List<string> ccAddressList = null, List<string> attachmentFiles = null)
	{
		if (null == smtpClient)
		{
			Debug.LogWarning("");
			return;
		}
		
		MailMessage mailMsg = ComposeMailMsg(subject, sendMsg, fromAddress, toAddressList, ccAddressList, attachmentFiles);
		try 
		{
			smtpClient.SendAsync(mailMsg, mailMsg);
		} 
		catch (System.Exception ex) {
			Debug.LogWarning(ex.Message);
		}
	}
	
	static void SendCompletedCallback(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
	{
		if (e.Cancelled)
		{
			Debug.LogWarning("The mail send operation is cancelled!");
			return;
		}
		
		if (null != e.Error)
		{
			Debug.LogError(e.Error.Message);
			return;
		}
		
		MailMessage mailMsg = e.UserState as MailMessage;
		Debug.Log("Send mail success, " + mailMsg.From.Address);
	}
	
	static bool CheckMailAttachments(List<string> files)
	{
		if (null == files)
		{
			return false;
		}
		
		long totalBytesLength = 0;
		foreach (string file in files)
		{
			FileInfo fileInfo = new FileInfo(file);
			totalBytesLength += fileInfo.Length;
		}
		
		int mbsLength = (int)(totalBytesLength / 1024 / 1024);
		if (mbsLength > EnclosureMBLimit)
		{
			return false;
		}
		
		return true;
	}
	
	static MailMessage ComposeMailMsg(string subject, string sendMsg, string fromAddress, List<string> toAddressList, 
		List<string> ccAddressList = null, List<string> attachmentFiles = null)
	{
   		MailMessage mailMsg = new MailMessage(fromAddress, toAddressList[0]);
		
		// mailMsg.From = fromAddress;
		
		mailMsg.To.Clear();
		foreach (string info in toAddressList)
		{
			if (!info.Contains("@"))
			{
				Debug.LogWarning("Have a wrong mailbox address! " + info);
				continue;
			}
			
			MailAddress to = new MailAddress(info);
			mailMsg.To.Add(to);
		}
		
		mailMsg.CC.Clear();
		if (null != ccAddressList)
		{
			foreach (string info in ccAddressList)
			{
				if (!info.Contains("@"))
				{
					Debug.LogWarning("Have a wrong mailbox address! " + info);
					continue;
				}
				
				MailAddress cc = new MailAddress(info);
				mailMsg.CC.Add(cc);
			}
		}
		
		mailMsg.Subject = subject;
		mailMsg.SubjectEncoding = System.Text.Encoding.UTF8;
		
		mailMsg.Body = sendMsg;
		mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
		mailMsg.IsBodyHtml = true;
		
		mailMsg.Priority = MailPriority.Normal;
		
		mailMsg.Attachments.Clear();
		if (CheckMailAttachments(attachmentFiles))
		{
			foreach (string info in attachmentFiles)
			{
				Attachment attach = new Attachment(info.Trim(), System.Net.Mime.MediaTypeNames.Application.Octet);
				mailMsg.Attachments.Add(attach);
			}
		}
		
		return mailMsg;
	}
}