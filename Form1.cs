using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace TileRoller
{
    public partial class TileRoller:Form {
        public TileRoller() {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e) {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if(open.ShowDialog() == DialogResult.OK) {
                Bitmap org = new Bitmap(open.FileName);
                pictureBox1.Image = org;
                toolTip1.SetToolTip(splitContainer1.Panel1, open.FileName);
                int h;
                int.TryParse(richTextBox1.Text, out h);
                if(h<=0||h>org.Height) {
                    int oh = org.Height;
                    if(oh>16)
                        h=16;
                    else
                        h=(int)Math.Pow(2, (int)Math.Log(oh, 2));
                }
                Color[] colors = new Color[org.Height * org.Width];
                for(int j = 0; j < org.Height; j++) {
                    for(int i = 0; i < org.Width; i++) {
                        colors[j * org.Width + i] = org.GetPixel(i, j);
                    }
                }
                Bitmap n = new Bitmap((org.Width * org.Height) / h, h);
                resultFormat = org.RawFormat;
                for(int i = 0; i < n.Width; i++) {
                    for(int j = 0; j < n.Height; j++) {
                        int x=i%org.Width;
                        int y=((i)/org.Width)*n.Height+j;
                        int index = y * org.Width + x;
                        if(index<colors.Length)
                            n.SetPixel(i, j, colors[index]);
                    }
                }
                pictureBox2.Image = n;
            }
        }
        public ImageFormat resultFormat;
        private void button2_Click(object sender, EventArgs e) {
            if(pictureBox2.Image!=null) {
                Stream s ;
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp";
                sfd.FilterIndex = 2;
                sfd.RestoreDirectory = true;

                if(sfd.ShowDialog() == DialogResult.OK) {
                    if((s = sfd.OpenFile()) != null) {
                        pictureBox2.Image.Save(s, resultFormat);
                        s.Close();
                    }
                }
            }
        }
    }
}
