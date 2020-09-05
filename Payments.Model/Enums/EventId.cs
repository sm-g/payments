using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Payments.Model
{
    public enum EventId
    {
        UserLoggedIn,
        JumpingToEntity,
        EntityDeleted,
        EntityCreated,
        EntityPicked,
        EntityPicking,
        ErrorOccured,
        UserLoggedOut
    }
}
