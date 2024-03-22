﻿namespace WebApplication5.Models
{
    public class StudentDetailsViewModel
    {
        public required Student Student { get; set; }
        public List<CourseEnrollment> _CourseEnrollments { get; set; } = new List<CourseEnrollment>();
    }
}
