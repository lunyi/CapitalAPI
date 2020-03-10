using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SKCOMLib;

namespace SKCOMTester
{
    public partial class SKOrder : UserControl
    {
        #region Define Variable
        //----------------------------------------------------------------------
        // Define Variable
        //----------------------------------------------------------------------
        private bool m_bfirst = true;
        private int m_nCode;

        public delegate void MyMessageHandler(string strType, int nCode, string strMessage);
        public event MyMessageHandler GetMessage;

        SKCOMLib.SKOrderLib m_pSKOrder = null;
        public SKOrderLib OrderObj
        {
            get { return m_pSKOrder; }
            set { m_pSKOrder = value; }
        }

        public string m_strLoginID = "";
        public string LoginID
        {
            get { return m_strLoginID; }
            set { m_strLoginID = value; }
        }
        private bool m_bOrderM = false;
        public bool ContinuousTrading
        {
            get { return m_bOrderM; }
            set { m_bOrderM = value; }
        }

        #endregion

        #region Initialize
        //----------------------------------------------------------------------
        // Initialize
        //----------------------------------------------------------------------
        public SKOrder()
        {
            InitializeComponent();
        }

        private void SKOrder_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region API Event
        //----------------------------------------------------------------------
        // API Event
        //----------------------------------------------------------------------

        void m_OrderObj_OnAccount(string bstrLogInID, string bstrAccountData)
        {
            string[] strValues;
            string strAccount;

            strValues = bstrAccountData.Split(',');
            strAccount = bstrLogInID + " " + strValues[1] + strValues[3];

            if (strValues[0] == "TS")
            {
                boxStockAccount.Items.Add(strAccount);
                if (m_bOrderM)
                    stockOrderControl1.ContinuousTrading = m_bOrderM;
                //boxExecutionAccount.Items.Add("證券 " + strAccount);
            }
            else if (strValues[0] == "TF")
            {
                boxFutureAccount.Items.Add(strAccount);
                withDrawInOutControl1.UserAccountTF = strValues[1] + strValues[3];
                //boxExecutionAccount.Items.Add("期貨 " + strAccount);
            }
            else if (strValues[0] == "OF")
            {
                boxOSFutureAccount.Items.Add(strAccount);
                withDrawInOutControl1.UserAccountOF = strValues[1] + strValues[3];
            }
            else if (strValues[0] == "OS")
            {
                boxOSStockAccount.Items.Add(strAccount);
            }

        }

        void m_pSKOrder_OnAsyncOrder(int nThreaID, int nCode, string bstrMessage)
        {
            WriteMessage("[OnAsyncOrder]Thread ID:" + nThreaID.ToString() + " Code:" + nCode.ToString() + " Message:" + bstrMessage);
        }

        void m_pSKOrder_OnAsyncOrderOLID(int nThreaID, int nCode, string bstrMessage, string bstrOrderLinkedID)
        {
            WriteMessage("[OnAsyncOrderOLID]Thread ID:" + nThreaID.ToString() + " OLID:" + bstrOrderLinkedID + " Code:" + nCode.ToString() + " Message:" + bstrMessage);
        }


        void m_pSKOrder_OnRealBalanceReport(string bstrData)
        {
            WriteMessage("[OnRealBalanceReport]" + bstrData);
        }

        void m_pSKOrder_OnOpenInterest(string bstrData)
        {
            WriteMessage("[OnOpenInterest]" + bstrData);
        }

        void m_pSKOrder_OnOverseaFutureOpenInterest(string bstrData)
        {
            WriteMessage("[OnOverseaFutureOpenInterest]" + bstrData);
        }

        void m_pSKOrder_OnStopLossReport(string bstrData)
        {
            WriteMessage("[OnStopLossReport]" + bstrData);
        }

        void m_pSKOrder_OnOverseaFuture(string bstrData)
        {
            WriteMessage("[OnOverseaFuture]" + bstrData);
        }

        void m_pSKOrder_OnOverseaOption(string bstrData)
        {
            WriteMessage("[OnOverseaOption]" + bstrData);
        }

        void m_pSKOrder_OnFutureRights(string bstrData)
        {
            WriteMessage("[OnFutureRights]" + bstrData);

        }

        void m_pSKOrder_OnRequestProfitReport(string bstrData)
        {
            WriteMessage("[OnRequestProfitReport]" + bstrData);
        }

        void m_pSKOrder_OnOverSeaFutureRight(string bstrData)
        {
            WriteMessage("[OnOverSeaFutureRight]" + bstrData);
        }

        void m_pSKOrder_OnMarginPurchaseAmountLimit(string bstrData)
        {
            WriteMessage("[OnMarginPurchaseAmountLimit]" + bstrData);
        }

