using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace naEkranie.pl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            DateTime date = DateTime.Today; 
            WebClient web = new WebClient();
            web.Encoding = Encoding.UTF8;   
            string html = web.DownloadString("http://naekranie.pl/kalendarz-premier-seriali?picked_date=" + date.ToShortDateString());  

            
            string expression = "item-program-title\">(?<programTitle>.*)</div>\n.*item-episode-number\">(?<episodeNumber>.*)</div>\n.*item-episode-title\">(?<episodeTitle>.*)</div>\n<a class=";
            string picture_exp = "background-image: url(?<url>.*)\"></div>";

            
            MatchCollection episode_matches = Regex.Matches(html, expression);  
            MatchCollection picture_matches = Regex.Matches(html, picture_exp); 
            
            int counter = 0;    
            char[] charsToTrim = { '\'', '(', ')' };     

            foreach (Match picture in picture_matches)
            {
                MemoryStream ms = new MemoryStream(web.DownloadData(picture.Groups["url"].Value.Trim(charsToTrim)));    
                imageList1.Images.Add(Image.FromStream(ms));    
                
            }            

            counter = 0;
            
            foreach (Match match in episode_matches)
            {
                ListViewItem item = new ListViewItem();
                item.Text = match.Groups["programTitle"].Value;
                item.SubItems.Add(match.Groups["episodeNumber"].Value);
                item.SubItems.Add(match.Groups["episodeTitle"].Value);
                item.ImageIndex = counter;  
                listView1.Items.Add(item);

                counter++;
            }
        }
    }
}
