using EventAggregator;
using Payments.Data;
using Payments.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PaymentsWpfApplication.ViewModels
{
    /// <summary>
    /// Представляет данные, средства управляения и навигации между.
    /// </summary>
    public class DataViewerViewModel : DataPresenterViewModel
    {
        private ICommand _goTo;
        private ICommand _print;
        private ICommand _saveItem;
        private ICommand _saveAll;
        private ICommand _deleteItem;
        private ICommand _editItem;
        private ICommand _selectRow;
        private ICommand _createItem;
        private PickUperViewModel pickup;

        private bool _isEditModeActive;
        private bool _isPickerModeActive;
        private List<object> _breadcrumbs;

        protected UserController uc = UserController.Instance;

        public List<object> Breadcrumbs
        {
            get { return _breadcrumbs; }
            private set
            {
                _breadcrumbs = value;
                OnPropertyChanged(() => Breadcrumbs);
            }
        }

        public bool IsEditModeActive
        {
            get { return _isEditModeActive; }
            set
            {
                if (_isEditModeActive != value)
                {
                    _isEditModeActive = value;

                    if (value && CurrentItem == null)
                    {
                        if (TotalCount == 0)
                        {
                            CreateEntity(ListedEntities);
                        }

                        // выбираем первый элемент, когда начинаем редактирование
                        CurrentIndex = 0;
                    }

                    OnPropertyChanged(() => IsEditModeActive);
                }
            }
        }

        public bool IsPickerModeActive
        {
            get { return _isPickerModeActive; }
            set
            {
                if (_isPickerModeActive != value)
                {
                    _isPickerModeActive = value;
                    OnPropertyChanged(() => IsPickerModeActive);
                }
            }
        }

        #region Commands

        // not used
        public ICommand Print
        {
            get
            {
                return _print ?? (_print = new RelayCommand<object>(
                    (grid) =>
                    {
                    }, (grid) => !IsEditModeActive && !IsPickerModeActive));
            }
        }

        public ICommand SaveItem
        {
            get
            {
                return _saveItem ?? (_saveItem = new RelayCommand<object>(SaveEntity,
                    (e) =>
                    {
                        return !IsPickerModeActive &&
                        uc.CanUpdate(e);
                    }));
            }
        }

        public ICommand SaveAll
        {
            get
            {
                return _saveAll ?? (_saveAll = new RelayCommand(SaveAllEntities,
                    () =>
                    {
                        return !IsPickerModeActive && CurrentItem != null &&
                        uc.CanUpdate(CurrentItem);
                    }));
            }
        }

        public ICommand DeleteItem
        {
            get
            {
                return _deleteItem ?? (_deleteItem = new RelayCommand<object>(DeleteEntity,
                    (e) =>
                    {
                        return !IsPickerModeActive &&
                        uc.CanDelete(e);
                    }));
            }
        }

        public ICommand EditItem
        {
            get
            {
                return _editItem ?? (_editItem = new RelayCommand<object>(
                    (e) => IsEditModeActive = true,
                    (e) => !IsPickerModeActive));
            }
        }

        public ICommand CreateItem
        {
            get
            {
                return _createItem ?? (_createItem = new RelayCommand(
                    () => CreateEntity(ListedEntities),
                    () =>
                    {
                        return !IsPickerModeActive &&
                        uc.CanCreate(ListedEntities);
                    }
                    ));
            }
        }

        public ICommand GoToEntity
        {
            get
            {
                return _goTo ?? (_goTo = new RelayCommand<object>(JumpTo,
                    (e) => !IsPickerModeActive && !EntityMapper.IsChildFree(EntityMapper.EntityOf(e))));
            }
        }

        public ICommand BeginPickup
        {
            get
            {
                return pickup.BeginPickup;
            }
        }

        /// <summary>
        /// Выбирает сущность.
        /// </summary>
        public ICommand SelectEntity
        {
            get
            {
                return _selectRow ?? (_selectRow = new RelayCommand<object>(
                    (e) =>
                    {
                        View.MoveCurrentTo(e);
                        if (IsPickerModeActive)
                        {
                            var eventParams = new KeyValuePair<string, object>[]
                            {
                                new KeyValuePair<string, object>(Messenger.EntityKey, e)
                            };

                            this.Send((int)EventId.EntityPicked, eventParams);
                        }
                    }));
            }
        }

        #endregion Commands

        public DataViewerViewModel(Entity type, object entity, bool showEntityChilds, bool editMode = false, bool pickerMode = false)
            : base(type, entity, showEntityChilds)
        {
            if (editMode)
            {
                IsEditModeActive = true;
            }

            if (pickerMode)
            {
                IsPickerModeActive = true;
            }

            pickup = new PickUperViewModel(OnEntityPicked);
            CreateBreadcrumbs();
        }

        public DataViewerViewModel()
            : base()
        {
        }

        /// <summary>
        /// Смена представления
        /// </summary>
        /// <param name="entity"></param>
        private void JumpTo(object entity)
        {
            if (EntityMapper.EntityOf(entity) != Entity.Empty)
            {
                ec.Current.Set(entity);

                var eventParams = new KeyValuePair<string, object>[]
                {
                    new KeyValuePair<string, object>(Messenger.EntityKey, entity),
                    new KeyValuePair<string, object>(Messenger.ShowChildsKey, true),
                    new KeyValuePair<string, object>(Messenger.EditModeKey, false)
                };

                this.Send((int)EventId.JumpingToEntity, eventParams);
            }
            else
            {
            }
        }

        private bool ProtectAdmin(object entity)
        {
            return EntityMapper.EntityOf(entity) == Entity.User &&
                   UserController.Instance.IsAdmin((User)entity);
        }

        private void DeleteEntity(object entity)
        {
            if (ProtectAdmin(entity))
                return;

            ec.Delete(entity);

            // переходим к соседней сущности
            if (CurrentIndex < TotalCount - 1)
            {
                CurrentIndex++;
            }
            else if (CurrentIndex > 0)
            {
                CurrentIndex--;
            }

            var eventParams = new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string, object>(Messenger.EntityKey, CurrentItem),
                new KeyValuePair<string, object>(Messenger.ShowChildsKey, false),
                new KeyValuePair<string, object>(Messenger.EditModeKey, IsEditModeActive)
            };

            this.Send((int)EventId.EntityDeleted, eventParams);
        }

        private void CreateEntity(Entity type)
        {
            var entity = ec.Create(type);
            Items.Add(entity);
            OnPropertyChanged(() => TotalCount);

            CurrentNumber = TotalCount;
            IsEditModeActive = true;
        }

        private void SaveEntity(object entity)
        {
            if (ProtectAdmin(entity))
                return;
            if (!ec.Exists(entity))
            {
                ec.Insert(entity);
            }
            else
            {
                ec.Update(entity);
            }
        }

        private void SaveAllEntities()
        {
            foreach (var entity in Items)
            {
                SaveEntity(entity);
            }
        }

        private void OnEntityPicked(EventMessage e)
        {
            var pickedEntity = e.GetValue<object>(Messenger.EntityKey);
            ec.SetChildEntity(CurrentItem, pickedEntity);
        }

        private void CreateBreadcrumbs()
        {
            Breadcrumbs = new List<object>();

            if (Entity == Entity.Flat)
            {
                Breadcrumbs.Add(ec.Current.Flat);
                Breadcrumbs.Add(ec.Current.House);
                Breadcrumbs.Add(ec.Current.Street);
                Breadcrumbs.Add(ec.Current.Settlement);
            }
            else if (Entity == Entity.House)
            {
                Breadcrumbs.Add(ec.Current.House);
                Breadcrumbs.Add(ec.Current.Street);
                Breadcrumbs.Add(ec.Current.Settlement);
            }
            else if (Entity == Entity.Street)
            {
                Breadcrumbs.Add(ec.Current.Street);
                Breadcrumbs.Add(ec.Current.Settlement);
            }
            else
                Breadcrumbs.Add(ec.Current[Entity]);

            Breadcrumbs.Reverse();
        }

        protected override IEnumerable<object> MakeItemsCollection()
        {
            string collection = null;
            if (IsListedChilds)
            {
                // show first child enities
                collection = EntityMapper.ChildrenCollectionName(Entity, 0);
            }
            else
            {
                collection = EntityMapper.CollectionName(Entity);
            }

            return ec.GetCollection(collection);
        }
    }
}