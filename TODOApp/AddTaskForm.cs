using System;
using System.Windows.Forms;

namespace TODOApp
{
    public partial class AddTaskForm : Form
    {
        private Task _task;
        public AddTaskForm(Task task)
        {
            InitializeComponent();
            // If the task we pass in to this form is null we are creating a new task
            //
            _task = task ?? new Task(String.Empty, String.Empty, DateTime.Now);
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
