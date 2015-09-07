﻿ using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BusinessLogic
{
    class RetrievalController
    {
        StationeryInventory_Team_05Entities ctx = new StationeryInventory_Team_05Entities();

        /// <summary>
        /// CreateRetrieval
        /// </summary>
        /// <param name="processRetList">ProcessRetrieval List (EmpID, ReqID)</param>
        /// <returns></returns>
        public int createRetrieval(List<ProcessRetrieval> processRetList)
        {
            int result = 0;
            
            //create and add new retrieval 
            Retrieval ret = new Retrieval();
            ret.Date = DateTime.Now;
            ret.EmpID = processRetList.First().EmpID;
            ret.Status = "PENDING";
            ctx.Retrieval.Add(ret);

            //obtain retID of newly added retrieval
            int RetID = ctx.Retrieval.Last().RetID;

            //hashmap-like to store itemID and collated qty
            Dictionary<string, int> itemQty = new Dictionary<string, int>();

            foreach (ProcessRetrieval processRet in processRetList)
            {
                //update RetID of requisition
                ctx.Requisition.Where(x => x.ReqID == processRet.ReqID).First().RetID = RetID;

                //obtain requisition detail list 
                List<RequisitionDetail> reqDetailList = ctx.RequisitionDetail.Where(x => x.ReqID == processRet.ReqID).ToList();
                
                foreach (RequisitionDetail reqDetail in reqDetailList)
                {
                    //if itemQty does not contain the item, add item to itemQty
                    if (!itemQty.ContainsKey(reqDetail.ItemID))
                    {
                        itemQty.Add(reqDetail.ItemID, (int)reqDetail.RequestQty);
                    }
                    //else if itemQty contains item, add the qty to existing qty
                    else
                    {
                        itemQty["reqDetail.ItemID"] += (int) reqDetail.RequestQty;
                    }
                }
            }

            //extract all keys and values in itemQty
            string[] itemQtyKeys = itemQty.Keys.ToArray();
            int[] itemQtyValues = itemQty.Values.ToArray();

            for (int i = 0; i < itemQty.Count; i++)
            {
                //create and add new retrieval detail
                RetrievalDetail retrievalDetail = new RetrievalDetail();
                retrievalDetail.RetID = RetID;
                retrievalDetail.ItemID = itemQtyKeys[i];
                retrievalDetail.RequestQty = itemQtyValues[i];
                ctx.RetrievalDetail.Add(retrievalDetail);
            }

            int count = ctx.SaveChanges();

            if (count > 0)
                result = RetID;

            return result;
        }

        /// <summary>
        /// GetList
        /// </summary>
        /// <param name="EmpID">Store Clerk EmpID</param>
        /// <param name="Status">Status (Pending / Retrieved)</param>
        /// <param name="RetID">Retrieval ID</param>
        /// <returns></returns>
        public List<Retrieval> getRetrieval(string EmpID, string Status, string RetID)
        {
            //start with all the records
            var query = from ret in ctx.Retrieval select ret;

            //filter the result set based on user inputs
            if (!string.IsNullOrEmpty(EmpID))
            {
                query = query.Where(x => x.EmpID.ToString().Contains(EmpID));
            }
            if (!string.IsNullOrEmpty(Status))
            {
                query = query.Where(x => x.Status.Contains(Status));
            }
            if (!string.IsNullOrEmpty(RetID))
            {
                query = query.Where(x => x.RetID.ToString().Contains(RetID));
            }

            //run the query on database and grab the results
            return query.ToList();
        }

        /// <summary>
        /// Submit
        /// </summary>
        /// <param name="retDetailList">RetrievalDetail List (RetID, ItemID, ActualQty)</param>
        /// <returns></returns>
        public bool submit(List<RetrievalDetail> retDetailList)
        {
            bool result = false;
            
            foreach (RetrievalDetail retDetail in retDetailList)
            {
                //update actual quantity of retrieval detail
                RetrievalDetail retDetailSearch = ctx.RetrievalDetail.Where(x => x.RetID == retDetail.RetID && x.ItemID == retDetail.ItemID).FirstOrDefault();
                retDetailSearch.ActualQty = retDetail.ActualQty;
            }

            //update status of retrieval to "Retrieved"
            Retrieval ret = ctx.Retrieval.Where(x => x.RetID == retDetailList.First().RetID).FirstOrDefault();
            ret.Status = "RETRIEVED";

            int count = ctx.SaveChanges();

            if (count > 0)
                result = true;

            return result;
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="retDetailList">RetrievalDetail List (RetID, ItemID, ActualQty)</param>
        /// <returns></returns>
        public bool save(List<RetrievalDetail> retDetailList)
        {
            bool result = false;

            //update actual quantity of retrieval detail
            foreach (RetrievalDetail retDetail in retDetailList)
            {
                RetrievalDetail retDetailSearch = ctx.RetrievalDetail.Where(x => x.RetID == retDetail.RetID && x.ItemID == retDetail.ItemID).FirstOrDefault();
                retDetailSearch.ActualQty = retDetail.ActualQty;
            }
            
            int count = ctx.SaveChanges();

            if (count > 0)
                result = true;

            return result;
        }

        /// <summary>
        /// GetRetrievalDetail
        /// </summary>
        /// <param name="RetID">Retrieval ID</param>
        /// <returns></returns>
        public List<ProcessRetSuccess> getRetrievalDetail(int RetID)
        {
            Retrieval ret = ctx.Retrieval.Where(x => x.RetID == RetID).FirstOrDefault();
            List<RetrievalDetail> retDetailList = ctx.RetrievalDetail.Where(x => x.RetID == RetID).ToList();

            List<ProcessRetSuccess> retSuccessList = new List<ProcessRetSuccess>();

            foreach(RetrievalDetail retDetail in retDetailList)
            {
                Item i = ctx.Item.Where(x => x.ItemID == retDetail.ItemID).FirstOrDefault();
                ProcessRetSuccess retSuccess = new ProcessRetSuccess();
                retSuccess.Date = (DateTime) ret.Date;
                retSuccess.Bin = i.Bin;
                retSuccess.ItemID = i.ItemID;
                retSuccess.ItemName = i.ItemName;
                retSuccess.TotalQty = (int) retDetail.RequestQty;
                retSuccess.ActualQty = (int) retDetail.ActualQty;

                retSuccessList.Add(retSuccess);
            }

            return retSuccessList;
        }

        /// <summary>
        /// GetReqAllocation
        /// </summary>
        /// <param name="RetID">Requisition ID</param>
        /// <returns></returns>
        public List<ReqAllocation> getReqAllocation(int RetID)
        {
            List<Requisition> reqList = ctx.Requisition.Where(x => x.RetID == RetID).ToList();

            List<ReqAllocation> reqAllocationList = new List<ReqAllocation>();

            foreach(Requisition req in reqList)
            {
                List<RequisitionDetail> reqDetailList = ctx.RequisitionDetail.Where(x => x.ReqID == req.ReqID).ToList();

                foreach (RequisitionDetail reqDetail in reqDetailList)
                {
                    ReqAllocation reqAllocation = new ReqAllocation();
                    reqAllocation.ItemID = reqDetail.ItemID;
                    reqAllocation.ReqID = req.ReqID;
                    reqAllocation.Dept = req.DeptID;
                    if(req.PriorityID == 1)
                        reqAllocation.Priority = "HIGH";
                    else
                        reqAllocation.Priority = "LOW";
                    reqAllocation.RequestQty = (int) reqDetail.RequestQty;
                    reqAllocation.IssueQty = (int)reqDetail.IssueQty;

                    reqAllocationList.Add(reqAllocation);
                }
            }
            return reqAllocationList;
        }


        /// <summary>
        /// confirmAllocation
        /// </summary>
        /// <param name="reqDetailList">RequisitionDetail List (ReqID, ItemID, IssueQty)</param>
        /// <returns></returns>
        public bool confirmAllocation(List<RequisitionDetail> reqDetailList)
        {
            bool result = false;
            
            foreach(RequisitionDetail reqDetail in reqDetailList)
            {
                //update requisition detail issue quantity
                RequisitionDetail reqDetailSearch = ctx.RequisitionDetail
                    .Where(x => x.ReqID == reqDetail.ReqID && x.ItemID == reqDetail.ItemID)
                    .FirstOrDefault();

                reqDetailSearch.IssueQty = reqDetail.IssueQty;

                //search for requisition to obtain the DeptID
                Requisition req = ctx.Requisition.Where(x => x.ReqID == reqDetail.ReqID).FirstOrDefault();
                //search for department name
                string deptName = ctx.Department.Where(x => x.DeptID == req.DeptID).FirstOrDefault().DeptName;

                //update stock card
                List<StockCard> stockCardList = ctx.StockCard.Where(x => x.ItemID == reqDetail.ItemID).ToList();
                int balance = 0;
                if (stockCardList.FirstOrDefault() != null)
                    balance = (int)stockCardList.Last().Balance;

                StockCard stockCard = new StockCard();
                stockCard.ItemID = reqDetail.ItemID;
                stockCard.Date = DateTime.Now;
                stockCard.Description = deptName;
                stockCard.Qty = -reqDetail.IssueQty;
                stockCard.Balance = balance - reqDetail.IssueQty;
                ctx.StockCard.Add(stockCard);
            }

            int count = ctx.SaveChanges();

            if (count > 0)
                result = true;

            return result;
        }

        /// <summary>
        /// GetRetByDept
        /// </summary>
        /// <param name="RetID">Retrieval ID</param>
        /// <returns></returns>
        public List<ReqAllocation> getRetByDept(int RetID)
        {
            List<ReqAllocation> reqAllocationList = new List<ReqAllocation>();

            //hashmap-like to store itemID and collated qty 
            Dictionary<string, int> itemQtyENGL = new Dictionary<string, int>();
            Dictionary<string, int> itemQtyCPSC = new Dictionary<string, int>();
            Dictionary<string, int> itemQtyCOMM = new Dictionary<string, int>();
            Dictionary<string, int> itemQtyREGR = new Dictionary<string, int>();
            Dictionary<string, int> itemQtyZOOL = new Dictionary<string, int>();

            //obtain list of requisition with specified RetID
            List<Requisition> reqList = ctx.Requisition.Where(x => x.RetID == RetID).ToList();

            foreach(Requisition req in reqList)
            {
                //obtain list of requisition detail with ReqID
                List<RequisitionDetail> reqDetailList = ctx.RequisitionDetail.Where(x => x.ReqID == req.ReqID).ToList();
                
                foreach(RequisitionDetail reqDetail in reqDetailList)
                {
                    if (req.DeptID == "ENGL")
                    {
                        //if itemQty does not contain the item, add item to itemQty
                        if (!itemQtyENGL.ContainsKey(reqDetail.ItemID))
                        {
                            itemQtyENGL.Add(reqDetail.ItemID, (int)reqDetail.IssueQty);
                        }
                        //else if itemQty contains item, add the qty to existing qty
                        else
                        {
                            itemQtyENGL["reqDetail.ItemID"] += (int)reqDetail.IssueQty;
                        }
                    }

                    if (req.DeptID == "CPSC")
                    {
                        //if itemQty does not contain the item, add item to itemQty
                        if (!itemQtyCPSC.ContainsKey(reqDetail.ItemID))
                        {
                            itemQtyCPSC.Add(reqDetail.ItemID, (int)reqDetail.IssueQty);
                        }
                        //else if itemQty contains item, add the qty to existing qty
                        else
                        {
                            itemQtyCPSC["reqDetail.ItemID"] += (int)reqDetail.IssueQty;
                        }
                    }

                    if (req.DeptID == "COMM")
                    {
                        //if itemQty does not contain the item, add item to itemQty
                        if (!itemQtyCOMM.ContainsKey(reqDetail.ItemID))
                        {
                            itemQtyCOMM.Add(reqDetail.ItemID, (int)reqDetail.IssueQty);
                        }
                        //else if itemQty contains item, add the qty to existing qty
                        else
                        {
                            itemQtyCOMM["reqDetail.ItemID"] += (int)reqDetail.IssueQty;
                        }
                    }

                    if (req.DeptID == "REGR")
                    {
                        //if itemQty does not contain the item, add item to itemQty
                        if (!itemQtyREGR.ContainsKey(reqDetail.ItemID))
                        {
                            itemQtyREGR.Add(reqDetail.ItemID, (int)reqDetail.IssueQty);
                        }
                        //else if itemQty contains item, add the qty to existing qty
                        else
                        {
                            itemQtyREGR["reqDetail.ItemID"] += (int)reqDetail.IssueQty;
                        }
                    }

                    if (req.DeptID == "ZOOL")
                    {
                        //if itemQty does not contain the item, add item to itemQty
                        if (!itemQtyZOOL.ContainsKey(reqDetail.ItemID))
                        {
                            itemQtyZOOL.Add(reqDetail.ItemID, (int)reqDetail.IssueQty);
                        }
                        //else if itemQty contains item, add the qty to existing qty
                        else
                        {
                            itemQtyZOOL["reqDetail.ItemID"] += (int)reqDetail.IssueQty;
                        }
                    }

                }
            }

            //extract all keys and values in itemQty
            string[] itemQtyENGLKeys = itemQtyENGL.Keys.ToArray();
            int[] itemQtyENGLValues = itemQtyENGL.Values.ToArray();

            string[] itemQtyCPSCKeys = itemQtyCPSC.Keys.ToArray();
            int[] itemQtyCPSCValues = itemQtyCPSC.Values.ToArray();

            string[] itemQtyCOMMKeys = itemQtyCOMM.Keys.ToArray();
            int[] itemQtyCOMMValues = itemQtyCOMM.Values.ToArray();

            string[] itemQtyREGRKeys = itemQtyREGR.Keys.ToArray();
            int[] itemQtyREGRValues = itemQtyREGR.Values.ToArray();

            string[] itemQtyZOOLKeys = itemQtyZOOL.Keys.ToArray();
            int[] itemQtyZOOLValues = itemQtyZOOL.Values.ToArray();
            
            //create and add ReqAllocation for ENGL Dept
            for (int i = 0; i < itemQtyENGL.Count; i++)
            {
                ReqAllocation reqAllocation = new ReqAllocation();
                reqAllocation.ItemID = itemQtyENGLKeys[i];
                reqAllocation.RequestQty = itemQtyENGLValues[i];
                reqAllocation.Dept = "ENGL";
                reqAllocationList.Add(reqAllocation);
            }

            //create and add ReqAllocation for CPSC Dept
            for (int i = 0; i < itemQtyCPSC.Count; i++)
            {
                //create and add new ReqAllocation
                ReqAllocation reqAllocation = new ReqAllocation();
                reqAllocation.ItemID = itemQtyCPSCKeys[i];
                reqAllocation.RequestQty = itemQtyCPSCValues[i];
                reqAllocation.Dept = "CPSC";
                reqAllocationList.Add(reqAllocation);
            }

            //create and add ReqAllocation for COMM Dept
            for (int i = 0; i < itemQtyCOMM.Count; i++)
            {
                //create and add new ReqAllocation
                ReqAllocation reqAllocation = new ReqAllocation();
                reqAllocation.ItemID = itemQtyCOMMKeys[i];
                reqAllocation.RequestQty = itemQtyCOMMValues[i];
                reqAllocation.Dept = "COMM";
                reqAllocationList.Add(reqAllocation);
            }

            //create and add ReqAllocation for REGR Dept
            for (int i = 0; i < itemQtyREGR.Count; i++)
            {
                //create and add new ReqAllocation
                ReqAllocation reqAllocation = new ReqAllocation();
                reqAllocation.ItemID = itemQtyREGRKeys[i];
                reqAllocation.RequestQty = itemQtyREGRValues[i];
                reqAllocation.Dept = "REGR";
                reqAllocationList.Add(reqAllocation);
            }

            //create and add ReqAllocation for ZOOL Dept
            for (int i = 0; i < itemQtyZOOL.Count; i++)
            {
                //create and add new ReqAllocation
                ReqAllocation reqAllocation = new ReqAllocation();
                reqAllocation.ItemID = itemQtyZOOLKeys[i];
                reqAllocation.RequestQty = itemQtyZOOLValues[i];
                reqAllocation.Dept = "ZOOL";
                reqAllocationList.Add(reqAllocation);
            }

            return reqAllocationList;
        }

    }
}