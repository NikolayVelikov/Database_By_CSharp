using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models
{
    public class Homework
    {  
        public int HomeworkId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [Column(TypeName ="varchar(255)")]
        public ContentType ContentType { get; set; }

        public DateTime SubmissionTime { get; set; }

        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
