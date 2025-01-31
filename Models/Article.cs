using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models;

public class Article
{
    public int ArticleId { get; set; }
    
    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
    public string? Title { get; set; }
    
    [Required]
    [StringLength(2048, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
    public string? Body { get; set; }
    
    [Required]
    [Display(Name = "Date Created")]
    public DateTime CreateDate { get; set; }
    
    [Required]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }
    
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }
    
    [EmailAddress]
    [StringLength(256)]
    [Display(Name = "Contributor Email")]
    public string? ContributorEmail { get; set; }
    
    [ForeignKey("ContributorEmail")]
    public BlogUser? Contributor { get; set; }
}