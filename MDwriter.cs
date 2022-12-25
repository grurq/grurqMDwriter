using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MDtohtml;
using MDwritercsharp;

// 参考 https://lets-csharp.com/textbox-undo/


class gets : RichTextBox
{
    List<string> undo = new List<string>();


    private void stockundo()
    {

        undo.Add(this.Text);
        if (undo.Count > 10) undo.RemoveAt(0);

    }
    //https://dobon.net/vb/dotnet/control/tbsuppressbeep.html


    protected override void OnKeyDown(KeyEventArgs e)
    {
        int pos = 0;
        if ((e.KeyCode & Keys.C) == Keys.C && e.Control && e.Shift)
        {
            if (SelectionLength > 0)
            {
                e.Handled = true;
                exe.sandwich(this, "`");
            }
            else
            {
                e.Handled = true;
                exe.putin(this, "\r\n\r\n```\r\n\r\n");
            }

        }
        else if ((e.KeyCode & Keys.R) == Keys.R && e.Control)
        {
                e.Handled = true;

        }
        else if ((e.KeyCode & Keys.L) == Keys.L && e.Control)
        {
            e.Handled = true;

        }
        else if (((int)e.KeyCode & 219) == (int)219 && e.Control)// '}' 219
        {
            if (SelectionLength > 0)
            {
                e.Handled = true;
                exe.putonhead(this, "- ");
            }
        }
        else if (((int)e.KeyCode & 221) == (int)221 && e.Control)// '{' 221
        {
            if (SelectionLength > 0)
            {
                e.Handled = true;
                exe.putonhead(this, "1. ");
            }
        }


        else if ((e.KeyCode & Keys.H) == Keys.H && e.Control)
        {
            if (e.KeyCode == Keys.H)
            {
                e.Handled = true;
                exe.putin(this, "\r\n---\r\n\r\n");

            }

        }
        else if ((e.KeyCode & Keys.D1) == Keys.D1 && e.Control)
        {
            e.Handled = true;
            exe.putin(this, "# ");
        }
        else if ((e.KeyCode & Keys.D2) == Keys.D2 && e.Control)
        {
            e.Handled = true;
            exe.putin(this, "## ");
        }
        else if ((e.KeyCode & Keys.D3) == Keys.D3 && e.Control)
        {
            e.Handled = true;
            exe.putin(this, "### ");
        }
        else if ((e.KeyCode & Keys.D4) == Keys.D4 && e.Control)
        {
            e.Handled = true;
            exe.putin(this, "#### ");
        }
        else if ((e.KeyCode & Keys.D5) == Keys.D5 && e.Control)
        {
            e.Handled = true;
            exe.putin(this, "##### ");
        }
        else if ((e.KeyCode & Keys.D6) == Keys.D6 && e.Control)
        {
            e.Handled = true;
            exe.putin(this, "###### ");
        }
       
        else if ((e.KeyCode & Keys.B) == Keys.B && e.Control && e.Shift)
        {

            if (SelectionLength > 0)
            {
                e.Handled = true;
                exe.putonhead(this, "> ");
            }


        }
        else if ((e.KeyCode & Keys.U) == Keys.U && e.Control)
        {
            if (SelectionLength > 0)
            {
                e.Handled = true;
                exe.sandwich(this, "*");
            }

        }
        else if ((e.KeyCode & Keys.B) == Keys.B && e.Control)
        {
            if (SelectionLength > 0)
            {
                e.Handled = true;
                exe.sandwich(this, "**");
            }

        }




        // これは一番最後に置く タブ挿入
        else if ((e.KeyCode & Keys.T) == Keys.T && e.Control)
        {
        if (SelectionLength > 0)
            {
            e.Handled = true;
            exe.putonhead(this, "\t");
            }
        }

    }

