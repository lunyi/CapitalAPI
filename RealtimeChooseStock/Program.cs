using System;
using System.Data;
using SKCOMLib;

namespace RealtimeChooseStock
{
    class Program
    {
        static void Main(string[] args)
        {

            SKCenterLib m_pSKCenter = new SKCenterLib() ;
            SKReplyLib m_pSKReply = new SKReplyLib();
            //skReply1.SKReplyLib = m_pSKReply;

            SKQuoteLib m_SKQuoteLib = new SKQuoteLib();
            //skQuote1.SKQuoteLib = m_pSKQuote;

            SKOSQuoteLib m_pSKOSQuote = new SKOSQuoteLib();
            //skosQuote1.SKOSQuoteLib = m_pSKOSQuote;

            SKOOQuoteLib m_pSKOOQuote = new SKOOQuoteLib();

            int m_nCode = m_pSKCenter.SKCenterLib_Login("M121591178", "1q2w3e4r");
            if (m_nCode == 0)
            {
                Console.WriteLine("登入成功!!");
            }
            else
            {
                Console.WriteLine("登入失敗!!");
            }

            //connection
            m_SKQuoteLib.OnConnection += new _ISKQuoteLibEvents_OnConnectionEventHandler(m_SKQuoteLib_OnConnection);
            m_SKQuoteLib.OnNotifyQuote += new _ISKQuoteLibEvents_OnNotifyQuoteEventHandler(m_SKQuoteLib_OnNotifyQuote);
            m_SKQuoteLib.OnNotifyHistoryTicks += new _ISKQuoteLibEvents_OnNotifyHistoryTicksEventHandler(m_SKQuoteLib_OnNotifyHistoryTicks);
            m_SKQuoteLib.OnNotifyTicks += new _ISKQuoteLibEvents_OnNotifyTicksEventHandler(m_SKQuoteLib_OnNotifyTicks);
            m_SKQuoteLib.OnNotifyBest5 += new _ISKQuoteLibEvents_OnNotifyBest5EventHandler(m_SKQuoteLib_OnNotifyBest5);
            m_SKQuoteLib.OnNotifyKLineData += new _ISKQuoteLibEvents_OnNotifyKLineDataEventHandler(m_SKQuoteLib_OnNotifyKLineData);
            m_SKQuoteLib.OnNotifyServerTime += new _ISKQuoteLibEvents_OnNotifyServerTimeEventHandler(m_SKQuoteLib_OnNotifyServerTime);
            m_SKQuoteLib.OnNotifyMarketTot += new _ISKQuoteLibEvents_OnNotifyMarketTotEventHandler(m_SKQuoteLib_OnNotifyMarketTot);
            m_SKQuoteLib.OnNotifyMarketBuySell += new _ISKQuoteLibEvents_OnNotifyMarketBuySellEventHandler(m_SKQuoteLib_OnNotifyMarketBuySell);
            m_SKQuoteLib.OnNotifyMarketHighLow += new _ISKQuoteLibEvents_OnNotifyMarketHighLowEventHandler(m_SKQuoteLib_OnNotifyMarketHighLow);
            m_SKQuoteLib.OnNotifyMACD += new _ISKQuoteLibEvents_OnNotifyMACDEventHandler(m_SKQuoteLib_OnNotifyMACD);
            m_SKQuoteLib.OnNotifyBoolTunel += new _ISKQuoteLibEvents_OnNotifyBoolTunelEventHandler(m_SKQuoteLib_OnNotifyBoolTunel);
            m_SKQuoteLib.OnNotifyFutureTradeInfo += new _ISKQuoteLibEvents_OnNotifyFutureTradeInfoEventHandler(m_SKQuoteLib_OnNotifyFutureTradeInfo);
            m_SKQuoteLib.OnNotifyStrikePrices += new _ISKQuoteLibEvents_OnNotifyStrikePricesEventHandler(m_SKQuoteLib_OnNotifyStrikePrices);
            m_SKQuoteLib.OnNotifyStockList += new _ISKQuoteLibEvents_OnNotifyStockListEventHandler(m_SKQuoteLib_OnNotifyStockList);


            string[] Stocks = txtStocks.Text.Trim().Split(new Char[] { ',' });

            foreach (string s in Stocks)
            {
                SKSTOCK pSKStock = new SKSTOCK();

                int nCode = m_SKQuoteLib.SKQuoteLib_GetStockByNo(s.Trim(), ref pSKStock);

                OnUpDateDataRow(pSKStock);

                if (nCode == 0)
                {
                    OnUpDateDataRow(pSKStock);
                }
            }

            m_nCode = m_SKQuoteLib.SKQuoteLib_RequestStocks(ref sPage, txtStocks.Text.Trim());

        }


