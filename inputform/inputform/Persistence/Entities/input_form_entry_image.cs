using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace inputform.Persistence.Entities;

public partial class input_form_entry_image
{
    [Key]
    public Guid entry_id { get; set; }

    public byte[] profile_image { get; set; } = null!;

    [StringLength(50)]
    public string content_type { get; set; } = null!;

    public int file_size { get; set; }

    public DateTime created_at { get; set; }

    [ForeignKey("entry_id")]
    [InverseProperty("input_form_entry_image")]
    public virtual input_form_entry entry { get; set; } = null!;
}
