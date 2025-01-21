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
    public partial class TodoForm : Form
    {
        private List<Task> tasks;
        private BindingSource bindingSource;

        public TodoForm()
        {
            InitializeComponent();
            tasks = new List<Task>();
            bindingSource = new BindingSource();

            // Set up DataGridView
            tasksDataGridView.AutoGenerateColumns = false;
            tasksDataGridView.DataSource = bindingSource;

            // Add Columns
            // BindingList<T> doesnt allow sorting in the DataGridView so I need to either manually sort them or add this override found here:
            // https://martinwilley.com/net/code/forms/sortablebindinglist.html
            tasksDataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Title", DataPropertyName = "Title", SortMode = DataGridViewColumnSortMode.Automatic });
            tasksDataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Description", DataPropertyName = "Description", SortMode = DataGridViewColumnSortMode.NotSortable });
            tasksDataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Due Date", DataPropertyName = "DueDate", SortMode = DataGridViewColumnSortMode.Automatic });
            tasksDataGridView.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Completed", DataPropertyName = "IsCompleted", SortMode = DataGridViewColumnSortMode.NotSortable });

            //CellContentClick only throws the event when the content such as a checkbox is clicked in a cell.  Not just a cell itself.
            tasksDataGridView.CellContentClick += OnTaskDataViewContentClicked;
            tasksDataGridView.CellClick += OnTaskRowClicked;
            RefreshTasks();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var task = new Task
            {
                Title = titleTextbox.Text,
                Description = descriptionTextbox.Text,
                DueDate = dateTimePicker1.Value
            };

            tasks.Add(task);
            ResetTaskEntry();
            RefreshTasks();
        }

        private void RefreshTasks()
        {
            bindingSource.DataSource = null;
            bindingSource.DataSource = tasks;
        }

        private void ResetTaskEntry()
        {
            titleTextbox.Text = "";
            descriptionTextbox.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }

        private void OnTaskRowClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var taskToUpdate = tasks[e.RowIndex];
                if (taskToUpdate == null)
                {
                    return;
                }

                titleTextbox.Text = taskToUpdate.Title;
                descriptionTextbox.Text = taskToUpdate.Description;
                dateTimePicker1.Value = taskToUpdate.DueDate;
            }
        }

        private void OnTaskDataViewContentClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < tasks.Count && e.ColumnIndex == 3)
            {
                var task = tasks[e.RowIndex];
                // If we are selecting the checkbox column we complete the task
                if (e.ColumnIndex == 3)
                {
                    // Make a pop-up window that confirms deletion
                    task.IsCompleted = true;

                    if (task.IsCompleted)
                    {
                        tasks.Remove(task);
                        RefreshTasks();
                    }

                    return;
                }

                // If we are just selecting the task row we want to populate the text fields with its information for editing
                // Since CellContentClick only works for objects within a cell need to use a different event for this.
                titleTextbox.Text = task.Title;
                descriptionTextbox.Text = task.Description;
                dateTimePicker1.Value = task.DueDate;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tasksDataGridView.CurrentRow != null)
            {
                tasks.Remove(tasksDataGridView.CurrentRow.DataBoundItem as Task);
                RefreshTasks();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (tasksDataGridView.CurrentRow != null)
            {
                var taskToUpdate = tasksDataGridView.CurrentRow.DataBoundItem as Task;
                taskToUpdate.Title = titleTextbox.Text;
                taskToUpdate.Description = descriptionTextbox.Text;
                taskToUpdate.DueDate = dateTimePicker1.Value;
                RefreshTasks();
            }
        }
    }
}
