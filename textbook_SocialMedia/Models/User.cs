﻿using System.ComponentModel.DataAnnotations;

namespace textbook_SocialMedia.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
    }
}
