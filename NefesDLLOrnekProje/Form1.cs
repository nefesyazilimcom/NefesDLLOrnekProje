using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using NefesWolvoxResimGetir;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace NefesDLLOrnekProje
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        FbConnection conn;
        private void button1_Click(object sender, EventArgs e)
        {

            string ConnectionString1 = "User ID="+textBox2.Text+";Password="+textBox3.Text+";" +
                "Database="+textBox1.Text+":"+buttonEdit1.Text+"; Port=3050;" +
                "DataSource=" +textBox1.Text+";Charset=NONE;";
            conn = new FbConnection(ConnectionString1);
            conn.Open();
            FbDataAdapter cmd = new FbDataAdapter("SELECT * from DOSYA", conn);
            DataTable dtb = new DataTable();
            cmd.Fill(dtb);

            if (DialogResult.OK==folderBrowserDialog1.ShowDialog())
            {
                ResimleriYukle(textBox5.Text, dtb.AsEnumerable());
            }
            MessageBox.Show("Resim Yüklemesi Başarılı! Resimler: "+folderBrowserDialog1.SelectedPath);
            
        }
        int resimsayi = 0;
        public void ResimleriYukle(string STBLKODU, EnumerableRowCollection<DataRow> dt)
        {
            foreach (var row in dt.Where(x => !string.IsNullOrEmpty(x.Field<string>("DOSYAADI")) && x.Field<string>("BAGKODU").Split('_')[3] == STBLKODU))
            {
                byte[] blob = row.Field<byte[]>("FILEDATA");
              
                using (Image image = Image.FromStream(NTools.TekResimGetir(blob, "N123", "1", "NBDemo")))
                {
                    var split = row.Field<string>("DOSYAADI").Split('.');
                    string isim =textBox4.Text+"-" + resimsayi.ToString() + "." + split[split.Length - 1];
                    image.Save(folderBrowserDialog1.SelectedPath + "\\" + isim);
                }
                resimsayi++;
            }
          
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (DialogResult.OK==openFileDialog1.ShowDialog())
            {
                buttonEdit1.Text = openFileDialog1.FileName;
            }
           
        }
    }
}
