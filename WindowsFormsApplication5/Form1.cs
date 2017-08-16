using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            FormGenerate FG = new FormGenerate();
            this.Controls.Add(FG.Container);
            this.AutoSize = true;
            InitializeComponent();
        }
    }
}
