using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RawImageViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void 새로운탭만들기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage newPage = new TabPage();
            ImageTabItem tabItem = new ImageTabItem();
            tabItem.UpdateTooltip += updateStatusTooltip;
            tabItem.Name = "TabItem";
            newPage.DataBindings.Add("Text", tabItem, "ImageName");
            newPage.Controls.Add(tabItem);
            tabControl1.TabPages.Add(newPage);
            tabControl1.SelectedTab = newPage;
            tabItem.removePage = () => tabControl1.TabPages.Remove(newPage);
        }

        private void updateStatusTooltip(object sender, EventArgs e)
        {
            fileSizeLabel.Text = (tabControl1.SelectedTab?.Controls["TabItem"] as ImageTabItem)?.FileSize + " bytes";
            widthLabel.Text = (tabControl1.SelectedTab?.Controls["TabItem"] as ImageTabItem)?.ImageWidth + " px";
            heightLabel.Text = (tabControl1.SelectedTab?.Controls["TabItem"] as ImageTabItem)?.ImageHeight + " px";
        }
    }
}
