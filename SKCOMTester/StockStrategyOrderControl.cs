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
    public partial class StockStrategyOrderControl : UserControl
    {
        #region Define Variable
        //----------------------------------------------------------------------
        // Define Variable
        //----------------------------------------------------------------------

        private int m_nCode;
        public string m_strMessage;


        public delegate void MyMessageHandler(string strType, int nCode, string strMessage);
        public event MyMessageHandler GetMessage;

        public delegate void StockDayTradeOrderHandler(string strLogInID, bool bAsyncOrder, STOCKSTRATEGYORDER pOrder);
        public event StockDayTradeOrderHandler OnStockStrategyDayTradeSignal;

        public delegate void StockAllCleanOutOrderHandler(string strLogInID, bool bAsyncOrder, STOCKSTRATEGYORDEROUT pOrder);
        public event StockAllCleanOutOrderHandler OnStockStrategyAllCleanOutSignal;

        public delegate void TSStrategyReportHandler(string strLogInID, string strAccount, string strMarketType, int nReportStatus, string strKind, string strDate);
        public event TSStrategyReportHandler OnTSStrategyReportSignal;

        public delegate void CancelTSStrategyOrderHandler(string strLogInID, bool bAsyncOrder, string strAccount, string strSmartKey, int nTradeType);
        public event CancelTSStrategyOrderHandler OnCancelTSStrategyOrderSignal;

        

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

        public StockStrategyOrderControl()
        {
            InitializeComponent();
            boxKindReport.SelectedIndex = 0;
            KeyTypeBox.SelectedIndex = 0;
            
        }

        private void btnDayTrade_Click(object sender, EventArgs e)
        {
            
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇證券帳號");
                return;
            }                     
            
	        string	strStockNo;		        //委託股票代號
	        int     nTradeType=0;		    //交易種類,一律為整股
	        int     nOrderType=0;			//委託-0:現;1:資;2:券
	        int     nBidAsk=0;			    //0買-1賣出-2無券賣出;出清現賣-資賣-券買	
            int     nQty=0;
	        int     nIsWarrant=0;			//權證
	        int	    nClearAllFlag=0;		//*clear all flag:
            string strClearCancelTime = "";	//*clear time
            int nFinalClearFlag = 0;		//*Final Clear flag
            string strClearAllOrderPrice = "";	//出清委託價	
	        //當沖條件//
            string strOrderPrice ="";			//委託價
            int nTakeProfitFlag = 0;			//停利Flag
            int nRDOTPPercent = 0;				//停利類型
            string strTPPercent = "";			//停利百分比
            string strTPTrigger = "";			//停利觸發價
	        int     nRDTPMarketPriceType=0;		//停利委託價方式=1:市價 2:限價 3:限價委託價
            string strTPOrderPrice = "";		//停利委託價
            int nStopLossFlag = 0;				//停損Flag
            int nRDOSLPercent = 0;				//停損類型
            string strSLPercent = "";			//停損百分比值
            string strSLTrigger = "";			//停損觸發價
            int nRDSLMarketPriceType = 0;		//停損委託價方式=1:市價 2:限價 3:限價委託價
	        string	strSLOrderPrice="";			//停損委託價	
            int nClearAllPriceType = 0;			//出清委託價方式=1:市價 2:限價
	

            if (txtStockNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strStockNo = txtStockNo.Text.Trim();

            if (boxBidAsk.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }
            nBidAsk = boxBidAsk.SelectedIndex;            
            
            double dOrderPrice = 0.0;
            if (double.TryParse(txtOrderPrice.Text.Trim(), out dOrderPrice) == false )
            {
                MessageBox.Show("委託價請輸入數字");
                return;
            }
            strOrderPrice = txtOrderPrice.Text.Trim();

            if (int.TryParse(txtQty.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }


            if (ckBoxLoss.Checked)                
            {

                double dLossPercent = 0.0;
                double dTriggerLoss = 0.0;
                nStopLossFlag = 1;
                if (radioLoss.Checked)
                {// 1:漲跌幅 0:觸發價
                    if (double.TryParse(txtLossPercent.Text.Trim(), out dLossPercent) == false)
                    {
                        MessageBox.Show("停損跌幅請輸入數字");
                        return;
                    } 
                    nRDOSLPercent = 1;
                    strSLPercent = txtLossPercent.Text.Trim();
                }
                else if (radioTriggerLoss.Checked) 
                {
                    nRDOSLPercent = 0;
                    if (double.TryParse(txtTriggerLoss.Text.Trim(), out dTriggerLoss) == false)
                    {
                        MessageBox.Show("停損觸發價請輸入數字");
                        return;                
                    }
                    strSLTrigger = txtTriggerLoss.Text.Trim();
                }
                if (radioOrderLoss.Checked)
                {//flag 0:None 1:市價 2:限價(以選定漲跌停為主)               
                    nRDSLMarketPriceType = 2;

                    if (double.TryParse(txtLossOrderPrice.Text.Trim(), out dTriggerLoss) == false)
                    {
                        MessageBox.Show("停損委託價請輸入數字");
                        return;                
                    }
                    strSLOrderPrice = txtLossOrderPrice.Text.Trim();                    
                }
                else if(radioOrderTypeLoss.Checked)
                {
                    nRDSLMarketPriceType = 1;
                }
            }
            else
                nStopLossFlag = 0;

            
            double dProfitPercent = 0.0;
            double dTriggerProfit = 0.0;
            if (ckboxProfit.Checked)
            {
                nTakeProfitFlag = 1;
                if (radioProfit.Checked)
                {// 1:漲跌幅 0:觸發價
                    nRDOTPPercent = 1;
                    
                    if (double.TryParse(txtProfitPercent.Text.Trim(), out dProfitPercent) == false)
                    {
                        MessageBox.Show("停利漲幅請輸入數字");
                        return;                
                    }                    
                    strTPPercent = txtProfitPercent.Text.Trim();
                }                
                else if (radioTriggerProfit.Checked) 
                {
                    nRDOTPPercent = 0;
                    if (double.TryParse(txtTriggerProfit.Text.Trim(), out dTriggerProfit) == false)
                    {
                        MessageBox.Show("停損觸發價請輸入數字");
                        return;                
                    }
                    strTPTrigger = txtTriggerProfit.Text.Trim();
                }
                if (radioOrderProfit.Checked)
                {//flag 0:None 1:市價 2:限價(以選定漲跌停為主)               
                    nRDTPMarketPriceType = 2;

                    if (double.TryParse(txtProfitOrderPrice.Text.Trim(), out dTriggerProfit) == false)
                    {
                        MessageBox.Show("停利委託價請輸入數字");
                        return;                
                    }
                    strTPOrderPrice = txtProfitOrderPrice.Text.Trim();                    
                }
                else if(RadioOrderTypeProfit.Checked)
                {
                    nRDTPMarketPriceType = 1;
                }
            }
            else
                nTakeProfitFlag = 0;
            if(CkBoxClear1.Checked)
            {
                nClearAllFlag = 1;
                strClearCancelTime = boxHr1.Text.Trim() + boxMin1.Text.Trim();
                if(radioClearType.Checked)
                    nClearAllPriceType = 1;
                else if (radioOrderClear1.Checked)
                {
                    nClearAllPriceType = 2;
                    if(double.TryParse(txtClearOrder.Text.Trim(), out dTriggerProfit) == false)
                    {
                        MessageBox.Show("出清委託價請輸入數字");
                        return;                
                    }
                    strClearAllOrderPrice = txtClearOrder.Text.Trim();
                }
            }
            else 
                nClearAllFlag= 0;

            if(ckBoxFinal.Checked)
                nFinalClearFlag = 1;
            else
                nFinalClearFlag = 0;

            STOCKSTRATEGYORDER pStockOrder = new STOCKSTRATEGYORDER();

            pStockOrder.bstrFullAccount = m_UserAccount;
            pStockOrder.bstrStockNo = strStockNo;
            pStockOrder.nBuySell = nBidAsk;
            pStockOrder.nQty = nQty;
            //tockOrder.sIsWarrant = (short)nIsWarrant;
            pStockOrder.bstrOrderPrice = strOrderPrice;
            pStockOrder.nTakeProfitFlag = nTakeProfitFlag;
            pStockOrder.nRDOTPPercent = nRDOTPPercent;
            pStockOrder.bstrTPPercent = strTPPercent;
            pStockOrder.bstrTPTrigger = strTPTrigger;
            pStockOrder.nRDTPMarketPriceType = nRDTPMarketPriceType;            
            pStockOrder.bstrTPOrderPrice = strTPOrderPrice;

            pStockOrder.nStopLossFlag = nStopLossFlag;
            pStockOrder.nRDOSLPercent = nRDOSLPercent;
            pStockOrder.bstrSLPercent = strSLPercent;
            pStockOrder.bstrSLTrigger = strSLTrigger;
            pStockOrder.nRDSLMarketPriceType = nRDSLMarketPriceType;
            pStockOrder.bstrSLOrderPrice = strSLOrderPrice;

            pStockOrder.nClearAllFlag = nClearAllFlag;
            pStockOrder.bstrClearCancelTime = strClearCancelTime;
            pStockOrder.nClearAllPriceType = nClearAllPriceType;
            pStockOrder.bstrClearAllOrderPrice = strClearAllOrderPrice;

            pStockOrder.nFinalClearFlag = nFinalClearFlag;

            if (OnStockStrategyDayTradeSignal != null)
            {
                OnStockStrategyDayTradeSignal(m_UserID, false, pStockOrder);
            }
        }

        private void btnAllCleanOut_Click(object sender, EventArgs e)
        {

            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇證券帳號");
                return;
            }
                        
            string strStockNo;		//委託股票代號
            int nTradeType;		    //交易種類,一律為整股
            int nOrderType;			//委託-0:現;1:資;2:券
            int nBidAsk;			//0買-1賣出-2無券賣出;出清現賣-資賣-券買	
            
            int nQty;
            int nIsWarrant;			//權證
            int nClearAllFlag;			//*clear all flag
            string strClearCancelTime="";	//*clear time
            int nFinalClearFlag;		//*Final Clear flag
            string strClearAllOrderPrice="";	//出清委託價	
            //出清條件//
            int nLTEFlag;					//LTE flag
            string strLTETriggerPrice="";		//LTE
            string strLTEOrderPrice="";		//LTE
            int nLTEMarketPrice = 0;			//Market Price flag
            int nGTEFlag;					//BTE flag
            string strGTETriggerPrice="";		//BTE
            string strGTEOrderPrice="";		//BTE
            int nGTEMarketPrice = 0;			//BTE Market Price flag

            int nTimeClearMarketPrice=0;		//Time Clear Market Price flag1:市價 2:限價



            if (txtStockNo2.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strStockNo = txtStockNo2.Text.Trim();

            if (boxBidAsk2.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }
            nBidAsk = boxBidAsk2.SelectedIndex;

            if (boxOrderType.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託別");
                return;
            }

            nOrderType = boxOrderType.SelectedIndex;

            double dOrderPrice = 0.0;


            if (int.TryParse(txtQty2.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }               

            if (boxGTE.Checked)
            {

                double dOrderGTE = 0.0;

                nGTEFlag = 1;
                if (double.TryParse(txtGTEPrice.Text.Trim(), out dOrderGTE) == false)
                {
                    MessageBox.Show("請輸入成交價大於條件-數字");
                    return;
                }
                strGTETriggerPrice = txtGTEPrice.Text.Trim();

                if (radioGTEOrder.Checked)
                {// 1:漲跌幅 0:觸發價

                    nGTEMarketPrice = 0;

                    if (double.TryParse(txtGTEOrder.Text.Trim(), out dOrderGTE) == false)
                    {
                        MessageBox.Show("請輸入成交價大於條件-委託價");
                        return;
                    }

                    strGTEOrderPrice = txtGTEOrder.Text.Trim();
                }
                else if (radioGTEOrderType.Checked)
                {
                    nGTEMarketPrice = 1;
                }
            }
            else
            {
                nGTEFlag = 0;
            }

            double dLTEPrice = 0.0;
            double dTriggerPrice = 0.0;
            if (boxLTE.Checked)
            {

                nLTEFlag = 1;
                if (double.TryParse(txtLTEPrice.Text.Trim(), out dLTEPrice) == false)
                {
                    MessageBox.Show("請輸入成交價小於條件-數字");
                    return;
                }
                strLTETriggerPrice = txtLTEPrice.Text.Trim();

                if (radioLTEOrder.Checked)
                {// 1:漲跌幅 0:觸發價

                    nLTEMarketPrice = 0;

                    if (double.TryParse(txtLTEOrder.Text.Trim(), out dTriggerPrice) == false)
                    {
                        MessageBox.Show("請輸入成交價小於條件-委託價");
                        return;
                    }

                    strLTEOrderPrice = txtLTEOrder.Text.Trim();
                    
                }
                else if (radioLTEOrderType.Checked)
                {
                    nLTEMarketPrice = 1;
                    
                }
                
            }
            else
            {
                nLTEFlag = 0;
            }

            if (boxClearOut2.Checked)
            {
                nClearAllFlag = 1;
                strClearCancelTime = boxHr2.Text.Trim() + boxMin2.Text.Trim();
                string strHr2 = boxHr2.Text.Trim();
                int nHrCancelTime = int.Parse(strHr2) * 10000;
                string strMin2 = boxMin2.Text.Trim();
                int nMinCancelTime = int.Parse(strMin2) * 100;
                if (nHrCancelTime + nMinCancelTime > 132000)
                {
                    MessageBox.Show("選擇出清時間已超過13:20,請重新選擇");
                    return;
                }
                if (radioClearType2.Checked)
                {
                    nTimeClearMarketPrice = 1;
                }
                else if (radioClearOrder2.Checked)
                {
                    nTimeClearMarketPrice = 2;
                    if (double.TryParse(txtClearOrder2.Text.Trim(), out dTriggerPrice) == false)
                    {
                        MessageBox.Show("出清委託價請輸入數字");
                        return;
                    }
                    strClearAllOrderPrice = txtClearOrder2.Text.Trim();
                }
            }
            else
            {
                nClearAllFlag = 0;
            }

            if (ckBoxFinal2.Checked)
            {
                nFinalClearFlag = 1;
            }
            else
            {
                nFinalClearFlag = 0;
            }

            STOCKSTRATEGYORDEROUT pStockOrder = new STOCKSTRATEGYORDEROUT();
            pStockOrder.bstrFullAccount = m_UserAccount;
            pStockOrder.bstrStockNo = strStockNo;
            pStockOrder.nBuySell = nBidAsk;
            pStockOrder.nOrderType = nOrderType;
            
            pStockOrder.nQty = nQty;            

            pStockOrder.nLTEFlag = nLTEFlag;
            pStockOrder.bstrLTETriggerPrice = strLTETriggerPrice;
            pStockOrder.nLTEMarketPrice = nLTEMarketPrice;
            pStockOrder.bstrLTEOrderPrice = strLTEOrderPrice;

            pStockOrder.nGTEFlag = nGTEFlag;
            pStockOrder.bstrGTETriggerPrice = strGTETriggerPrice;
            pStockOrder.nGTEMarketPrice = nGTEMarketPrice;
            pStockOrder.bstrGTEOrderPrice = strGTEOrderPrice;

            pStockOrder.nClearAllFlag = nClearAllFlag;
            pStockOrder.bstrClearCancelTime = strClearCancelTime;
            pStockOrder.nClearAllPriceType = nTimeClearMarketPrice;
            pStockOrder.bstrClearAllOrderPrice = strClearAllOrderPrice;

            pStockOrder.nFinalClearFlag = nFinalClearFlag;

            if (OnStockStrategyAllCleanOutSignal != null)
            {
                OnStockStrategyAllCleanOutSignal(m_UserID, false, pStockOrder);
            }
            
        }

        private void btnDayTradeAsync_Click(object sender, EventArgs e)
        {

            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇證券帳號");
                return;
            }

            string strStockNo;		    //委託股票代號
            
            int nBidAsk = 0;			//0買-1賣出-2無券賣出;出清現賣-資賣-券買	
            int nQty = 0;
            
            int nClearAllFlag = 0;			//*clear all flag
            string strClearCancelTime = "";	//*clear time
            int nFinalClearFlag = 0;		//*Final Clear flag
            string strClearAllOrderPrice = "";	//出清委託價	
            //當沖條件//
            string strOrderPrice = "";			//委託價
            int nTakeProfitFlag = 0;			//停利Flag
            int nRDOTPPercent = 0;				//停利類型
            string strTPPercent = "";			//停利百分比
            string strTPTrigger = "";			//停利觸發價
            int nRDTPMarketPriceType = 0;		//停利委託價方式=1:市價 2:限價 3:限價委託價
            string strTPOrderPrice = "";		//停利委託價
            int nStopLossFlag = 0;				//停損Flag
            int nRDOSLPercent = 0;				//停損類型
            string strSLPercent = "";			//停損百分比值
            string strSLTrigger = "";			//停損觸發價
            int nRDSLMarketPriceType = 0;		//停損委託價方式=1:市價 2:限價 3:限價委託價
            string strSLOrderPrice = "";		//停損委託價	
            int nClearAllPriceType = 0;			//出清委託價方式=1:市價 2:限價


            if (txtStockNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strStockNo = txtStockNo.Text.Trim();

            if (boxBidAsk.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }
            nBidAsk = boxBidAsk.SelectedIndex;

            double dOrderPrice = 0.0;
            if (double.TryParse(txtOrderPrice.Text.Trim(), out dOrderPrice) == false)
            {
                MessageBox.Show("委託價請輸入數字");
                return;
            }
            strOrderPrice = txtOrderPrice.Text.Trim();

            if (int.TryParse(txtQty.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }
            
            if (ckBoxLoss.Checked)
            {

                double dLossPercent = 0.0;
                double dTriggerLoss = 0.0;
                nStopLossFlag = 1;
                if (radioLoss.Checked)
                {// 1:漲跌幅 0:觸發價
                    if (double.TryParse(txtLossPercent.Text.Trim(), out dLossPercent) == false)
                    {
                        MessageBox.Show("停損跌幅請輸入數字");
                        return;
                    }
                    nRDOSLPercent = 1;
                    strSLPercent = txtLossPercent.Text.Trim();
                }
                else if (radioTriggerLoss.Checked)
                {
                    nRDOSLPercent = 0;
                    if (double.TryParse(txtTriggerLoss.Text.Trim(), out dTriggerLoss) == false)
                    {
                        MessageBox.Show("停損觸發價請輸入數字");
                        return;
                    }
                    strSLTrigger = txtTriggerLoss.Text.Trim();
                }
                if (radioOrderLoss.Checked)
                {//flag 0:None 1:市價 2:限價(以選定漲跌停為主)               
                    
                    nRDSLMarketPriceType = 2;
                    if (double.TryParse(txtLossOrderPrice.Text.Trim(), out dTriggerLoss) == false)
                    {
                        MessageBox.Show("停損委託價請輸入數字");
                        return;
                    }
                    strSLOrderPrice = txtLossOrderPrice.Text.Trim();
                }
                else if (radioOrderTypeLoss.Checked)
                {
                    
                    nRDSLMarketPriceType = 1;
                }
            }
            else
                nStopLossFlag = 0;


            double dProfitPercent = 0.0;
            double dTriggerProfit = 0.0;
            if (ckboxProfit.Checked)
            {
                nTakeProfitFlag = 1;
                if (radioProfit.Checked)
                {// 1:漲跌幅 0:觸發價
                    nRDOTPPercent = 1;

                    if (double.TryParse(txtProfitPercent.Text.Trim(), out dProfitPercent) == false)
                    {
                        MessageBox.Show("停利漲幅請輸入數字");
                        return;
                    }
                    strTPPercent = txtProfitPercent.Text.Trim();
                }
                else if (radioTriggerProfit.Checked)
                {
                    nRDOTPPercent = 0;
                    if (double.TryParse(txtTriggerProfit.Text.Trim(), out dTriggerProfit) == false)
                    {
                        MessageBox.Show("停損觸發價請輸入數字");
                        return;
                    }
                    strTPTrigger = txtTriggerProfit.Text.Trim();
                }
                if (radioOrderProfit.Checked)
                {//flag 0:None 1:市價 2:限價(以選定漲跌停為主)               
                    nRDTPMarketPriceType = 2;

                    if (double.TryParse(txtProfitOrderPrice.Text.Trim(), out dTriggerProfit) == false)
                    {
                        MessageBox.Show("停利委託價請輸入數字");
                        return;
                    }
                    strTPOrderPrice = txtProfitOrderPrice.Text.Trim();
                }
                else if (RadioOrderTypeProfit.Checked)
                {
                    nRDTPMarketPriceType = 1;
                }
            }
            else
                nTakeProfitFlag = 0;
            if (CkBoxClear1.Checked)
            {
                nClearAllFlag = 1;
                strClearCancelTime = boxHr1.Text.Trim() + boxMin1.Text.Trim();
                if (radioClearType.Checked)
                {
                    nClearAllPriceType = 1;                    
                }
                else if (radioOrderClear1.Checked)
                {
                    nClearAllPriceType = 2;
                    
                    if (double.TryParse(txtClearOrder.Text.Trim(), out dTriggerProfit) == false)
                    {
                        MessageBox.Show("出清委託價請輸入數字");
                        return;
                    }
                    strClearAllOrderPrice = txtClearOrder.Text.Trim();
                }
                //pStockOrder.sTimeClearMarketPrice = (short)nClearAllPriceType;
            }
            else
                nClearAllFlag = 0;

            if (ckBoxFinal.Checked)
                nFinalClearFlag = 1;
            else
                nFinalClearFlag = 0;

            STOCKSTRATEGYORDER pStockOrder = new STOCKSTRATEGYORDER();
            pStockOrder.bstrFullAccount = m_UserAccount;
            pStockOrder.bstrStockNo = strStockNo;
            pStockOrder.nBuySell = nBidAsk;
            pStockOrder.nQty = nQty;
            //tockOrder.sIsWarrant = (short)nIsWarrant;
            pStockOrder.bstrOrderPrice = strOrderPrice;
            pStockOrder.nTakeProfitFlag = nTakeProfitFlag;
            pStockOrder.nRDOTPPercent = nRDOTPPercent;
            pStockOrder.bstrTPPercent = strTPPercent;
            pStockOrder.bstrTPTrigger = strTPTrigger;
            pStockOrder.nRDTPMarketPriceType = nRDTPMarketPriceType;
            pStockOrder.bstrTPOrderPrice = strTPOrderPrice;

            pStockOrder.nStopLossFlag = nStopLossFlag;
            pStockOrder.nRDOSLPercent = nRDOSLPercent;
            pStockOrder.bstrSLPercent = strSLPercent;
            pStockOrder.bstrSLTrigger = strSLTrigger;
            pStockOrder.nRDSLMarketPriceType = nRDSLMarketPriceType;
            pStockOrder.bstrSLOrderPrice = strSLOrderPrice;

            pStockOrder.nClearAllFlag = nClearAllFlag;
            pStockOrder.bstrClearCancelTime = strClearCancelTime;
            pStockOrder.nClearAllPriceType = nClearAllPriceType;
            pStockOrder.bstrClearAllOrderPrice = strClearAllOrderPrice;

            pStockOrder.nFinalClearFlag = nFinalClearFlag;

            if (OnStockStrategyDayTradeSignal != null)
            {
                OnStockStrategyDayTradeSignal(m_UserID, true, pStockOrder);
            }
        }

        private void btnAllCleanOutAsync_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇證券帳號");
                return;
            }

            string strStockNo;		//委託股票代號
            int nTradeType;		    //交易種類,一律為整股
            int nOrderType;			//委託-0:現;1:資;2:券
            int nBidAsk;			//0買-1賣出-2無券賣出;出清現賣-資賣-券買	
            string strOrderPrice = "";   //
            int nQty;
            int nIsWarrant;			//權證
            int nClearAllFlag;			//*clear all flag
            string strClearCancelTime = "";	//*clear time
            int nFinalClearFlag;		//*Final Clear flag
            string strClearAllOrderPrice = "";	//出清委託價	
            //出清條件//
            int nLTEFlag;					//LTE flag
            string strLTETriggerPrice = "";		//LTE
            string strLTEOrderPrice = "";		//LTE
            int nLTEMarketPrice = 0;			//Market Price flag
            int nGTEFlag;					//BTE flag
            string strGTETriggerPrice = "";		//BTE
            string strGTEOrderPrice = "";		//BTE
            int nGTEMarketPrice = 0;			//BTE Market Price flag

            int nTimeClearMarketPrice = 0;		//Time Clear Market Price flag1:市價 2:限價



            if (txtStockNo2.Text.Trim() == "")
            {
                MessageBox.Show("請輸入商品代碼");
                return;
            }
            strStockNo = txtStockNo2.Text.Trim();

            if (boxBidAsk2.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇買賣別");
                return;
            }
            nBidAsk = boxBidAsk2.SelectedIndex;

            if (boxOrderType.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇委託別");
                return;
            }

            nOrderType = boxOrderType.SelectedIndex;

            double dOrderPrice = 0.0;
            

            if (int.TryParse(txtQty2.Text.Trim(), out nQty) == false)
            {
                MessageBox.Show("委託量請輸入數字");
                return;
            }                   

            if (boxGTE.Checked)
            {

                double dOrderGTE = 0.0;

                nGTEFlag = 1;
                if (double.TryParse(txtGTEPrice.Text.Trim(), out dOrderGTE) == false)
                {
                    MessageBox.Show("請輸入成交價大於條件-數字");
                    return;
                }
                strGTETriggerPrice = txtGTEPrice.Text.Trim();

                if (radioGTEOrder.Checked)
                {// 1:漲跌幅 0:觸發價

                    nGTEMarketPrice = 0;

                    if (double.TryParse(txtGTEOrder.Text.Trim(), out dOrderGTE) == false)
                    {
                        MessageBox.Show("請輸入成交價大於條件-委託價");
                        return;
                    }

                    strGTEOrderPrice = txtGTEOrder.Text.Trim();
                }
                else if (radioGTEOrderType.Checked)
                {
                    nGTEMarketPrice = 1;
                }
            }
            else
            {
                nGTEFlag = 0;
            }

            double dLTEPrice = 0.0;
            double dTriggerPrice = 0.0;
            if (boxLTE.Checked)
            {

                nLTEFlag = 1;
                if (double.TryParse(txtLTEPrice.Text.Trim(), out dLTEPrice) == false)
                {
                    MessageBox.Show("請輸入成交價小於條件-數字");
                    return;
                }
                strLTETriggerPrice = txtLTEPrice.Text.Trim();

                if (radioLTEOrder.Checked)
                {// 1:漲跌幅 0:觸發價

                    nLTEMarketPrice = 0;

                    if (double.TryParse(txtLTEOrder.Text.Trim(), out dTriggerPrice) == false)
                    {
                        MessageBox.Show("請輸入成交價小於條件-委託價");
                        return;
                    }

                    strLTEOrderPrice = txtLTEOrder.Text.Trim();

                }
                else if (radioLTEOrderType.Checked)
                {
                    nLTEMarketPrice = 1;
                }

            }
            else
            {
                nLTEFlag = 0;
            }

            if (boxClearOut2.Checked)
            {
                nClearAllFlag = 1;
                strClearCancelTime = boxHr2.Text.Trim() + boxMin2.Text.Trim();
                string strHr2 = boxHr2.Text.Trim();
                int nHrCancelTime = int.Parse(strHr2) * 10000;
                string strMin2 = boxMin2.Text.Trim();
                int nMinCancelTime = int.Parse(strMin2) * 100;
                if (nHrCancelTime + nMinCancelTime > 132000)
                {
                    MessageBox.Show("選擇出清時間已超過13:20,請重新選擇");
                    return;
                }
                if (radioClearType2.Checked)
                {
                    nTimeClearMarketPrice = 1;
                }
                else if (radioClearOrder2.Checked)
                {
                    nTimeClearMarketPrice = 2;
                    if (double.TryParse(txtClearOrder2.Text.Trim(), out dTriggerPrice) == false)
                    {
                        MessageBox.Show("出清委託價請輸入數字");
                        return;
                    }
                    strClearAllOrderPrice = txtClearOrder2.Text.Trim();
                }
            }
            else
            {
                nClearAllFlag = 0;
            }

            if (ckBoxFinal2.Checked)
            {
                nFinalClearFlag = 1;
            }
            else
            {
                nFinalClearFlag = 0;
            }

            STOCKSTRATEGYORDEROUT pStockOrder = new STOCKSTRATEGYORDEROUT();
            pStockOrder.bstrFullAccount = m_UserAccount;
            pStockOrder.bstrStockNo = strStockNo;
            pStockOrder.nBuySell = nBidAsk;
            pStockOrder.nOrderType = nOrderType;
            //pStockOrder.bstrOrderPrice = strOrderPrice;
            pStockOrder.nQty = nQty;
            //pStockOrder.sIsWarrant = (short)nIsWarrant;

            pStockOrder.nLTEFlag = nLTEFlag;
            pStockOrder.bstrLTETriggerPrice = strLTETriggerPrice;
            pStockOrder.nLTEMarketPrice = nLTEMarketPrice;
            pStockOrder.bstrLTEOrderPrice = strLTEOrderPrice;

            pStockOrder.nGTEFlag = nGTEFlag;
            pStockOrder.bstrGTETriggerPrice = strGTETriggerPrice;
            pStockOrder.nGTEMarketPrice = nGTEMarketPrice;
            pStockOrder.bstrGTEOrderPrice = strGTEOrderPrice;

            pStockOrder.nClearAllFlag = nClearAllFlag;
            pStockOrder.bstrClearCancelTime = strClearCancelTime;
            pStockOrder.nClearAllPriceType = nTimeClearMarketPrice;//[-0306-]
            pStockOrder.bstrClearAllOrderPrice = strClearAllOrderPrice;

            pStockOrder.nFinalClearFlag = nFinalClearFlag;

            if (OnStockStrategyAllCleanOutSignal != null)
            {
                OnStockStrategyAllCleanOutSignal(m_UserID, true, pStockOrder);
            }
        }

        private void btn_GetTSStategy_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇證券帳號");
                return;
            }
            int nTypeReport;
            string strMarketType;
            string strKindReport;
            string strStartDate;

            if (txtMarketType.Text == "")
            {
                MessageBox.Show("請輸入市場類型");
                return;
            }
            strMarketType = txtMarketType.Text;
            if (boxTypeReport.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇類型");
                return;
            }
            nTypeReport = boxTypeReport.SelectedIndex;

            if (boxKindReport.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇種類");
                return;
            }
            strKindReport = boxKindReport.Text.Trim();

            if (StartDateBox.Text.Trim() == "" || StartDateBox.Text.Trim() == "YYYYMMDD")
            {
                MessageBox.Show("請輸入查詢日期");
                return;
            }
            strStartDate = StartDateBox.Text.Trim();

            if (OnTSStrategyReportSignal != null)
            {

                OnTSStrategyReportSignal(m_UserID, m_UserAccount, strMarketType, nTypeReport, strKindReport, strStartDate);
            }
        }

        private void radioLoss_CheckedChanged(object sender, EventArgs e)
        {
            if (radioLoss.Checked == true)
            {
                radioTriggerLoss.Checked = false;
                
            }
            
        }

        private void radioProfit_CheckedChanged(object sender, EventArgs e)
        {
            if (radioProfit.Checked == true)
            {
                radioTriggerProfit.Checked = false;
                
            }
            
        }

        private void radioTriggerLoss_CheckedChanged(object sender, EventArgs e)
        {
            if (radioTriggerLoss.Checked == true)
            {
                radioLoss.Checked = false;
                txtLossPercent.Text = "";
            }
            
        }

        private void radioOrderLoss_CheckedChanged(object sender, EventArgs e)
        {
            if (radioOrderLoss.Checked == true)
            {
                radioOrderTypeLoss.Checked = false;
                
            }          
            
        }

        private void radioOrderTypeLoss_CheckedChanged(object sender, EventArgs e)
        {
            if (radioOrderTypeLoss.Checked == true)
            {
                radioOrderLoss.Checked = false;                
            }
            
        }

        private void radioOrderProfit_CheckedChanged(object sender, EventArgs e)
        {
            if (radioOrderProfit.Checked == true)
            {
                
                RadioOrderTypeProfit.Checked = false;
            }
            
        }

        private void RadioOrderTypeProfit_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioOrderTypeProfit.Checked == true)
            {                
                radioOrderProfit.Checked = false;
            }
           
        }

        private void radioTriggerProfit_CheckedChanged(object sender, EventArgs e)
        {
            if (radioTriggerProfit.Checked == true)
            {                
                radioProfit.Checked = false;
            }
            
        }

        private void radioOrderClear_CheckedChanged(object sender, EventArgs e)
        {
            if (radioOrderClear1.Checked == true)
            {
                
                radioClearType.Checked = false;
            }
        }

        private void radioClearType_CheckedChanged(object sender, EventArgs e)
        {
            if (radioClearType.Checked == true)
            {
                
                radioOrderClear1.Checked = false;
            }
            
        }

        private void radioGTEOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (radioGTEOrder.Checked == true)
            {

                radioGTEOrderType.Checked = false;
            }
        }

        private void radioGTEOrderType_CheckedChanged(object sender, EventArgs e)
        {
            if (radioGTEOrderType.Checked == true)
            {

                radioGTEOrder.Checked = false;
            }
        }

        private void radioLTEOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (radioLTEOrder.Checked == true)
            {
                radioLTEOrderType.Checked = false;
            }
        }

        private void radioLTEOrderType_CheckedChanged(object sender, EventArgs e)
        {
            if (radioLTEOrderType.Checked == true)
            {
                radioLTEOrder.Checked = false;
            }
        }

        private void radioClearOrder_CheckedChanged(object sender, EventArgs e)
        {

            if (radioClearOrder2.Checked == true)
            {
                radioClearType2.Checked = false;
            }
        }

        private void radioClearType2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioClearType2.Checked == true)
            {
                radioClearOrder2.Checked = false;
            }
        }

        private void boxBidAsk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (boxBidAsk.SelectedIndex == 0)
            {
                radioOrderTypeLoss.Text = "跌停價";
                RadioOrderTypeProfit.Text = "跌停價";
                radioClearType.Text = "跌停價";
            }
            else if (boxBidAsk.SelectedIndex == 1)
            {
                radioOrderTypeLoss.Text = "漲停價";
                RadioOrderTypeProfit.Text = "漲停價";
                radioClearType.Text = "漲停價";
            }
        }

        private void boxOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (boxOrderType.SelectedIndex == 0)
            {
                radioGTEOrderType.Text = "跌停價";
                radioLTEOrderType.Text = "跌停價";
                radioClearType2.Text = "跌停價";
            }
            else if (boxOrderType.SelectedIndex == 1)
            {
                radioGTEOrderType.Text = "跌停價";
                radioLTEOrderType.Text = "跌停價";
                radioClearType2.Text = "跌停價";
            }
            else if (boxOrderType.SelectedIndex == 2)
            {
                radioGTEOrderType.Text = "漲停價";
                radioLTEOrderType.Text = "漲停價";
                radioClearType2.Text = "漲停價";
            }
        }

        private void CancelTSOrder_Click(object sender, EventArgs e)
        {
            if (m_UserAccount == "")
            {
                MessageBox.Show("請選擇證券帳號");
                return;
            }
            string strMarket;
            string strKeyNo;
            int nTradeType;
            

            if (txtSmartNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入智慧單序號");
                return;
            }
            strKeyNo = txtSmartNo.Text.Trim();

            if (KeyTypeBox.SelectedIndex == -1 || KeyTypeBox.Text.Trim() == "")
            {
                MessageBox.Show("請輸入智慧單類別(1當沖母單/2當沖未成交入場單/3當沖已進場單/4出清)");
                return;
            }

            int nTradeTypeOfDel = KeyTypeBox.SelectedIndex + 1;

            if (OnCancelTSStrategyOrderSignal != null)
            {
                OnCancelTSStrategyOrderSignal(m_UserID, true, m_UserAccount, strKeyNo, nTradeTypeOfDel);
            }
        }
               
    }
}