        void m_pSKOrder_OnBalanceQueryReport(string bstrData)
        {
            WriteMessage("[OnBalanceQuery]" + bstrData);
        }
        void m_pSKOrder_OnTSStrategyReport(string bstrData)
        {
            WriteMessage("[OnTSStrategyReport]" + bstrData);
        }
        

        #endregion

        #region Component Event
        //----------------------------------------------------------------------
        // Component Event
        //----------------------------------------------------------------------
        private void btnInitialize_Click(object sender, EventArgs e)
        {
            if (m_bfirst == true)
            {
                m_pSKOrder.OnAccount += new _ISKOrderLibEvents_OnAccountEventHandler(m_OrderObj_OnAccount);
                m_pSKOrder.OnAsyncOrder += new _ISKOrderLibEvents_OnAsyncOrderEventHandler(m_pSKOrder_OnAsyncOrder);
                m_pSKOrder.OnAsyncOrderOLID += new _ISKOrderLibEvents_OnAsyncOrderOLIDEventHandler(m_pSKOrder_OnAsyncOrderOLID);
                m_pSKOrder.OnRealBalanceReport += new _ISKOrderLibEvents_OnRealBalanceReportEventHandler(m_pSKOrder_OnRealBalanceReport);
                m_pSKOrder.OnOpenInterest += new _ISKOrderLibEvents_OnOpenInterestEventHandler(m_pSKOrder_OnOpenInterest);
                m_pSKOrder.OnOverseaFutureOpenInterest += new _ISKOrderLibEvents_OnOverseaFutureOpenInterestEventHandler(m_pSKOrder_OnOverseaFutureOpenInterest);
                m_pSKOrder.OnStopLossReport += new _ISKOrderLibEvents_OnStopLossReportEventHandler(m_pSKOrder_OnStopLossReport);
                m_pSKOrder.OnOverseaFuture += new _ISKOrderLibEvents_OnOverseaFutureEventHandler(m_pSKOrder_OnOverseaFuture);
                m_pSKOrder.OnOverseaOption += new _ISKOrderLibEvents_OnOverseaOptionEventHandler(m_pSKOrder_OnOverseaOption);
                m_pSKOrder.OnFutureRights += new _ISKOrderLibEvents_OnFutureRightsEventHandler(m_pSKOrder_OnFutureRights);
                m_pSKOrder.OnRequestProfitReport += new _ISKOrderLibEvents_OnRequestProfitReportEventHandler(m_pSKOrder_OnRequestProfitReport);
                m_pSKOrder.OnOverSeaFutureRight += new _ISKOrderLibEvents_OnOverSeaFutureRightEventHandler(m_pSKOrder_OnOverSeaFutureRight);
                m_pSKOrder.OnMarginPurchaseAmountLimit += new _ISKOrderLibEvents_OnMarginPurchaseAmountLimitEventHandler(m_pSKOrder_OnMarginPurchaseAmountLimit);
                m_pSKOrder.OnBalanceQuery += new _ISKOrderLibEvents_OnBalanceQueryEventHandler(m_pSKOrder_OnBalanceQueryReport);
                m_pSKOrder.OnTSSmartStrategyReport += new _ISKOrderLibEvents_OnTSSmartStrategyReportEventHandler(m_pSKOrder_OnTSStrategyReport);

                m_bfirst = false;
            }

            m_nCode = m_pSKOrder.SKOrderLib_Initialize();

            SendReturnMessage("Order", m_nCode, "SKOrderLib_Initialize");
        }

        private void btnReadCert_Click(object sender, EventArgs e)
        {
            m_nCode = m_pSKOrder.ReadCertByID(m_strLoginID);

            SendReturnMessage("Order", m_nCode, "ReadCertByID");
        }

        private void btnGetAccount_Click(object sender, EventArgs e)
        {
            boxStockAccount.Items.Clear();
            boxFutureAccount.Items.Clear();
            boxOSFutureAccount.Items.Clear();
            boxOSStockAccount.Items.Clear();

            m_nCode = m_pSKOrder.GetUserAccount();

            SendReturnMessage("Order", m_nCode, "GetUserAccount");
        }

        private void btnSetMaxQty_Click(object sender, EventArgs e)
        {
            int nQty = 0;
            int.TryParse(txtMaxQty.Text.Trim(), out nQty);

            m_nCode = m_pSKOrder.SetMaxQty(boxOrderLimit.SelectedIndex, nQty);

            SendReturnMessage("Order", m_nCode, "SetMaxQty");
        }

        private void btnSetMaxCount_Click(object sender, EventArgs e)
        {
            int nCount = 0;
            int.TryParse(txtMaxCount.Text.Trim(), out nCount);

            m_nCode = m_pSKOrder.SetMaxCount(boxOrderLimit.SelectedIndex, nCount);
            SendReturnMessage("Order", m_nCode, "SetMaxCount");
        }

