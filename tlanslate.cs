using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Security;
namespace MDtohtml
{


    class tlanslate
    {

        delegate void rptags(ref string text);
        

        public tlanslate()
        {

        }

        public void p(ref string txt)
        {
            Regex regex = new Regex("(\r\n\r\n)(.+?)(\r\n\r\n)", RegexOptions.Singleline);
            while (regex.IsMatch(txt))
            {
                txt = regex.Replace(txt, "\r\n<p>\r\n$2\r\n</p>\r\n\r\n", 1);
            }
        }
        public void br(ref string txt) 
        {
            txt = Regex.Replace(txt, "(  \r\n)", "<br>\r\n", RegexOptions.Singleline);
        }
        
         public string trimemptyrows(string txt)
        {

            string[] row = txt.Split(new char[] { '\n' });
            int i;
            string newtxt="";
            for (i = 0; i < row.Length; i++) if(!Regex.IsMatch(row[i],@"\S"))row[i]="";
            for (i = 0; i < row.Length; i++)if(row[i].EndsWith("\r"))newtxt+= row[i]+"\n";
            return newtxt;
        }
        public string hn1_2(string txt)
        {

            //2020/04/21　今時点でうまく行っていないもの　
            string[] row = txt.Split('\n');

            int h, i, r;


            Regex[] hn = {
            new Regex(@"^\r", RegexOptions.Multiline),
            new Regex(@"\S", RegexOptions.Multiline),
             new Regex(@"^(=){1,}(\r)", RegexOptions.Multiline),
             new Regex(@"^(-){1,}(\r)", RegexOptions.Multiline),
             new Regex(@"^\r", RegexOptions.Multiline)};

            r = 1;
            h = 0;

            for (i = 0; i < row.Length; i++)
            {
                if (r == 2)
                {
                    if (hn[2].IsMatch(row[i]))h = 1;
                    if (hn[3].IsMatch(row[i])) h = 2;
                    r = (h > 0) ? 4:0;
                }
                else
                {
                    r = (hn[r].IsMatch(row[i])) ? r + 1 : 0;
                    if (r == 4) h++;
                    if (r == 5)
                    {
                        //変換
                        if(i>2)row[i - 3] = "";
                        row[i - 2] = "<h" + h.ToString() + ">\r" + row[i - 2];
                        row[i - 2] = row[i - 2].Insert(row[i - 2].Length - 1, "</h" + h.ToString() + ">\r");
                        row[i - 1] = "";
                        row[i] = "";

                        r = 0;
                    }
                    if (r == 0) h = 0;
                }
            }


            for (i = 0; i < row.Length; i++)
            {
                if (row[i].EndsWith("\r")) row[i] += "\n";
                if (i > 0) row[0] += row[i];
            }



            return row[0];
             
             

        }
        public void hn(ref string txt)
        {
            int h;
            string hn = "";
            string hs = "";
            string he = "";
            string hx = "";

            
            for (h = 1; h < 8; h++)
            {
                hn = String.Concat("#" + hn);
                hs = hn + " ";
                he = " " + hn;
                hx = "h"+h.ToString();
                //# ... # のための
                txt = Regex.Replace(txt, @"(^(" + hs + ")(.+))("+he+")", "<" + hx + ">$3</" + hx + ">\r\n", RegexOptions.Multiline);

                txt = Regex.Replace(txt, @"(^(" + hs + ")(.+))", "<" + hx + ">$3</"+hx + ">\r\n", RegexOptions.Multiline);
                

            }




        }
        public void hr(ref string txt)
        {
            string[] pattern = { "(-)", "(- )", "(_)", "(_ )", "(\\*)", "(\\* )" };
            string newtxt = "";
            for(int h = 0; h < 2; h++)
            {
                for (int i = 0; i < 6; i++)
                {
                    txt = (i % 2 == 0) ? Regex.Replace(txt, pattern[i] + "{3,}\r\n", "<hr>\r\n", RegexOptions.Multiline)
                        : Regex.Replace(txt, pattern[i] + "{2,}" + pattern[i - 1] + " ?\r\n", "<hr>\r\n", RegexOptions.Multiline);
                }
                newtxt = txt;
                if(h==0)txt += "\r\n";
            }
            if (newtxt + "\r\n" == txt) txt = newtxt;
            
            

        }
        public void emstrong(ref string txt, bool underline, bool bold)
        {
            int h, i;
            string matching;
            string[] replaced = { "<em>$2</em>", "<strong>$2</strong>", "<em><strong>$2</strong></em>" };
            //(\*\*)(.*?)(\*\*)
            for (i = 2; i >= 0; i--)
            {
                h = i + 1;
                matching = "(\\*){" + h.ToString() + "}";
                if (underline) replaced[i] = Regex.Replace(replaced[i], "em>", "u>", RegexOptions.None);
                if (bold) replaced[i] = Regex.Replace(replaced[i], "strong>", "b>", RegexOptions.None);
                txt = Regex.Replace(txt, matching + @"(.*?)" + matching, replaced[i], RegexOptions.Multiline);
            }
            //(\_\_)(.*?)(\_\_)
            for (i = 2; i >= 0; i--)
            {
                h = i + 1;
                matching = "(_){" + h.ToString() + "}";
                if (underline) replaced[i] = Regex.Replace(replaced[i], "em>", "u>", RegexOptions.None);
                if (bold) replaced[i] = Regex.Replace(replaced[i], "strong>", "b>", RegexOptions.None);
                txt = Regex.Replace(txt, matching + @"(.*?)" + matching, replaced[i], RegexOptions.Multiline);
            }
            txt = Regex.Replace(txt, @"(~~)(.*?)(~~)", "<s>$2</s>", RegexOptions.Multiline);


        }

