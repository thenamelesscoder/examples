using System.ComponentModel.DataAnnotations;

namespace SimpleBearerAuth.Models
{
  public class TokenForCreateDto
  {
    [Required]
    [EmailAddress]
    public string MailAddress { get; set; }

    [Required]
    public string Password { get; set; }
  }
}
