using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Payments.Model
{
    public enum Entity
    {
        Empty, // for returns 

        Settlement,
        Street,
        House,
        Flat,

        StreetType,
        FlatType,
        Meter,
        MeterType,
        Reading,
        Payment,
        Rate,
        Benefit,
        Service,
        ServiceType,
        Unit,
        Registration,
        Payer,
        User,
        UserGroup
    }
}
