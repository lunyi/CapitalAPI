﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SKCOMLib;

namespace SKOrderTester
{
    public partial class OverseaFutureOrderControl : UserControl
    {
        #region Define Variable
        //----------------------------------------------------------------------
        // Define Variable
        //----------------------------------------------------------------------

        private int m_nCode;
        public string m_strMessage;

        public delegate void MyMessageHandler(string strType, int nCode, string strMessage);
        public event MyMessageHandler GetMessage;

        public delegate void OrderHandler(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDER pStock);
        public event OrderHandler OnOverseaFutureOrderSignal;

        public delegate void OrderOLIDHandler(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDER pStock, string strOrderLinkedID);
        public event OrderOLIDHandler OnOverseaFutureOrderOLIDSignal;

        public delegate void SpreadOrderHandler(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDER pStock);
        public event SpreadOrderHandler OnOverseaFutureOrderSpreadSignal;

        public delegate void SpreadOrderOLIDHandler(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDER pStock, string strOrderLinkedID);
        public event SpreadOrderOLIDHandler OnOverseaFutureOrderSpreadOLIDSignal;

        public delegate void DecreaseOrderHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo, int nDecreaseQty);
        public event DecreaseOrderHandler OnDecreaseOrderSignal;

        public delegate void OpenInterestHandler(string strLogInID, string strAccount);
        public event OpenInterestHandler OnOpenInterestSignal;

        public delegate void OverseaFuturesHandler();
        public event OverseaFuturesHandler OnOverseaFuturesSignal;

        public delegate void OverSeaFutureRightSignalHandler(string strLogInID, string strAccount);
        public event OverSeaFutureRightSignalHandler OnOverSeaFutureRightSignal;

