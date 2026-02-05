using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace inputform.Persistence.Entities;

[Index("email", Name = "input_form_entries_email_key", IsUnique = true)]
[Index("created_at", Name = "ix_input_form_entries_created", AllDescending = true)]
public partial class input_form_entry
{
    [Key]
    public Guid id { get; set; }

    [StringLength(100)]
    public string first_name { get; set; } = null!;

    [StringLength(100)]
    public string last_name { get; set; } = null!;

    [Column(TypeName = "citext")]
    public string email { get; set; } = null!;

    [StringLength(20)]
    public string phone { get; set; } = null!;

    [StringLength(100)]
    public string occupation { get; set; } = null!;

    [StringLength(10)]
    public string sex { get; set; } = null!;

    public DateOnly birth_day { get; set; }

    public DateTime created_at { get; set; }

    [InverseProperty("entry")]
    public virtual input_form_entry_image? input_form_entry_image { get; set; }
}
