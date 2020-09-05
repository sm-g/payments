using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Model
{
    /// <summary>
    /// Ключи для передачи параметров в сообщениях и в страничной навигации.
    /// </summary>
    public class Messenger
    {
        public const string EntityKey = "pickedEntity";
        public const string SenderKey = "sender";
        public const string TypeKey = "type";
        public const string ShowChildsKey = "showChilds";
        public const string EditModeKey = "editMode";
        public const string PickModeKey = "pickMode";
        public const string ErrorKey = "error";
    }
}
