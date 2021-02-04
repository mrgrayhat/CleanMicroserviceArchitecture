﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STS.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace STS.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}
