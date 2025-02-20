using System.ComponentModel.DataAnnotations;

namespace FlowCash.Models;

public class User
{
    public User(string? resetToken)
    {
        ResetToken = resetToken;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = String.Empty;
    
    [Required]
    public string LastName { get; set; } = String.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = String.Empty;

    [Required]
    public string Password { get; set; } = String.Empty;

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    
    //password recovery
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpiration  { get; set; }
}