﻿namespace AspNetCoreAPI
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? UserId { get; set; }
        public string? CheckId { get; set; }
        public bool? Admin { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime StartTime { get; set; }
    }
}
