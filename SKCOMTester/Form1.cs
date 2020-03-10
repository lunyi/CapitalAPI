using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SKCOMLib;

namespace SKCOMTester
{
    public partial class Form1 : Form
    {
        #region Environment Variable
        //----------------------------------------------------------------------
        // Environment Variable
        //----------------------------------------------------------------------
        int m_nCode;

        SKCenterLib m_pSKCenter;
        SKCenterLib m_pSKCenter2;
        SKOrderLib m_pSKOrder;

        SKReplyLib m_pSKReply;
        SKQuoteLib m_pSKQuote;
        SKOSQuoteLib m_pSKOSQuote;
        SKOOQuoteLib m_pSKOOQuote;
        SKReplyLib m_pSKReply2;
        SKQuoteLib m_pSKQuote2;
        SKOrderLib m_pSKOrder2;

        #endregion

        #region Initialize
        //----------------------------------------------------------------------
        // Initialize
        //----------------------------------------------------------------------

        public Form1()
        {
            InitializeComponent();
            m_pSKCenter = new SKCenterLib();
            m_pSKCenter2 = new SKCenterLib();   

            m_pSKOrder = new SKOrderLib();
            skOrder1.OrderObj = m_pSKOrder;

            m_pSKReply = new SKReplyLib();
            skReply1.SKReplyLib = m_pSKReply;

            m_pSKReply.OnReplyMessage += new _ISKReplyLibEvents_OnReplyMessageEventHandler(this.OnAnnouncement);
            

            m_pSKQuote = new SKQuoteLib();            
            skQuote1.SKQuoteLib = m_pSKQuote;            

            m_pSKOSQuote = new SKOSQuoteLib();
            skosQuote1.SKOSQuoteLib = m_pSKOSQuote;

            m_pSKOOQuote = new SKOOQuoteLib();
            skooQuote1.SKOOQuoteLib = m_pSKOOQuote;

            m_pSKCenter2.OnShowAgreement += new _ISKCenterLibEvents_OnShowAgreementEventHandler(this.OnShowAgreement);
            m_pSKCenter2.OnNotifySGXAPIOrderStatus += new _ISKCenterLibEvents_OnNotifySGXAPIOrderStatusEventHandler(this.m_pSKCenter_OnSGXAPIOrderStatus);

        }

        #endregion

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            
            
            if (ckBox_OrderM.Checked == true)
            {
                m_nCode = m_pSKCenter.SKCenterLib_LoginOrderM(txtAccount.Text.Trim().ToUpper(), txtPassWord.Text.Trim());
            }
            else
            {
                if (checkSGXDMA.Checked == true)
                    m_pSKCenter.SKCenterLib_SetAuthority(0);
                else
                    m_pSKCenter.SKCenterLib_SetAuthority(1);

                //[-for API exam switch-0513-add-]      
                if (Chk_Env.Checked == true)
                    m_pSKCenter.SKCenterLib_ResetServer("morder1.capital.com.tw");
            
                m_nCode = m_pSKCenter.SKCenterLib_Login(txtAccount.Text.Trim().ToUpper(), txtPassWord.Text.Trim());
            }
            if (m_nCode == 0)
            {
                
                
                WriteMessage(DateTime.Now.TimeOfDay.ToString() + "登入成功");
                skOrder1.LoginID = txtAccount.Text.Trim().ToUpper();
                skOrder1.ContinuousTrading = ckBox_OrderM.Checked;//[for OrderM]

                skReply1.LoginID = txtAccount.Text.Trim().ToUpper();
                skReply1.OrderM = ckBox_OrderM.Checked;//[for OrderM]
                
                skQuote1.LoginID = txtAccount.Text.Trim().ToUpper();
                skosQuote1.LoginID = txtAccount.Text.Trim().ToUpper();                
            }
            else
                WriteMessage(m_nCode);

            
        }

        private void btn_Center_Log_Click(object sender, EventArgs e)
        {
            m_pSKCenter.SKCenterLib_SetLogPath(txt_Center_LogPath.Text.Trim());
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
            listInformation.HorizontalExtent = hzSize;
        }

        public void WriteMessage(int nCode)
        {
            listInformation.Items.Add( m_pSKCenter.SKCenterLib_GetReturnCodeMessage(nCode) );

            listInformation.SelectedIndex = listInformation.Items.Count - 1;

            //listInformation.HorizontalScrollbar = true;

            // Create a Graphics object to use when determining the size of the largest item in the ListBox.
            Graphics g = listInformation.CreateGraphics();

            // Determine the size for HorizontalExtent using the MeasureString method using the last item in the list.
            int hzSize = (int)g.MeasureString(listInformation.Items[listInformation.Items.Count - 1].ToString(), listInformation.Font).Width;
            // Set the HorizontalExtent property.
            listInformation.HorizontalExtent = hzSize;
        }

        private void GetMessage(string strType, int nCode, string strMessage)
        {
            string strInfo = "";

            if (nCode != 0)
                strInfo ="【"+ m_pSKCenter.SKCenterLib_GetLastLogInfo()+ "】";

            WriteMessage("【" + strType + "】【" + strMessage + "】【" + m_pSKCenter.SKCenterLib_GetReturnCodeMessage(nCode) + "】" + strInfo);
        }
        
        void OnShowAgreement(string strData)
        {
            WriteMessage("[OnShowAgreement]" + strData);
        }
        
        void OnAnnouncement(string strUserID, string bstrMessage, out short nConfirmCode)
        {
            WriteMessage(strUserID + "_" + bstrMessage);
            nConfirmCode =-1;
            
        }

        void m_pSKCenter_OnSGXAPIOrderStatus(int nStatus, string strAccount)
        {
            if (nStatus == 3001)
            {
                //if (nStatus == 0)
                //{
                lblSignalSGXAPI.ForeColor = Color.Yellow;
                //}
            }
            else if (nStatus == 3002)
            {
                lblSignalSGXAPI.ForeColor = Color.Red;
            }
            else if (nStatus == 3026)
            {
                lblSignalSGXAPI.ForeColor = Color.Green;
            }
            else if (nStatus == 1054)
            {
                lblSignalSGXAPI.ForeColor = Color.Black;


            }

             WriteMessage("【OF:" + strAccount + "】【SGX API Order Connection:" + nStatus + "】");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_nCode = m_pSKCenter2.SKCenterLib_Login(txtAccount2.Text.Trim().ToUpper(), txtPassWord2.Text.Trim());

            if (m_nCode == 0)
            {                
                //skOrder21.LoginID = txtAccount2.Text.Trim().ToUpper();
                skReply1.LoginID2 = txtAccount2.Text.Trim().ToUpper();
                skQuote1.LoginID2 = txtAccount2.Text.Trim().ToUpper();
                WriteMessage("登入成功" );
                //skosQuote1.LoginID = txtAccount2.Text.Trim().ToUpper();
            }
            else
                WriteMessage(m_nCode);
        }

        private void btnRequestAgreement_Click(object sender, EventArgs e)
        {
            m_nCode = m_pSKCenter.SKCenterLib_RequestAgreement(txtAccount.Text.Trim().ToUpper());
            GetMessage("Center", m_nCode, "RequestAgreement");
        }
    }
}