        public void code(ref string txt, ref List<string> codes, ref List<string> keys)
        {
            Regex cddic = new Regex("(<code>(.+?)</code>)",RegexOptions.Singleline);
           
            Random random = new Random();

            string key = "";

            
            txt = Regex.Replace(txt, "(\r\n```)(.*?)(\r\n```\r\n)", "<code><pre>\r\n$2</pre></code>\r\n", RegexOptions.Singleline);
            txt = Regex.Replace(txt, "(`)(.*?)(`)", "<code>$2</code>", RegexOptions.None);


            MatchCollection match =cddic.Matches(txt);

            for(int i = 0; i < match.Count; i++)
            {
                key = Membership.GeneratePassword(128, 0);
                key = Regex.Replace(key, "[^0-9a-zA-Z]", "");


                codes.Add(match[i].Value);
                keys.Add(key);
                txt = cddic.Replace(txt, key, 1);
            }

            

        }

        public void codereput(ref string txt, ref List<string> codes, ref List<string> keys)
        {
            for (int i = 0; i < codes.Count(); i++)
            {
                txt = Regex.Replace(txt, keys[i], codes[i], RegexOptions.Singleline);
            }
        }


        public bool ulol(ref string txt)
        {
            int h, i,rgx, stt, stp;
            rgx = 0;
            stt = -1;
            stp = 0;
            
            string rptext;

            string[] row = txt.Split('\n');
            bool block1, block2,replaced;
            replaced = false;
            string[] rgxkey = { "(\\d{1,}\\. )", "(- |\\* |\\+ )", "^(    )", "^(\t)" };

            string[] rgxtag = { "<ol>", "<ul>", "</ol>", "</ul>" };

            rptext = "";
            do
            {
                block1 = false;
                block2 = false;
                for (i = 0; i < row.Length; i++)
                {
                    switch (block1)
                    {

                        case false:
                            if (Regex.IsMatch(row[i], @"^(\d{1,}\. |- |\* |\+ )"))
                            {
                                for (h = 0; h < 2; h++)
                                {
                                    if (Regex.IsMatch(row[i], "^" + rgxkey[h]))
                                    {
                                        rgx = h;
                                        stt = i;
                                        block1 = true;

                                        row[i] = Regex.Replace(row[i], "^" + rgxkey[h] + "(.+)", "\r\n<li>$2</li>\r");
                                        row[stt] = rgxtag[rgx] + "\r\n" + row[stt] +"\r";
                                        replaced = true;
                                        
                                    }
                                }
                            }
                            if (Regex.IsMatch(row[i], @"^(\s+)(\d{1,}\. |- |\* |\+ )"))
                            {

                                if(row[i].StartsWith("\t"))row[i] ="   "+ row[i].Substring(1,row[i].Length-1);
                                if(row[i].StartsWith(" ")) row[i] = row[i].Substring(1, row[i].Length - 1);
                                replaced = true;
                            }

                            break;

                        case true:
                            if (Regex.IsMatch(row[i], "^" + rgxkey[rgx]))
                            {
                                row[i] = Regex.Replace(row[i], "^" + rgxkey[rgx] + "(.+)", "<li>$2</li>\r");
                            }
                            else if (Regex.IsMatch(row[i], @"^(\s+)(\d{1,}\. |- |\* |\+ )"))
                            {
                                if (row[i].StartsWith("\t")) row[i] = "   " + row[i].Substring(1, row[i].Length - 1);
                                if (row[i].StartsWith(" ")) row[i] = row[i].Substring(1, row[i].Length - 1);
                                replaced = true;
                            }
                            else
                            {
                                
                                stp = i;
                                row[stp - 1] += "\r\n" + rgxtag[rgx + 2] + "\r";
                                block1 = false;
                                block2 = true;
                                stt = -1;


                            }

                            break;
                    }

                }
            } while (block2 == true);


            for (i = 0; i < row.Length; i++)
            {
                row[i] = Regex.Replace(row[i], "((\r)|\n)$", "\r\n");
                rptext += row[i];

            }
            txt = rptext;

            return replaced;

        }