    class outs : WebBrowser
    {
        protected override void OnNavigated(WebBrowserNavigatedEventArgs e)
        {
            Stop();
            base.OnNavigated(e);
        }
    }
    class prop : Form //プロパティ用フォーム
    {
        /*
             public bool underline;
        public bool bold;
        public int fontsize;
        public string fonts;
        prop(bool ud,bool bd,int fs,string ft)
        {
            underline = ud;
            bold = bd;
            fontsize = fs;
            fonts = ft;
        }
             */
        
        public bool underline;
        public bool bold;
        public int fontsize;
        public string fonts;
        prop(bool ud, bool bd, int fs, string ft)
        {
            underline = ud;
            bold = bd;
            fontsize = fs;
            fonts = ft;
        }
        public void setprop(ref bool ud, ref bool bd, ref int fs, ref string ft)
        {

        }


    }
    class exe : Form
    {
        public const int WIDTH = 800;
        public const int HEIGHT = 600;
        public const int UNDODOC = 10;
        private const int MENUITEMS = 5;
        private const int MENUDROPS = 15;

        private gets inp;
        private outs otp;
        private string charcode = "utf-8";

        private MenuStrip menu;
        List<List<ToolStripMenuItem>> item = new List<List<ToolStripMenuItem>>();

        



        private gets setinp(gets o)
        {
            o.Dock = DockStyle.Left;


            //Tabキーでタブ記号が入力されるようにする
            o.AcceptsTab = true;
            o.DetectUrls = false;

            o.Multiline = true;
            o.Width = WIDTH / 10 * 4;
            o.Height = HEIGHT;
            o.WordWrap = true;
            o.ScrollBars = RichTextBoxScrollBars.Vertical;
            o.HideSelection = false;

            o.TextChanged += new EventHandler(write);
            o.Parent = this;

            return o;
        }

        public static void sandwich(gets o, string chars)
        {
            if (o.SelectionLength > 0)
            {
                string cp = Clipboard.GetText();

                o.Cut();
                Clipboard.SetText(chars + Clipboard.GetText() + chars);
                o.Paste();
                Clipboard.SetText(cp);
                o.Focus();
            }


        }
        public static void putin(gets o, string chars)
        {

            string Clip = Clipboard.GetText();
            if (Clip == null) Clip = "";
            Clipboard.SetText(chars);
            o.Paste();
            Clipboard.SetText(Clip);
            o.Focus();
        }
        public static void putonhead(gets o, string chars)
        {
            if (o.SelectionLength > 0)
            {
                string cp = Clipboard.GetText();

                o.Cut();
                Clipboard.SetText(chars + Clipboard.GetText().Replace("\n", "\n" + chars));
                o.Paste();
                Clipboard.SetText(cp);
                o.Focus();
            }
        }
        private outs setotp(outs o)
        {
            o.Dock = DockStyle.Right;
            o.Navigate("");
            o.Width = WIDTH / 20 * 11;
            o.Height = HEIGHT;

            o.Parent = this;
            return o;
        }

