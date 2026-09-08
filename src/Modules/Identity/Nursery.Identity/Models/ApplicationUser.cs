using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nursery.Identity.Models;

public class ApplicationUser :IdentityUser
{
    [Required]
    [MaxLength(30)]
    public string FirstName { get; set; } = default!;
    [MaxLength(30)]
    public string LastName { get; set; }= default!;

    [NotMapped]
    public string? Role { get; set; }
}
