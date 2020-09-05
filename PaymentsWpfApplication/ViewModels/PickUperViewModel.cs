using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using EventAggregator;
using Payments.Model;
using System.Threading.Tasks;

namespace PaymentsWpfApplication.ViewModels
{
    public class PickUperViewModel : ViewModelBase
    {
        private ICommand _beginPickup;

        public ICommand BeginPickup
        {
            get
            {
                return _beginPickup ?? (_beginPickup = new RelayCommand<Entity>(
                    (typeToSet) =>
                    {
                        var eventParams = new KeyValuePair<string, object>[]
                            {
                                new KeyValuePair<string, object>(Messenger.TypeKey, typeToSet),
                                new KeyValuePair<string, object>(Messenger.PickModeKey, true)
                            };

                        EventMessageHandler handler = null;
                        handler = this.Subscribe((int)EventId.EntityPicked, (e) =>
                        {
                            OnEntityPicked(e);

                            handler.Dispose();
                        });

                        this.Send((int)EventId.EntityPicking, eventParams);
                    }));
            }
        }

        public Action<EventMessage> OnEntityPicked
        {
            get;
            set;
        }

        public PickUperViewModel(Action<EventMessage> onentitypicked)
        {
            OnEntityPicked = onentitypicked;
        }
    }
}