        private MenuStrip setmenu(MenuStrip o)
        {
            int i, j;
            ToolStripMenuItem bar = new ToolStripMenuItem("---------");
            item.Add(new List<ToolStripMenuItem>());
            item[0].Add(new ToolStripMenuItem("ファイル(&F)"));
            item[0].Add(new ToolStripMenuItem("新規(&N)"));
            item[0][1].ShortcutKeys = Keys.Control | Keys.N;
            item[0].Add(new ToolStripMenuItem("開く(&O)"));
            item[0][2].ShortcutKeys = Keys.Control | Keys.O;
            item[0].Add(new ToolStripMenuItem("名前をつけて保存(&A)"));
            item[0][3].ShortcutKeys = Keys.Control |Keys.Shift | Keys.S;
            item[0].Add(new ToolStripMenuItem("上書き保存(&S)"));
            item[0][4].ShortcutKeys = Keys.Control | Keys.S;
            item[0].Add(new ToolStripMenuItem("htmlに出力(&H)"));
            item[0][5].ShortcutKeys = Keys.Control | Keys.Shift | Keys.H;
            item[0].Add(new ToolStripMenuItem("終了(&X)"));
            for (i = 1; i < item[0].Count; i++) item[0][0].DropDownItems.Add(item[0][i]);
            item.Add(new List<ToolStripMenuItem>());
            item[1].Add(new ToolStripMenuItem("編集(&E)"));
            item[1].Add(new ToolStripMenuItem("もとに戻す(&U)"));
            item[1][1].ShortcutKeys = Keys.Control | Keys.Z;
            item[1].Add(new ToolStripMenuItem("やり直し(&R)"));
            item[1][2].ShortcutKeys = Keys.Control | Keys.Y;
            item[1].Add(bar);
            item[1].Add(new ToolStripMenuItem("切り取り(&T)"));
            item[1][4].ShortcutKeys = Keys.Control | Keys.X;
            item[1].Add(new ToolStripMenuItem("コピー(&C)"));
            item[1][5].ShortcutKeys = Keys.Control | Keys.C;
            item[1].Add(new ToolStripMenuItem("貼り付け(&P)"));
            item[1][6].ShortcutKeys = Keys.Control | Keys.V;
            for (i = 1; i < item[1].Count; i++) item[1][0].DropDownItems.Add(item[1][i]);
            /*
            item.Add(new List<ToolStripMenuItem>());
            item[2].Add(new ToolStripMenuItem("挿入"));
            item[2].Add(new ToolStripMenuItem("強調"));
            item.Add(new List<ToolStripMenuItem>());
            item[3].Add(new ToolStripMenuItem("表示"));
            item[3].Add(new ToolStripMenuItem("戻る"));
            */
            item.Add(new List<ToolStripMenuItem>());
            item[2].Add(new ToolStripMenuItem("ヘルプ(&H)"));
            item[2].Add(new ToolStripMenuItem("変換設定(&T)"));
            item[2].Add(new ToolStripMenuItem("アプリ概要(&A)"));

            for (i = 1; i < item[2].Count; i++) item[2][0].DropDownItems.Add(item[2][i]);

            for (i = 0; i < item.Count; i++)
            {
                o.Items.Add(item[i][0]);
            }

            for (i = 0; i < item.Count; i++)
            {
                for (j = 0; j < item[i].Count; j++)
                {
                    item[i][j].Click += new EventHandler(item_click);
                }
            }
            this.MainMenuStrip = o;
            o.Parent = this;
            return o;
        }
        public void openfilemenu()
        {
            OpenFileDialog dr = new OpenFileDialog();

            dr.InitialDirectory = this.Text.Substring(0, this.Text.LastIndexOf('\\'));
            dr.Filter = "markdownファイル|*.md";
            if (dr.ShowDialog() == DialogResult.OK)
            {
                string getpath = dr.FileName;
                StreamReader sr = new StreamReader(dr.FileName, System.Text.Encoding.GetEncoding(charcode));
                this.Text = getpath + " - MDwriter";
                inp.Text = sr.ReadToEnd();
                sr.Close();
            }
        }
        public void savehtml()
        {
            
            SaveFileDialog sfd = new SaveFileDialog();
            string fpath = this.Text.Substring(0, this.Text.LastIndexOf(" - MDwriter"));
            string fname = fpath.Substring(fpath.LastIndexOf('\\')+1);
            if (fname.Contains("."))fname = fname.Substring(0,fname.LastIndexOf('.'));

            sfd.FileName = fname + ".html";
            sfd.InitialDirectory = this.Text.Substring(0, this.Text.LastIndexOf('\\')); 
            sfd.Filter = "htmlファイル(*.html)|*.html|すべてのファイル(*.*)|*.*";
            sfd.Title = "保存先";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, "<html>\r\n<title>"+fname+"</title>\r\n<body>\r\n"+otp.DocumentText+"</body>\r\n</html>", System.Text.Encoding.GetEncoding(charcode));

            }
        }
        public void savefile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            /*
            string date = DateTime.Now.ToString();
            date = date.Substring(0, date.LastIndexOf(':'));
            date = date.Replace("/", "");
            date = date.Replace(":", "");
            date = date.Replace(" ", "_");
            */

            sfd.FileName ="無題.md";
            sfd.InitialDirectory = this.Text.Substring(0, this.Text.LastIndexOf('\\'));
            sfd.Filter = "markdownファイル(*.md)|*.md|すべてのファイル(*.*)|*.*";
            sfd.Title = "保存先";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, inp.Text, System.Text.Encoding.GetEncoding(charcode));
                this.Text= sfd.FileName + " - MDwriter";
            }
        }
        public void overwritesave()
        {
            string fname = this.Text.Substring(0, this.Text.LastIndexOf(" - MDwriter"));
            if (File.Exists(fname))
            {
                File.WriteAllText(fname, inp.Text, System.Text.Encoding.GetEncoding(charcode));
            }
        }
        public void newfile()
        {
            inp.Text = "";
            System.Environment.CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            this.Text = System.Environment.CurrentDirectory + "\\無題 - MDwriter";
        }
        public void item_click(Object sender, EventArgs e)
        {
            ToolStripMenuItem it = (ToolStripMenuItem)sender;
            switch (it.Text)
            {
                case "終了(&X)":
                    this.Close();
                    break;
                case "開く(&O)":
                    openfilemenu();
                    break;
                case "名前をつけて保存(&A)":
                    savefile();
                    break;
                case "htmlに出力(&H)":
                    savehtml();
                    break;
                case "上書き保存(&S)":
                    overwritesave();
                    break;
                case "新規(&N)":
                    newfile();
                    break;
                case "リロード":
                    tlanslate tl = new tlanslate();
                    inp.Text = inp.Text.Replace("\r\n", "\n");
                    string text = inp.Text.Replace("\n", "\r\n");
                    otp.DocumentText = tl.tlanslation(inp.Text, true, true);

                    break;
                case "もとに戻す(&U)":
                    inp.Undo();
                    break;
                case "やり直し(&R)":
                    inp.Redo();
                    break;
                case "切り取り(&T)":
                    inp.Cut();
                    break;
                case "コピー(&C)":
                    inp.Copy();
                    break;
                case "貼り付け(&P)":
                    inp.Paste();
                    break;
                case "斜体":
                    break;
                case "傍線":
                    break;
                case "アプリ概要(&A)":
                    infomation info = new infomation();
                    info.ShowDialog(this);
                    info.Dispose();
                    break;
                case "変換設定(&T)":
                    settings set = new settings();
                    set.ShowDialog(this);
                    set.Dispose();
                    break;
                case "プロパティ":
                    break;

            }
        }
        public exe()
        {
            MDwritercsharp.Properties.Settings.Default.Upgrade();
            Form fm = new Form();
            inp = setinp(new gets());
            otp = setotp(new outs());
            menu = setmenu(new MenuStrip());
            if (Clipboard.GetText().Length == 0) Clipboard.SetText("\r");
            //デバッグ用
            //Clipboard.Clear();
            //Environment.GetFolderPath(Environment.SpecialFolder.Personal)
            newfile();


            this.Width = WIDTH; this.Height = HEIGHT;
            this.MinimumSize = new System.Drawing.Size(WIDTH, HEIGHT);
            this.MaximumSize = new System.Drawing.Size(WIDTH, HEIGHT);




        }
        private void write(object sender, EventArgs e)
        {
            
            gets tmp = (gets)sender;
            tlanslate tl = new tlanslate();
            Text = Text.Replace("\r\n", "\n");
            string text = tmp.Text.Replace("\n", "\r\n");
            string input = tl.tlanslation(text, MDwritercsharp.Properties.Settings.Default.underbar, MDwritercsharp.Properties.Settings.Default.bold);

            otp.DocumentText = input;
            

            tmp.SelectionStart = tmp.SelectionStart;
            tmp.Focus();

        }


        [STAThread]
        public static void Main()
        {
            
            Application.Run(new exe());


        }

    }
}