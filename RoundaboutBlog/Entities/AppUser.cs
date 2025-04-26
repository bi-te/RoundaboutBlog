using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RoundaboutBlog.Entities;

public enum Gender
{
    Empty,
    Male,
    Female
}

public class AppUser : IdentityUser
{
    [PersonalData]
    [StringLength(100)]
    public string? FirstName { get; set; }
    
    [PersonalData]
    [StringLength(100)]
    public string? SecondName { get; set; }
    
    [PersonalData]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }
    
    [PersonalData]
    public Gender Gender { get; set; }
    
    [PersonalData]
    [StringLength(200)]
    public string? Address { get; set; }
    
    [PersonalData]
    [StringLength(200)]
    public string? Website { get; set; }
}