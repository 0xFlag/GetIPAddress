using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetIPAddressDAT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox1.Text == "")
                {
                    MessageBox.Show("请输入IP");
                }
                else
                {
                    bool ips = false;
                    this.textBox2.Clear();
                    Regex reg = new Regex(@"((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)");

                    if (this.comboBox1.SelectedIndex == 0)
                    {
                        for (int i = 0; i < this.textBox1.Lines.Length; i++)
                        {
                            ips = reg.IsMatch(this.textBox1.Lines[i]);
                            if (ips == true)
                            {
                                string text = "http://freeapi.ipip.net/" + this.textBox1.Lines[i];
                                List<string> list = new List<string>();
                                list.Add(i.ToString());
                                list.Add(text);
                                this.get_address(list);
                            }
                            else
                            {
                                this.textBox2.AppendText(this.textBox1.Lines[i] + ",不合法的IP地址\r\n");
                            }
                        }
                    }
                    else if (this.comboBox1.SelectedIndex == 1)
                    {
                        for (int i = 0; i < this.textBox1.Lines.Length; i++)
                        {
                            ips = reg.IsMatch(this.textBox1.Lines[i]);
                            if (ips == true)
                            {
                                string text = "http://ip.taobao.com/service/getIpInfo.php?ip=" + this.textBox1.Lines[i];
                                List<string> list = new List<string>();
                                list.Add(i.ToString());
                                list.Add(text);
                                this.get_address(list);
                            }
                            else
                            {
                                this.textBox2.AppendText(this.textBox1.Lines[i] + ",不合法的IP地址\r\n");
                            }
                        }
                    }
                    else if (this.comboBox1.SelectedIndex == 2)
                    {
                        for (int i = 0; i < this.textBox1.Lines.Length; i++)
                        {
                            ips = reg.IsMatch(this.textBox1.Lines[i]);
                            if (ips == true)
                            {
                                string text = "http://ip-api.com/json/" + this.textBox1.Lines[i] + "?lang=zh-CN";
                                List<string> list = new List<string>();
                                list.Add(i.ToString());
                                list.Add(text);
                                this.get_address(list);
                            }
                            else
                            {
                                this.textBox2.AppendText(this.textBox1.Lines[i] + ",不合法的IP地址\r\n");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void get_address(object obj)
        {
            string url = (obj as List<string>)[1];
            string res = string.Empty;
            res = this.get_json(url);
        }

        public string get_json(string url)
        {
            string result;
            try
            {
                Uri uri = new Uri(url);
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
                myReq.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; BOIE9;ZHCN)";
                myReq.Accept = "*/*";
                myReq.KeepAlive = true;
                myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                myReq.Method = "GET";
                myReq.Referer = "https://www.baidu.com";
                HttpWebResponse resulta = (HttpWebResponse)myReq.GetResponse();
                Stream receviceStream = resulta.GetResponseStream();
                StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("UTF-8"));
                string strHTML = readerOfStream.ReadToEnd();
                this.textBox2.AppendText(strHTML + "\r\n");
                readerOfStream.Close();
                receviceStream.Close();
                resulta.Close();
                return strHTML;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox1.Text == "")
                {
                    MessageBox.Show("请输入IP");
                }
                else
                {
                    this.textBox2.Clear();
                    Regex reg = new Regex(@"((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)");

                    IP.EnableFileWatch = true;
                    IP.Load("17monipdb.dat");

                    string resultMsg = "";
                    string[] ipArr = this.textBox1.Text.Split('\n');
                    for (int i = 0; i < ipArr.Length; i++)
                    {
                        if (ipArr[i] != "")
                        {
                            try
                            {
                                string ip = reg.Match(ipArr[i]).Value;

                                resultMsg += ip + "," + string.Join(",", IP.Find(ip)) + "\r\n";
                            }
                            catch
                            {
                                resultMsg += ipArr[i] + ",不合法的IP地址\r\n";
                            }
                        }
                    }
                    this.textBox2.Text = resultMsg;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();
            this.textBox2.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.txt|*.txt";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                StreamReader streamReader = new StreamReader(openFileDialog.FileName, Encoding.Default);
                this.textBox1.Text = streamReader.ReadToEnd();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.textBox2.Text != "")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = Process.GetCurrentProcess().ProcessName + ".txt";
                if (saveFileDialog.FileName != "" && saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    saveFileDialog.RestoreDirectory = true;
                    saveFileDialog.CreatePrompt = true;
                    StreamWriter streamWriter = File.CreateText(saveFileDialog.FileName);
                    streamWriter.WriteLine(this.textBox2.Text);
                    streamWriter.Close();
                    MessageBox.Show("Save Success!");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();
            this.textBox2.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "暂不支持在线更新IP离线库\r\n\r\n请手动更新\r\nhttp://www.ipip.net/download.html";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "单IP支持在线查询功能和离线查询功能\r\n\r\n批量IP建议不要用在线查询功能\r\n\r\n防止接口频繁访问导致封禁\r\n\r\n在线查询结果是未经过解析的原始JSON数据\r\n\r\nIP离线库：\r\n17monipdb.dat\r\n\r\n非常感谢17mon开源IP库解析代码\r\nhttps://github.com/17mon/csharp\r\n\r\n2019/11/20";
        }
    }
}