        private void btnOrderUnlock_Click(object sender, EventArgs e)
        {
            m_nCode = m_pSKOrder.UnlockOrder(boxOrderLimit.SelectedIndex);
            SendReturnMessage("Order", m_nCode, "UnlockOrder");
        }

        private void btnDownloadOS_Click(object sender, EventArgs e)
        {

            m_nCode = m_pSKOrder.SKOrderLib_LoadOSCommodity();
            SendReturnMessage("Order", m_nCode, "SKOrderLib_LoadOSCommodity");
        }

        private void btnDownloadOO_Click(object sender, EventArgs e)
        {

            m_nCode = m_pSKOrder.SKOrderLib_LoadOOCommodity();
            SendReturnMessage("Order", m_nCode, "SKOrderLib_LoadOOCommodity");
        }

        /////////////////////////////////////////////////////////////////////////////////
        private void boxStockAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strInfo = boxStockAccount.Text;

            string[] strValues;
            strValues = strInfo.Split(' ');

            stockOrderControl1.UserID = strValues[0];
            stockOrderControl1.UserAccount = strValues[1];
            stockStrategyOrderControl1.UserID = strValues[0];
            stockStrategyOrderControl1.UserAccount = strValues[1];
        }

        private void boxOSStockAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strInfo = boxOSStockAccount.Text;

            string[] strValues;
            strValues = strInfo.Split(' ');

