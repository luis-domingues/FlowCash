using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowCash.Models;

public class Transaction
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public string Type { get; set; } = String.Empty;
    
    [Required]
    public string Category { get; set; } = String.Empty;
    
    [Required]
    public DateTime Date { get; set; }
    
    public User User { get; set; }
}