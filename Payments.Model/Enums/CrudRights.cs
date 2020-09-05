using System;

namespace Payments.Model
{
    [Flags]
    public enum CrudRights
    {
        None = 0,
        Retrieve = 1,
        Create = 2,
        Update = 4,
        Delete = 8,
        All = 15
    }
}