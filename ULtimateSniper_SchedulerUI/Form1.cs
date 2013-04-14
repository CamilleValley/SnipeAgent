using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ULtimateSniper_SchedulerUI
{
    public partial class Form1 : Form
    {
        public UltimateSniper_SchedulerEngine.Scheduler_Engine scheduler = new UltimateSniper_SchedulerEngine.Scheduler_Engine();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!this.checkBoxBidOptimizer.Checked && !this.checkBoxSnipe.Checked)
            {
                MessageBox.Show("Select at least one type.");
                return;
            }

            this.button1.Enabled = false;
            this.button2.Enabled = true;

            scheduler.BidOptimizerEnabled = this.checkBoxBidOptimizer.Checked;
            scheduler.SnipeEnabled = this.checkBoxSnipe.Checked;
            
            scheduler.StartScheduler();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = true;
            this.button2.Enabled = false;
            scheduler.StopScheduler();
        }
    }
}
