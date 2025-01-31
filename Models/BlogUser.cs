using System.ComponentModel.DataAnnotations;
using Azure.Core;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Models;

public class BlogUser : IdentityUser
{
    [Required]
    [Display(Name = "First Name")]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
    public string? FirstName { get; set; }
    
    [Required]
    [Display(Name = "Last Name")]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
    public string? LastName { get; set; }
    
    public ICollection<Article>? Articles { get; set; } = new List<Article>();
}