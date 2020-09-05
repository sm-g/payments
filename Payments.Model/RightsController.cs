using Payments.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payments.Model
{
    internal static class RightsController
    {
        private static Dictionary<Entity, Func<object, User, bool>> checkAuthorityMap;

        static RightsController()
        {
            checkAuthorityMap = new Dictionary<Entity, Func<object, User, bool>>();
            checkAuthorityMap.Add(Entity.Flat, (e, user) => ((Flat)e).Registrations.Where(reg => reg.Payer.UserID == user.UserID).Any());
            checkAuthorityMap.Add(Entity.Payer, (e, user) => ((Payer)e).UserID == user.UserID);
            checkAuthorityMap.Add(Entity.Registration, (e, user) => ((Registration)e).Payer.UserID == user.UserID);
            checkAuthorityMap.Add(Entity.Payment, (e, user) => ((Payment)e).Registration.Payer.UserID == user.UserID);
            checkAuthorityMap.Add(Entity.Meter, (e, user) => ((Meter)e).Registration.Payer.UserID == user.UserID);
            checkAuthorityMap.Add(Entity.Reading, (e, user) => ((Reading)e).Meter.Registration.Payer.UserID == user.UserID);
        }

        public static bool IsAutorizedRetrievingAllowed(object entity, User user)
        {
            var type = EntityMapper.EntityOf(entity);
            Func<object, User, bool> check;
            if (AssociatedWithUser(type) && checkAuthorityMap.TryGetValue(type, out check))
            {
                return check(entity, user);
            }
            return false;
        }

        public static bool IsAutorizedUpdatingAllowed(object entity, User user)
        {
            var type = EntityMapper.EntityOf(entity);
            Func<object, User, bool> check;
            if (UserCanUpdate(type) && checkAuthorityMap.TryGetValue(type, out check))
            {
                return check(entity, user);
            }
            return false;
        }

        public static bool IsAutorizedDeletingAllowed(object entity, User user)
        {
            var type = EntityMapper.EntityOf(entity);
            Func<object, User, bool> check;
            if (UserCanDelete(type) && checkAuthorityMap.TryGetValue(type, out check))
            {
                return check(entity, user);
            }
            return false;
        }

        public static bool UserCanCreate(Entity type)
        {
            return
                type == Entity.Payer ||
                type == Entity.Registration ||
                type == Entity.Payment ||
                type == Entity.Reading ||
                type == Entity.Meter;
        }

        public static bool AssociatedWithUser(Entity type)
        {
            return
                type == Entity.Payer ||
                type == Entity.Registration ||
                type == Entity.Payment ||
                type == Entity.Reading ||
                type == Entity.Meter;
        }

        private static bool UserCanUpdate(Entity type)
        {
            return type == Entity.Flat ||
                type == Entity.Payer ||
                type == Entity.Registration ||
                type == Entity.Payment ||
                type == Entity.Reading;
        }

        private static bool UserCanDelete(Entity type)
        {
            return type == Entity.Payment ||
                type == Entity.Reading;
        }
    }
}