        public void blockquote(ref string txt)
        {
            int i, stt, stp;
            stt = 0;
            stp = 0;
            bool block1, block2;
            string[] rgxkey = { "^(> )", "^((> )|(  ))" };
            string[] row = txt.Split('\n');
            string rptext = "";
            do
            {
                block1 = false;
                block2 = false;
                for (i = 0; i < row.Length; i++)
                {

                    if (Regex.IsMatch(row[i], rgxkey[0]) == true && block1 == false)
                    {
                        stt = i;
                        block1 = true;
                        row[i] = Regex.Replace(row[i], rgxkey[0], "<blockquote>", RegexOptions.Multiline);

                    }
                    else if (block1 == true)
                    {

                        if (Regex.IsMatch(row[i], rgxkey[1]) == true)
                        {
                            row[i] = Regex.Replace(row[i], rgxkey[1], "", RegexOptions.Multiline);

                        }
                        else
                        {
                            stp = i;
                            row[i] = "</blockquote>\r\n" + row[i];
                            block2 = true;
                            block1 = false;
                            do
                            {
                                if (Regex.IsMatch(row[stt], "<br>\r") == false)
                                {
                                    row[stt] += "<br>\r";
                                    stt++;
                                }
                            } while (stt == stp);
                        }
                    }
                }
            } while (block2 == true);
            for (i = 0; i < row.Length; i++)
            {
                row[i] = Regex.Replace(row[i], "((\r)|\n)$", "\r\n");
                rptext += row[i];

            }
            txt = rptext;
        }

        public void urlorimg(ref string txt)
        {
            int i = 0;
            bool replaced = false;
            string[] adr = new string[2];


            txt = Regex.Replace(txt, "<([^<>\"].*?(\\.|@).*?)>", "<a href = \"$1\">$1</a>", RegexOptions.Multiline);

            anchor(ref txt);

            txt = Regex.Replace(txt, @"\!\[(.*?)\]\((([^<>].*?(\.)(png|PNG|jpg|JPG|jpeg|JPEG|gif|GIF|webp|WEBP)))\)", "<img src=\"$2\" alt=\"$1\">", RegexOptions.Multiline);
            txt =Regex.Replace(txt, @"\[(.*?)\]\(([^<>].*?(\.|@).*?)\)", "<a href= \"$2\">$1</a>", RegexOptions.Multiline);

            string[] row = txt.Split('\n');

            
            for (i = 0; i<row.Length; i++)
            {
                
                if (Regex.IsMatch(row[i], @"\[[^\^](.*?)\]\:([^<>](.*?(\.|@).*?))"))
                {
                    adr[0] = Regex.Replace(row[i], @"\[(.*?)\]\:([^<>](.*?(\.|@).*?))", "$2");
                    adr[1] = Regex.Replace(row[i], @"\[(.*?)\]\:([^<>](.*?(\.|@).*?))", "$1");
                   txt= Regex.Replace(txt, @"\[(.*?)\]\[(" + adr[1] + ")\\]", "<a href=\"" + adr[0] + "\">$1</a>", RegexOptions.Multiline);
                    txt = Regex.Replace(txt, "^"+row[i], "",RegexOptions.Multiline);
                    replaced = true;
                }
            }
            if(replaced)txt= Regex.Replace(txt, @"^(.{1,})\]\:(.{1,}(\.|@).{1,})", "",RegexOptions.Multiline);

            
        }
        public void anchor(ref string txt)
        {
            Regex rgx1 = new Regex(@"^(\[\^)(.*?)(\]\:)(.*)",RegexOptions.Multiline);
            Regex rgx2 = new Regex(@"(\[\^)(.*?)(\])(.*)", RegexOptions.Multiline);

            string anchortext = "";
    

            var footnote = new SortedDictionary<int,string>();

            int h, i;

            MatchCollection match = rgx1.Matches(txt);
            txt = rgx1.Replace(txt, "");

            for (i = 0; i < match.Count; i++)
                {
                    anchortext = "[^" + rgx1.Replace(match[i].Value, "$2") + "]";
                
                  
                    if (txt.IndexOf(anchortext) >= 0)
                    {
                        footnote.Add(txt.IndexOf(anchortext), rgx1.Replace(match[i].Value, "<li><a name=\"$2\">$4</a></li>\r\n"));
                    }

                }


            if (footnote.Count > 0)
            {
                h = 0;
                txt += "\r\n<hr>\r\n<ol>\r\n";
                foreach (KeyValuePair<int, string> k in footnote)
                {
                    h++;

                    txt += k.Value;

                    // Regex rgx3 = new Regex(@" ^ (\[\^)(.*?)(\])(.*)", RegexOptions.Multiline);

                    txt = rgx2.Replace(txt,"<a href=\"#$2\">^" + h.ToString() + "</a>$4", 1);

                    //txt = Regex.Replace(txt,anchortext, "<a href=\"#$2\">" + h.ToString() + "</a>", RegexOptions.Multiline);
                    // txt = Regex.Replace(txt, anchortext, "<a href=\"#$2\">" + h.ToString()+"</a>",RegexOptions.Multiline);
                }
            }

        }

