using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities.Abstract;

public abstract class BaseEntity
{
    /// <summary>
    /// Date time of creation
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Date time of last modification
    /// </summary>
    public DateTime ModifiedAt { get; set; }
}