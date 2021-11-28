using DrakeUI.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MLDChecker;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;
using System.Globalization;
using System.Threading;
using System.Net;
using Microsoft.VisualBasic;
using MLDChecker.Properties;
using Leaf.xNet;

namespace MLDChecker
{
    public partial class FormMain : UIMainForm
    {
       #region Khởi Tạo
        private int Count;
        private string[] gmailres;
        private string[] hotmailtres;
        private string[] postdatas;
        private string[] urlslist;
        private string[] yahoores;
        //---------------------------------------------
        public FormMain()
        {
            this.Count = 0;
            this.urlslist = new string[] { "https://edit.yahoo.com/reg_json?AccountID=%MAIL%&PartnerName=yahoo_default&ApiName=ValidateFields", "https://accounts.google.com/InputValidator?resource=SignUp&service=mail", "https://signup.live.com/API/CheckAvailableSigninNames" };
            this.yahoores = new string[] { "'ResultCode':'SUCCESS'", "'SuggestedIDList':['" };
            this.gmailres = new string[] { "'Valid':'true'", "'Valid':'false'" };
            this.hotmailtres = new string[] { "'isAvailable':true", "{'error':" };
            this.postdatas = new string[] { "{'input01':{'Input':'GmailAddress','GmailAddress':'%MAIL%'},'Locale':'en'}", "new_username=%SKID%", "{'signInName':'%MAIL%'}" };
            this.InitializeComponent();


            //Added to support default instance behavour in C#
            if (defaultInstance == null)
                defaultInstance = this;
        }

        //
        //
        //
        //
        //
        //
       
        private void FormMain_Load(object sender, EventArgs e)
        {
            
            //
            textBox1.Enabled = false;

            Control.CheckForIllegalCrossThreadCalls = false;
        }
#endregion

        #region  Chức năng Tool 1 và 2 - Đóng khung để không lẫn
        #region Default Instance

        private static FormMain defaultInstance;
       

        /// <summary>
        /// Added by the VB.Net to C# Converter to support default instance behavour in C#
        /// </summary>
        public static FormMain Default
        {
            get
            {
                if (defaultInstance == null)
                {
                    defaultInstance = new FormMain();
                    defaultInstance.FormClosed += new System.Windows.Forms.FormClosedEventHandler(defaultInstance_FormClosed);
                }

                return defaultInstance;
            }
            set
            {
                defaultInstance = value;
            }
        }

        public static List<string> Listdata { get; internal set; }

        static void defaultInstance_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            defaultInstance = null;
        }

        #endregion

        

        #region Tool Lọc Mail - Tool 1 Trước khi Leecher
        private void button1_Click(object sender, EventArgs e)
        {
            this.ListBox1.Items.AddRange(Clipboard.GetText().ToString().Split(new string[] { "\r" + "\n" }, StringSplitOptions.RemoveEmptyEntries));
            this.Label1.Text = (Convert.ToString(this.ListBox1.Items.Count));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Timer1.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            Timer1.Stop();
            ListBox1.Items.Clear();
            Lgmail.Items.Clear();
            Lyahoo.Items.Clear();
            Lhotmail.Items.Clear();
            lother.Items.Clear();
            gg.Text = "0";
            yahoolabel.Text = "0";
            lm.Text = "0";
            Label9.Text = "0";
            Label1.Text = "0";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.Lgmail.Items.Count == 0)
            {
                MessageBox.Show(
                                "Bạn cần lọc mail trước khi sử dụng chức năng này.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information
                               );
            }
            else
            {
                IEnumerator enumerator = default(IEnumerator);
                StringBuilder builder = new StringBuilder();
                try
                {
                    enumerator = this.Lgmail.Items.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        builder.AppendLine(RuntimeHelpers.GetObjectValue(enumerator.Current).ToString());
                    }
                }
                finally
                {
                    if (enumerator is IDisposable)
                    {
                        (enumerator as IDisposable).Dispose();
                    }
                }

