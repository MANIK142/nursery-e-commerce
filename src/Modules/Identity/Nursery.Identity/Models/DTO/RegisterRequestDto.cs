using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Nursery.Identity.Models.DTO;

public class RegisterRequestDto
{

    public string FirstName { get; set; } = default!;
  
    public string LastName { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string[] Roles { get; set; } = default!;
}
