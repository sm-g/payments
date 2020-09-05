using Payments.Data;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Payments.Model
{
    public static class EntityMapper
    {
        private static Dictionary<Entity, Entity[]> childsMap;
        private static Dictionary<Entity, Entity> parentMap;
        private static Dictionary<Entity, Type> typeMap;
        private static Dictionary<Entity, string> titleMap;

        static EntityMapper()
        {
            parentMap = new Dictionary<Entity, Entity>();
            parentMap.Add(Entity.Street, Entity.Settlement);
            parentMap.Add(Entity.House, Entity.Street);
            parentMap.Add(Entity.Flat, Entity.House);

            // child free: 
            // Rate, Payment, Reading

            childsMap = new Dictionary<Entity, Entity[]>();
            childsMap.Add(Entity.Settlement, new[] { Entity.Street });
            childsMap.Add(Entity.Street, new[] { Entity.House });
            childsMap.Add(Entity.House, new[] { Entity.Flat });
            childsMap.Add(Entity.StreetType, new[] { Entity.Street });
            childsMap.Add(Entity.Flat, new[] { Entity.Registration });
            childsMap.Add(
                Entity.FlatType, new[] {
                Entity.Flat,//
                Entity.Rate});
            childsMap.Add(Entity.Payer, new[] { Entity.Registration });
            childsMap.Add(
                Entity.Registration, new[] {
                Entity.Payment,//
                Entity.Meter});
            childsMap.Add(Entity.Meter, new[] { Entity.Reading });
            childsMap.Add(Entity.MeterType, new[] { Entity.Meter });
            childsMap.Add(
                Entity.Benefit, new[] {
                Entity.Payer, //
                Entity.Rate});
            childsMap.Add(
                Entity.Service, new[] {
                Entity.Payment,//
                Entity.Rate});
            childsMap.Add(
                Entity.Unit, new[] {
                Entity.Service,//
                Entity.MeterType
                });
            childsMap.Add(Entity.ServiceType, new[] { Entity.Service });
            childsMap.Add(Entity.User, new[] { Entity.Payer });
            childsMap.Add(Entity.UserGroup, new[] { Entity.User });

            typeMap = new Dictionary<Entity, Type>(){
                {Entity.Settlement, typeof(Settlement)},
                {Entity.Street, typeof(Street)},
                {Entity.House, typeof(House)},
                {Entity.Flat, typeof(Flat)},
                {Entity.StreetType, typeof(StreetType)},
                {Entity.FlatType, typeof(FlatType)},
                {Entity.Meter, typeof(Meter)},
                {Entity.MeterType, typeof(MeterType)},
                {Entity.Reading, typeof(Reading)},
                {Entity.Payment, typeof(Payment)},
                {Entity.Rate, typeof(Rate)},
                {Entity.Benefit, typeof(Benefit)},
                {Entity.Service, typeof(Service)},
                {Entity.ServiceType, typeof(ServiceType)},
                {Entity.Unit, typeof(Unit)},
                {Entity.Registration, typeof(Registration)},
                {Entity.Payer, typeof(Payer)},
                {Entity.User, typeof(User)},
                {Entity.UserGroup, typeof(UserGroup)},
            };
            titleMap = new Dictionary<Entity, string>() {
                {Entity.Settlement, "Города"},
                {Entity.Street, "Улицы"},
                {Entity.House, "Дома"},
                {Entity.Flat, "Квартиры"},
                {Entity.StreetType, "Типы улиц"},
                {Entity.FlatType, "Типы квартир"},
                {Entity.Meter, "Счётчики"},
                {Entity.MeterType, "Типы счётчиков"},
                {Entity.Reading, "Показания"},
                {Entity.Payment, "Платежи"},
                {Entity.Rate, "Тарифы"},
                {Entity.Benefit, "Льготы"},
                {Entity.Service, "Услуги"},
                {Entity.ServiceType, "Типы услуг"},
                {Entity.Unit, "Единицы измерения"},
                {Entity.Registration, "Регистрации"},
                {Entity.Payer, "Плательщики"},
                {Entity.User, "Пользователи"},
                {Entity.UserGroup, "Группы пользователей"},
            };
        }

        public static Type TypeOf(Entity type)
        {
            Type result = typeof(object);
            typeMap.TryGetValue(type, out result);

            return result;
        }
        public static Type TypeOf(object entity)
        {
            return TypeOf(EntityOf(entity));
        }

        /// <summary>
        /// Определяет тип Entity экземпляра сущности.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Entity EntityOf(object entity)
        {
            if (entity is Settlement) return Entity.Settlement;
            if (entity is Street) return Entity.Street;
            if (entity is House) return Entity.House;
            if (entity is Flat) return Entity.Flat;
            if (entity is StreetType) return Entity.StreetType;
            if (entity is FlatType) return Entity.FlatType;
            if (entity is Meter) return Entity.Meter;
            if (entity is MeterType) return Entity.MeterType;
            if (entity is Reading) return Entity.Reading;
            if (entity is Payment) return Entity.Payment;
            if (entity is Rate) return Entity.Rate;
            if (entity is Benefit) return Entity.Benefit;
            if (entity is Service) return Entity.Service;
            if (entity is ServiceType) return Entity.ServiceType;
            if (entity is Unit) return Entity.Unit;
            if (entity is Registration) return Entity.Registration;
            if (entity is Payer) return Entity.Payer;
            if (entity is User) return Entity.User;
            if (entity is UserGroup) return Entity.UserGroup;
            return Entity.Empty;
        }

        public static Entity Parent(Entity type)
        {
            Entity result = Entity.Empty;
            parentMap.TryGetValue(type, out result);

            return result;
        }

        /// <summary>
        /// Список подчиненных сущностей.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Entity> Childs(Entity type)
        {
            Entity[] result;
            childsMap.TryGetValue(type, out result);

            return result ?? Enumerable.Empty<Entity>();
        }

        public static string Title(Entity type)
        {
            string result = "";
            titleMap.TryGetValue(type, out result);

            return result;
        }

        /// <summary>
        /// Вовращает имя коллеции сущностей.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string CollectionName(Entity e)
        {
            return e.ToString() + "s";
        }

        /// <summary>
        /// Возвращает имя коллекции подчиненных сущностей вида UsersInUsergroup
        /// </summary>
        /// <param name="e"></param>
        /// <param name="childIndex">Индекс подчиненной сущности в списке, возвращаемом <see cref="Childs" /> </param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException" />
        /// <exception cref="ArgumentOutOfRangeException" />
        public static string ChildrenCollectionName(Entity e, int childIndex = 0)
        {
            if (IsChildFree(e))
                throw new NotSupportedException("У сущности нет подчиненных.");
            var childs = Childs(e);
            if (childs.Count() <= childIndex)
                throw new ArgumentOutOfRangeException("childIndex", "У сущности меньше подчиненных.");

            return string.Format("{0}sIn{1}", childs.ElementAt(childIndex), e);
        }

        public static bool IsChildFree(Entity type)
        {
            return Childs(type).Count() == 0;
        }
    }
}