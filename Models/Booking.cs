﻿using System.ComponentModel.DataAnnotations;

namespace Biluthyrning.Models
{
    public class Booking
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public int CarId { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }
        [Required]
        [Display(Name = "User Id")]
        public int UserId { get; set; }
    }
}
