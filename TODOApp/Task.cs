using System;

public class Task
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }

    public Task(string title, string description, DateTime dueDate, bool isCompleted = false)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        IsCompleted = isCompleted;
    }   
}

