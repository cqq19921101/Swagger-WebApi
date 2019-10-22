using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Liteon.Mes.Utility
{
    /// <summary>
    /// 发送邮件类
    /// </summary>
    public class Mail
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sender">发送人</param>
        /// <param name="smtpServer">发送SMTP服务器</param>
        /// <param name="account">发送账号</param>
        /// <param name="password">发送密码</param>
        /// <param name="receiver">接收人</param>
        /// <param name="cc">抄送，没有填null</param>
        /// <param name="subject">主题</param>
        /// <param name="body">正文</param>
        /// <param name="attr">附件數組</param>
        public static void SendMail(string sender, string smtpServer, string account,string password, string receiver,string cc, string subject, string body, string[] attr)
        {
            MailAddressCollection MailList;
            MailList = CheckMailAddress(receiver);

            MailAddressCollection MailCCList = null;
            if (cc != null)
            {
                MailCCList = CheckMailAddress(cc);
            }

            if (MailList.Count <= 0) { return; };

            try
            {
                string MailFrom = sender;
                string MailSMTP = smtpServer;
                //string MailCC = "";
                string[] MailAttach ;
                if (attr == null)
                {
                    MailAttach = null;
                }
                else
                {
                    MailAttach = attr;
                }
                MailMessage mailObj = new MailMessage();
                mailObj.From = new MailAddress(MailFrom);
                foreach (MailAddress m in MailList)
                {
                    mailObj.To.Add(m);
                }
                if (MailCCList != null)
                {
                    foreach (MailAddress m in MailCCList)
                    {
                        mailObj.CC.Add(m);
                    }
                }
                mailObj.Subject = subject;
                mailObj.Body = body;
                mailObj.IsBodyHtml = true;
                //mailObj.CC.Add(MailCC);


                //if (!String.IsNullOrEmpty(MailAttach))
                //{
                //    Attachment attchfile = new Attachment(MailAttach);
                //    mailObj.Attachments.Add(attchfile);
                //}

                if (MailAttach!=null && MailAttach.Length > 0)
                {
                    foreach (string oneAttr in MailAttach)
                    {
                        if (!String.IsNullOrEmpty(oneAttr))
                        {
                            Attachment attchfile = new Attachment(oneAttr);
                            mailObj.Attachments.Add(attchfile);
                        }
                    }
                }

                SmtpClient SMTPServer = new SmtpClient(MailSMTP);
                if (String.IsNullOrEmpty(account))
                {
                    SMTPServer.UseDefaultCredentials = true;

                }
                else
                {
                    SMTPServer.Credentials = new System.Net.NetworkCredential(account, password);
                }
                SMTPServer.Send(mailObj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static MailAddressCollection CheckMailAddress(string strMail)
        {
            MailAddressCollection MailCollection = new MailAddressCollection();
            string errMail = "";
            try
            {
                foreach (string s in strMail.Split(';'))
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (IsMail(s))
                        {
                            MailCollection.Add(new MailAddress(s));
                        }
                        else
                        {
                            errMail += s + ";";
                        }
                    }
                }
                return MailCollection;
            }
            catch (Exception e)
            {
                errMail += e.Message;
                return MailCollection;
            }
            finally
            {
                //if (errMail != "")
                //{
                //    Param.ErrorLog(errMail, "CheckMailAddress");
                //}
            }

        }



        private static bool IsMail(string strMail)
        {
            Regex re = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.None);
            MatchCollection mc = re.Matches(strMail);
            if (mc.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
