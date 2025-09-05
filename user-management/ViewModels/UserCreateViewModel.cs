using System.Collections.Generic;
using user_management.Models;

namespace user_management.ViewModels
{
    public class UserCreateViewModel
    {
        public User NewUser { get; set; }
        public IEnumerable<User> Users { get; set; } = new List<User>();
    }
}
