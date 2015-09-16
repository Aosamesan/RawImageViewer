using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RawImageViewer
{
    public partial class WidthSetter : Form, INotifyPropertyChanged
    {
        private int width;
        public int MyWidth
        {
            get { return width; }
            set
            {
                if(value > 0)
                {
                    width = value;
                    OnPropertyChanged(nameof(MyWidth));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string m)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(m));

        public WidthSetter()
        {
            InitializeComponent();
            textBox1.DataBindings.Add("Text", this, nameof(MyWidth));
        }

        public WidthSetter(int width) : this()
        {
            MyWidth = width;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyWidth = Convert.ToInt32(textBox1.Text);
            DialogResult = DialogResult.OK;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }

        private void WidthSetter_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            textBox1.SelectAll();
        }
    }
}
