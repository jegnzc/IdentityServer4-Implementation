using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace Entities;

public class User : IdentityUser
{
    public User(string userName) : base(userName)
    {
    }

    public User()
    {
    }

    public string TestProperty { get; set; }
}