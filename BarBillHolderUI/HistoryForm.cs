using BarBillHolderLibrary.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarBillHolderUI
{
    public partial class HistoryForm : Form
    {
        public HistoryForm()
        {
            InitializeComponent();
            string rootPath = FileProcessor.historyCSV;
            string[] days = Directory.GetDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);
            foreach (string day in days)
            {
                TabPage tab = new()
                {
                    Location = new Point(4, 24),
                    Name = Path.GetFileName(day),
                    Padding = new Padding(3),
                    Size = new Size(768, 340),
                    TabIndex = 0,
                    Text = Path.GetFileName(day),
                    UseVisualStyleBackColor = true,
                    AutoScroll = true
                };
                historyTabControl.Controls.Add(tab);
                string[] instances = Directory.GetFiles(day);
                Size size = new(200, 400);
                Point labelPoint = new(5, 5);
                Point listBoxPoint = new(5, 30);
                foreach (string instance in instances)
                {
                    Label label = new()
                    {
                        Text = Path.GetFileName(instance),
                        Location = labelPoint
                    };
                    tab.Controls.Add(label);
                    labelPoint.X += size.Width;

                    ListBox listBox = new()
                    {
                        Size = size,
                        Location = listBoxPoint
                    };
                    string[] lines = File.ReadAllLines(instance);
                    foreach (string line in lines)
                    {
                        listBox.Items.Add(line + "€");
                    }
                    tab.Controls.Add(listBox);
                    listBoxPoint.X += size.Width;
                }
            }
        }
    }
}
