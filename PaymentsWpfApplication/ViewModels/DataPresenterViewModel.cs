using EventAggregator;
using Payments.Data;
using Payments.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Linq;


namespace PaymentsWpfApplication.ViewModels
{
    /// <summary>
    /// Представляет данные.
    /// </summary>
    public class DataPresenterViewModel : ViewModelBase
    {
        private Entity _listedType;
        private Entity _parentType;
        private ICollectionView _view;
        private ObservableCollection<object> _items;

        ICommand _moveNext;
        ICommand _movePrev;
        ICommand _moveFirst;
        ICommand _moveLast;

        protected EntitiesController ec = EntitiesController.Instance;
        private int initIndex;

        /// <summary>
        /// Открытая сущность.
        /// </summary>
        public Entity Entity
        {
            get
            {
                return _listedType;
            }
            private set
            {
                if (_listedType != value)
                {
                    _listedType = value;
                    OnPropertyChanged(() => Entity);
                }
            }
        }
        /// <summary>
        /// Сущности в списке.
        /// </summary>
        public Entity ListedEntities
        {
            get
            {
                return _parentType;
            }
            private set
            {
                if (_parentType != value)
                {
                    _parentType = value;
                    OnPropertyChanged(() => ListedEntities);
                    OnPropertyChanged(() => ListedEntityTitle);
                    OnPropertyChanged(() => IsListedChilds);
                }
            }
        }

        public string ListedEntityTitle
        {
            get
            {
                return EntityMapper.Title(ListedEntities);
            }
        }

        public bool IsListedChilds
        {
            get
            {
                return Entity != ListedEntities;
            }
        }

        public ICollectionView View
        {
            get { return _view; }
            private set
            {
                _view = value;
                OnPropertyChanged(() => View);
            }
        }

        public ObservableCollection<object> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new ObservableCollection<object>(MakeItemsCollection());
                    View = CollectionViewSource.GetDefaultView(_items);
                    View.CurrentChanged += View_CurrentChanged;
                    CurrentIndex = initIndex;
                    OnPropertyChanged(() => TotalCount);
                }

                return _items;
            }
        }

        public object CurrentItem
        {
            get
            {
                if (View != null)
                    return View.CurrentItem;
                return null;
            }
        }

        /// <summary>
        /// Количество сущностей в коллекции.
        /// </summary>
        public int TotalCount
        {
            get
            {
                if (Items != null)
                    return Items.Count;
                return 0;
            }
        }

        /// <summary>
        /// Индекс текущей сущности в коллекции, начиная с 0.
        /// </summary>
        public int CurrentIndex
        {
            get
            {
                if (View != null)
                    return View.CurrentPosition;
                return -1;
            }
            set
            {
                if (View != null && View.CurrentPosition != value && value >= -1 && value < TotalCount)
                {
                    View.MoveCurrentToPosition(value);
                    OnPropertyChanged(() => CurrentIndex);
                    OnPropertyChanged(() => CurrentNumber);
                    OnPropertyChanged(() => CurrentItem);
                }
            }
        }
        /// <summary>
        /// Номер текущей сущности в коллекции, начиная с 1.
        /// </summary>
        public int CurrentNumber
        {
            get
            {
                return CurrentIndex + 1;
            }
            set
            {
                CurrentIndex = value - 1;
            }
        }

        #region Navigation

        void CreateCommands()
        {
            _moveNext = new RelayCommand(
                    () => View.MoveCurrentToNext(),
                    () => CurrentIndex < TotalCount - 1);

            _movePrev = new RelayCommand(
                    () => View.MoveCurrentToPrevious(),
                    () => CurrentIndex > 0);

            _moveFirst = new RelayCommand(
                    () => View.MoveCurrentToFirst(),
                    () => CurrentIndex != 0);

            _moveLast = new RelayCommand(
                    () => View.MoveCurrentToLast(),
                    () => CurrentIndex != TotalCount - 1);
        }

        public ICommand MoveNext { get { return _moveNext; } }
        public ICommand MovePrev { get { return _movePrev; } }
        public ICommand MoveFirst { get { return _moveFirst; } }
        public ICommand MoveLast { get { return _moveLast; } }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Тип отображаемой сущности.</param>
        /// <param name="entity">Конкретная сущность, устанавливаемая текущей.</param>
        /// <param name="showEntityChilds">Показывать ли в коллекции дочерние сущности.</param>
        /// <param name="childIndex">Индекс дочерней сущности для показа.</param>
        public DataPresenterViewModel(Entity type, object entity, bool showEntityChilds = false, int childIndex = 0)
            : this(type,
                   entity,
                   showEntityChilds ? EntityMapper.Childs(type).ElementAt(childIndex) : Entity.Empty)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Тип отображаемой сущности.</param>
        /// <param name="entity">Конкретная сущность, устанавливаемая текущей.</param>
        /// <param name="child">Дочерняя сущность для показа</param>
        public DataPresenterViewModel(Entity type, object entity, Entity child = Entity.Empty)
        {
            Entity = type;
            if (EntityMapper.Childs(type).Contains(child))
            {
                ListedEntities = child;
            }
            else
            {
                ListedEntities = Entity;
            }

            // передана конкретная сущность для показа в списке сущностей этого типа
            if (entity != null && child == Entity.Empty)
            {
                // after delete to show near entity
                initIndex = ec.FindIndexOfSameEntity(entity, Items);
            }
            else
            {
                initIndex = -1;
            }
            CreateCommands();
        }

        public DataPresenterViewModel()
            : this(0, null, Entity.Empty)
        {
        }

        /// <summary>
        /// Создает коллекцию сущностей для просмотра.
        /// </summary>
        protected virtual IEnumerable<object> MakeItemsCollection()
        {
            return Enumerable.Empty<object>();
        }

        private void View_CurrentChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(() => CurrentItem);
            OnPropertyChanged(() => CurrentIndex);
            OnPropertyChanged(() => CurrentNumber);
        }
    }
}
