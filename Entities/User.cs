using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationFormWPF.Entities;


[Index(nameof(Username), IsUnique = true)]
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [StringLength(16, MinimumLength = 3)]
    public string Username { get; set; }

    [StringLength(32, MinimumLength = 4)]
    public string Password { get; set; }

    public User (string  username, string password)
    {
        Username = username;
        Password = password;
    }
}