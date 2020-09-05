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
    public class DataNavigatorViewModel : ViewModelBase
    {
        ICommand _moveNext;
        ICommand _movePrev;
        ICommand _moveFirst;
        ICommand _moveLast;

        ICollectionView View;
        public ObservableCollection<object> Items;

        public ICommand MoveNext
        {
            get
            {
                return _moveNext ?? (_moveNext = new RelayCommand(
                    () => View.MoveCurrentToNext(),
                    () => View.CurrentPosition < Items.Count-1
                    ));
            }
        }
        public ICommand MovePrev
        {
            get
            {
                return _movePrev ?? (_movePrev = new RelayCommand(
                    () => View.MoveCurrentToPrevious(),
                    () => View.CurrentPosition > 0));
            }
        }
        public ICommand MoveFisrt
        {
            get
            {
                return _moveFirst ?? (_moveFirst = new RelayCommand(
                    () => View.MoveCurrentToFirst(),
                    () => View.CurrentPosition != 1));
            }
        }
        public ICommand MoveLast
        {
            get
            {
                return _moveLast ?? (_moveLast = new RelayCommand(
                    () => View.MoveCurrentToLast(),
                    () => View.CurrentPosition != Items.Count));
            }
        }

        public DataNavigatorViewModel(ObservableCollection<object> items)
        {
            Items = items;
            View = CollectionViewSource.GetDefaultView(Items);
        }
    }
}
