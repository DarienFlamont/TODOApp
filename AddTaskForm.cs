using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TODOApp
{
    public partial class AddTaskForm : Form
    {
        private Task _task;
        public AddTaskForm(Task task)
        {
            InitializeComponent();
            _task = task ?? new Task("", "", DateTime.Now);
            titleTextbox.Text = _task.Title;
            descriptionTextbox.Text = _task.Description;
            dateTimePicker1.Value = _task.DueDate;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(titleTextbox.Text))
            {
                MessageBox.Show("The title of your task is empty.  Please add a valid title and try again.", "Task Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dateTimePicker1.Value < DateTime.Now) 
            {
                MessageBox.Show("You have picked a due date in the past.  Please pick a valid due date and try again.", "Task Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _task.Title = titleTextbox.Text;
            _task.Description = descriptionTextbox.Text;
            _task.DueDate = dateTimePicker1.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void AddTaskForm_Load(object sender, EventArgs e)
        {

        }
    }
}
