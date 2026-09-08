using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Nursery.Identity.Models.DTO;

public class RegisterRequestDto
{
    [Required]
    [MaxLength(30)]
    public string FirstName { get; set; } = default!;
    [Required]
    [MaxLength(30)]
    public string LastName { get; set; } = default!;
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    public string[] Roles { get; set; } = default!;
}