        public delegate void CancelOrderHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo);
        public event CancelOrderHandler OnCancelOrderSignal;

        public delegate void CancelOrderByBookHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo);
        public event CancelOrderByBookHandler OnCancelOrderByBookSignal;

        public delegate void CorrectOrderByBookHandler(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDERFORGW pOrder);
        public event CorrectOrderByBookHandler OnCorrectOrderByBookSignal;

        public delegate void CorrectSpreadOrderByBookHandler(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDERFORGW pOrder);
        public event CorrectSpreadOrderByBookHandler OnCorrectSpreadOrderByBookSignal;

        public delegate void CorrectOptionOrderByBookHandler(string strLogInID, bool bAsyncOrder, OVERSEAFUTUREORDERFORGW pOrder);
        public event CorrectOptionOrderByBookHandler OnCorrectOptionOrderByBookSignal;

        public delegate void CorrectPriceOrderHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo, string strCorrectPrice);
        public event CorrectPriceOrderHandler OnCorrectPriceOrderSignal;

        private string m_UserID = "";
        public string UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }

        private string m_UserAccount = "";
        public string UserAccount
        {
            get { return m_UserAccount; }
            set { m_UserAccount = value; }
        }

        #endregion

        #region Initialize
        //----------------------------------------------------------------------
        // Initialize
        //----------------------------------------------------------------------
        public OverseaFutureOrderControl()
        {
            InitializeComponent();
            boxNewClose.SelectedItem = 0;
        }

        #endregion

        private void btnSendOverseaFutureOrder_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇海期帳號");
                return;
            }

            string strTradeName;
            string strStockNo;
            string strYearMonth;
            int nBuySell;
            int nNewClose;
            int nDayTrade;
            int nTradeType;
            int nSpecialTradeType;
            string strOrder;
            string strOrderNumerator;
            string strTrigger;
            string strTriggerNumerator;
            int nQty;

            if (txtTradeNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入交易所代號");
                return;
            }
            strTradeName = txtTradeNo.Text.Trim();

            if (txtStockNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strStockNo = txtStockNo.Text.Trim();

            if (txtYearMonth.Text.Trim() == "")
            {
                MessageBox.Show("請輸入年月");
                return;
            }
            strYearMonth = txtYearMonth.Text.Trim();

            double dPrice = 0.0;

            if (boxSpecialTradeType.SelectedIndex == 0 || boxSpecialTradeType.SelectedIndex == 2)
            {
                if (double.TryParse(txtOrder.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("委託價請輸入數字");
                    return;
                }
            }
            strOrder = txtOrder.Text.Trim();


            if (boxSpecialTradeType.SelectedIndex == 0 || boxSpecialTradeType.SelectedIndex == 2)
            {
                if (double.TryParse(txtOrderNumerator.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("委託價分子請輸入數字");
                    return;
                }
            }
            strOrderNumerator = txtOrderNumerator.Text.Trim();


            if (boxSpecialTradeType.SelectedIndex == 2 || boxSpecialTradeType.SelectedIndex == 3)
            {
                if (double.TryParse(txtTrigger.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("觸發價請輸入數字");
                    return;
                }
            }
            strTrigger = txtTrigger.Text.Trim();


            if (boxSpecialTradeType.SelectedIndex == 2 || boxSpecialTradeType.SelectedIndex == 3)
            {
                if (double.TryParse(txtTriggerNumerator.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("觸發價分子請輸入數字");
                    return;
                }
            }
            strTriggerNumerator = txtTriggerNumerator.Text.Trim();


            if (int.TryParse(txtQty.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }

            if (boxBidAsk.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }
            nBuySell = boxBidAsk.SelectedIndex;

            if (boxNewClose.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇倉別");
                return;
            }
            nNewClose = boxNewClose.SelectedIndex;

            if (boxFlag.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇當沖與否");
                return;
            }
            nDayTrade = boxFlag.SelectedIndex;

            if (boxPeriod.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託條件");
                return;
            }
            nTradeType = boxPeriod.SelectedIndex;

            if (boxSpecialTradeType.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託類型");
                return;
            }
            nSpecialTradeType = boxSpecialTradeType.SelectedIndex;

            OVERSEAFUTUREORDER pOSOrder = new OVERSEAFUTUREORDER();

            pOSOrder.bstrFullAccount        = m_UserAccount;
            pOSOrder.bstrExchangeNo         = strTradeName;
            pOSOrder.bstrOrder              = strOrder;
            pOSOrder.bstrOrderNumerator     = strOrderNumerator;
            pOSOrder.bstrStockNo            = strStockNo;
            pOSOrder.bstrTrigger            = strTrigger;
            pOSOrder.bstrTriggerNumerator   = strTriggerNumerator;
            pOSOrder.bstrYearMonth          = strYearMonth;
            pOSOrder.nQty                   = nQty;
            pOSOrder.sBuySell               = (short)nBuySell;
            pOSOrder.sDayTrade              = (short)nDayTrade;
            pOSOrder.sNewClose              = (short)nNewClose;
            pOSOrder.sSpecialTradeType      = (short)nSpecialTradeType;
            pOSOrder.sTradeType             = (short)nTradeType;

            if (OnOverseaFutureOrderSignal != null )
            {
                OnOverseaFutureOrderSignal(m_UserID, false, pOSOrder);
            }
        }

        private void btnSendOverseaFutureOrderAsync_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇海期帳號");
                return;
            }

            string strTradeName;
            string strStockNo;
            string strYearMonth;
            int nBuySell;
            int nNewClose;
            int nDayTrade;
            int nTradeType;
            int nSpecialTradeType;
            string strOrder;
            string strOrderNumerator;
            string strTrigger;
            string strTriggerNumerator;
            int nQty;

            if (txtTradeNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入交易所代號");
                return;
            }
            strTradeName = txtTradeNo.Text.Trim();

            if (txtStockNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strStockNo = txtStockNo.Text.Trim();

            if (txtYearMonth.Text.Trim() == "")
            {
                MessageBox.Show("請輸入年月");
                return;
            }
            strYearMonth = txtYearMonth.Text.Trim();

            double dPrice = 0.0;
            if (boxSpecialTradeType.SelectedIndex == 0 || boxSpecialTradeType.SelectedIndex == 2)
            {
                if (double.TryParse(txtOrder.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("委託價請輸入數字");
                     return;
                }
            }
            strOrder = txtOrder.Text.Trim();

            if (boxSpecialTradeType.SelectedIndex == 0 || boxSpecialTradeType.SelectedIndex == 2)
            {
                
                if (double.TryParse(txtOrderNumerator.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("委託價分子請輸入數字");
                    return;
                }
            }
            strOrderNumerator = txtOrderNumerator.Text.Trim();


            if (boxSpecialTradeType.SelectedIndex == 2 || boxSpecialTradeType.SelectedIndex == 3)
            {
                
                if (double.TryParse(txtTrigger.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("觸發價請輸入數字");
                    return;
                }
            }
            strTrigger = txtTrigger.Text.Trim();


            if (boxSpecialTradeType.SelectedIndex == 2 || boxSpecialTradeType.SelectedIndex == 3)
            {

                
                if (double.TryParse(txtTriggerNumerator.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("觸發價分子請輸入數字");
                    return;
                }
            }


            strTriggerNumerator = txtTriggerNumerator.Text.Trim();


            if (int.TryParse(txtQty.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }

            if (boxBidAsk.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }
            nBuySell = boxBidAsk.SelectedIndex;

            if (boxNewClose.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇倉別");
                return;
            }
            nNewClose = boxNewClose.SelectedIndex;

            if (boxFlag.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇當沖與否");
                return;
            }
            nDayTrade = boxFlag.SelectedIndex;

            if (boxPeriod.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託條件");
                return;
            }
            nTradeType = boxPeriod.SelectedIndex;

            if (boxSpecialTradeType.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託類型");
                return;
            }
            nSpecialTradeType = boxSpecialTradeType.SelectedIndex;

            OVERSEAFUTUREORDER pOSOrder = new OVERSEAFUTUREORDER();

            pOSOrder.bstrFullAccount = m_UserAccount;
            pOSOrder.bstrExchangeNo = strTradeName;
            pOSOrder.bstrOrder = strOrder;
            pOSOrder.bstrOrderNumerator = strOrderNumerator;
            pOSOrder.bstrStockNo = strStockNo;
            pOSOrder.bstrTrigger = strTrigger;
            pOSOrder.bstrTriggerNumerator = strTriggerNumerator;
            pOSOrder.bstrYearMonth = strYearMonth;
            pOSOrder.nQty = nQty;
            pOSOrder.sBuySell = (short)nBuySell;
            pOSOrder.sDayTrade = (short)nDayTrade;
            pOSOrder.sNewClose = (short)nNewClose;
            pOSOrder.sSpecialTradeType = (short)nSpecialTradeType;
            pOSOrder.sTradeType = (short)nTradeType;

            if (OnOverseaFutureOrderSignal != null)
            {
                OnOverseaFutureOrderSignal(m_UserID, true, pOSOrder);
            }

        }

        private void btnDecreaseQty_Click(object sender, EventArgs e)
        {
            int nQty = 0;

            if (int.TryParse(txtDecreaseQty.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("改量請輸入數字");
            }
            else
            {
                if (OnDecreaseOrderSignal != null)
                {
                    OnDecreaseOrderSignal(m_UserID, true, m_UserAccount, txtModifySeqNo.Text.Trim(), nQty);
                }
            }
        }

        private void btnSendOverseaFutureSpreadOrder_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇海期帳號");
                return;
            }

            string strTradeName;
            string strStockNo;
            string strYearMonth;
            string strYearMonth2;
            int nBuySell;
            int nNewClose;
            int nDayTrade;
            int nTradeType;
            int nSpecialTradeType;
            string strOrder;
            string strOrderNumerator;
            string strTrigger;
            string strTriggerNumerator;
            int nQty;

            if (txtTradeNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入交易所代號");
                return;
            }
            strTradeName = txtTradeNo.Text.Trim();

            if (txtStockNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strStockNo = txtStockNo.Text.Trim();

            if (txtYearMonth.Text.Trim() == "")
            {
                MessageBox.Show("請輸入年月");
                return;
            }
            strYearMonth = txtYearMonth.Text.Trim();


            if (txtYearMonth2.Text.Trim() == "")
            {
                MessageBox.Show("請輸入遠月年月");
                return;
            }
            strYearMonth2 = txtYearMonth2.Text.Trim();

            double dPrice = 0.0;
            if (double.TryParse(txtOrder.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("委託價請輸入數字");
                return;
            }
            strOrder = txtOrder.Text.Trim();

            if (double.TryParse(txtOrderNumerator.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("委託價分子請輸入數字");
                return;
            }
            strOrderNumerator = txtOrderNumerator.Text.Trim();

            //if (double.TryParse(txtTrigger.Text.Trim(), out dPrice) == false)
            //{
            //    MessageBox.Show("觸發價請輸入數字");
            //    return;
            //}
            strTrigger = txtTrigger.Text.Trim();

            //if (double.TryParse(txtTriggerNumerator.Text.Trim(), out dPrice) == false)
            //{
            //    MessageBox.Show("觸發價分子請輸入數字");
            //    return;
            //}
            strTriggerNumerator = txtTriggerNumerator.Text.Trim();


            if (int.TryParse(txtQty.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }

            if (boxBidAsk.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }
            nBuySell = boxBidAsk.SelectedIndex;

            if (boxNewClose.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇倉別");
                return;
            }
            nNewClose = boxNewClose.SelectedIndex;

            if (boxFlag.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇當沖與否");
                return;
            }
            nDayTrade = boxFlag.SelectedIndex;

            if (boxPeriod.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託條件");
                return;
            }
            nTradeType = boxPeriod.SelectedIndex;

            if (boxSpecialTradeType.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託類型");
                return;
            }
            nSpecialTradeType = boxSpecialTradeType.SelectedIndex;

            OVERSEAFUTUREORDER pOSOrder = new OVERSEAFUTUREORDER();

            pOSOrder.bstrFullAccount = m_UserAccount;
            pOSOrder.bstrExchangeNo = strTradeName;
            pOSOrder.bstrOrder = strOrder;
            pOSOrder.bstrOrderNumerator = strOrderNumerator;
            pOSOrder.bstrStockNo = strStockNo;
            pOSOrder.bstrTrigger = strTrigger;
            pOSOrder.bstrTriggerNumerator = strTriggerNumerator;
            pOSOrder.bstrYearMonth = strYearMonth;
            pOSOrder.bstrYearMonth2 = strYearMonth2;
            pOSOrder.nQty = nQty;
            pOSOrder.sBuySell = (short)nBuySell;
            pOSOrder.sDayTrade = (short)nDayTrade;
            pOSOrder.sNewClose = (short)nNewClose;
            pOSOrder.sSpecialTradeType = (short)nSpecialTradeType;
            pOSOrder.sTradeType = (short)nTradeType;

            if (OnOverseaFutureOrderSpreadSignal != null)
            {
                OnOverseaFutureOrderSpreadSignal(m_UserID, true, pOSOrder);
            }
        }

        private void btnGetOverseaFutureOpenInterest_Click(object sender, EventArgs e)
        {
            if (OnOpenInterestSignal != null)
            {
                OnOpenInterestSignal(m_UserID, m_UserAccount);
            }
        }

        private void btnGetOverseaFutures_Click(object sender, EventArgs e)
        {
            if (OnOverseaFuturesSignal != null)
            {
                OnOverseaFuturesSignal();
            }
        }

        private void btnCancelOrderBySeqNo_Click(object sender, EventArgs e)
        {
            if (OnCancelOrderSignal != null)
            {
                OnCancelOrderSignal(m_UserID,true,m_UserAccount,txtModifySeqNo.Text.Trim());
            }
        }

        private void btnCancelOrderByBookNo_Click(object sender, EventArgs e)
        {
            if (OnCancelOrderByBookSignal != null)
            {
                OnCancelOrderByBookSignal(m_UserID, true, m_UserAccount, txtModifyBookNo.Text.Trim());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (OnOverSeaFutureRightSignal != null)
            {
                OnOverSeaFutureRightSignal(m_UserID, m_UserAccount);
            }
        }

        private void OFCorrectPrice_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇海期帳號");
                return;
            }

            string strExchangeName;
            string strStockNo;
            string strYearMonth;
            int nBuySell;
            int nNewClose;
            int nDayTrade;
            int nTradeType;
            int nSpecialTradeType;
            string strOrder;
            string strOrderNumerator;
            string strOrderD;
            //string strStrike1;
            //int nCallPut1;
            //string strTrigger;
            //string strTriggerNumerator;
            //int nQty;

            if (txt_ExchangeNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入原委託交易所代號");
                return;
            }
            strExchangeName = txt_ExchangeNo.Text.Trim();

            if (txt_StockNo1.Text.Trim() == "")
            {
                MessageBox.Show("請輸入原委託商品代碼");
                return;
            }
            strStockNo = txt_StockNo1.Text.Trim();

            if (txt_YearM1.Text.Trim() == "")
            {
                MessageBox.Show("請輸入原委託年月");
                return;
            }
            strYearMonth = txt_YearM1.Text.Trim();

            double dPrice = 0.0;

            strOrder = txt_CorrectPrice.Text.Trim();


            if (boxSpecialTradeType.SelectedIndex == 0 )
            {

                if (double.TryParse(txt_CorrectPriceN.Text.Trim(), out dPrice) == false)
                {
                   MessageBox.Show("委託價分子請輸入數字");
                   return;
                }
            }
            strOrderNumerator = txt_CorrectPriceN.Text.Trim();

            
            if (boxSpecialTradeType.SelectedIndex == 0)
            {

               if (double.TryParse(txt_CorrectPriceD.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("委託價分母請輸入數字");
                    return;
                }
            }
            strOrderD = txt_CorrectPriceD.Text.Trim();
                                       
            

            if (box_NewClose_1.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇原委託倉別");
                return;
            }
            nNewClose = box_NewClose_1.SelectedIndex;

           

            if (box_TradeType_1.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇原委託委託條件");
                return;
            }
            nTradeType = box_TradeType_1.SelectedIndex;

            

            if (box_SpecOrderType.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇改價後委託類型");
                return;
            }
            nSpecialTradeType = box_SpecOrderType.SelectedIndex;

            OVERSEAFUTUREORDERFORGW pOFOrder = new OVERSEAFUTUREORDERFORGW();

            pOFOrder.bstrFullAccount = m_UserAccount;
            pOFOrder.bstrExchangeNo = strExchangeName;
            pOFOrder.bstrOrderPrice = strOrder;
            pOFOrder.bstrOrderNumerator = strOrderNumerator;
            pOFOrder.bstrOrderDenominator = strOrderD;
            pOFOrder.bstrStockNo = strStockNo;
            
            pOFOrder.bstrYearMonth = strYearMonth;
            
            pOFOrder.nNewClose = nNewClose;
            pOFOrder.nSpecialTradeType = nSpecialTradeType;
            pOFOrder.nTradeType = nTradeType;
            pOFOrder.bstrBookNo = txt_BookNo2.Text.Trim();
            pOFOrder.bstrSeqNo = txt_SeqNo2.Text.Trim();

           
            
            if (OnCorrectOrderByBookSignal != null)
            {
                OnCorrectOrderByBookSignal(m_UserID, true, pOFOrder);
            }
        }

        private void OF_SpreadCorrectPrice_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇海期帳號");
                return;
            }

            string strExchangeName;
            string strStockNo;
            string strStockNo2;
            string strYearMonth;
            string strYearMonth2;
            
            int nNewClose;
            
            int nTradeType;
            int nSpecialTradeType;
            string strOrder;
            string strOrderNumerator;
            string strOrderD;
            //string strTrigger;
            //string strTriggerNumerator;
            //int nQty;

            if (txt_ExchangeNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入原委託交易所代號");
                return;
            }
            strExchangeName = txt_ExchangeNo.Text.Trim();

            if (txt_StockNo1.Text.Trim() == "")
            {
                MessageBox.Show("請輸入原委託商品代碼");
                return;
            }
            strStockNo = txt_StockNo1.Text.Trim();

            if (txt_StockNo2.Text.Trim() == "")
            {
                MessageBox.Show("請輸入原委託第二腳商品代碼");
                return;
            }
            strStockNo2 = txt_StockNo2.Text.Trim();

            if (txt_YearM1.Text.Trim() == "")
            {
                MessageBox.Show("請輸入原委託年月");
                return;
            }
            strYearMonth = txt_YearM1.Text.Trim();

            if (txt_YearM2.Text.Trim() == "")
            {
                MessageBox.Show("請輸入原委託第二腳年月");
                return;
            }
            strYearMonth2 = txt_YearM2.Text.Trim();

            double dPrice = 0.0;

            strOrder = txt_CorrectPrice.Text.Trim();


            if (boxSpecialTradeType.SelectedIndex == 0 )
            {

                if (double.TryParse(txt_CorrectPriceN.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("委託價分子請輸入數字");
                    return;
                }
            }
            strOrderNumerator = txt_CorrectPriceN.Text.Trim();

            
            if (boxSpecialTradeType.SelectedIndex == 0)
            {

                if (double.TryParse(txt_CorrectPriceD.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("委託價分母請輸入數字");
                    return;
                }
            }
            strOrderD = txt_CorrectPriceD.Text.Trim();
                                       
            

            if (box_NewClose_1.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇原委託倉別");
                return;
            }
            nNewClose = box_NewClose_1.SelectedIndex;

            if (box_TradeType_1.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇原委託委託條件");
                return;
            }
            nTradeType = box_TradeType_1.SelectedIndex;

            if (box_SpecOrderType.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇改價後委託類型");
                return;
            }
            nSpecialTradeType = box_SpecOrderType.SelectedIndex;

            OVERSEAFUTUREORDERFORGW pOFOrder = new OVERSEAFUTUREORDERFORGW();

            pOFOrder.bstrFullAccount = m_UserAccount;
            pOFOrder.bstrExchangeNo = strExchangeName;
            pOFOrder.bstrOrderPrice = strOrder;
            pOFOrder.bstrOrderNumerator = strOrderNumerator;
            pOFOrder.bstrOrderDenominator = strOrderD;
            pOFOrder.bstrStockNo = strStockNo;
            pOFOrder.bstrStockNo2 = strStockNo2;
            pOFOrder.bstrYearMonth = strYearMonth;
            pOFOrder.bstrYearMonth2 = strYearMonth2;
            
            //pOFOrder.sBuySell = (short)nBuySell;
            //pOFOrder.sBuySell2 =(short)nBuySell2;

            //pOFOrder.sDayTrade = (short)nDayTrade;
            pOFOrder.nNewClose = nNewClose;
            pOFOrder.nSpecialTradeType = nSpecialTradeType;
            pOFOrder.nTradeType = nTradeType;

            pOFOrder.bstrBookNo = txt_BookNo2.Text.Trim();
            pOFOrder.bstrSeqNo = txt_SeqNo2.Text.Trim();
            
            if (OnCorrectSpreadOrderByBookSignal != null)
            {
                OnCorrectSpreadOrderByBookSignal(m_UserID, true, pOFOrder);
            }
        
        }

        private void btnSendOverseaFutureOrderAsyncOLID_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇海期帳號");
                return;
            }

            string strTradeName;
            string strStockNo;
            string strYearMonth;
            int nBuySell;
            int nNewClose;
            int nDayTrade;
            int nTradeType;
            int nSpecialTradeType;
            string strOrder;
            string strOrderNumerator;
            string strTrigger;
            string strTriggerNumerator;
            int nQty;

            if (txtTradeNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入交易所代號");
                return;
            }
            strTradeName = txtTradeNo.Text.Trim();

            if (txtStockNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strStockNo = txtStockNo.Text.Trim();

            if (txtYearMonth.Text.Trim() == "")
            {
                MessageBox.Show("請輸入年月");
                return;
            }
            strYearMonth = txtYearMonth.Text.Trim();

            double dPrice = 0.0;
            if (boxSpecialTradeType.SelectedIndex == 0 || boxSpecialTradeType.SelectedIndex == 2)
            {
                ;
                if (double.TryParse(txtOrder.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("委託價請輸入數字");
                    return;
                }
            }
            strOrder = txtOrder.Text.Trim();


            if (boxSpecialTradeType.SelectedIndex == 0 || boxSpecialTradeType.SelectedIndex == 2)
            {

                if (double.TryParse(txtOrderNumerator.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("委託價分子請輸入數字");
                    return;
                }
            }
            strOrderNumerator = txtOrderNumerator.Text.Trim();


            if (boxSpecialTradeType.SelectedIndex == 2 || boxSpecialTradeType.SelectedIndex == 3)
            {

                if (double.TryParse(txtTrigger.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("觸發價請輸入數字");
                    return;
                }
            }
            strTrigger = txtTrigger.Text.Trim();


            if (boxSpecialTradeType.SelectedIndex == 2 || boxSpecialTradeType.SelectedIndex == 3)
            {


                if (double.TryParse(txtTriggerNumerator.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("觸發價分子請輸入數字");
                    return;
                }
            }


            strTriggerNumerator = txtTriggerNumerator.Text.Trim();


            if (int.TryParse(txtQty.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }

            if (boxBidAsk.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }
            nBuySell = boxBidAsk.SelectedIndex;

            if (boxNewClose.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇倉別");
                return;
            }
            nNewClose = boxNewClose.SelectedIndex;

            if (boxFlag.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇當沖與否");
                return;
            }
            nDayTrade = boxFlag.SelectedIndex;

            if (boxPeriod.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託條件");
                return;
            }
            nTradeType = boxPeriod.SelectedIndex;

            if (boxSpecialTradeType.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託類型");
                return;
            }
            nSpecialTradeType = boxSpecialTradeType.SelectedIndex;

            OVERSEAFUTUREORDER pOSOrder = new OVERSEAFUTUREORDER();

            pOSOrder.bstrFullAccount = m_UserAccount;
            pOSOrder.bstrExchangeNo = strTradeName;
            pOSOrder.bstrOrder = strOrder;
            pOSOrder.bstrOrderNumerator = strOrderNumerator;
            pOSOrder.bstrStockNo = strStockNo;
            pOSOrder.bstrTrigger = strTrigger;
            pOSOrder.bstrTriggerNumerator = strTriggerNumerator;
            pOSOrder.bstrYearMonth = strYearMonth;
            pOSOrder.nQty = nQty;
            pOSOrder.sBuySell = (short)nBuySell;
            pOSOrder.sDayTrade = (short)nDayTrade;
            pOSOrder.sNewClose = (short)nNewClose;
            pOSOrder.sSpecialTradeType = (short)nSpecialTradeType;
            pOSOrder.sTradeType = (short)nTradeType;

            if (OnOverseaFutureOrderOLIDSignal != null)
            {
                OnOverseaFutureOrderOLIDSignal(m_UserID, true, pOSOrder, txtOrderLinkedID.Text);
            }
        }

        private void btnSendOverseaFutureSpreadOrderOLID_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇海期帳號");
                return;
            }

            string strTradeName;
            string strStockNo;
            string strYearMonth;
            string strYearMonth2;
            int nBuySell;
            int nNewClose;
            int nDayTrade;
            int nTradeType;
            int nSpecialTradeType;
            string strOrder;
            string strOrderNumerator;
            string strTrigger;
            string strTriggerNumerator;
            int nQty;

            if (txtTradeNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入交易所代號");
                return;
            }
            strTradeName = txtTradeNo.Text.Trim();

            if (txtStockNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strStockNo = txtStockNo.Text.Trim();

            if (txtYearMonth.Text.Trim() == "")
            {
                MessageBox.Show("請輸入年月");
                return;
            }
            strYearMonth = txtYearMonth.Text.Trim();


            if (txtYearMonth2.Text.Trim() == "")
            {
                MessageBox.Show("請輸入遠月年月");
                return;
            }
            strYearMonth2 = txtYearMonth2.Text.Trim();

            double dPrice = 0.0;
            if (double.TryParse(txtOrder.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("委託價請輸入數字");
                return;
            }
            strOrder = txtOrder.Text.Trim();

            if (double.TryParse(txtOrderNumerator.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("委託價分子請輸入數字");
                return;
            }
            strOrderNumerator = txtOrderNumerator.Text.Trim();

            //if (double.TryParse(txtTrigger.Text.Trim(), out dPrice) == false)
            //{
            //    MessageBox.Show("觸發價請輸入數字");
            //    return;
            //}
            strTrigger = txtTrigger.Text.Trim();

            //if (double.TryParse(txtTriggerNumerator.Text.Trim(), out dPrice) == false)
            //{
            //    MessageBox.Show("觸發價分子請輸入數字");
            //    return;
            //}
            strTriggerNumerator = txtTriggerNumerator.Text.Trim();


            if (int.TryParse(txtQty.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }

            if (boxBidAsk.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }
            nBuySell = boxBidAsk.SelectedIndex;

            if (boxNewClose.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇倉別");
                return;
            }
            nNewClose = boxNewClose.SelectedIndex;

            if (boxFlag.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇當沖與否");
                return;
            }
            nDayTrade = boxFlag.SelectedIndex;

            if (boxPeriod.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託條件");
                return;
            }
            nTradeType = boxPeriod.SelectedIndex;

            if (boxSpecialTradeType.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託類型");
                return;
            }
            nSpecialTradeType = boxSpecialTradeType.SelectedIndex;

            OVERSEAFUTUREORDER pOSOrder = new OVERSEAFUTUREORDER();

            pOSOrder.bstrFullAccount = m_UserAccount;
            pOSOrder.bstrExchangeNo = strTradeName;
            pOSOrder.bstrOrder = strOrder;
            pOSOrder.bstrOrderNumerator = strOrderNumerator;
            pOSOrder.bstrStockNo = strStockNo;
            pOSOrder.bstrTrigger = strTrigger;
            pOSOrder.bstrTriggerNumerator = strTriggerNumerator;
            pOSOrder.bstrYearMonth = strYearMonth;
            pOSOrder.bstrYearMonth2 = strYearMonth2;
            pOSOrder.nQty = nQty;
            pOSOrder.sBuySell = (short)nBuySell;
            pOSOrder.sDayTrade = (short)nDayTrade;
            pOSOrder.sNewClose = (short)nNewClose;
            pOSOrder.sSpecialTradeType = (short)nSpecialTradeType;
            pOSOrder.sTradeType = (short)nTradeType;

            if (OnOverseaFutureOrderSpreadOLIDSignal != null)
            {
                OnOverseaFutureOrderSpreadOLIDSignal(m_UserID, true, pOSOrder, txtOrderLinkedID.Text);
            }
        }

        private void OOCorrectPrice_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇海期帳號");
                return;
            }

            string strExchangeName;
            string strStockNo;
            string strYearMonth;
            int nBuySell;
            int nNewClose;
            int nDayTrade;
            int nTradeType;
            int nSpecialTradeType;
            string strOrder;
            string strOrderNumerator;
            string strOrderD;
            string strStrike1;
            int nCallPut1;
            //string strTrigger;
            //string strTriggerNumerator;
            //int nQty;

            if (txt_ExchangeNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入原委託交易所代號");
                return;
            }
            strExchangeName = txt_ExchangeNo.Text.Trim();

            if (txt_StockNo1.Text.Trim() == "")
            {
                MessageBox.Show("請輸入原委託商品代碼");
                return;
            }
            strStockNo = txt_StockNo1.Text.Trim();

            if (txt_YearM1.Text.Trim() == "")
            {
                MessageBox.Show("請輸入原委託年月");
                return;
            }
            strYearMonth = txt_YearM1.Text.Trim();

            double dPrice = 0.0;

            strOrder = txt_CorrectPrice.Text.Trim();


            if (boxSpecialTradeType.SelectedIndex == 0)
            {

                if (double.TryParse(txt_CorrectPriceN.Text.Trim(), out dPrice) == false)
                {
                        MessageBox.Show("委託價分子請輸入數字");
                        return;
                }
            }
            strOrderNumerator = txt_CorrectPriceN.Text.Trim();


            if (boxSpecialTradeType.SelectedIndex == 0)
            {

                if (double.TryParse(txt_CorrectPriceD.Text.Trim(), out dPrice) == false)
                {
                    MessageBox.Show("委託價分母請輸入數字");
                    return;
                }
            }
            strOrderD = txt_CorrectPriceD.Text.Trim();



            if (box_NewClose_1.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇原委託倉別");
                return;
            }
            nNewClose = box_NewClose_1.SelectedIndex;



            if (box_TradeType_1.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇原委託委託條件");
                return;
            }
            nTradeType = box_TradeType_1.SelectedIndex;

            if (boxCP1.SelectedIndex < 0)
            {
                   MessageBox.Show("請選擇原委託委託Call/Put");
                   return;
            }
            nCallPut1 = boxCP1.SelectedIndex;

            if (box_SpecOrderType.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇改價後委託類型");
                return;
            }
            nSpecialTradeType = box_SpecOrderType.SelectedIndex;

            OVERSEAFUTUREORDERFORGW pOFOrder = new OVERSEAFUTUREORDERFORGW();

            pOFOrder.bstrFullAccount = m_UserAccount;
            pOFOrder.bstrExchangeNo = strExchangeName;
            pOFOrder.bstrOrderPrice = strOrder;
            pOFOrder.bstrOrderNumerator = strOrderNumerator;
            pOFOrder.bstrOrderDenominator = strOrderD;
            pOFOrder.bstrStockNo = strStockNo;

            pOFOrder.bstrYearMonth = strYearMonth;

            pOFOrder.nNewClose = nNewClose;
            pOFOrder.nSpecialTradeType = nSpecialTradeType;
            pOFOrder.nTradeType = nTradeType;
            pOFOrder.bstrBookNo = txt_BookNo2.Text.Trim();
            pOFOrder.bstrSeqNo = txt_SeqNo2.Text.Trim();

            pOFOrder.bstrStrikePrice = txt_strike1.Text.Trim();
            pOFOrder.nCallPut = nCallPut1;

            if (OnCorrectOptionOrderByBookSignal != null)
            {
                OnCorrectOptionOrderByBookSignal(m_UserID, true, pOFOrder);
            }
        }

        private void btnCorrectPrice_Click(object sender, EventArgs e)
        {
            double dPrice = 0.0;
            if (double.TryParse(txtCorrectPrice.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("委託價請輸入數字");
                return;
            }
            else
            {
                if (OnCorrectPriceOrderSignal != null)
                {
                    OnCorrectPriceOrderSignal(m_UserID, true, m_UserAccount, txtModifySeqNo.Text.Trim(), txtCorrectPrice.Text.Trim());
                }
            }
        }



    }
}
