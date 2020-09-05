using Payments.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Payments.Model
{
    public partial class EntitiesController
    {

        public class Requests
        {
            private readonly EntitiesController ec;
            private readonly UserController uc;


            public Requests(EntitiesController ec, UserController uc)
            {
                this.ec = ec;
                this.uc = uc;
            }
            public IEnumerable<Settlement> Settlements { get { return GetCollectionOf(Entity.Settlement).Cast<Settlement>(); } }
            public IEnumerable<Street> Streets { get { return GetCollectionOf(Entity.Street).Cast<Street>(); } }
            public IEnumerable<House> Houses { get { return GetCollectionOf(Entity.House).Cast<House>(); } }
            public IEnumerable<Flat> Flats { get { return GetCollectionOf(Entity.Flat).Cast<Flat>(); } }
            public IEnumerable<StreetType> StreetTypes { get { return GetCollectionOf(Entity.StreetType).Cast<StreetType>(); } }
            public IEnumerable<FlatType> FlatTypes { get { return GetCollectionOf(Entity.FlatType).Cast<FlatType>(); } }
            public IEnumerable<Meter> Meters { get { return GetCollectionOf(Entity.Meter).Cast<Meter>(); } }
            public IEnumerable<MeterType> MeterTypes { get { return GetCollectionOf(Entity.MeterType).Cast<MeterType>(); } }
            public IEnumerable<Reading> Readings { get { return GetCollectionOf(Entity.Reading).Cast<Reading>(); } }
            public IEnumerable<Payment> Payments { get { return GetCollectionOf(Entity.Payment).Cast<Payment>(); } }
            public IEnumerable<Rate> Rates { get { return GetCollectionOf(Entity.Rate).Cast<Rate>(); } }
            public IEnumerable<Benefit> Benefits { get { return GetCollectionOf(Entity.Benefit).Cast<Benefit>(); } }
            public IEnumerable<Service> Services { get { return GetCollectionOf(Entity.Service).Cast<Service>(); } }
            public IEnumerable<ServiceType> ServiceTypes { get { return GetCollectionOf(Entity.ServiceType).Cast<ServiceType>(); } }
            public IEnumerable<Unit> Units { get { return GetCollectionOf(Entity.Unit).Cast<Unit>(); } }
            public IEnumerable<Registration> Registrations { get { return GetCollectionOf(Entity.Registration).Cast<Registration>(); } }
            public IEnumerable<Payer> Payers { get { return GetCollectionOf(Entity.Payer).Cast<Payer>(); } }
            public IEnumerable<User> Users { get { return GetCollectionOf(Entity.User).Cast<User>(); } }
            public IEnumerable<UserGroup> UserGroups { get { return GetCollectionOf(Entity.UserGroup).Cast<UserGroup>(); } }
            public IEnumerable<Street> StreetsInSettlement
            {
                get
                {
                    return from s in Streets
                           where s.SettlementID == ec.Current.Settlement.SettlementID
                           select s;
                }
            }

            public IEnumerable<House> HousesInStreet
            {
                get
                {
                    return from h in Houses
                           where h.StreetID == ec.Current.Street.StreetID
                           select h;
                }
            }

            public IEnumerable<Flat> FlatsInHouse
            {
                get
                {
                    return from f in Flats
                           where f.HouseID == ec.Current.House.HouseID
                           select f;
                }
            }

            public IEnumerable<Registration> RegistrationsInFlat
            {
                get
                {
                    return from r in Registrations
                           where r.FlatID == ec.Current.Flat.FlatID
                           select r;
                }
            }

            public IEnumerable<Street> StreetsInStreetType
            {
                get
                {
                    return from r in Streets
                           where r.StreetTypeID == ec.Current.StreetType.StreetTypeID
                           select r;
                }
            }

            public IEnumerable<Flat> FlatsInFlatType
            {
                get
                {
                    return from f in Flats
                           where f.FlatTypeID == ec.Current.FlatType.FlatTypeID
                           select f;
                }
            }

            public IEnumerable<Reading> ReadingsInMeter
            {
                get
                {
                    return from r in Readings
                           where r.MeterID == ec.Current.Meter.MeterID
                           select r;
                }
            }

            public IEnumerable<Meter> MetersInMeterType
            {
                get
                {
                    return from r in Meters
                           where r.MeterTypeID == ec.Current.MeterType.MeterTypeID
                           select r;
                }
            }

            public IEnumerable<Payer> PayersInBenefit
            {
                get
                {
                    return from r in Payers
                           where r.BenefitID == ec.Current.Benefit.BenefitID
                           select r;
                }
            }

            public IEnumerable<Payment> PaymentsInService
            {
                get
                {
                    return from r in Payments
                           where r.ServiceID == ec.Current.Service.ServiceID
                           select r;
                }
            }

            public IEnumerable<Service> ServicesInServiceType
            {
                get
                {
                    return from r in Services
                           where r.ServiceTypeID == ec.Current.ServiceType.ServiceTypeID
                           select r;
                }
            }

            public IEnumerable<Service> ServicesInUnit
            {
                get
                {
                    return from r in Services
                           where r.UnitID == ec.Current.Unit.UnitID
                           select r;
                }
            }

            public IEnumerable<Registration> RegistrationsInPayer
            {
                get
                {
                    return from r in Registrations
                           where r.PayerID == ec.Current.Payer.PayerID
                           select r;
                }
            }

            public IEnumerable<Payment> PaymentsInRegistration
            {
                get
                {
                    return from p in Payments
                           where p.RegistrationID == ec.Current.Registration.RegistrationID
                           select p;
                }
            }

            public IEnumerable<Payer> PayersInUser
            {
                get
                {
                    return from p in Payers
                           where p.UserID == ec.Current.User.UserID
                           select p;
                }
            }

            public IEnumerable<User> UsersInUserGroup
            {
                get
                {
                    return from p in Users
                           where p.UserGroupID == ec.Current.UserGroup.UserGroupID
                           select p;
                }
            }

            public IEnumerable<Payment> FilteredPaymentsInRegistration(DateTime @from, DateTime to)
            {
                return from p in Payments
                       where p.RegistrationID == ec.Current.Registration.RegistrationID
                       where p.Date <= to
                       where p.Date >= @from
                       select p;
            }

            public IEnumerable<Reading> FilteredReadingsInRegistration(DateTime @from, DateTime to)
            {
                return from read in Readings
                       join met in Meters on read.MeterID equals met.MeterID
                       where met.RegistrationID == ec.Current.Registration.RegistrationID
                       where read.Date <= to
                       where read.Date >= @from
                       select read;
            }

            public IEnumerable<Payment> FilteredPaymentsInService(DateTime @from, DateTime to)
            {
                return from p in Payments
                       where p.ServiceID == ec.Current.Service.ServiceID
                       where p.Date <= to
                       where p.Date >= @from
                       select p;
            }

            private IEnumerable<object> GetCollectionOf(Entity type)
            {
                var t = EntityMapper.TypeOf(type);
                // next is like
                // Users =
                // (from e in DB.Context.Users.ToList()
                // where uc.CanRetrieve(e)
                // select e).ToList();

                var queryable = DB.Context.GetType().GetProperty(EntityMapper.CollectionName(type)).GetValue(DB.Context, null);
                var pe = Expression.Parameter(t, "p");

                var selector = Expression.Lambda(pe, pe);
                var selectCall = Expression.Call(typeof(Queryable), "Select", new[] { t, t }, Expression.Constant(queryable), selector);

                var select = Expression.Lambda(selectCall).Compile();
                var all = select.DynamicInvoke();
                var list = MakeGenericList(type, all);

                // filter by right
                var canCall = Expression.Call(Expression.Constant(uc), typeof(UserController).GetMethod("CanRetrieve"), pe);
                var predicate = Expression.Lambda(canCall, pe);
                var whereCall = Expression.Call(typeof(Enumerable), "Where", new[] { t }, Expression.Constant(list), predicate);

                dynamic result = Expression.Lambda(whereCall).Compile().DynamicInvoke();
                var filtered = Enumerable.ToList(result);
                return filtered;
            }
        }

    }
}
