using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1.MovieBooking
{
    /// <summary>
    /// based on the seat selling, calculate the income of each showing and the total income of the day
    /// A periodic income for each movie is also calculated(based on the selected time period)
    /// add showing, delete showing, update showing
    /// </summary>
    public partial class ShowingManage : Form
    {
        public ShowingManage()
        {
            InitializeComponent();
        }
    }
}
