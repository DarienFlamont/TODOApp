namespace TodoApp.Tests
{
    [TestClass]
    public class TaskTests
    {
        private List<Task> tasks;

        [TestInitialize]
        public void Initialize()
        {
            tasks = new List<Task>
            {
                new Task("Task 1", "Description 1", DateTime.Now.AddDays(1)),
                new Task("Task 2", "Description 2", DateTime.Now.AddDays(2), true),
                new Task("Task 3", "Description 3", DateTime.Now.AddDays(3))
            };
        }

        [TestMethod]
        public void AddTask_ShouldIncreaseTaskCount()
        {
            var newTask = new Task("Task 4", "Description 4", DateTime.Now.AddDays(4));
            tasks.Add(newTask);

            Assert.AreEqual(4, tasks.Count);
        }

        [TestMethod]
        public void EditTask_ShouldUpdateTaskDetails()
        {
            var taskToEdit = tasks[0];
            taskToEdit.Title = "Updated Task 1";
            taskToEdit.Description = "Updated Description 1";
            taskToEdit.DueDate = DateTime.Now.AddDays(5);
            taskToEdit.IsCompleted = true;

            Assert.AreEqual("Updated Task 1", tasks[0].Title);
            Assert.AreEqual("Updated Description 1", tasks[0].Description);
            Assert.AreEqual(DateTime.Now.AddDays(5).Date, tasks[0].DueDate.Date);
            Assert.AreEqual(true, tasks[0].IsCompleted);
        }

        [TestMethod]
        public void DeleteTask_ShouldDecreaseTaskCount()
        {
            var taskToDelete = tasks[0];
            tasks.Remove(taskToDelete);

            Assert.AreEqual(2, tasks.Count);
        }

        [TestMethod]
        public void FilterTasks_ShouldReturnCorrectCount()
        {
            var incompleteTasks = tasks.Where(t => t.IsCompleted == false).ToList();
            var completeTasks = tasks.Where(t => t.IsCompleted == true).ToList();

            Assert.AreEqual(2, incompleteTasks.Count);
            Assert.AreEqual(1, completeTasks.Count);
        }
    }
}
