using System;

namespace SaeedRezayi.ViewModels.Types
{
    /// <summary>
    /// sort orders
    /// </summary>
    [Flags]
    public enum SortingOrderTypes
    {
        NONE = 0,
        Ascending = 1,
        Descending = 2,
    }
}
