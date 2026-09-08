using System.ComponentModel.DataAnnotations;

namespace Nursery.Identity.Models.DTO;

public class LoginRequestDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string UserName { get; set; } = default!;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
}
