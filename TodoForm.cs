using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace TODOApp
{
    public enum TaskFilter
    {
        All,
        Completed,
        Uncompleted
    }

    public partial class TodoForm : Form
    {
        private List<Task> tasks;
        private BindingSource bindingSource;
        private bool sortAscendingTitle = true;
        private bool sortAscendingDueDate = true;

        public TodoForm()
        {
            InitializeComponent();
            InitializeFilter();
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

            tasksDataGridView.CellContentClick += OnTaskDataViewContentClicked;
            tasksDataGridView.CellClick += OnTaskRowClicked;
            tasksDataGridView.ColumnHeaderMouseClick += OnColumnHeaderClicked;
            RefreshTasks();
        }

        private void InitializeFilter()
        {
            filterComboBox.DataSource = Enum.GetValues(typeof(TaskFilter));
            filterComboBox.SelectedIndexChanged += (s,e) => RefreshTasks();
        }

        private void OnColumnHeaderClicked(object sender, DataGridViewCellMouseEventArgs e)
        {
            // This is the column that corresponds to our title column
            if (e.ColumnIndex == 0) 
            { 
                if (sortAscendingTitle)
                {
                    tasks = tasks.OrderBy(t => t.Title).ToList();
                }
                else
                {
                    tasks = tasks.OrderByDescending(t => t.Title).ToList();
                }

                sortAscendingTitle = !sortAscendingTitle;
                RefreshTasks();
            }

            // This is the column that corresponds to the DueDate column
            if (e.ColumnIndex == 2)
            {
                if (sortAscendingDueDate)
                {
                    tasks = tasks.OrderBy(t => t.DueDate).ToList();
                }
                else
                {
                    tasks = tasks.OrderByDescending(t => t.DueDate).ToList();
                }

                sortAscendingDueDate = !sortAscendingDueDate;
                RefreshTasks();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var newTask = new Task("", "", DateTime.Now);
            var addTaskForm = new AddTaskForm(newTask);
            if (addTaskForm.ShowDialog() == DialogResult.OK)
            {
                tasks.Add(newTask);
                // Remove this line once 2 page task addition is setup
                ResetTaskEntry();
                RefreshTasks();
            }
        }

        private void RefreshTasks()
        {
            if ((TaskFilter) filterComboBox.SelectedItem == TaskFilter.All)
            {
                bindingSource.DataSource = null;
                bindingSource.DataSource = tasks;
                return;
            }

            if ((TaskFilter) filterComboBox.SelectedItem == TaskFilter.Completed)
            {
                var filteredTasks = tasks.Where(t => t.IsCompleted).ToList();
                bindingSource.DataSource = null;
                bindingSource.DataSource = filteredTasks;
                return;
            }

            if((TaskFilter) filterComboBox.SelectedItem == TaskFilter.Uncompleted)
            {
                var filteredTasks = tasks.Where(t => !t.IsCompleted).ToList();
                bindingSource.DataSource = null;
                bindingSource.DataSource = filteredTasks;
                return;
            }


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
                    task.IsCompleted = !task.IsCompleted;
                    RefreshTasks();
                    return;
                }
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
                var editTaskForm = new AddTaskForm(taskToUpdate);
                if (editTaskForm.ShowDialog() == DialogResult.OK)
                {
                    RefreshTasks();
                }
            }
        }
    }
}
