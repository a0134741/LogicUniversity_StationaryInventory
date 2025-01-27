﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AnalyticsAPI" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AnalyticsAPI.svc or AnalyticsAPI.svc.cs at the Solution Explorer and start debugging.
    public class AnalyticsAPI : IAnalyticsAPI
    {
        public List<Report> getReports()
        {
            BusinessLogic.AnalyticsController BL = new BusinessLogic.AnalyticsController();
            return BL.getReports();
        }
        public bool updateReport(Report rp)
        {
            BusinessLogic.AnalyticsController BL = new BusinessLogic.AnalyticsController();
            return BL.updateReport(rp);
        }
        public string generateNewReport(string EmpID, string Title, string StartD, string EndD, string Remark, string Type, string Criteria, string Precriteria)
        {
            BusinessLogic.AnalyticsController BL = new BusinessLogic.AnalyticsController();
            return BL.generateNewReport(EmpID, Title, StartD, EndD, Remark, Type, Criteria, Precriteria);
        }
        public List<ReportResult> generateExistingReport(string reportID)
        {
            BusinessLogic.AnalyticsController BL = new BusinessLogic.AnalyticsController();
            return BL.generateExistingReport(reportID);
        }

        public List<ReportResult> generateExistingReportStyle2(string reportID)
        {
            BusinessLogic.AnalyticsController BL = new BusinessLogic.AnalyticsController();
            return BL.generateExistingReportStyle2(reportID);
        }

        public List<ReportResultWeb> generateExistingReportStyle3(string reportID, string type)
        {
            BusinessLogic.AnalyticsController BL = new BusinessLogic.AnalyticsController();
            return BL.generateExistingReportStyle3(reportID, type);
        }

        public Report getReport(string reportID)
        {
            BusinessLogic.AnalyticsController BL = new BusinessLogic.AnalyticsController();
            return BL.getReport(reportID);
        }
    }
}
