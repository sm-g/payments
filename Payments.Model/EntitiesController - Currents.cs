using EventAggregator;
using Payments.Data;
using System;
using System.Linq;

namespace Payments.Model
{
    public partial class EntitiesController
    {
        public class Currents
        {
            public object this[Entity e]
            {
                get
                {
                    return Get(e);
                }
                set
                {
                    Set(value);
                }
            }

            public Settlement Settlement { get; set; }

            public Street Street { get; set; }

            public House House { get; set; }

            public Flat Flat { get; set; }

            public StreetType StreetType { get; set; }

            public FlatType FlatType { get; set; }

            public Meter Meter { get; set; }

            public MeterType MeterType { get; set; }

            public Reading Reading { get; set; }

            public Payment Payment { get; set; }

            public Rate Rate { get; set; }

            public Benefit Benefit { get; set; }

            public Service Service { get; set; }

            public ServiceType ServiceType { get; set; }

            public Unit Unit { get; set; }

            public Registration Registration { get; set; }

            public Payer Payer { get; set; }

            public User User { get; set; }

            public UserGroup UserGroup { get; set; }

            public object Get(Entity e)
            {
                return typeof(Currents).GetProperty(e.ToString()).GetValue(this, null);
            }

            /// <summary>
            /// Устанавливает текущую сущность.
            /// </summary>
            /// <param name="entity"></param>
            public void Set(object entity)
            {
                var e = EntityMapper.EntityOf(entity);
                typeof(Currents).GetProperty(e.ToString()).SetValue(this, entity, null);

                if (EntityMapper.Parent(e) != Entity.Empty)
                {
                    MakeParentsChain(e);
                }

                ClearChilds(e);
            }

            /// <summary>
            /// Восстанавливает цепь родительских сущностей.
            /// </summary>
            private void MakeParentsChain(Entity type)
            {
                switch (type)
                {
                    case Entity.Street:
                        this.Settlement = this.Street.Settlement;
                        break;

                    case Entity.House:
                        this.Street = this.House.Street;
                        this.Settlement = this.Street.Settlement;
                        break;

                    case Entity.Flat:
                        this.House = this.Flat.House;
                        this.Street = this.House.Street;
                        this.Settlement = this.Street.Settlement;
                        break;
                }
            }

            /// <summary>
            /// Очищает текущую сущность.
            /// </summary>
            /// <param name="type"></param>
            private void ClearCurrent(Entity e)
            {
                typeof(Currents).GetProperty(e.ToString()).SetValue(this, null, null);
            }

            /// <summary>
            /// Очищает дочерние текущие сущности.
            /// </summary>
            /// <param name="type"></param>
            private void ClearChilds(Entity type)
            {
                foreach (var chType in EntityMapper.Childs(type))
                {
                    ClearCurrent(chType);
                }
            }
        }
    }
}