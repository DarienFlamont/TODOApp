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
        private List<Task> filteredTasks;
        private BindingSource bindingSource;
        private bool sortAscendingTitle = true;
        private bool sortAscendingDueDate = true;

        public TodoForm()
        {
            tasks = new List<Task>();
            filteredTasks = new List<Task>();
            bindingSource = new BindingSource();

            InitializeComponent();
            InitializeFilter();
            LoadTasksFromJSON();

            // Set up DataGridView
            tasksDataGridView.AutoGenerateColumns = false;
            tasksDataGridView.DataSource = bindingSource;

            // Add Columns
            // TODO: Columns are still not word wrapping properly.  Figure out the proper setup to word wrap.
            tasksDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Title",
                DataPropertyName = "Title",
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = { WrapMode = DataGridViewTriState.True }
            });

            tasksDataGridView.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Description", 
                DataPropertyName = "Description",
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DefaultCellStyle = { WrapMode = DataGridViewTriState.True } 
            });

            tasksDataGridView.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Due Date", DataPropertyName = "DueDate",
                SortMode = DataGridViewColumnSortMode.Automatic ,
                DefaultCellStyle = { WrapMode = DataGridViewTriState.True }
            });

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

        private void LoadTasksFromJSON()
        {
            tasks = JsonHelper.LoadTasks();
            RefreshTasks();
        }

        private void SaveTasks()
        {
            JsonHelper.SaveTasks(tasks);
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

        private void OnTaskRowClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var taskToUpdate = tasks[e.RowIndex];
                if (taskToUpdate == null)
                {
                    return;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void RefreshTasks()
        {
            switch((TaskFilter)filterComboBox.SelectedItem)
            {
                case TaskFilter.All:
                    bindingSource.DataSource = null;
                    bindingSource.DataSource = tasks;
                    return;
                case TaskFilter.Completed:
                    filteredTasks = tasks.Where(t => t.IsCompleted).ToList();
                    bindingSource.DataSource = null;
                    bindingSource.DataSource = filteredTasks;
                    return;
                case TaskFilter.Uncompleted:
                    filteredTasks = tasks.Where(t => !t.IsCompleted).ToList();
                    bindingSource.DataSource = null;
                    bindingSource.DataSource = filteredTasks;
                    return;
            }
        }

        private void OnTaskDataViewContentClicked(object sender, DataGridViewCellEventArgs e)
        {
            // Use the binding source as our task list.
            // When using self.tasks with a filter the indices expect the original task list indicies not the filtered version.
            List<Task> tasks = bindingSource.DataSource as List<Task>;
            if (e.RowIndex >= 0 && e.RowIndex < tasks.Count && e.ColumnIndex == 3)
            {
                var task = tasks[e.RowIndex];
                // If we are selecting the checkbox column we complete the task
                if (e.ColumnIndex == 3)
                {
                    task.IsCompleted = !task.IsCompleted;
                    RefreshTasks();
                    SaveTasks();
                    return;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var newTask = new Task("", "", DateTime.Now);
            var addTaskForm = new AddTaskForm(newTask);
            if (addTaskForm.ShowDialog() == DialogResult.OK)
            {
                tasks.Add(newTask);
                RefreshTasks();
                SaveTasks();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tasksDataGridView.CurrentRow != null)
            {
                Task taskToRemove = tasksDataGridView.CurrentRow.DataBoundItem as Task;
                var returnPrompt = MessageBox.Show($"Are you sure you want to delete the task titled: {taskToRemove.Title}?", "Confirm Task Deletion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (returnPrompt == DialogResult.OK)
                {
                    tasks.Remove(taskToRemove);
                    RefreshTasks();
                    SaveTasks();
                }
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
                    SaveTasks();
                }
            }
        }
    }
}
