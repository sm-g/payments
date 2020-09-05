using EventAggregator;
using Payments.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Web;
using System.Collections.Generic;

namespace PaymentsWpfApplication.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _title;
        private string _error;
        private bool _isMenuVisible;
        private bool _isMsgVisible;
        private ICommand _navigateToPageCommand;
        private ICommand _navigateToPageWithContextCommand;
        private ICommand _logOut;
        private ICommand _closeMeassage;
        private NavigationService navigationService;
        private const string DataScreenUri = "/Screens/DataViewerScreen.xaml";

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(() => Title);
                }
            }
        }

        public string Error
        {
            get { return _error; }
            set
            {
                if (_error != value)
                {
                    _error = value;
                    OnPropertyChanged(() => Error);
                }
            }
        }

        public string Username
        {
            get { return UserController.Instance.CurrentUserName; }
            set { } // this is datacontext for Login for short time
        }

        public bool IsMenuVisible
        {
            get { return _isMenuVisible; }
            set
            {
                if (_isMenuVisible != value)
                {
                    _isMenuVisible = value;
                    OnPropertyChanged(() => IsMenuVisible);
                }
            }
        }

        public bool IsMsgBoxVisible
        {
            get { return _isMsgVisible; }
            set
            {
                if (_isMsgVisible != value)
                {
                    _isMsgVisible = value;
                    OnPropertyChanged(() => IsMsgBoxVisible);
                }
            }
        }

        public MainWindowViewModel(NavigationService navService)
        {
            navigationService = navService;

            IsMenuVisible = false;
            Title = "Учёт квартплаты";

            SubscribeToEvents();

#if DEBUG
            UserController.Instance.LoginAdmin();
            IsMenuVisible = true;
#endif
        }

        public MainWindowViewModel()
            : this(null)
        {
        }

        private void SubscribeToEvents()
        {
            navigationService.LoadCompleted += _nav_LoadCompleted;

            Action<EventMessage> onNavFull = (e) =>
            {
                NavigateToPage(new PageParams(DataScreenUri,
                    e.GetValue<object>(Messenger.EntityKey),
                    e.GetValue<bool>(Messenger.ShowChildsKey),
                    e.GetValue<bool>(Messenger.EditModeKey)));
            };

            this.Subscribe((int)EventId.JumpingToEntity, onNavFull);
            this.Subscribe((int)EventId.EntityDeleted, onNavFull);
            this.Subscribe((int)EventId.EntityCreated, onNavFull);
            this.Subscribe((int)EventId.EntityPicking, (e) =>
            {
                NavigateToPage(new PageParams(DataScreenUri,
                    e.GetValue<Entity>(Messenger.TypeKey),
                    e.GetValue<bool>(Messenger.PickModeKey)));
            });
            this.Subscribe((int)EventId.EntityPicked, (e) =>
            {
                navigationService.GoBack();
            });

            this.Subscribe((int)EventId.ErrorOccured, (e) =>
            {
                Error = e.GetValue<string>(Messenger.ErrorKey);
                IsMsgBoxVisible = true;
            });

            this.Subscribe((int)EventId.UserLoggedIn, (e) =>
            {
                OnPropertyChanged(() => Username);
                IsMenuVisible = true;
                NavigateToPage(new PageParams(DataScreenUri, Entity.Settlement));
            });
        }

        /// <summary>
        /// Устанавливает DataContext для загруженной страницы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _nav_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var page = e.Content as FrameworkElement;

            if (page != null && page.DataContext == null)
            {
                // разбор параметров uri для передачи в конструктор DataViewerViewModel
                string typeParam = null;
                string childsParam = null;
                string editParam = null;
                string pickParam = null;

                foreach (var item in e.Uri.OriginalString.Split('?', '&'))
                {
                    var keyValuePair = item.Split('=');
                    if (keyValuePair[0] == Messenger.TypeKey)
                        typeParam = keyValuePair[1];
                    if (keyValuePair[0] == Messenger.ShowChildsKey)
                        childsParam = keyValuePair[1];
                    if (keyValuePair[0] == Messenger.EditModeKey)
                        editParam = keyValuePair[1];
                    if (keyValuePair[0] == Messenger.PickModeKey)
                        pickParam = keyValuePair[1];
                }

                if (typeParam != null)
                {
                    object entity = e.ExtraData;

                    var entityType = (Entity)Enum.Parse(typeof(Entity), typeParam, true);
                    bool showChilds = false;
                    bool editMode = false;
                    bool pickMode = false;

                    if (childsParam != null)
                    {
                        showChilds = Convert.ToBoolean(childsParam);
                    }
                    if (editParam != null)
                    {
                        editMode = Convert.ToBoolean(editParam);
                    }
                    if (editParam != null)
                    {
                        pickMode = Convert.ToBoolean(pickParam);
                    }

                    page.DataContext = new DataViewerViewModel(entityType, entity, showChilds, editMode, pickMode);
                }
                else
                {
                    // страницы, не связанные с сущностью
                }
            }

        }

        public ICommand CloseMessage
        {
            get
            {
                return _closeMeassage ?? (_closeMeassage = new RelayCommand(
                        () => IsMsgBoxVisible = false));
            }
        }

        #region Page Navigation

        public ICommand Navigate
        {
            get
            {
                return _navigateToPageCommand ?? (_navigateToPageCommand = new RelayCommand<string>(
                        p => NavigateToPage(p)));
            }
        }

        public ICommand LogOut
        {
            get
            {
                return _logOut ?? (_logOut = new RelayCommand<string>(
                        (p) =>
                        {
                            UserController.Instance.LogOut();
                            this.Send((int)EventId.UserLoggedOut);

                            IsMenuVisible = false;
                            NavigateClear(p);

                            navigationService.RemoveBackEntry();
                        }));
            }
        }

        public ICommand NavigateWithParams
        {
            get
            {
                return _navigateToPageWithContextCommand ?? (_navigateToPageWithContextCommand = new RelayCommand<PageParams>(
                       pp => NavigateToPage(pp),
                       pp => UserController.Instance.IsAdminLoggedIn || !UserController.OnlyAdmin(pp.Type)));
            }
        }

        /// <summary>
        /// Переходит к странице, удаляя историю навигации.
        /// </summary>
        void NavigateClear(String uri)
        {
            if (!navigationService.CanGoBack && !navigationService.CanGoForward)
            {
                return;
            }

            var entry = navigationService.RemoveBackEntry();
            while (entry != null)
            {
                entry = navigationService.RemoveBackEntry();
            }

            NavigateToPage(uri);

        }

        /// <summary>
        /// Переход к странице по uri.
        /// </summary>
        /// <param name="uri"></param>
        private void NavigateToPage(String uri)
        {
            NavigateToPage(new PageParams(uri));
        }

        /// <summary>
        /// Переход к странице с указанием параметров.
        /// </summary>
        /// <param name="pp"></param>
        private void NavigateToPage(PageParams pp)
        {
            if (pp.Type == Entity.Empty)
            {
                // страница не связана с сущностями
                navigationService.Navigate(new Uri("pack://application:,,," + pp.PageUri, UriKind.Absolute));
                return;
            }

            var uri = new Uri(
                string.Format("pack://application:,,,{0}?{1}={2}&{3}={4}&{5}={6}&{7}={8}", pp.PageUri,
                    Messenger.TypeKey, pp.Type,
                    Messenger.ShowChildsKey, pp.ShowChilds,
                    Messenger.EditModeKey, pp.EditMode,
                    Messenger.PickModeKey, pp.PickerMode),
                UriKind.Absolute);
            if (navigationService.CurrentSource != null && '/' + navigationService.CurrentSource.OriginalString == uri.PathAndQuery)
            {
                // когда нужна перезгрузка страницы, добавляем параметр к uri 
                navigationService.Navigate(new Uri(uri + "&re"), pp.Entity);
            }
            else
            {
                navigationService.Navigate(uri, pp.Entity);
            }

        }

        #endregion Page Navigation

    }
}