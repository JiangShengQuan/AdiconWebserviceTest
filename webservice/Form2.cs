using DataGridViewAutoFilter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace webservice
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = textBox1.Text;
            this.dataGridView1.Rows[index].Cells[1].Value = textBox2.Text;
            this.dataGridView1.Rows[index].Cells[2].Value = textBox3.Text;
        }

        private void showAllLabel_Click(object sender, EventArgs e)
        {
            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(dataGridView1);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
            {
                DataGridViewAutoFilterColumnHeaderCell filterCell =
                    dataGridView1.CurrentCell.OwningColumn.HeaderCell as
                    DataGridViewAutoFilterColumnHeaderCell;
                if (filterCell != null)
                {
                    filterCell.ShowDropDownList();
                    e.Handled = true;
                }
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            String filterStatus = DataGridViewAutoFilterColumnHeaderCell
                .GetFilterStatus(dataGridView1);
            if (String.IsNullOrEmpty(filterStatus))
            {
                showAllLabel.Visible = false;
                filterStatusLabel.Visible = false;
            }
            else
            {
                showAllLabel.Visible = true;
                filterStatusLabel.Visible = true;
                filterStatusLabel.Text = filterStatus;
            }
        }
    }
}
