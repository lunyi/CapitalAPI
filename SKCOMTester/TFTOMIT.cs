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
    public partial class TFTOMIT : UserControl
    {
        #region Define Variable
        //----------------------------------------------------------------------
        // Define Variable
        //----------------------------------------------------------------------

        private int m_nCode;
        public string m_strMessage;

        public delegate void MyMessageHandler(string strType, int nCode, string strMessage);
        public event MyMessageHandler GetMessage;

        public delegate void OrderHandler(string strLogInID, bool bAsyncOrder, FUTUREORDER pOrder);
        public event OrderHandler OnFutureMITOrderSignal;

        public delegate void OptionOrderHandler(string strLogInID, bool bAsyncOrder, FUTUREORDER pOrder);
        public event OptionOrderHandler OnOptionMITOrderSignal;

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

        public TFTOMIT()
        {
            InitializeComponent();
        }

        private void btnSendFutureMITOrder_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇期貨帳號");
                return;
            }

            string strFutureNo;
            int nBidAsk;
            int nPeriod;
            int nFlag;
            string strDealPrice;
            string strTigger;
            int nQty;
            int nNewClose;            

            if (txtStockNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strFutureNo = txtStockNo.Text.Trim();

            if (boxBidAsk.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }

            if (boxNewCloseSTP.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇倉別");
                return;
            }
            nNewClose = boxNewCloseSTP.SelectedIndex;

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
            if (double.TryParse(txtDealPrice.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("成交價請輸入數字");
                return;
            }
            strDealPrice = txtDealPrice.Text.Trim();

            if (int.TryParse(txtQty.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }

            if (double.TryParse(txtTrigger.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("觸發價請輸入數字");
                return;
            }
            strTigger = txtTrigger.Text.Trim();



            FUTUREORDER pFutureOrder = new FUTUREORDER();

            pFutureOrder.bstrFullAccount = m_UserAccount;
            
            pFutureOrder.bstrStockNo = strFutureNo;
            pFutureOrder.nQty = nQty;
            pFutureOrder.sBuySell = (short)nBidAsk;
            pFutureOrder.sDayTrade = (short)nFlag;
            pFutureOrder.sNewClose = (short)nNewClose;
            pFutureOrder.sTradeType = (short)nPeriod;
            pFutureOrder.bstrTrigger = strTigger;
            pFutureOrder.bstrDealPrice = strDealPrice;

            pFutureOrder.bstrMovingPoint = "";
            pFutureOrder.bstrPrice = "";

            if (OnFutureMITOrderSignal != null)
            {
                OnFutureMITOrderSignal(m_UserID, false, pFutureOrder);
            }
        }

        private void btnSendOptionMITOrder_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇期貨帳號");
                return;
            }

            string strFutureNo;
            int nBidAsk;
            int nPeriod;
            int nFlag;
            string strDealPrice;
            string strTigger;
            int nQty;
            int nNewClose;

            if (txtOPStockNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strFutureNo = txtOPStockNo.Text.Trim();

            if (boxBidAsk3.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }

            if (boxNewCloseOP.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇倉別");
                return;
            }
            nNewClose = boxNewCloseOP.SelectedIndex;

            nBidAsk = boxBidAsk3.SelectedIndex;

            if (boxPeriod3.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託條件");
                return;
            }
            nPeriod = boxPeriod3.SelectedIndex;


            double dPrice = 0.0;
            if (double.TryParse(txtDealPrice2.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("成交價請輸入數字");
                return;
            }
            strDealPrice = txtDealPrice2.Text.Trim();

            if (int.TryParse(txtQty3.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }

            if (double.TryParse(txtTrigger3.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("觸發價請輸入數字");
                return;
            }
            strTigger = txtTrigger3.Text.Trim();



            FUTUREORDER pFutureOrder = new FUTUREORDER();

            pFutureOrder.bstrFullAccount = m_UserAccount;
            
            pFutureOrder.bstrStockNo = strFutureNo;
            pFutureOrder.nQty = nQty;
            pFutureOrder.sBuySell = (short)nBidAsk;
            
            pFutureOrder.sNewClose = (short)nNewClose;
            pFutureOrder.sTradeType = (short)nPeriod;
            pFutureOrder.bstrTrigger = strTigger;
            pFutureOrder.bstrDealPrice = strDealPrice;

            pFutureOrder.bstrMovingPoint = "";
            pFutureOrder.bstrPrice = "";

            if (OnOptionMITOrderSignal != null)
            {
                OnOptionMITOrderSignal(m_UserID, false, pFutureOrder);
            }
        
        }

        private void btnSendOptionMITOrderAsync_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇期貨帳號");
                return;
            }

            string strFutureNo;
            int nBidAsk;
            int nPeriod;
            
            string strDealPrice;
            string strTigger;
            int nQty;
            int nNewClose;

            if (txtOPStockNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strFutureNo = txtOPStockNo.Text.Trim();

            if (boxBidAsk3.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }

            if (boxNewCloseOP.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇倉別");
                return;
            }
            nNewClose = boxNewCloseOP.SelectedIndex;

            nBidAsk = boxBidAsk3.SelectedIndex;

            if (boxPeriod3.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託條件");
                return;
            }
            nPeriod = boxPeriod3.SelectedIndex;


            double dPrice = 0.0;
            if (double.TryParse(txtDealPrice2.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("成交價請輸入數字");
                return;
            }
            strDealPrice = txtDealPrice2.Text.Trim();

            if (int.TryParse(txtQty3.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }

            if (double.TryParse(txtTrigger3.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("觸發價請輸入數字");
                return;
            }
            strTigger = txtTrigger3.Text.Trim();



            FUTUREORDER pFutureOrder = new FUTUREORDER();

            pFutureOrder.bstrFullAccount = m_UserAccount;

            pFutureOrder.bstrStockNo = strFutureNo;
            pFutureOrder.nQty = nQty;
            pFutureOrder.sBuySell = (short)nBidAsk;

            pFutureOrder.sNewClose = (short)nNewClose;
            pFutureOrder.sTradeType = (short)nPeriod;
            pFutureOrder.bstrTrigger = strTigger;
            pFutureOrder.bstrDealPrice = strDealPrice;
            pFutureOrder.bstrMovingPoint = "";
            pFutureOrder.bstrPrice = "";

            if (OnOptionMITOrderSignal != null)
            {
                OnOptionMITOrderSignal(m_UserID, true, pFutureOrder);
            }
        }

        private void btnSendFutureMITOrderAsync_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇期貨帳號");
                return;
            }

            string strFutureNo;
            int nBidAsk;
            int nPeriod;
            int nFlag;
            string strDealPrice;
            string strTrigger;
            int nQty;
            int nNewClose;

            if (txtStockNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strFutureNo = txtStockNo.Text.Trim();

            if (boxBidAsk.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }

            if (boxNewCloseSTP.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇倉別");
                return;
            }
            nNewClose = boxNewCloseSTP.SelectedIndex;

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
            if (double.TryParse(txtDealPrice.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("成交價請輸入數字");
                return;
            }
            strDealPrice = txtDealPrice.Text.Trim();

            if (int.TryParse(txtQty.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }

            if (double.TryParse(txtTrigger.Text.Trim(), out dPrice) == false)
            {
                MessageBox.Show("觸發價請輸入數字");
                return;
            }
            strTrigger = txtTrigger.Text.Trim();



            FUTUREORDER pFutureOrder = new FUTUREORDER();

            pFutureOrder.bstrFullAccount = m_UserAccount;

            pFutureOrder.bstrStockNo = strFutureNo;
            pFutureOrder.nQty = nQty;
            pFutureOrder.sBuySell = (short)nBidAsk;
            pFutureOrder.sDayTrade = (short)nFlag;
            pFutureOrder.sNewClose = (short)nNewClose;
            pFutureOrder.sTradeType = (short)nPeriod;
            pFutureOrder.bstrTrigger = strTrigger;
            pFutureOrder.bstrDealPrice = strDealPrice;
            pFutureOrder.bstrMovingPoint = "";
            pFutureOrder.bstrPrice = "";

            if (OnFutureMITOrderSignal != null)
            {
                OnFutureMITOrderSignal(m_UserID, false, pFutureOrder);
            }
        }
    }
}
