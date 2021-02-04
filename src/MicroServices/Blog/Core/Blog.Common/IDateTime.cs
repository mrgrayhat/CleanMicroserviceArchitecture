using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}
