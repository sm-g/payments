using EventAggregator;
using Payments.Data;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;

namespace Payments.Model
{
    public partial class EntitiesController
    {
        public Currents Current;
        public Requests Get;
        private static readonly Lazy<EntitiesController> lazyInstance = new Lazy<EntitiesController>(() => new EntitiesController());
        private UserController uc = UserController.Instance;
        private EntitiesController()
        {
            Current = new Currents();
            Get = new Requests(this, uc);
        }

        #region CRUD

        /// <summary>
        /// Возвращает коллекцию сущностей по имени.
        /// </summary>
        public IEnumerable<object> GetCollection(string name)
        {
            try
            {
                var result = (IEnumerable<object>)typeof(Requests).GetProperty(name).GetValue(Get, null);
                return result;
            }
            catch (Exception e)
            {
                Trace.TraceError("GetCollection '{0}' error: {1}", name, e);

                return Enumerable.Empty<Object>();
            }
        }

        /// <summary>
        /// Обновляет сущность в БД.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(object entity)
        {
            UpdateAll(entity.ToIEnumerable());
        }

        /// <summary>
        /// Обновляет данные сущностей в БД.
        /// </summary>
        /// <param name="entities"></param>
        public void UpdateAll(IEnumerable<object> entities)
        {
            if (entities.First() != null)
            {
                List<object> ready = new List<object>(); try
                {
                    foreach (var entity in entities)
                    {
                        if (uc.CanUpdate(entity))
                        {
                            ready.Add(entity);
                        }
                        else
                        {
                            Trace.TraceWarning("{0} try to update {1}", uc.CurrentUserName, entity);
                        }
                    }
                    DB.Context.Refresh(RefreshMode.KeepCurrentValues, ready.ToArray());
                    DB.Context.SubmitChanges();
                }
                catch (Exception e)
                {
                    var eventParams = new KeyValuePair<string, object>[]
                    {
                        new KeyValuePair<string, object>(Messenger.ErrorKey, e.Message)
                    };
                    this.Send((int)EventId.ErrorOccured, eventParams);
                }
            }
        }

        /// <summary>
        /// Опеределяет, существует ли сущность в БД.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Exists(dynamic entity)
        {
            var e = EntityMapper.EntityOf(entity);
            dynamic @enum = typeof(Requests).GetProperty(EntityMapper.CollectionName(e)).GetValue(Get, null);
            return @enum.Contains(entity);
        }

        /// <summary>
        /// Удаляет сущность из БД.
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(object entity)
        {
            if (uc.CanDelete(entity))
            {
                var type = EntityMapper.EntityOf(entity);
                try
                {
                    DeleteFromDb(entity);
                }
                catch (Exception e)
                {
                    var eventParams = new KeyValuePair<string, object>[]
                    {
                        new KeyValuePair<string, object>(Messenger.ErrorKey, e.Message)
                    };
                    this.Send((int)EventId.ErrorOccured, eventParams);
                }
            }
            else
            {
                Trace.TraceWarning("{0} try to delete {1}", uc.CurrentUserName, entity);
            }
        }

        /// <summary>
        /// Добавляет сущность в БД.
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(object entity)
        {
            var type = EntityMapper.EntityOf(entity);
            if (uc.CanCreate(type))
            {
                try
                {
                    InsertToDb(entity);
                }
                catch (Exception e)
                {
                    var eventParams = new KeyValuePair<string, object>[]
                    {
                        new KeyValuePair<string, object>(Messenger.ErrorKey, e.Message)
                    };
                    this.Send((int)EventId.ErrorOccured, eventParams);
                }
            }
            else
            {
                Trace.TraceWarning("{0} try to insert {1}", uc.CurrentUserName, entity);
            }
        }

