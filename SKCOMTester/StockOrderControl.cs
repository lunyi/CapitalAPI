using System;
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
    public partial class StockOrderControl : UserControl
    {
        #region Define Variable
        //----------------------------------------------------------------------
        // Define Variable
        //----------------------------------------------------------------------
        
        private int m_nCode;
        public string m_strMessage;

        public delegate void MyMessageHandler(string strType, int nCode, string strMessage);
        public event MyMessageHandler GetMessage;

        public delegate void OrderHandler(string strLogInID, bool bAsyncOrder, STOCKORDER pStock);
        public event OrderHandler OnOrderSignal;

        public delegate void DecreaseOrderHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo, int nDecreaseQty );
        public event DecreaseOrderHandler OnDecreaseOrderSignal;

        public delegate void CancelOrderHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo);
        public event CancelOrderHandler OnCancelOrderSignal;

        public delegate void CancelOrderByStockHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strStockNo);
        public event CancelOrderByStockHandler OnCancelOrderByStockSignal;

        public delegate void RealBalanceHandler(string strLogInID, string strAccount);
        public event RealBalanceHandler OnRealBalanceSignal;

        public delegate void RequestProfitReportHandler(string strLogInID, string strAccount);
        public event RequestProfitReportHandler OnRequestProfitReportSignal;

        public delegate void RequestAmountLimitHandler(string strLogInID, string strAccount, string strStockNo);
        public event RequestAmountLimitHandler OnRequestAmountLimitSignal;

        public delegate void RequestBalanceQueryHandler(string strLogInID, string strAccount, string strStockNo);
        public event RequestBalanceQueryHandler OnRequestBalanceQuerySignal;

        public delegate void CancelOrderByBookHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strBookNo);
        public event CancelOrderByBookHandler OnCancelOrderByBookSignal;

        //public delegate void ContinuousTradingHandler(string strLogInID, bool bAsyncOrder, STOCKORDER pStock);
        //public event ContinuousTradingHandler OnContinuousTradingSignal;

        //public delegate void CancelOrderBySeqNoContinuousTradingHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo);
        //public event CancelOrderBySeqNoContinuousTradingHandler OnCancelOrderBySeqNoContinuousTradingSignal;

        //public delegate void DecreaseOrderContinuousTradingHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strSymbol, string strBookNo, int nDecreaseQty);
        //public event DecreaseOrderContinuousTradingHandler OnDecreaseOrderContinuousTradingSignal;

        //public delegate void CorrectPriceBySeqNoContinuousTradingHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strSymbol, string strSeqNo, string strPrice, int nTradeType);
        //public event CorrectPriceBySeqNoContinuousTradingHandler OnCorrectPriceBySeqNoContinuousTrading;

        //public delegate void CorrectPriceByBookNoContinuousTradingHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strSymbol, string strBookNo, string strPrice, int nTradeType);
        //public event CorrectPriceByBookNoContinuousTradingHandler OnCorrectPriceByBookNoContinuousTrading;

        public delegate void CorrectPriceBySeqNoHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strSeqNo, string strPrice, int nTradeType);
        public event CorrectPriceBySeqNoHandler OnCorrectPriceBySeqNo;

        public delegate void CorrectPriceByBookNoHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strSymbol, string strSeqNo, string strPrice, int nTradeType);
        public event CorrectPriceByBookNoHandler OnCorrectPriceByBookNo;

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
        public StockOrderControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Component Event
        //----------------------------------------------------------------------
        // Component Event
        //----------------------------------------------------------------------
        private void btnSendStockOrder_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇證券帳號");
                return;
            }

            
            string strStockNo;
            int nPrime;
            int nBidAsk;
            int nPeriod;
            int nFlag;
            string strPrice;
            int nQty;


            if (txtStockNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strStockNo = txtStockNo.Text.Trim();

            if (boxPrime.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇上市櫃-興櫃");
                return;
            }
            nPrime = boxPrime.SelectedIndex;

            if (boxBidAsk.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }
            nBidAsk = boxBidAsk.SelectedIndex;

            if (boxPeriod.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託條件");
                return;
            }
            nPeriod = boxPeriod.SelectedIndex;

            if (boxFlag.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇當沖與否");
                return;
            }
            nFlag = boxFlag.SelectedIndex;

            double dPrice = 0.0;
            if (double.TryParse(txtPrice.Text.Trim(), out dPrice) == false
                && txtPrice.Text.Trim() != "M"
                && txtPrice.Text.Trim() != "H"
                && txtPrice.Text.Trim() != "h"
                && txtPrice.Text.Trim() != "C"
                && txtPrice.Text.Trim() != "c"
                && txtPrice.Text.Trim() != "L"
                && txtPrice.Text.Trim() != "l")
            {
                MessageBox.Show("委託價請輸入數字");
                return;
            }
            strPrice = txtPrice.Text.Trim();

            if (int.TryParse(txtQty.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }

            if (m_bOrderM)
            {

                int nCond, nSpecTradeType;

                if (boxCond.SelectedIndex < 0)
                {
                    MessageBox.Show("請選擇委託條件(R/I/F)");
                    return;
                }
                nCond = boxCond.SelectedIndex;

                if (boxSpecialTradeType.SelectedIndex < 0)
                {
                    MessageBox.Show("請選擇委託價格類型(限/市價)");
                    return;
                }
                nSpecTradeType = boxSpecialTradeType.SelectedIndex;


                SKCOMLib.STOCKORDER pOrder = new STOCKORDER();

                pOrder.bstrFullAccount = m_UserAccount;
                pOrder.bstrPrice = strPrice;
                pOrder.bstrStockNo = strStockNo;
                pOrder.nQty = nQty;
                pOrder.sPrime = (short)nPrime;
                pOrder.sBuySell = (short)nBidAsk;
                pOrder.sFlag = (short)nFlag;
                pOrder.sPeriod = (short)nPeriod;
                pOrder.nTradeType = nCond;
                pOrder.nSpecialTradeType = nSpecTradeType;

                if (OnOrderSignal != null)
                {
                    OnOrderSignal(m_UserID, false, pOrder);
                }
            
            
            }
            else
            {
                SKCOMLib.STOCKORDER pOrder = new STOCKORDER();

                pOrder.bstrFullAccount = m_UserAccount;
                pOrder.bstrPrice = strPrice;
                pOrder.bstrStockNo = strStockNo;
                pOrder.nQty = nQty;
                pOrder.sPrime = (short)nPrime;
                pOrder.sBuySell = (short)nBidAsk;
                pOrder.sFlag = (short)nFlag;
                pOrder.sPeriod = (short)nPeriod;

                if (OnOrderSignal != null)
                {
                    OnOrderSignal(m_UserID, false, pOrder);
                }
            }
        }

        private void btnSendStockOrderAsync_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇證券帳號");
                return;
            }

            string strStockNo;
            int nPrime;
            int nBidAsk;
            int nPeriod;
            int nFlag;
            string strPrice;
            int nQty;

            if (txtStockNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strStockNo = txtStockNo.Text.Trim();

            if (boxPrime.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇上市櫃-興櫃");
                return;
            }
            nPrime = boxPrime.SelectedIndex;

            if (boxBidAsk.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }
            nBidAsk = boxBidAsk.SelectedIndex;

            if (boxPeriod.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託條件");
                return;
            }
            nPeriod = boxPeriod.SelectedIndex;

            if (boxFlag.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇當沖與否");
                return;
            }
            nFlag = boxFlag.SelectedIndex;

            double dPrice = 0.0;
            if (double.TryParse(txtPrice.Text.Trim(), out dPrice) == false
                && txtPrice.Text.Trim() != "M"
                && txtPrice.Text.Trim() != "H"
                && txtPrice.Text.Trim() != "h"
                && txtPrice.Text.Trim() != "C"
                && txtPrice.Text.Trim() != "c"
                && txtPrice.Text.Trim() != "L"
                && txtPrice.Text.Trim() != "l")
            {
                MessageBox.Show("委託價請輸入數字");
                return;
            }
            strPrice = txtPrice.Text.Trim();

            if (int.TryParse(txtQty.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }

            if (m_bOrderM)
            {

                int nCond, nSpecTradeType;

                if (boxCond.SelectedIndex < 0)
                {
                    MessageBox.Show("請選擇委託條件(R/I/F)");
                    return;
                }
                nCond = boxCond.SelectedIndex;

                if (boxSpecialTradeType.SelectedIndex < 0)
                {
                    MessageBox.Show("請選擇委託價格類型(限/市價)");
                    return;
                }
                nSpecTradeType = boxSpecialTradeType.SelectedIndex;


                SKCOMLib.STOCKORDER pOrder = new STOCKORDER();

                pOrder.bstrFullAccount = m_UserAccount;
                pOrder.bstrPrice = strPrice;
                pOrder.bstrStockNo = strStockNo;
                pOrder.nQty = nQty;
                pOrder.sPrime = (short)nPrime;
                pOrder.sBuySell = (short)nBidAsk;
                pOrder.sFlag = (short)nFlag;
                pOrder.sPeriod = (short)nPeriod;
                pOrder.nTradeType = nCond;
                pOrder.nSpecialTradeType = nSpecTradeType;

                if (OnOrderSignal != null)
                {
                    OnOrderSignal(m_UserID, true, pOrder);
                }


            }
            else
            {


                SKCOMLib.STOCKORDER pOrder = new STOCKORDER();

                pOrder.bstrFullAccount = m_UserAccount;
                pOrder.bstrPrice = strPrice;
                pOrder.bstrStockNo = strStockNo;
                pOrder.nQty = nQty;
                pOrder.sPrime = (short)nPrime;
                pOrder.sBuySell = (short)nBidAsk;
                pOrder.sFlag = (short)nFlag;
                pOrder.sPeriod = (short)nPeriod;

                if (OnOrderSignal != null)
                {
                    OnOrderSignal(m_UserID, true, pOrder);
                }
            }
        }

        #endregion

        private void btnDecreaseQty_Click(object sender, EventArgs e)
        {
            int nQty = 0;
            
            if( int.TryParse(txtDecreaseQty.Text.Trim(),out nQty) == false)
            {
                MessageBox.Show("改量請輸入數字");
            }


            //if (m_bOrderM)
            //{
                    //if (MarketBox.SelectedIndex < 0)
                    //{
                    //    MessageBox.Show("請選擇市場簡稱");
                    //    return;
                    //}

            //    if (OnDecreaseOrderContinuousTradingSignal != null)
            //    {
            //         OnDecreaseOrderContinuousTradingSignal(m_UserID, true, m_UserAccount, MarketBox.Text.Trim(), txtDecreaseBookNo.Text.Trim(), nQty);
            //    }

            //}
            //else            
            {
                if (OnDecreaseOrderSignal != null)
                {
                    OnDecreaseOrderSignal(m_UserID, true, m_UserAccount, txtDecreaseBookNo.Text.Trim(), nQty);
                }
            }
        }

        private void btnCancelOrderBySeqNo_Click(object sender, EventArgs e)
        {
            //if (m_bOrderM)
            //{
            //    if (OnCancelOrderBySeqNoContinuousTradingSignal != null)
            //    {
            //        OnCancelOrderBySeqNoContinuousTradingSignal(m_UserID, true, m_UserAccount, txtCancelSeqNo.Text.Trim());
            //    }

            //}
            //else
            {
                if (OnCancelOrderSignal != null)
                {
                    OnCancelOrderSignal(m_UserID, true, m_UserAccount, txtCancelSeqNo.Text.Trim());
                }
            }
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (OnCancelOrderByStockSignal != null)
            {
                OnCancelOrderByStockSignal(m_UserID, true, m_UserAccount, txtCancelStockNo.Text.Trim());
            }
        }

        private void btnGetRealBalanceReport_Click(object sender, EventArgs e)
        {
            if (OnRealBalanceSignal != null)
            {
                OnRealBalanceSignal(m_UserID, m_UserAccount);
            }
        }

        private void btnGetRequestProfitReport_Click(object sender, EventArgs e)
        {
            if (OnRequestProfitReportSignal != null)
            {
                OnRequestProfitReportSignal(m_UserID, m_UserAccount);
            }
        }

        private void btnGetAmountLimit_Click(object sender, EventArgs e)
        {
            string strStockNo = txtAmountLimitStockNo.Text;
            if (OnRequestAmountLimitSignal != null)
            {
                OnRequestAmountLimitSignal(m_UserID, m_UserAccount, strStockNo);
            }
        }

        private void GetBalanceQueryReport_Click(object sender, EventArgs e)
        {
            string strStockNo = txtBalanceQueryStockNo.Text;
            if (OnRequestBalanceQuerySignal != null)
            {
                OnRequestBalanceQuerySignal(m_UserID, m_UserAccount, strStockNo);
            }
        }

        private void btnCancelOrderByBookNo_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇期貨帳號");
                return;
            }
            string strBookNo;

            if (txtCancelBookNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入委託書號");
                return;
            }
            strBookNo = txtCancelBookNo.Text.Trim();
            if (OnCancelOrderByBookSignal != null)
            {
                OnCancelOrderByBookSignal(m_UserID, true, m_UserAccount, strBookNo);
            }
        }

        private void btnCorrectPriceBySeqNo_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇證券帳號");
                return;
            }
            
            int nTradeType;
            string strSeqNo;
            string strPrice;

            if (txtCorrectSeqNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入委託序號");
                return;
            }
            strSeqNo = txtCorrectSeqNo.Text.Trim();

            double dPrice = 0.0;
            if (double.TryParse(txtCorrectPrice.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("修改價格請輸入數字");
                return;
            }
            strPrice = txtCorrectPrice.Text.Trim();

            
            if (boxCorrectTradeType.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託條件");
                return;
            }
            nTradeType = boxCorrectTradeType.SelectedIndex;

               
           
            
            if (m_bOrderM)
            {
                if (OnCorrectPriceBySeqNo != null)
                {
                    OnCorrectPriceBySeqNo(m_UserID, true, m_UserAccount,  strSeqNo, strPrice, nTradeType);
                }

            }
            else
            {
                //
            }
            
        }

        private void btnCorrectPriceByBookNo_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇證券帳號");
                return;
            }
            
            int nTradeType;
            string strBookNo;
            string strPrice;

            if (txtCorrectBookNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入委託書號");
                return;
            }
            strBookNo = txtCorrectBookNo.Text.Trim();

            double dPrice = 0.0;
            if (double.TryParse(txtCorrectPrice.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("修改價格請輸入數字");
                return;
            }
            strPrice = txtCorrectPrice.Text.Trim();

            if (boxCorrectSymbol.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇市場簡稱");
                return;
            }
            nTradeType = boxCorrectTradeType.SelectedIndex;

            if (boxCorrectTradeType.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託條件");
                return;
            }

            //
            
            if (m_bOrderM)
            {
                if (OnCorrectPriceByBookNo != null)
                {
                    OnCorrectPriceByBookNo(m_UserID, true, m_UserAccount, boxCorrectSymbol.Text.Trim(), strBookNo, strPrice, nTradeType);
                }

            }
            else
            { 
            //
            }
        }

    }
}
