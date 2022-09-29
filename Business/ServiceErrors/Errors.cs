using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ServiceErrors;

public static class Errors
{
    public static class Job
    {
        public static Error NotFound => Error.NotFound(
            code: "Job.NotFound",
            description: "Job not found"
            );
    }
}