                SaveFileDialog pp = new SaveFileDialog();
                pp.Filter = "|*.txt";
                pp.FileName = "GmailDotCom";
                if (pp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StreamWriter k = new StreamWriter(pp.FileName);
                    k.Write(builder.ToString());
                    k.Close();
                }
                MessageBox.Show("Lưu thành công.", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.Lyahoo.Items.Count == 0)
            {
                MessageBox.Show("Bạn cần lọc mail trước khi sử dụng chức năng này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                IEnumerator enumerator = default(IEnumerator);
                StringBuilder builder = new StringBuilder();
                try
                {
                    enumerator = this.Lyahoo.Items.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        builder.AppendLine(RuntimeHelpers.GetObjectValue(enumerator.Current).ToString());
                    }
                }
                finally
                {
                    if (enumerator is IDisposable)
                    {
                        (enumerator as IDisposable).Dispose();
                    }
                }
                SaveFileDialog pp = new SaveFileDialog();
                pp.Filter = "|*.txt";
                pp.FileName = "YahooDotCom";
                if (pp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StreamWriter k = new StreamWriter(pp.FileName);
                    k.Write(builder.ToString());
                    k.Close();
                }
                MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.Lhotmail.Items.Count == 0)
            {
                MessageBox.Show("Bạn cần lọc mail trước khi sử dụng chức năng này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                IEnumerator enumerator = default(IEnumerator);
                StringBuilder builder = new StringBuilder();
                try
                {
                    enumerator = this.Lhotmail.Items.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        builder.AppendLine(RuntimeHelpers.GetObjectValue(enumerator.Current).ToString());
                    }
                }
                finally
                {
                    if (enumerator is IDisposable)
                    {
                        (enumerator as IDisposable).Dispose();
                    }
                }

                SaveFileDialog pp = new SaveFileDialog();
                pp.Filter = "|*.txt";
                pp.FileName = "HotmailDotCom";
                if (pp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StreamWriter k = new StreamWriter(pp.FileName);
                    k.Write(builder.ToString());
                    k.Close();
                }

                MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (this.lother.Items.Count == 0)
            {
                MessageBox.Show("Bạn cần lọc mail trước khi sử dụng chức năng này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                IEnumerator enumerator = default(IEnumerator);
                StringBuilder builder = new StringBuilder();
                try
                {
                    enumerator = this.lother.Items.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        builder.AppendLine(RuntimeHelpers.GetObjectValue(enumerator.Current).ToString());
                    }
                }
                finally
                {
                    if (enumerator is IDisposable)
                    {
                        (enumerator as IDisposable).Dispose();
                    }
                }

                SaveFileDialog pp = new SaveFileDialog();
                pp.Filter = "|*.txt";
                pp.FileName = "Danh sách Mail Khác.";
                if (pp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StreamWriter k = new StreamWriter(pp.FileName);
                    k.Write(builder.ToString());
                    k.Close();
                }

                MessageBox.Show("Bạn cần lọc mail trước khi sử dụng chức năng này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                ListBox1.SelectedIndex++;

                if (ListBox1.SelectedItem.ToString().Contains("gmail")) //gmail
                {
                    Lgmail.Items.Add(ListBox1.SelectedItem.ToString());


                }
                else if (ListBox1.SelectedItem.ToString().Contains("hotmail")) //Hotmail
                {

                    Lhotmail.Items.Add(ListBox1.SelectedItem.ToString());
                }
                else if (ListBox1.SelectedItem.ToString().Contains("yahoo")) //Yahoo
                {

                    Lyahoo.Items.Add(ListBox1.SelectedItem.ToString());
                }
                else
                {
                    lother.Items.Add(ListBox1.SelectedItem.ToString());
                }

            }
            catch (Exception)
            {
                Timer1.Stop();
            }
            gg.Text = System.Convert.ToString(Lgmail.Items.Count);
            yahoolabel.Text = System.Convert.ToString(Lyahoo.Items.Count);
            other.Text = System.Convert.ToString(lother.Items.Count);
            lm.Text = System.Convert.ToString(Lhotmail.Items.Count);
            Label9.Text = System.Convert.ToString(ListBox1.SelectedIndex);
        }

        #endregion


        #region Tool Leecher Mail từ dang Mail:Pass sang Mail - Tool 2 Trước Khi Checker
        private void btnLeecher_Click(object sender, EventArgs e)
        {
            TCMainMenu.SelectTab(tabLeecher);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.listBox3.Items.Clear();
            OpenFileDialog dialog = new OpenFileDialog()
            {
                RestoreDirectory = true,
                Multiselect = false,
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 1
            };
            dialog.ShowDialog();
            if (Operators.CompareString(dialog.FileName, null, true) != 0)
            {
                List<string> list = new List<string>();
                string fileName = dialog.FileName;
                StreamReader reader = new StreamReader(fileName);
                while (!reader.EndOfStream)
                {
                    this.listBox3.Items.Add(reader.ReadLine());
                }
                using (StreamReader reader2 = new StreamReader(dialog.FileName))
                {
                    while (reader2.Peek() != -1)
                    {
                        list.Add(reader2.ReadLine());
                        this.textBox1.Text = fileName;
                    }
                }

            }
        }


        private void button8_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            ListBox2.Items.Clear();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (this.listBox3.Items.Count > 0)
            {
                this.ListBox2.Items.Clear();
                string text = this.textBox1.Text;
                Cursor.Current = Cursors.WaitCursor;
                if (Path.GetDirectoryName(text).Length == 0)
                {
                    text = Application.StartupPath + "\\" + text;
                }
                string source = "";
                if (Operators.CompareString(Path.GetExtension(text).ToLower(), ".txt", true) == 0)
                {
                    StreamReader reader = default(StreamReader);
                    try
                    {
                        reader = new StreamReader(text);
                        source = reader.ReadToEnd();
                    }
                    catch (Exception exception1)
                    {
                        ProjectData.SetProjectError(exception1);
                        Exception exception = exception1;
                        Interaction.MsgBox("Không thể đọc tệp văn bản", MsgBoxStyle.ApplicationModal, null);
                        source = "";
                        ProjectData.ClearProjectError();
                    }
                    finally
                    {
                        if (!ReferenceEquals(reader, null))
                        {
                            reader.Close();
                        }
                    }
                }
                else
                {
                    object objectValue = null;
                    try
                    {
                        object obj3 = null;
                        try
                        {
                            objectValue = RuntimeHelpers.GetObjectValue(Interaction.CreateObject("Word.Application", ""));
                        }
                        catch (Exception exception5)
                        {
                            ProjectData.SetProjectError(exception5);
                            Exception exception2 = exception5;
                            Interaction.MsgBox("Không thể lấy địa chỉ email", MsgBoxStyle.ApplicationModal, null);
                            throw (exception2);
                        }
                        try
                        {
                            object[] arguments = new object[] { text };
                            bool[] copyBack = new bool[] { true };
                            if (copyBack[0])
                            {
                                text = System.Convert.ToString(Conversions.ChangeType(RuntimeHelpers.GetObjectValue(arguments[0]), typeof(string)));
                            }
                            obj3 = RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(NewLateBinding.LateGet(objectValue, null, "Các tài liệu", new object[0 - 1 + 1], null, null, null), null, "Mở", arguments, null, null, copyBack));
                        }
                        catch (Exception exception6)
                        {
                            ProjectData.SetProjectError(exception6);
                            Exception exception3 = exception6;
                            Interaction.MsgBox("Lỗi không thể nhận biết", MsgBoxStyle.ApplicationModal, null);
                            throw (exception3);
                            ProjectData.ClearProjectError();
                        }
                        source = Conversions.ToString(NewLateBinding.LateGet(NewLateBinding.LateGet(obj3, null, "Nội Dung", new object[0 - 1 + 1], null, null, null), null, "Văn Bản", new object[0 - 1 + 1], null, null, null));
                    }
                    catch (Exception exception7)
                    {
                        ProjectData.SetProjectError(exception7);
                        Exception exception4 = exception7;
                        source = "";
                        ProjectData.ClearProjectError();
                    }
                    finally
                    {
                        if (!ReferenceEquals(objectValue, null))
                        {
                            NewLateBinding.LateCall(objectValue, null, "Từ bỏ", new object[0 - 1 + 1], null, null, null, true);
                        }
                    }
                }
                if (source.Length == 0)
                {
                    return;
                }
                StringBuilder builder = new StringBuilder();
                string str4 = "";
                foreach (string tempLoopVar_str4 in this.ExtractEmailAddressesFromString(source))
                {
                    str4 = tempLoopVar_str4;
                    this.ListBox2.Items.Add(str4);
                    this.TongLeecher.Text = Conversions.ToString(this.ListBox2.Items.Count);
                }
            }
            if (this.listBox3.Items.Count == 0)
            {
                MessageBox.Show("Nhập một danh sách tổ hợp trước");
            }
        }

        //-----------------------------------Leeching Emails----------------------------//

        private string[] ExtractEmailAddressesFromString(string source)
        {
            MatchCollection matchs = Regex.Matches(source, "([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})");
            string[] strArray2 = new string[((matchs.Count - 1) + 1) - 1 + 1];
            int num2 = strArray2.Length - 1;
            int i = 0;
            while (i <= num2)
            {
                strArray2[i] = matchs[i].Value;
                i++;
            }
            return strArray2;
        }
        //------------------------------------------------------------------------------//
        private void button9_Click(object sender, EventArgs e)
        {

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter writer = new StreamWriter(dialog.FileName);
                    int num2 = this.ListBox2.Items.Count - 1;
                    int i = 0;
                    while (i <= num2)
                    {
                        writer.WriteLine(RuntimeHelpers.GetObjectValue(this.ListBox2.Items[i]));
                        i++;
                    }
                    writer.Close();
                }
            }
        }

        #endregion

        #region  Chức năng Menu
        private void drakeUILinkLabel1_Click(object sender, EventArgs e)
        {
            Process.Start("https://drakelam.com/");
        }

        private void btnChecker_Click(object sender, EventArgs e)
        {
            //TCMainMenu.SelectTab(tabChecker);
            TCMainMenu.SelectTab(tabChecker);
        }
      
        private void drakeUIImageButton4_Click(object sender, EventArgs e)
        {
           
        }

        private void drakeUIImageButton5_Click(object sender, EventArgs e)
        {
            TCMainMenu.SelectTab(tabAbout); 
        }
        private void btnFilter_Click(object sender, EventArgs e)
        {
            TCMainMenu.SelectTab(tabFilter);

        }

        #endregion

        #endregion

        #region  Chức năng Tool 3: Check Gmail và Hotmail không cần Proxy chạy mượt - Đóng khung để không lẫn

        #region  Check Gmail và Hotmail không cần Proxy
        private string _DrakeLamDotCom4(Match a0)
        {
            return Conversions.ToString(this.UnescapeCallabck(a0));
        }

        //_______________________________________________________________
        private char UnescapeCallabck(Match m)
        {
            return Conversions.ToChar(char.ToString(Convert.ToChar(UInt16.Parse(m.Groups[1].Value, NumberStyles.AllowHexSpecifier))));
        }

        private string UnescapeUnicode(string value)
        {
            return Regex.Replace(value, "\\\\[Uu]([0-9A-Fa-f]{4})", new MatchEvaluator(this._DrakeLamDotCom4));
        }


        //______________________________________________________________________
        public class CloudDrakeLam1
        {
            // Methods
            [DebuggerNonUserCode]
            public CloudDrakeLam1()
            {
            }

            [DebuggerNonUserCode]
            public CloudDrakeLam1(CloudDrakeLam1 other)
            {
                if (!ReferenceEquals(other, null))
                {
                    this.CSLocal_URI = other.CSLocal_URI;
                    this.ToiCodeCSharp = other.ToiCodeCSharp;
                }
            }

            [CompilerGenerated]
            public void _DrakeLamDotCom1()
            {
                this.ToiCodeCSharp.Checker(this.CSLocal_URI, "GET", "", this.ToiCodeCSharp.yahoores);
            }


            // Fields
            public string CSLocal_URI;
            public FormMain ToiCodeCSharp;
        }

        [CompilerGenerated]
        public class CloudDrakeLam2
        {
            // Methods
            [DebuggerNonUserCode]
            public CloudDrakeLam2()
            {
            }

            [DebuggerNonUserCode]
            public CloudDrakeLam2(CloudDrakeLam2 other)
            {
                if (!ReferenceEquals(other, null))
                {
                    this.CSLocal_data = other.CSLocal_data;
                    this.ToiCodeCSharp = other.ToiCodeCSharp;
                }
            }

            [CompilerGenerated]
            public void _DrakeLamDotCom2()
            {
                this.ToiCodeCSharp.Checker(this.ToiCodeCSharp.urlslist[1], "POST", this.CSLocal_data, this.ToiCodeCSharp.gmailres);
            }


            // Fields
            public string CSLocal_data;
            public FormMain ToiCodeCSharp;
        }

        [CompilerGenerated]
        public class CloudDrakeLam3
        {
            // Methods
            [DebuggerNonUserCode]
            public CloudDrakeLam3()
            {
            }

            [DebuggerNonUserCode]
            public CloudDrakeLam3(CloudDrakeLam3 other)
            {
                if (!ReferenceEquals(other, null))
                {
                    this.ToiCodeCSharp = other.ToiCodeCSharp;
                    this.CSLocal_data = other.CSLocal_data;
                }
            }

            [CompilerGenerated]
            public void _DrakeLamDotCom3()
            {
                this.ToiCodeCSharp.Checker(this.ToiCodeCSharp.urlslist[2], "POST", this.CSLocal_data, this.ToiCodeCSharp.hotmailtres);
            }


            // Fields
            public string CSLocal_data;
            public FormMain ToiCodeCSharp;
        }

        [CompilerGenerated]
        public class CloudDrakeLam4
        {
            // Methods
            [DebuggerNonUserCode]
            public CloudDrakeLam4()
            {
            }

            [DebuggerNonUserCode]
            public CloudDrakeLam4(CloudDrakeLam4 other)
            {
                if (!ReferenceEquals(other, null))
                {
                    this.CSLocal_URI = other.CSLocal_URI;
                    this.ToiCodeCSharp = other.ToiCodeCSharp;
                }
            }

            [CompilerGenerated]
            public void _DrakeLamDotCom5()
            {
                this.ToiCodeCSharp.Checker(this.CSLocal_URI, "GET", "", this.ToiCodeCSharp.yahoores);
            }


            // Fields
            public string CSLocal_URI;
            public FormMain ToiCodeCSharp;
        }

        [CompilerGenerated]
        public class CloudDrakeLam5
        {
            // Methods
            [DebuggerNonUserCode]
            public CloudDrakeLam5()
            {
            }

            [DebuggerNonUserCode]
            public CloudDrakeLam5(CloudDrakeLam5 other)
            {
                if (!ReferenceEquals(other, null))
                {
                    this.CSLocal_data = other.CSLocal_data;
                    this.ToiCodeCSharp = other.ToiCodeCSharp;
                }
            }

            [CompilerGenerated]
            public void _DrakeLamDotCom6()
            {
                this.ToiCodeCSharp.Checker(this.ToiCodeCSharp.urlslist[1], "POST", this.CSLocal_data, this.ToiCodeCSharp.gmailres);
            }


            // Fields
            public string CSLocal_data;
            public FormMain ToiCodeCSharp;
        }

        [CompilerGenerated]
        public class CloudDrakeLam6
        {
            // Methods
            [DebuggerNonUserCode]
            public CloudDrakeLam6()
            {
            }

            [DebuggerNonUserCode]
            public CloudDrakeLam6(CloudDrakeLam6 other)
            {
                if (!ReferenceEquals(other, null))
                {
                    this.CSLocal_data = other.CSLocal_data;
                    this.ToiCodeCSharp = other.ToiCodeCSharp;
                }
            }

            [CompilerGenerated]
            public void _DrakeLamDotCom7()
            {
                this.ToiCodeCSharp.Checker(this.ToiCodeCSharp.urlslist[2], "POST", this.CSLocal_data, this.ToiCodeCSharp.hotmailtres);
            }


            // Fields
            public string CSLocal_data;
            public FormMain ToiCodeCSharp;
        }
        //______________________________________________________________________
        public void Check()
        {
            if (this.Count < (this.Danh_Sach_Mail.Items.Count - 1))
            {
                this.Cont();
            }
            else
            {
                this.Enable(2);
                this.timer2.Start();
            }
        }

        public void Checker(string url, string method, string postdata, string[] resarry)
        {
            HttpWebRequest request = default(HttpWebRequest);
            HttpWebResponse response = default(HttpWebResponse);
            string expression = "";
            string str = "";
            CookieContainer container = new CookieContainer();
            string str4 = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.84 Safari/537.36";
            if (method == "POST")
            {
                if (this.cbHotmail.Checked)
                {
                    try
                    {
                        request = (HttpWebRequest)(WebRequest.Create("https://signup.live.com/"));
                        request.UserAgent = str4;
                        request.CookieContainer = container;
                        response = (HttpWebResponse)(request.GetResponse());
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            expression = reader.ReadToEnd();
                            reader.Dispose();
                        }

                        str = expression.Substring(expression.IndexOf("apiCanary") + 12);
                        str = str.Substring(0, str.IndexOf("\""));
                        str = this.UnescapeUnicode(str);
                        request.Abort();
                        response.Close();
                    }
                    catch (Exception exception1)
                    {
                        ProjectData.SetProjectError(exception1);
                        Exception exception = exception1;
                        this.Edit(3);
                        this.Check();
                        ProjectData.ClearProjectError();
                        return;
                    }
                }
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(postdata);
                    request = (HttpWebRequest)(WebRequest.Create(url));
                    request.Method = "POST";
                    request.UserAgent = "Mozilla/5.0 (X11; Linux x86_64; rv:31.0) Gecko/20100101 Firefox/31.0 Iceweasel/31.3.0" ;
                    if (this.cbGmail.Checked)
                    {
                        request.ContentType = "application/json";
                    }
                    else
                    {
                        request.ContentType = "application/x-www-form-urlencoded";
                    }
                    if (this.cbHotmail.Checked)
                    {
                        request.Headers.Add("canary", str);
                        request.CookieContainer = container;
                    }
                    request.ContentLength = bytes.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Dispose();
                    response = (HttpWebResponse)(request.GetResponse());
                    using (StreamReader reader2 = new StreamReader(response.GetResponseStream()))
                    {
                        expression = reader2.ReadToEnd();
                        reader2.Dispose();
                    }

                    expression = Strings.Replace(expression, "\"", "'", 1, -1, CompareMethod.Binary);
                    if (expression.Contains(resarry[0]))
                    {
                        request.Abort();
                        response.Close();
                        this.Edit(1);
                        this.Check();
                    }
                    else if (expression.Contains(resarry[1]))
                    {
                        request.Abort();
                        response.Close();
                        this.Edit(2);
                        this.Check();
                    }
                    else
                    {
                        request.Abort();
                        response.Close();
                        this.Edit(3);
                        this.Check();
                    }
                }
                catch (Exception exception4)
                {
                    ProjectData.SetProjectError(exception4);
                    Exception exception2 = exception4;
                    this.Edit(3);
                    this.Check();
                    ProjectData.ClearProjectError();
                    return;
                    //					ProjectData.ClearProjectError();
                }
            }
            else if (method == "GET")
            {
                try
                {
                    request = (HttpWebRequest)(WebRequest.Create(url));
                    request.Method = "GET";
                    response = (HttpWebResponse)(request.GetResponse());
                    using (StreamReader reader3 = new StreamReader(response.GetResponseStream()))
                    {
                        expression = reader3.ReadToEnd();
                        reader3.Dispose();
                    }

                    expression = Strings.Replace(expression, "\"", "'", 1, -1, CompareMethod.Binary);
                    if (expression.Contains(resarry[0]))
                    {
                        request.Abort();
                        response.Close();
                        this.Edit(1);
                        this.Check();
                    }
                    else if (expression.Contains(resarry[1]))
                    {
                        request.Abort();
                        response.Close();
                        this.Edit(2);
                        this.Check();
                    }
                    else
                    {
                        request.Abort();
                        response.Close();
                        this.Edit(3);
                        this.Check();
                    }
                }
                catch (Exception exception5)
                {
                    ProjectData.SetProjectError(exception5);
                    Exception exception3 = exception5;
                    this.Edit(3);
                    this.Check();
                    ProjectData.ClearProjectError();
                    return;
                    //					ProjectData.ClearProjectError();
                }
            }
            else
            {
                this.Edit(3);
                this.Check();
            }
        }

        public void Cont()
        {
            this.Count++;
            if (this.cbGmail.Checked)
            {
                CloudDrakeLam5 Lam2 = new CloudDrakeLam5()
                {
                    ToiCodeCSharp = this,
                    CSLocal_data = this.postdatas[0]
                };
                string replacement = Strings.Replace(this.Danh_Sach_Mail.Items[this.Count].Text, "@gmail.com", "", 1, -1, CompareMethod.Binary);
                Lam2.CSLocal_data = Strings.Replace(Lam2.CSLocal_data, "%MAIL%", replacement, 1, -1, CompareMethod.Binary);
                Lam2.CSLocal_data = Strings.Replace(Lam2.CSLocal_data, "'", "\"", 1, -1, CompareMethod.Binary);
                Thread OO = new Thread(new ThreadStart(Lam2._DrakeLamDotCom6));
                OO.Start();
            }
            else if (this.cbHotmail.Checked)
            {
                CloudDrakeLam6 Lam3 = new CloudDrakeLam6()
                {
                    ToiCodeCSharp = this,
                    CSLocal_data = this.postdatas[2]
                };
                Lam3.CSLocal_data = Strings.Replace(Lam3.CSLocal_data, "%MAIL%", this.Danh_Sach_Mail.Items[this.Count].Text, 1, -1, CompareMethod.Binary);
                Lam3.CSLocal_data = Strings.Replace(Lam3.CSLocal_data, "'", "\"", 1, -1, CompareMethod.Binary);
                Thread II = new Thread(new ThreadStart(Lam3._DrakeLamDotCom7));
                II.Start();
            }
        }


        public void Edit(int Num)
        {
            if (Num == 1)
            {
                this.Danh_Sach_Mail.Items[this.Count].ForeColor = Color.Red;
                this.Danh_Sach_Mail.Items[this.Count].SubItems[1].Text = "Chưa đăng ký";
            }
            else if (Num == 2)
            {
                this.Danh_Sach_Mail.Items[this.Count].ForeColor = Color.Green;
                this.Danh_Sach_Mail.Items[this.Count].SubItems[1].Text = "Đã đăng ký";
            }
            else if (Num == 3)
            {
                this.Danh_Sach_Mail.Items[this.Count].ForeColor = Color.Red;
                this.Danh_Sach_Mail.Items[this.Count].SubItems[1].Text = "Lỗi xử lý";
            }
        }

        public void Enable(int Num)
        {
            if (Num == 1)
            {
                this.btnStart.Enabled = false;
                this.cbGmail.Enabled = false;
                this.cbHotmail.Enabled = false;
            }
            else if (Num == 2)
            {
                this.btnStart.Enabled = true;
                this.cbGmail.Enabled = true;
                this.cbHotmail.Enabled = true;
            }
        }
        //______________________________________________________________________


        //______________________________________________________________________


        //______________________________________________________________________


        //______________________________________________________________________
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.Enable(1);
            this.timer2.Stop();
            this.Count = 0;
            if (this.cbGmail.Checked)
            {
                CloudDrakeLam2 Lam2 = new CloudDrakeLam2()
                {
                    ToiCodeCSharp = this,
                    CSLocal_data = this.postdatas[0]
                };
                string replacement = Strings.Replace(this.Danh_Sach_Mail.Items[this.Count].Text, "@gmail.com", "", 1, -1, CompareMethod.Binary);
                Lam2.CSLocal_data = Strings.Replace(Lam2.CSLocal_data, "%MAIL%", replacement, 1, -1, CompareMethod.Binary);
                Lam2.CSLocal_data = Strings.Replace(Lam2.CSLocal_data, "'", "\"", 1, -1, CompareMethod.Binary);
                Thread JJ = new Thread(new ThreadStart(Lam2._DrakeLamDotCom2));
                JJ.Start();
            }
           
            else if (this.cbHotmail.Checked)
            {
                CloudDrakeLam3 Lam3 = new CloudDrakeLam3()
                {
                    ToiCodeCSharp = this,
                    CSLocal_data = this.postdatas[2]
                };
                Lam3.CSLocal_data = Strings.Replace(Lam3.CSLocal_data, "%MAIL%", this.Danh_Sach_Mail.Items[this.Count].Text, 1, -1, CompareMethod.Binary);
                Lam3.CSLocal_data = Strings.Replace(Lam3.CSLocal_data, "'", "\"", 1, -1, CompareMethod.Binary);
                Thread MM = new Thread(new ThreadStart(Lam3._DrakeLamDotCom3));
                MM.Start();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (this.Danh_Sach_Mail.Items.Count != 0)
            {
                this.btnStart.Enabled = true;
                this.LuuMailChuaDK.Enabled = true;
            }
            else
            {
                this.btnStart.Enabled = false;
                this.LuuMailChuaDK.Enabled = false;
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProjectData.EndApp();

           
        }

       

        private void btnOpenMail_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            TextBox box = new TextBox();
            dialog.Title = "Open";
            dialog.Filter = "Text File(.txt)|*.txt";
            dialog.FileName = null;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                box.Text = File.ReadAllText(dialog.FileName);
                string str = "";
                foreach (string tempLoopVar_str in box.Lines)
                {
                    str = tempLoopVar_str;
                    ListViewItem item = new ListViewItem() { Text = str };
                    item.SubItems.Add("None");
                    item.ForeColor = Color.Black;
                    this.Danh_Sach_Mail.Items.Add(item);
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Danh_Sach_Mail.Items.Clear();
        }

        private void LuuMailDaDK_Click(object sender, EventArgs e)
        {
            string text = null;
            int num2 = this.Danh_Sach_Mail.Items.Count - 1;
            int i = 0;
            while (i <= num2)
            {
                if (this.Danh_Sach_Mail.Items[i].SubItems[1].Text == "Chưa đăng ký")
                {
                    if (this.cbGmail.Checked)
                    {
                        text = text + this.Danh_Sach_Mail.Items[i].Text + "@gmail.com" + System.Convert.ToString("\r") + System.Convert.ToString("\n");
                    }
                    else if (this.cbHotmail.Checked)
                    {
                        text = text + this.Danh_Sach_Mail.Items[i].Text + "@hotmail.com" + System.Convert.ToString("\r") + System.Convert.ToString("\n");
                    }
                }
                i++;
            }
            Clipboard.SetText(text);
            MessageBox.Show("Đã Copy", "Xong!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        private void LuuMailChuaDK_Click(object sender, EventArgs e)
        {

            string text = null;
            int num2 = this.Danh_Sach_Mail.Items.Count - 1;
            int i = 0;
            while (i <= num2)
            {
                if (this.Danh_Sach_Mail.Items[i].SubItems[1].Text == "Đã đăng ký")
                {
                    if (this.cbGmail.Checked)
                    {
                        text = text + this.Danh_Sach_Mail.Items[i].Text + "@gmail.com" + System.Convert.ToString("\r") + System.Convert.ToString("\n");
                    }
                    else if (this.cbHotmail.Checked)
                    {
                        text = text + this.Danh_Sach_Mail.Items[i].Text + "@hotmail.com" + System.Convert.ToString("\r") + System.Convert.ToString("\n");
                    }
                }
                i++;
            }
            Clipboard.SetText(text);
            MessageBox.Show("Đã Copy", "Xong!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        #endregion




        /* Bước lưu này bạn có thể hiểu như sau:
         * 
         * SubItems[1] 
         * ======>>>> 1 : Đã đăng ký.
         * ======>>>> 2 : Chưa đăng ký.
         * ***********************************
         * SubItems[1]  : Đã đăng ký.
         * SubItems[2]  : Chưa đăng ký.
         * Trong cả 2 TH: check hotmail và gmail. 
         * Sau quá trình check thành công nếu là hot mail "SubItems[1] " sẽ lấy thông tin mail đã đăng ký và ngược lại.
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         */
        #endregion

        private void drakeUIImageButton8_Click(object sender, EventArgs e)
        {
            Process.Start("https://t.me/ToolMMOBusiness");
        }

        private void drakeUIImageButton9_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/dev.drakelam");
        }

        private void drakeUIImageButton6_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/drakelam");
        }

        private void drakeUIImageButton7_Click(object sender, EventArgs e)
        {
            Process.Start("https://drakelam.com/");
        }
    }
}