        public void table(ref string txt)
        {
            string[] row = txt.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string aln = "";
            string tc = "th";
            int tstart = -1;

            if (row.Length > 1)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    row[i]=row[i].Trim();

                    if (ctchar(row[i]) > 0 && tstart < 0) tstart = i;
                    if (ctchar(row[i]) <= 0 && tstart >= 0)
                    {
                            aln = tbalign(row[tstart + 1]);
                        if (aln != "")
                        {
                            row[tstart + 1] = row[tstart];
                            row[tstart] = "<table>";
                            tc = "th";

                            for (int h = tstart + 1; h < i; h++)
                            {
                                row[h] = tbcells(row[h], aln, tc);
                                tc = "td";
                            }
                            row[i] = "</table>" + row[i];
                        }
                        
                        tstart = -1;
                    }


                }

            }

            txt = String.Join("\r\n", row);
        }

        public int ctchar(string txt)
        {
            string[] cell = txt.Trim().Split('|');
            return cell.Length-1;
        }

        public string tbalign(string txt)
        {
            string[] cell = txt.Trim().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            string alns = "";
            string alkey = "";
            for (int i = 0; i < cell.Length; i++)
            {
                alkey = "";
                if (cell[i].EndsWith(":"))
                {
                    cell[i]=cell[i].TrimEnd(':');
                    alkey = "r";
                }
                if (cell[i].StartsWith(":"))
                {
                    cell[i] = cell[i].TrimStart(':');
                    if (alkey == "r") alkey = "c";
                }
                if (cell[i].Replace("-", "").Length > 0) return "";
                if (cell[i].Length >= 3)
                {
                    if (alkey == "") alkey = "n";
                    alns += alkey;
                }
            }
            return alns;
        }

        public string tbcells(string txt, string aln, string tc)
        {
            if (ctchar(txt) == 0) return "";

            string[] cell = txt.Trim().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            

            string tce = "</" + tc + ">";
            string tcs;
            string align;

            for (int i = 0; i < cell.Length; i++)
            {
                align = "";
                if (i<aln.Length)
                {
                    switch (aln[i])
                    {
                        case 'c':
                            align = "center";
                            break;
                        case 'r':
                            align = "right";
                            break;
                        default:
                            align = "left";
                            break;
                    }
                    
                }
                tcs = align != "" ? "<" + tc + " align=\"" + align + "\">" : "<" + tc + ">";
                cell[i] = tcs + cell[i] + tce;

            }
            
            return "<tr>\r\n"+String.Join("\r\n", cell)+"\r\n</tr>";

        }
        public string tlanslation(string text,bool underline,bool bold)
        {
            List<String> codes = new List<String>();
            List<String> keys = new List<String>();

            // 下記の順番は意味を持つ。ずらさないこと。
            code(ref text, ref codes, ref keys);
            hn(ref text);
            text = hn1_2(text);
            


            urlorimg(ref text);
            p(ref text);
            br(ref text);
            blockquote(ref text);
            hr(ref text);

            while (ulol(ref text));


            emstrong(ref text, underline, bold);

            table(ref text);
            text=trimemptyrows(text);

            codereput(ref text, ref codes, ref keys);

            return text;
             

        }


    }

   

}