        /// <summary>
        /// Создает сущность.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Create(Entity type)
        {
            var T = EntityMapper.TypeOf(type);
            var entity = Activator.CreateInstance(T);

            switch (type)
            {
                //case Entity.Settlement:
                //    break;
                case Entity.Street:
                    ((Street)entity).Settlement = Current.Settlement ?? null;
                    ((Street)entity).StreetType = Current.StreetType ?? null;
                    break;

                case Entity.House:
                    ((House)entity).Street = Current.Street ?? null;
                    break;

                case Entity.Flat:
                    ((Flat)entity).House = Current.House ?? null;
                    ((Flat)entity).FlatType = Current.FlatType ?? null;
                    break;
                //case Entity.StreetType:
                //    break;
                //case Entity.FlatType:
                //    break;
                case Entity.Meter:
                    ((Meter)entity).MeterType = Current.MeterType ?? null;
                    ((Meter)entity).Registration = Current.Registration ?? null;
                    break;
                //case Entity.MeterType:
                //    break;
                case Entity.Reading:
                    ((Reading)entity).Meter = Current.Meter ?? null;
                    ((Reading)entity).Date = DateTime.Today;
                    break;

                case Entity.Payment:
                    ((Payment)entity).Registration = Current.Registration ?? null;
                    ((Payment)entity).Service = Current.Service ?? null;
                    ((Payment)entity).Date = DateTime.Today;
                    ((Payment)entity).TargetMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    break;

                case Entity.Rate:
                    ((Rate)entity).BeginDate = DateTime.Today;
                    break;
                //case Entity.Benefit:
                //    break;
                case Entity.Service:
                    ((Service)entity).ServiceType = Current.ServiceType ?? null;
                    break;
                //case Entity.ServiceType:
                //    break;
                //case Entity.Unit:
                //    break;
                case Entity.Registration:
                    ((Registration)entity).PaymentStartDate = DateTime.Today;
                    ((Registration)entity).Flat = Current.Flat ?? null;
                    ((Registration)entity).Payer = Current.Payer ?? null;
                    break;

                case Entity.Payer:
                    ((Payer)entity).User = Current.User ?? null;
                    ((Payer)entity).Sex = 1; // male
                    if (!uc.IsAdminLoggedIn)
                        ((Payer)entity).User = uc.CurrentUser;
                    break;

                case Entity.User:
                    ((User)entity).Salt = PasswordMaker.GenerateSalt();
                    ((User)entity).UserGroupID = (byte)uc.GetRegularUserGroup();
                    break;
                //case Entity.UserGroup:
                //    break;
            }

            return entity;
        }

        private static void InsertToDb(object entity)
        {
            var type = EntityMapper.TypeOf(entity);
            DB.Context.GetTable(type).InsertOnSubmit(entity);
            DB.Context.SubmitChanges();
        }

        private static void DeleteFromDb(object entity)
        {
            var type = EntityMapper.TypeOf(entity);
            DB.Context.GetTable(type).DeleteOnSubmit(entity);
            DB.Context.SubmitChanges();
        }

        #endregion CRUD

        public static EntitiesController Instance
        {
            get
            {
                return lazyInstance.Value;
            }
        }

        /// <summary>
        /// Ищет индекс сущности в списке.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="collection"></param>
        /// <returns>-1, если не найден.</returns>
        public int FindIndexOfSameEntity(object entity, IList<object> collection)
        {
            var type = EntityMapper.EntityOf(entity);
            var entityID = entity.GetType().GetProperty(type.ToString() + "ID").GetValue(entity, null);

            return collection.FindIndex(e =>
            {
                var eID = e.GetType().GetProperty(type.ToString() + "ID").GetValue(e, null);
                return Object.Equals(eID, entityID);
            });
        }

        /// <summary>
        /// Задает у сущности подчинённую сущность.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityToSet"></param>
        /// <returns></returns>
        public object SetChildEntity(object entity, object entityToSet)
        {
            var prop = entity.GetType().GetProperties()
                .Where(pi => pi.PropertyType == entityToSet.GetType())
                .FirstOrDefault();

            if (prop != null)
                prop.SetValue(entity, entityToSet, null);

            return entity;
        }

        private static object MakeGenericList(Entity item, object initData = null)
        {
            var d1 = typeof(List<>);
            Type[] typeArgs = { EntityMapper.TypeOf(item) };
            var makeme = d1.MakeGenericType(typeArgs);
            return initData == null ?
                Activator.CreateInstance(makeme) :
                Activator.CreateInstance(makeme, initData);
        }
    }
}