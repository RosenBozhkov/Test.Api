using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Confiig;

public class UserProfileConfigurations : BaseEntityConfigurations<UserProfile>
{
    public override void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        base.Configure(builder); // Must call this

        // other configurations here
    }
}
