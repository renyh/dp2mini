using common;
using DigitalPlatform.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnitTestPrice
{
    [TestClass]
    public class UnitTest1
    {

        /*
         * 
         * 
         * 
         */



        [TestMethod]
        public void TestMethod1()
        {
            //纯数字或带运算符
            string prices = @"60.00	Y	N
100	Y	N
100/2	Y	N
200*2	Y	N
10+20	N	N
10-5	N	N
+20	Y	N
-5	Y	N
";
            string[] list = lines2array(prices);
            foreach (string price in list)
            {
                if (price.Trim() == "")
                    continue;
                 this.CheckOnePrice(price);
            }

        }

        [TestMethod]
        public void TestMethod2()
        {
            //前缀+数值
            string prices = @"CNY150.00	Y	N
CNY10.	Y	N
USD50.00	Y	N
A50.00	Y	N
Y10.00	Y	N
中国10.00	Y	N
民国大洋(钱)10	N	N
CNY	N	N
人民币	N	N
";
            string[] list = lines2array(prices);
            foreach (string price in list)
            {
                if (price.Trim() == "")
                    continue;
                this.CheckOnePrice(price);
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            //3.乘除符号
            string prices = @"CNY38.00/2	Y	N	
CNY45.00*2	Y	N	
*39.00	N	N	
10*	N	N	
CNY10*	N	N	
10/	N	N	
/10	N	N	
CNY10/	N	N	
CNY10+20	N	N	
CNY10-5	N	N	
CNY+10	N	N	
CNY-5	N	N	
CNY10+CNY20	N	N	
";
            CheckPrices(prices);
        }

        [TestMethod]
        public void TestMethod4()
        {
            //4.中间有逗号
            string prices = @"CNY58.00,60	N	Y	CNY58.00
10，20	N	Y	10
10,15	N	Y	10
10,15,25	N	Y	10
USD20,30	N	Y	USD20
";
            CheckPrices(prices);
        }

        [TestMethod]
        public void TestMethod5()
        {
            //5.前缀是特殊符号
            string prices = @"$198.00	N	N	
￥68.00	N	Y	CNY68.00
#49.80	N	N	
";
            CheckPrices(prices);
        }

        [TestMethod]
        public void TestMethod6()
        {
            //6.数值中间有特殊字符，认作后缀，但（和（）可以修正
            string prices = @"CNY10!1	N	N	
CNY10()2	N	Y	CNY102
CNY10(2	N	Y	CNY10
CNY)3	N	N	
CNY10（）1	N	Y	CNY101
11()1	N	Y	111
11(中)2	N	N	
10;1	N	N	
";
            CheckPrices(prices);
        }

        [TestMethod]
        public void TestMethod7()
        {
            //7.出现后缀且无法修正
            string prices = @"CNY10。	N	N	
CNY68.00元	N	Y	CNY68.00
CNY3人1.00	N	N	
CNY10a	N	N	
CNY10人	N	N	
CNY48.00(套)	N	N	
CNY48.00(中国)	N	N	
CNY31.00(人民币)	N	Y	人民币31.00
USD20.00(人民币)	N	N	
USD20.00(美元）	N	N	
";
            CheckPrices(prices);
        }

        [TestMethod]
        public void TestMethod8()
        {
            //8.后缀可以被修正删除
            string prices = @"CNY14.00精装	N	Y	CNY14.00
CNY15.00平装	N	Y	CNY15.00
CNY16.00每册	N	Y	CNY16.00
CNY10()	N	Y	CNY10
CNY17.00（平装）	N	Y	CNY17.00
CNY18.00（精装）	N	Y	CNY18.00
CNY10.00（平）	N	Y	CNY10.00
CNY20.00(精)	N	Y	CNY20.00
";
            CheckPrices(prices);
        }




        [TestMethod]
        public  void TestMethod9()
        {
            //9.括号里的值可以换成除号
            string prices = @"CNY21.00(上下册)	N	Y	CNY21.00/2
CNY22.00(上下)	N	Y	CNY22.00/2
CNY23.00(上下卷)	N	Y	CNY23.00/2
CNY24.00(上下编)	N	Y	CNY24.00/2
CNY26.00上下	N	N	
CNY25.00(两册)	N	Y	CNY25.00/2
CNY27.00(上中下册)	N	Y	CNY27.00/3
CNY28.00(上中下)	N	Y	CNY28.00/3
CNY29.00(5卷)	N	Y	CNY29.00/5
CNY30.00(2册)	N	Y	CNY30.00/2
";

            CheckPrices(prices);

        }


        public void CheckPrices(string prices)
        {
            string[] list = lines2array(prices);
            foreach (string price in list)
            {
                if (price.Trim() == "")
                    continue;

                this.CheckOnePrice(price);
            }
        }
       

        private void CheckOnePrice(string price)
        {
            string strError = "";
            PriceItem priceItem = new PriceItem(price);

            // 检查是否合法
            List<string> errros = VerifyItem.VerifyPrice(priceItem.sourePrice);
            bool bRight = false;
            if (errros.Count > 0)
            {
                bRight = false;
                strError = string.Join(",", errros);
            }
            else
            {
                bRight = true;
            }

            if (string.IsNullOrEmpty(strError) == false)
            {
                Console.Write(priceItem.sourePrice + "\t" + strError);
            }
            else
            {
                Console.Write(priceItem.sourePrice + "\t通过");
            }
            Assert.AreEqual(priceItem.isRight, bRight);

            // 检查修复
            // (全10册) 转为除法形态
            string strPrice = priceItem.sourePrice;
            VerifyItem.CorrectPrice(ref strPrice);

            if (priceItem.canCorrect == true)
            {
                Console.WriteLine("\t"+strPrice);
                Assert.AreEqual(strPrice, priceItem.targetPrice);
            }
            else
            {
                Console.WriteLine("\t" + "不修正");
                Assert.AreEqual(strPrice, priceItem.sourePrice);
            }

            //    if (priceItem.sourePrice != strPrice)
            //{
            //    if (IsPriceCorrect(strPrice) == true)
            //    {
            //        DomUtil.SetElementText(itemdom.DocumentElement, "price", strPrice);
            //        bChanged = true;
            //        errors.Add("^价格字符串 '" + strOldPrice + "' 被自动修改为 '" + strPrice + "'"); // ^ 开头表示修改提示，而不是错误信息
            //    }
            //    else
            //        errors.Add("*** 价格字符串 '" + strOldPrice + "' 无法被自动修改 2");
            //}
            //else
            //    errors.Add("*** 价格字符串 '" + strOldPrice + "' 无法被自动修改 1");

        

        }

        public string[] lines2array(string s)
        {
            s = s.Replace("\r\n", "\n");
            return s.Split('\n');
        }

        public class PriceItem
        {
            public PriceItem(string full)
            {
                //用tab键分隔
                string[] s = full.Split('\t');
                if (s.Length < 3)
                    throw new Exception("提交了价格测试字符串不合要求，格式为：待校验价格   预期是否正确  预期是否可修正 修正结果");
                sourePrice = s[0];

                if (s[1] == "Y")
                    isRight = true;

                if (s[2] == "Y")
                    canCorrect = true;

                if (s.Length >= 4)
                    targetPrice = s[3];


                if (s.Length >= 5)
                    checkErrorInfo = s[4];
            }

            public string sourePrice = ""; // 原始价格
            public bool isRight = false;
            public bool canCorrect = false;
            public string targetPrice = "";//预期修改后的人本和

            public string checkErrorInfo = "";
        }
    }


    
}
