using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BuildingBlocks.Common;

public class BaseDomainModel
{
    public string? CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? ModifiedBy { get; set; } = string.Empty;
    public DateTime ModifiedAt { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; private set; } = default!;

    protected void SetCreated(string createdBy)
    {
        CreatedBy = createdBy;
        CreatedAt = DateTime.Now;
    }

    protected void SetModified(string modifiedBy)
    {
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.Now;
    }
}
