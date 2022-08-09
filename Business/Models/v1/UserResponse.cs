using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.v1;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
}
