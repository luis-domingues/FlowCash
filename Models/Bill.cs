using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowCash.Models;

public class Bill
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    
    [Required]
    public string Description { get; set; } = String.Empty;
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    [Required]
    public bool IsPaid { get; set; }
    
    public User User { get; set; }
}