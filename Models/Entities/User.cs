using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static ChatForLife.Pages.Chat.GroupModel;

namespace ChatForLife.Models.Entities
{
    public class User
    {
        public User()
        {
            
            SentMessages = new HashSet<Message>();
            ReceivedMessages = new HashSet<Message>();
            GroupMemberships = new HashSet<GroupMember>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string FullName { get; set; }

        public string ProfilePictureUrl { get; set; }

        public string Bio { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Navigation properties
       
        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<Message> ReceivedMessages { get; set; }
        public virtual ICollection<GroupMember> GroupMemberships { get; set; }
    }
}