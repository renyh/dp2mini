using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bookshelf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            this.EnableCtrl(false);
            try
            {
                // 检查输入参数
                string serverUrl = this.textBox_serverUrl.Text.Trim();
                if (string.IsNullOrEmpty(serverUrl))
                {
                    MessageBox.Show(this, "服务器地址不能为空。");
                    return;
                }
                if (this.textBox_area.Text.Trim() == "" || this.textBox_jia.Text.Trim() == "")
                {
                    MessageBox.Show(this, "区和架不能为空。");
                    return;
                }
                string strLocation = this.textBox_area.Text + "," + this.textBox_jia.Text;


                // 调接口

                using (HttpClient httpClient = new HttpClient())
                {
                    param p = new param() {
                        location = strLocation,
                    };
                    string strquest= JsonConvert.SerializeObject(p);

                    this.textBox_result.Text = "请求信息:" + strquest + "\r\n\r\n";


                    HttpContent content = new StringContent(strquest);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    
                    HttpResponseMessage response =  httpClient.PostAsync(
                        GetMethodUrl(serverUrl, "OpenColumn"),
                        content).Result;


                    response.EnsureSuccessStatusCode();//用来抛异常的


                    string responseBody =  response.Content.ReadAsStringAsync().Result;
                    this.textBox_result.Text += "返回结果:" + responseBody;
                }


                // .net6才能使用
                //    // json参数
                //    StringContent jsonContent = new(
                //      JsonSerializer.Serialize(new
                //      {
                //          location = strLocation,
                //      }),
                //       Encoding.UTF8,
                //       "application/json");

                //this.textBox_result.Text = "请求信息:" + jsonContent.ReadAsStringAsync().Result + "\r\n\r\n";


                //using HttpResponseMessage response = httpClient.PostAsync(
                //    GetMethodUrl(serverUrl, "OrderOpen"),
                //    jsonContent).Result;

                //var jsonResponse = response.Content.ReadAsStringAsync().Result;
                //this.textBox_result.Text += "返回结果:" + jsonResponse;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "打开接口异常:" + ex.Message);
                return;
            }
            finally
            {
                this.EnableCtrl(true);
            }
        }

        public class param
        {
            // 区
            public string location { get; set; }
        }

        public class param2
        {
            public string strUserName { get; set; }

            public string strPassword { get; set; }

            public string strParameters { get; set; }
        }


        public void EnableCtrl(bool bEnable)
        {
            this.button_close.Enabled = bEnable;
            this.button_open.Enabled = bEnable;
            this.button_test.Enabled = bEnable;
        }
        string GetMethodUrl(string strServerUrl, string strMethod)
        {
            if (string.IsNullOrEmpty(strServerUrl) == true)
                return strMethod;

            if (strServerUrl[strServerUrl.Length - 1] == '/')
                return strServerUrl + strMethod;

            return strServerUrl + "/" + strMethod;
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            this.EnableCtrl(false);
            try
            {


                // 调接口

                using (HttpClient httpClient = new HttpClient())
                {
                    param2 p = new param2()
                    {
                        strUserName = "supervisor",
                        strPassword ="",
                        strParameters = "type=worker,client=practice|0.01",
                    };
                    string strquest = JsonConvert.SerializeObject(p);

                    this.textBox_result.Text = "请求信息:" + strquest + "\r\n\r\n";


                    HttpContent content = new StringContent(strquest);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    HttpResponseMessage response = httpClient.PostAsync(
                        "http://localhost/dp2library/xe/rest/login",
                        content).Result;


                    response.EnsureSuccessStatusCode();//用来抛异常的


                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    this.textBox_result.Text += "返回结果:" + responseBody;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "异常:" + ex.Message);
                return;
            }
            finally
            {
                this.EnableCtrl(true);
            }
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            this.EnableCtrl(false);
            try
            {
                // 检查输入参数
                string serverUrl = this.textBox_serverUrl.Text.Trim();
                if (string.IsNullOrEmpty(serverUrl))
                {
                    MessageBox.Show(this, "服务器地址不能为空。");
                    return;
                }
                if (this.textBox_area.Text.Trim() == "")
                {
                    MessageBox.Show(this, "区不能为空。");
                    return;
                }
                string strLocation = this.textBox_area.Text ;


                // 调接口
                using (HttpClient httpClient = new HttpClient())
                {
                    param p = new param()
                    {
                        location = strLocation,
                    };
                    string strquest = JsonConvert.SerializeObject(p);

                    this.textBox_result.Text = "请求信息:" + strquest + "\r\n\r\n";


                    HttpContent content = new StringContent(strquest);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    HttpResponseMessage response = httpClient.PostAsync(
                        GetMethodUrl(serverUrl, "CloseArea"),
                        content).Result;


                    response.EnsureSuccessStatusCode();//用来抛异常的


                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    this.textBox_result.Text += "返回结果:" + responseBody;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "关闭接口异常:" + ex.Message);
                return;
            }
            finally
            {
                this.EnableCtrl(true);
            }
        }

        private void button_WriteRes_help_Click(object sender, EventArgs e)
        {
            // 密集架API 帮助文档
            Process.Start("https://jihulab.com/DigitalPlatform/dp2doc/-/issues/106");
        }
    }
}
