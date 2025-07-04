﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoundaboutBlog.Entities;

public class Post
{
  public int PostId { get; set; }

  [StringLength(200)]
  public required string Title { get; set; }

  [StringLength(5000)]
  public required string Content { get; set; }

  public DateTime CreatedAt { get; set; }

  public string? UserId { get; set; }

  public AppUser? User { get; set; }

  public ICollection<Comment> Comments { get; } = new List<Comment>();
}