        private void OnUpDateDataRow(SKSTOCK pStock)
        {

            string strStockNo = pStock.bstrStockNo;

            DataRow drFind = m_dtStocks.Rows.Find(strStockNo);
            if (drFind == null)
            {
                try
                {
                    DataRow myDataRow = m_dtStocks.NewRow();

                    myDataRow["m_sStockidx"] = pStock.sStockIdx;
                    myDataRow["m_sDecimal"] = pStock.sDecimal;
                    myDataRow["m_sTypeNo"] = pStock.sTypeNo;
                    myDataRow["m_cMarketNo"] = pStock.bstrMarketNo;
                    myDataRow["m_caStockNo"] = pStock.bstrStockNo;
                    myDataRow["m_caName"] = pStock.bstrStockName;
                    myDataRow["m_nOpen"] = pStock.nOpen / (Math.Pow(10, pStock.sDecimal));
                    myDataRow["m_nHigh"] = pStock.nHigh / (Math.Pow(10, pStock.sDecimal));
                    myDataRow["m_nLow"] = pStock.nLow / (Math.Pow(10, pStock.sDecimal));
                    myDataRow["m_nClose"] = pStock.nClose / (Math.Pow(10, pStock.sDecimal));
                    myDataRow["m_nTickQty"] = pStock.nTickQty;
                    myDataRow["m_nRef"] = pStock.nRef / (Math.Pow(10, pStock.sDecimal));
                    myDataRow["m_nBid"] = pStock.nBid / (Math.Pow(10, pStock.sDecimal));
                    myDataRow["m_nBc"] = pStock.nBc;
                    myDataRow["m_nAsk"] = pStock.nAsk / (Math.Pow(10, pStock.sDecimal));
                    m_nSimulateStock = pStock.nSimulate;                 //成交價/買價/賣價;揭示
                    myDataRow["m_nAc"] = pStock.nAc;
                    myDataRow["m_nTBc"] = pStock.nTBc;
                    myDataRow["m_nTAc"] = pStock.nTAc;
                    myDataRow["m_nFutureOI"] = pStock.nFutureOI;
                    myDataRow["m_nTQty"] = pStock.nTQty;
                    myDataRow["m_nYQty"] = pStock.nYQty;
                    myDataRow["m_nUp"] = pStock.nUp / (Math.Pow(10, pStock.sDecimal));
                    myDataRow["m_nDown"] = pStock.nDown / (Math.Pow(10, pStock.sDecimal));

                    m_dtStocks.Rows.Add(myDataRow);

                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
            else
            {
                drFind["m_sStockidx"] = pStock.sStockIdx;
                drFind["m_sDecimal"] = pStock.sDecimal;
                drFind["m_sTypeNo"] = pStock.sTypeNo;
                drFind["m_cMarketNo"] = pStock.bstrMarketNo;
                drFind["m_caStockNo"] = pStock.bstrStockNo;
                drFind["m_caName"] = pStock.bstrStockName;
                drFind["m_nOpen"] = pStock.nOpen / (Math.Pow(10, pStock.sDecimal));
                drFind["m_nHigh"] = pStock.nHigh / (Math.Pow(10, pStock.sDecimal));
                drFind["m_nLow"] = pStock.nLow / (Math.Pow(10, pStock.sDecimal));
                drFind["m_nClose"] = pStock.nClose / (Math.Pow(10, pStock.sDecimal));
                drFind["m_nTickQty"] = pStock.nTickQty;
                drFind["m_nRef"] = pStock.nRef / (Math.Pow(10, pStock.sDecimal));
                drFind["m_nBid"] = pStock.nBid / (Math.Pow(10, pStock.sDecimal));
                drFind["m_nBc"] = pStock.nBc;
                drFind["m_nAsk"] = pStock.nAsk / (Math.Pow(10, pStock.sDecimal));
                drFind["m_nAc"] = pStock.nAc;
                drFind["m_nTBc"] = pStock.nTBc;
                drFind["m_nTAc"] = pStock.nTAc;
                drFind["m_nFutureOI"] = pStock.nFutureOI;
                drFind["m_nTQty"] = pStock.nTQty;
                drFind["m_nYQty"] = pStock.nYQty;
                drFind["m_nUp"] = pStock.nUp / (Math.Pow(10, pStock.sDecimal));
                drFind["m_nDown"] = pStock.nDown / (Math.Pow(10, pStock.sDecimal));
                m_nSimulateStock = pStock.nSimulate;                 //成交價/買價/賣價;揭示
            }
        }
    }
}
