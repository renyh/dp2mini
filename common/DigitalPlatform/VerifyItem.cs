using DigitalPlatform;
using DigitalPlatform.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    public class VerifyItem
    {
        static string VerifyPricePrefix(string prefix)
        {
            foreach (var ch in prefix)
            {
                if (char.IsLetter(ch) == false)
                    return $"货币名称 '{prefix}' 中出现了非字母的字符";
            }

            return null;
        }

        public static List<string> VerifyPrice(string strPrice)
        {
            List<string> errors = new List<string>();

            // 解析单个金额字符串。例如 CNY10.00 或 -CNY100.00/7
            int nRet = PriceUtil.ParseSinglePrice(strPrice,
                out CurrencyItem item,
                out string strError);
            if (nRet == -1)
                errors.Add(strError);

            // 2020/7/8
            // 检查货币字符串中是否出现了字母以外的字符
            if (string.IsNullOrEmpty(item.Postfix) == false)
                errors.Add($"金额字符串 '{strPrice}' 中出现了后缀 '{item.Postfix}' ，这很不常见，一般意味着错误");

            string error1 = VerifyPricePrefix(item.Prefix);
            if (error1 != null)
                errors.Add(error1);

            string new_value = StringUtil.ToDBC(strPrice);
            if (new_value.IndexOfAny(new char[] { '(', ')' }) != -1)
            {
                errors.Add("价格字符串中不允许出现括号 '" + strPrice + "'");
            }

            if (new_value.IndexOf(',') != -1)
            {
                errors.Add("价格字符串中不允许出现逗号 '" + strPrice + "'");
            }

            return errors;
        }


        // 去掉价格字符串中的 "(...)" 部分
        // TODO: 检查小数点后的位数，多于 2 位的要删除
        // return:
        //      false   没有发生修改
        //      true    发生了修改
        public static bool CorrectPrice(ref string strText)
        {
            string strSaved = strText;

            strText = strText.Trim();

            //2017/6/17
            strText = strText.Replace("￥", "CNY");

            // 2017/6/17
            strText = strText.Replace("精装", "")
                .Replace("平装", "")
                .Replace("每册", "");

            // 2023/6/19
            strText = strText.Replace("元", "");

            strText = StringUtil.ToDBC(strText);

            // 2023/6/19
            strText = strText.Replace("(精)", "")
    .Replace("(平)", "");

            // 截掉逗号右侧的部分
            List<string> parts = StringUtil.ParseTwoPart(strText, ",");
            strText = parts[0];

            int nStart = strText.IndexOf("(");
            if (nStart == -1)
                return false;

            // 右边剩余部分
            string strRight = strText.Substring(nStart + 1);

            strText = strText.Substring(0, nStart).Trim();
            int nEnd = strRight.IndexOf(")");
            if (nEnd == -1)
                return true;

            string strFragment = strRight.Substring(0, nEnd).Trim();
            strText += strRight.Substring(nEnd + 1).Trim();

            // 判断是否为 全5册 情况
            if (string.IsNullOrEmpty(strFragment) == false)
            {
                bool bChanged = false;

                if (strFragment == "上下册"
                    || strFragment == "上下"
                    || strFragment == "上下卷"
                    || strFragment == "上下编"
                    || strFragment == "两册")
                {
                    strText += "/2";
                    bChanged = true;
                }
                else if (strFragment == "上中下册"
                    || strFragment == "上中下")
                {
                    strText += "/3";
                    bChanged = true;
                }
                else if (strFragment.EndsWith("册")
                    || strFragment.EndsWith("卷"))
                {
                    /*
                    if (strFragment.StartsWith("全"))
                        strFragment = strFragment.Substring(1);
                    */

                    // 数字+册
                    string strNumber = strFragment.Substring(0, strFragment.Length - 1).Trim();

                    int v = 0;
                    if (TryParseNumber(strNumber, out string value))
                    {
                        strText += "/" + value;
                        bChanged = true;
                    }
                }

                if (bChanged == false)
                {
                    // 2023/6/19
                    // 全套三卷
                    strFragment = strFragment.Replace("全套", "全").Replace("全套共", "全");

                    string strNumber = StringUtil.Unquote(strFragment, "全册共册全卷共卷全编");
                    if (strNumber != strFragment)
                    {
                        int v = 0;
                        if (TryParseNumber(strNumber, out string value))
                        {
                            strText += "/" + value;
                            // strError = "被变换为每册平均价格形态";
                            bChanged = true;
                        }
                    }
                }

                // 2023/6/19
                // CNY???(?币)
                if (bChanged == false
                    && strFragment.EndsWith("币")
                    && strText.Contains("CNY"))
                {
                    strText = strText.Replace("CNY", "");
                    strText = strFragment + strText;
                    bChanged = true;
                }

                // 2023/6/19
                if (bChanged == false && string.IsNullOrEmpty(strFragment) == false)
                {
                    strText = strSaved;
                    return false;
                }
            }

            // 2017/7/1
            if (strText.EndsWith(".00.00"))
                strText = strText.Substring(0, strText.Length - 3);

            return true;
        }

        static bool TryParseNumber(string strNumber,
    out string number)
        {
            number = "";
            try
            {
                var value = NumberConvert.ParseCnToInt(strNumber);
                number = value.ToString();
                return true;
            }
            catch
            {

            }

            if (StringUtil.IsPureNumber(strNumber) && Int32.TryParse(strNumber, out int v))
            {
                number = strNumber;
                return true;
            }

            return false;
        }


    }
}