            foreignStockOrderControl1.UserID = strValues[0];
            foreignStockOrderControl1.UserAccount = strValues[1];
        }

        private void boxOSFutureAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strInfo = boxOSFutureAccount.Text;

            string[] strValues;
            strValues = strInfo.Split(' ');

            overseaFutureOrderControl1.UserID = strValues[0];
            overseaFutureOrderControl1.UserAccount = strValues[1];

            overseaOptionOrderControl1.UserID = strValues[0];
            overseaOptionOrderControl1.UserAccount = strValues[1];

            withDrawInOutControl1.UserID = strValues[0];
            //withDrawInOut1.UserAccountOF = strValues[1];

        }

        private void boxFutureAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strInfo = boxFutureAccount.Text;

            string[] strValues;
            strValues = strInfo.Split(' ');

            futureOrderControl1.UserID = strValues[0];
            futureOrderControl1.UserAccount = strValues[1];

            optionOrderControl1.UserID = strValues[0];
            optionOrderControl1.UserAccount = strValues[1];

            futureStopLossControl1.UserID = strValues[0];
            futureStopLossControl1.UserAccount = strValues[1];

            tftomit1.UserID = strValues[0];
            tftomit1.UserAccount = strValues[1];

            withDrawInOutControl1.UserID = strValues[0];
            //withDrawInOut1.UserAccountTF = strValues[1];

        }


        private void stockOrderControl1_OnOrderSignal(string strLogInID, bool bAsyncOrder, STOCKORDER pStock)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendStockOrder(strLogInID, bAsyncOrder, pStock, out strMessage);

            WriteMessage("證券委託 :" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendStockOrder");
        }

        private void futureOrderControl1_OnFutureOrderSignal(string strLogInID, bool bAsyncOrder, FUTUREORDER pStock)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendFutureOrder(strLogInID, bAsyncOrder, pStock, out strMessage);

            WriteMessage("期貨委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendFutureOrder");
        }

        private void futureOrderControl1_OnFutureOrderCLRSignal(string strLogInID, bool bAsyncOrder, FUTUREORDER pStock)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendFutureOrderCLR(strLogInID, bAsyncOrder, pStock, out strMessage);

            WriteMessage("期貨委託含倉位盤別：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendFutureOrderCLR");
        }

        private void optionOrderControl1_OnOptionOrderSignal(string strLogInID, bool bAsyncOrder, FUTUREORDER pStock)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendOptionOrder(strLogInID, bAsyncOrder, pStock, out strMessage);

            WriteMessage("選擇權委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendOptionOrder");
        }

        private void optionOrderControl1_OnDuplexOrderSignal(string strLogInID, bool bAsyncOrder, FUTUREORDER pStock)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendDuplexOrder(strLogInID, bAsyncOrder, pStock, out strMessage);

            WriteMessage("選擇權複式單委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendDuplexOrder");
        }

        private void optionOrderControl1_OnAssembleOptionSignal(string strLogInID, bool bAsyncOrder, FUTUREORDER pFutureOrder)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.AssembleOptions(strLogInID, bAsyncOrder, pFutureOrder, out strMessage);

            WriteMessage("選擇權組合單委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendAssemleOption");
        }

        private void optionOrderControl1_OnTwoOrderToDisassemblySignal(string strLogInID, bool bAsyncOrder, FUTUREORDER pFutureOrder)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.DisassembleOptions(strLogInID, bAsyncOrder, pFutureOrder, out strMessage);

            WriteMessage("複式單拆解：" + strMessage);
            SendReturnMessage("Order", m_nCode, "TwoOrderToDisassembly");
        }

        private void optionOrderControl1_OnCoverAllProductSignal(string strLogInID, bool bAsyncOrder, FUTUREORDER pFutureOrder)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CoverAllProduct(strLogInID, bAsyncOrder, pFutureOrder, out strMessage);

            WriteMessage("雙邊部位了結：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CoverAllProduct");
        }

        private void overseaFutureOrderControl1_OnOverseaFutureOrderSignal(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDER pStock)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendOverseaFutureOrder(strLogInID, bAsyncOrder, pStock, out strMessage);

            WriteMessage("海期委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendOverseaFutureOrder");
        }
        private void overseaFutureOrderControl1_OnOverseaFutureOrderOLIDSignal(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDER pStock, string strOrderLinkedID)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendOverseaFutureOrderOLID(strLogInID, bAsyncOrder, pStock, strOrderLinkedID, out strMessage);

            WriteMessage("海期委託by OrderLinkedID：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendOverseaFutureOrderOLID");
        }

        private void overseaFutureOrderControl1_OnOverseaFutureOrderSpreadSignal(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDER pStock)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendOverseaFutureSpreadOrder(strLogInID, bAsyncOrder, pStock, out strMessage);

            WriteMessage("海期價差委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendOverseaFutureSpreadOrder");
        }

        private void overseaFutureOrderControl1_OnOverseaFutureOrderSpreadOLIDSignal(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDER pStock, string strOrderLinkedID)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendOverseaFutureSpreadOrderOLID(strLogInID, bAsyncOrder, pStock, strOrderLinkedID, out strMessage);

            WriteMessage("海期價差委託by OrderLinkedID：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendOverseaFutureSpreadOrderOLID");
        }

        private void overseaOptionOrderControl1_OnOverseaOptionOrderSignal(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDER pStock)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendOverseaOptionOrder(strLogInID, bAsyncOrder, pStock, out strMessage);

            WriteMessage("海選委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendOverseaOptionOrder");
        }

        private void futureStopLossControl1_OnFutureStopLossOrderSignal(string strLogInID, bool bAsyncOrder, FUTUREORDER pOrder)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendFutureStopLossOrder(strLogInID, bAsyncOrder, pOrder, out strMessage);

            WriteMessage("期貨停損委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendFutureStopLossOrder");
        }

        private void futureStopLossControl1_OnMovingStopLossOrderSignal(string strLogInID, bool bAsyncOrder, FUTUREORDER pOrder)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendMovingStopLossOrder(strLogInID, bAsyncOrder, pOrder, out strMessage);

            WriteMessage("期貨移動停損委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendMovingStopLossOrder");
        }

        private void futureStopLossControl1_OnOptionStopLossOrderSignal(string strLogInID, bool bAsyncOrder, FUTUREORDER pOrder)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendOptionStopLossOrder(strLogInID, bAsyncOrder, pOrder, out strMessage);

            WriteMessage("選擇權停損委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendOptionStopLossOrder");
        }

        private void futureStopLossControl1_OnFutureOCOOrderSignal(string strLogInID, bool bAsyncOrder, FUTUREOCOORDER pOrder)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendFutureOCOOrder(strLogInID, bAsyncOrder, pOrder, out strMessage);

            WriteMessage("期貨OCO委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendFutureOCOOrder");
        }

        private void ForeignOrderControl1_OnForeignOrderSignal(string strLogInID, bool bAsyncOrder, FOREIGNORDER pStock)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendForeignStockOrder(strLogInID, bAsyncOrder, pStock, out strMessage);

            WriteMessage("複委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendForeignStockOrder");
        }

        private void ForeignOrderControl1_OnCancelForeignOrderBySeqSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo, string strExchangeNo)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CancelForeignStockOrderBySeqNo(strLogInID, bAsyncOrder, strAccount, strSeqNo, strExchangeNo, out strMessage);

            WriteMessage("複委託刪單BySeq：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CancelForeignStockOrderBySeqNo");
        }

        private void ForeignOrderControl1_OnCancelForeignOrderByBookSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strBookNo, string strExchangeNo)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CancelForeignStockOrderByBookNo(strLogInID, bAsyncOrder, strAccount, strBookNo, strExchangeNo, out strMessage);

            WriteMessage("複委託刪單ByBook：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CancelForeignStockOrderByBookNo");
        }

        private void stockOrderControl1_OnDecreaseOrderSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo, int nDecreaseQty)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.DecreaseOrderBySeqNo(strLogInID, bAsyncOrder, strAccount, strSeqNo, nDecreaseQty, out strMessage);

            WriteMessage("改量：" + strMessage);
            SendReturnMessage("Order", m_nCode, "DecreaseOrderBySeqNo");
        }
        /*
        private void stockOrderControl1_OnDecreaseOrderContinuousTradingSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strSymbol, string strBookNo, int nDecreaseQty)
        {
            string strMessage = "";
            //m_nCode = m_pSKOrder.DecreaseOrderByBookNoContinuousTrading(strLogInID, bAsyncOrder, strAccount, strSymbol, strBookNo, nDecreaseQty, out strMessage);

            WriteMessage("擬真改量：" + strMessage);
            SendReturnMessage("Order", m_nCode, "DecreaseOrderByBookNoContinuousTrading");
        }

        private void stockOrderControl1_OnCorrectPriceBySeqNoContinuousTrading(string strLogInID, bool bAsyncOrder, string strAccount, string strSymbol, string strSeqNo, string strPrice, int nTradeType)
        {
            string strMessage = "";
            //m_nCode = m_pSKOrder.CorrectPriceBySeqNoContinuousTrading(strLogInID, bAsyncOrder, strAccount, strSymbol, strSeqNo, strPrice, nTradeType, out strMessage);

            WriteMessage("擬真依序號權改價：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CorrectPriceBySeqNoContinuousTrading");
        }
        private void stockOrderControl1_OnCorrectPriceByBookNoContinuousTrading(string strLogInID, bool bAsyncOrder, string strAccount, string strSymbol, string strBookNo, string strPrice, int nTradeType)
        {
            string strMessage = "";
            //m_nCode = m_pSKOrder.CorrectPriceByBookNoContinuousTrading(strLogInID, bAsyncOrder, strAccount, strSymbol, strBookNo, strPrice, nTradeType, out strMessage);

            WriteMessage("擬真依書號改價：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CorrectPriceByBookNoContinuousTrading");
        }*/
        private void stockOrderControl1_OnCorrectPriceBySeqNo(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo, string strPrice, int nTradeType)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CorrectPriceBySeqNo(strLogInID, bAsyncOrder, strAccount, strSeqNo, strPrice, nTradeType, out strMessage);

            WriteMessage("證期權依序號改價：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CorrectPriceBySeqNo");
        }
        private void stockOrderControl1_OnCorrectPriceByBookNo(string strLogInID, bool bAsyncOrder, string strAccount, string strSymbol, string strBookNo, string strPrice, int nTradeType)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CorrectPriceByBookNo(strLogInID, bAsyncOrder, strAccount, strSymbol, strBookNo, strPrice, nTradeType, out strMessage);

            WriteMessage("證期權依書號改價：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CorrectPriceByBookNo");
        }


        private void overseaFutureOrderControl1_OnDecreaseOrderSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo, int nDecreaseQty)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.OverSeaDecreaseOrderBySeqNo(strLogInID, bAsyncOrder, strAccount, strSeqNo, nDecreaseQty, out strMessage);

            WriteMessage("海期改量：" + strMessage);
            SendReturnMessage("Order", m_nCode, "OverSeaDecreaseOrderBySeqNo");
        }

        private void overseaFutureOrderControl1_OnCorrectPriceOrderSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo, string strCorrectPrice)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.OverSeaCorrectPriceBySGXAPISeqNo(strLogInID, bAsyncOrder, strAccount, strSeqNo, strCorrectPrice, out strMessage);

            WriteMessage("海期改價(SGXAPI)：" + strMessage);
            SendReturnMessage("Order", m_nCode, "OverSeaCorrectPriceBySGXAPISeqNo");
        }

        private void stockOrderControl1_OnCancelOrderSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CancelOrderBySeqNo(strLogInID, bAsyncOrder, strAccount, strSeqNo, out strMessage);

            WriteMessage("刪單：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CancelOrderBySeqNo");
        }

        private void stockOrderControl1_OnCancelOrderByBookSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strBookNo)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CancelOrderByBookNo(strLogInID, bAsyncOrder, strAccount, strBookNo, out strMessage);

            WriteMessage("刪單：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CancelOrderByBookNo");
        }

        private void stockOrderControl1_OnCancelOrderByStockSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strStockNo)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CancelOrderByStockNo(strLogInID, bAsyncOrder, strAccount, strStockNo, out strMessage);

            WriteMessage("刪單：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CancelOrderByStockNo");
        }

        /*private void stockOrderControl1_OnContinuousTradingSignal(string strLogInID, bool bAsyncOrder, STOCKORDER pStock)
        {
   
            string strMessage = "";
            //m_nCode = m_pSKOrder.SendStockContinuousTrading(strLogInID, bAsyncOrder, pStock, out strMessage);
                    

            WriteMessage("擬真平台委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "ContinuousTrading");
        }

        private void stockOrderControl1_OnCancelOrderBySeqNoContinuousTradingSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo)
        {
            string strMessage = "";
            //m_nCode = m_pSKOrder.CancelOrderBySeqNoContinuousTrading(strLogInID, bAsyncOrder, strAccount, strSeqNo, out strMessage);

            WriteMessage("擬真平台刪單：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CancelOrderBySeqNoContinuousTrading");
        }*/

        private void futureStopLossControl1_OnCancelFutureStopLossOrderSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strBookNo, string strSymbol)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CancelFutureStopLoss(strLogInID, bAsyncOrder, strAccount, strBookNo, strSymbol, out strMessage);

            WriteMessage("停損刪單：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CancelFutureStopLoss");
        }

        private void futureStopLossControl1_OnCancelMovingStopLossOrderSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strBookNo, string strSymbol)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CancelMovingStopLoss(strLogInID, bAsyncOrder, strAccount, strBookNo, strSymbol, out strMessage);

            WriteMessage("移動停損刪單：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CancelMovingStopLoss");
        }

        private void futureStopLossControl1_OnCancelOptionStopLossOrderSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strBookNo, string strSymbol)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CancelOptionStopLoss(strLogInID, bAsyncOrder, strAccount, strBookNo, strSymbol, out strMessage);

            WriteMessage("選擇權停損刪單：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CancelOptionStopLoss");
        }

        private void futureStopLossControl1_OnCancelFutureOCOOrderSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strBookNo, string strSymbol)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CancelFutureOCO(strLogInID, bAsyncOrder, strAccount, strBookNo, strSymbol, out strMessage);

            WriteMessage("OCO刪單：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CancelFutureOCO");
        }

        private void futureStopLossControl1_OnCancelFutureMITOrderSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strBookNo, string strSymbol)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CancelFutureMIT(strLogInID, bAsyncOrder, strAccount, strBookNo, strSymbol, out strMessage);

            WriteMessage("FutureMIT刪單：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CancelFutureMIT");
        }

        private void futureStopLossControl1_OnCancelOptionMITOrderSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strBookNo, string strSymbol)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CancelOptionMIT(strLogInID, bAsyncOrder, strAccount, strBookNo, strSymbol, out strMessage);

            WriteMessage("OptionMIT刪單：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CancelOptionMIT");
        }

        private void futureOrderControl1_OnCorrectPriceBySeqNo(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo, string strPrice, int nTradeType)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CorrectPriceBySeqNo(strLogInID, bAsyncOrder, strAccount, strSeqNo, strPrice, nTradeType, out strMessage);

            WriteMessage("期權改價：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CorrectPriceBySeqNo");
        }
        private void futureOrderControl1_OnCorrectPriceByBookNo(string strLogInID, bool bAsyncOrder, string strAccount, string strSymbol, string strBookNo, string strPrice, int nTradeType)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CorrectPriceByBookNo(strLogInID, bAsyncOrder, strAccount, strSymbol, strBookNo, strPrice, nTradeType, out strMessage);

            WriteMessage("期權依書號改價：" + strMessage);
            SendReturnMessage("Order", m_nCode, "CorrectPriceByBookNo");
        }

        private void overseaFutureOrderControl1_OnCancelOrderSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.OverSeaCancelOrderBySeqNo(strLogInID, bAsyncOrder, strAccount, strSeqNo, out strMessage);

            WriteMessage("海期刪單：" + strMessage);
            SendReturnMessage("Order", m_nCode, "OverSeaCancelOrderBySeqNo");
        }
        private void overseaFutureOrderControl1_OnCancelOrderByBookSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strBookNo)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.OverSeaCancelOrderByBookNo(strLogInID, bAsyncOrder, strAccount, strBookNo, out strMessage);

            WriteMessage("海期依書號刪單：" + strMessage);
            SendReturnMessage("Order", m_nCode, "OverSeaCancelOrderByBookNo");
        }

        private void overseaFutureOrderControl1_OnCorrectOrderByBookSignal(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDERFORGW pOrder)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.OverSeaCorrectPriceByBookNo(strLogInID, bAsyncOrder, pOrder, out strMessage);
            WriteMessage("海期依書號改價：" + strMessage);
            SendReturnMessage("Order", m_nCode, "OverSeaCorrectPriceOrderByBookNo");

        }

        private void overseaFutureOrderControl1_OnCorrectSpreadOrderByBookSignal(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDERFORGW pOrder)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.OverSeaCorrectPriceSpreadByBookNo(strLogInID, bAsyncOrder, pOrder, out strMessage);
            WriteMessage("海期價差依書號改價：" + strMessage);
            SendReturnMessage("Order", m_nCode, "OverSeaCorrectPriceSpreadOrderByBookNo");

        }

        private void overseaFutureOrderControl1_OnCorrectOptionOrderByBookSignal(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDERFORGW pOrder)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.OverSeaOptionCorrectPriceByBookNo(strLogInID, bAsyncOrder, pOrder, out strMessage);
            WriteMessage("海選依書號改價：" + strMessage);
            SendReturnMessage("Order", m_nCode, "OverSeaOptionCorrectPriceOrderByBookNo");

        }

        private void stockOrderControl1_OnRealBalanceSignal(string strLogInID, string strAccount)
        {
            m_nCode = m_pSKOrder.GetRealBalanceReport(strLogInID, strAccount);
            SendReturnMessage("Order", m_nCode, "GetRealBalanceReport");
        }

        private void stockOrderControl1_OnRequestProfitReportSignal(string strLogInID, string strAccount)
        {
            m_nCode = m_pSKOrder.GetRequestProfitReport(strLogInID, strAccount);
            SendReturnMessage("Order", m_nCode, "GetRequestProfitReport");
        }

        private void stockOrderControl1_OnRequestAmountLimitSignal(string strLogInID, string strAccount, string strStockNo)
        {
            m_nCode = m_pSKOrder.GetMarginPurchaseAmountLimit(strLogInID, strAccount, strStockNo);
            SendReturnMessage("Order", m_nCode, "GetMarginPurchaseAmountLimit");
        }

        private void stockOrderControl1_OnRequestBalanceQuerySignal(string strLogInID, string strAccount, string strStockNo)
        {
            m_nCode = m_pSKOrder.GetBalanceQuery(strLogInID, strAccount, strStockNo);
            SendReturnMessage("Order", m_nCode, "GetBalanceQuery");
        }

        private void futureOrderControl1_OnOpenInterestSignal(string strLogInID, string strAccount)
        {
            m_nCode = m_pSKOrder.GetOpenInterest(strLogInID, strAccount);
            SendReturnMessage("Order", m_nCode, "GetOpenInterest");
        }

        private void futureOrderControl1_OnOpenInterestWithFormatSignal(string strLogInID, string strAccount,int nFormat)
        {
            m_nCode = m_pSKOrder.GetOpenInterestWithFormat(strLogInID, strAccount,nFormat);
            SendReturnMessage("Order", m_nCode, "GetOpenInterestWithFormat");
        }

        private void overseaFutureOrderControl1_OnOpenInterestSignal(string strLogInID, string strAccount)
        {
            m_nCode = m_pSKOrder.GetOverseaFutureOpenInterest(strLogInID, strAccount);
            SendReturnMessage("Order", m_nCode, "GetOverseaFutureOpenInterest");
        }
        private void futureStopLossControl1_OnStopLossReportSignal(string strLogInID, string strAccount, int nReportStatus, string strKind, string strDate)
        {
            m_nCode = m_pSKOrder.GetStopLossReport(strLogInID, strAccount, nReportStatus, strKind, strDate);
            SendReturnMessage("Order", m_nCode, "GetStopLossReport");
        }

        private void overseaFutureOrderControl1_OnOverseaFuturesSignal()
        {
            m_nCode = m_pSKOrder.GetOverseaFutures();
            SendReturnMessage("Order", m_nCode, "GetOverseaFutures");
        }

        private void overseaOptionOrderControl1_OnOnOverseaOptionSignal()
        {
            m_nCode = m_pSKOrder.GetOverseaOptions();
            SendReturnMessage("Order", m_nCode, "GetOverseaOptions");
        }

        private void overseaFutureOrderControl1_OnOverSeaFutureRightSignal(string strLogInID, string strAccount)
        {
            m_nCode = m_pSKOrder.GetRequestOverSeaFutureRight(strLogInID, strAccount);
            SendReturnMessage("Order", m_nCode, "GetRequestOverSeaFutureRight");
        }

        
        private void futureOrderControl1_OnFutureRightsSignal(string strLogInID, string strAccount, int nCoinType)
        {
            m_nCode = m_pSKOrder.GetFutureRights(strLogInID, strAccount, (short)nCoinType);
            SendReturnMessage("Order", m_nCode, "GetFutureRights");
        }

        private void TFTOMIT_OnFutureMITOrderSignal(string strLogInID, bool bAsyncOrder, FUTUREORDER pOrder)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendFutureMITOrder(strLogInID, bAsyncOrder, pOrder, out strMessage);

            WriteMessage("期貨MIT委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendFutureMITOrder");
        }

        private void futureOrderControl1_OnSendTXOffsetSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strYearMonth, int nBuySell, int nQty)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendTXOffset(strLogInID, bAsyncOrder, strAccount, strYearMonth, nBuySell, nQty, out strMessage);

            WriteMessage("大小台互抵：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendTXOffset");
        }
        
        private void TFTOMIT_OnOptionMITOrderSignal(string strLogInID, bool bAsyncOrder, FUTUREORDER pOrder)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendOptionMITOrder(strLogInID, bAsyncOrder, pOrder, out strMessage);

            WriteMessage("選擇權MIT委託：" + strMessage);
            SendReturnMessage("Order", m_nCode, "SendOptionMITOrder");
        }

        private void withDrawInOutControl1_OnWithDrawSignal(string strLogInID, string strAccountOut, int nTypeOut, string strAccountIn, int nTypeIn, int nCurrency, string strDollars, string strPWD)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.WithDraw(strLogInID, strAccountOut, nTypeOut, strAccountIn,nTypeIn, nCurrency, strDollars,  strPWD, out strMessage);

            WriteMessage("國內外出入金互轉：" + strMessage);
            SendReturnMessage("Order", m_nCode, "WithDrawInOut");
        }

        private void StockStrategyOrderControl1_OnStockStrategyDayTradeSignal(string strLogInID, bool bAsyncOrder, STOCKSTRATEGYORDER pOrder)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendStockStrategyDayTrade(strLogInID, bAsyncOrder, pOrder, out strMessage);

            WriteMessage("證券智動單-當沖組合：" + strMessage);
            SendReturnMessage("Order", m_nCode, "StockStrategyDayTrade");
        }

        private void StockStrategyOrderControl1_OnStockStrategyAllCleanOutSignal(string strLogInID, bool bAsyncOrder, STOCKSTRATEGYORDEROUT pOrder)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.SendStockStrategyClear(strLogInID, bAsyncOrder, pOrder, out strMessage);

            WriteMessage("證券智動單-出清策略：" + strMessage);
            SendReturnMessage("Order", m_nCode, "StockStrategyAllCleanOut");
        }

        private void StockStrategyOrderControl1_OnTSStrategyReportSignal(string strLogInID, string strAccount, string strMarketType, int nTypeReport, string strKindReport, string strStartDate)
        {
            m_nCode = m_pSKOrder.GetTSSmartStrategyReport(strLogInID, strAccount, strMarketType, nTypeReport, strKindReport, strStartDate);
            SendReturnMessage("Order", m_nCode, "GetTSStrategyReport");
        
        }
        private void StockStrategyOrderControl1_OnCancelTSStrategyOrderSignal(string strLogInID, bool bAsyncOrder, string strAccount, string strSmartKey, int nTradeType)
        {
            string strMessage = "";
            m_nCode = m_pSKOrder.CancelTSStrategyOrder(strLogInID, bAsyncOrder, strAccount, strSmartKey, nTradeType, out strMessage);
            if (nTradeType >= 1 && nTradeType <= 3)
            {
                WriteMessage("證券智動單刪單-當沖：" + strMessage);
                SendReturnMessage("Order", m_nCode, "CancelTSStrategyOrderDayTrade");
            }
            else
            {
                WriteMessage("證券智動單刪單-出清：" + strMessage);
                SendReturnMessage("Order", m_nCode, "CancelTSStrategyOrderClearOut");
            }
        }
       
        #endregion


        #region Custom Method
        //----------------------------------------------------------------------
        // Custom Method
        //----------------------------------------------------------------------

        void SendReturnMessage(string strType, int nCode, string strMessage)
        {
            if (GetMessage != null)
            {
                GetMessage(strType, nCode, strMessage);
            }
        }

        public void WriteMessage(string strMsg)
        {
            listInformation.Items.Add(strMsg);

            listInformation.SelectedIndex = listInformation.Items.Count - 1;

            //listInformation.HorizontalScrollbar = true;

            // Create a Graphics object to use when determining the size of the largest item in the ListBox.
            Graphics g = listInformation.CreateGraphics();

            // Determine the size for HorizontalExtent using the MeasureString method using the last item in the list.
            int hzSize = (int)g.MeasureString(listInformation.Items[listInformation.Items.Count - 1].ToString(), listInformation.Font).Width;
            // Set the HorizontalExtent property.
            //[20170607-d-]listInformation.HorizontalExtent = hzSize;
        }

        #endregion

        private void Btn_SGXAPI_Click(object sender, EventArgs e)
        {
            if (boxOSFutureAccount.SelectedIndex == -1)
            {
                MessageBox.Show("請選擇海期帳號");
                return;
            }


            string strInfo = boxOSFutureAccount.Text;

            string[] strValues;
            strValues = strInfo.Split(' ');

            m_pSKOrder.AddSGXAPIOrderSocket(strValues[0] ,strValues[1]);
        }


    }
}
