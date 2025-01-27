﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Configuration;
using System.Web;
using System.Net;
using System.Net.Mail;
using Model;

namespace BusinessLogic
{
    public class EmailController
    {
        StationeryInventory_Team_05Entities ctx = new StationeryInventory_Team_05Entities();

        /// <summary>
        /// Send Email to Dept Head when emp submit new request form
        /// </summary>
        /// <param name="empid">Employee ID</param>
        public void SendMailToEmpHead(int empid)
        {
            Employee emp = ctx.Employee.Where(x => x.EmpID == empid).FirstOrDefault();

            try
            {
                SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
                var mail = new MailMessage();
                mail.From = new MailAddress("logicuniversity.team5@hotmail.com");
                mail.To.Add("logicuniversity.depthead@hotmail.com");
                mail.Subject = string.Format("A new Inventory Request by {0} has been raised.", emp.EmpName);
                mail.IsBodyHtml = true;
                string htmlBody;
                htmlBody = string.Format("Please process the pending requisition forms");
                mail.Body = htmlBody;
                SmtpServer.Port = 25;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("logicuniversity.team5@hotmail.com", "logicuniversity123");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (SmtpException ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Reply from Dept Head to Emp that Approve or Reject
        /// </summary>
        /// <param name="empid">Employee ID</param>
        /// <param name="status">Approve/Reject</param>
        /// <param name="reqid">Requistion ID</param>
        public void SendMailToEmp(int empid, string status, int reqid)
        {
            Employee emp = ctx.Employee.Where(x => x.EmpID == empid).FirstOrDefault();
            string recipentEmail = emp.Email;

            try
            {
                SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
                var mail = new MailMessage();
                mail.From = new MailAddress("logicuniversity.team5@hotmail.com");
                mail.To.Add("logicuniversity.employee@hotmail.com");
                mail.Subject = string.Format("Your requesition {0} is {1} .", reqid, status);
                mail.IsBodyHtml = true;
                string htmlBody;
                htmlBody = string.Format("Hi {0}, your requesition form has been {1} .", emp.EmpName, status);
                mail.Body = htmlBody;
                SmtpServer.Port = 25;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("logicuniversity.team5@hotmail.com", "logicuniversity123");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (SmtpException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
