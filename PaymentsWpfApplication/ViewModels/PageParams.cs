using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payments.Model;

namespace PaymentsWpfApplication.ViewModels
{
    /// <summary>
    /// Параметры для перехода на страницу.
    /// </summary>
    public class PageParams
    {
        /// <summary>
        /// Uri представления.
        /// </summary>
        public string PageUri { get; set; }

        /// <summary>
        /// Тип сущности. 0, если страница не связана с сущностью.
        /// </summary>
        public Entity Type { get; set; }

        /// <summary>
        /// Cущность для показа.
        /// </summary>
        public object Entity { get; set; }

        /// <summary>
        /// Показывать ли подчиненные сущности.
        /// </summary>
        public bool ShowChilds { get; set; }

        /// <summary>
        /// Включать ли режим редактирования.
        /// </summary>
        public bool EditMode { get; set; }

        /// <summary>
        /// Включать ли режим выбора.
        /// </summary>
        public bool PickerMode { get; set; }

        public object ExtraData { get; set; }

        public PageParams()
        { }

        PageParams(string uri, object entity, bool showChilds, bool editMode, bool pickerMode, Entity type)
        {
            PageUri = uri;
            Entity = entity;
            ShowChilds = showChilds;
            EditMode = editMode;
            PickerMode = pickerMode;
            Type = type;
        }

        public PageParams(string uri, object entity, bool showChilds, bool editMode, object extra = null)
        {
            PageUri = uri;
            Entity = entity;
            ShowChilds = showChilds;
            EditMode = editMode;
            ExtraData = extra;

            Type = EntityMapper.EntityOf(entity);
        }

        public PageParams(string uri, Entity type, bool pickerMode)
        {
            PageUri = uri;
            Type = type;
            PickerMode = pickerMode;
        }

        public PageParams(string uri, Entity type)
        {
            PageUri = uri;
            Type = type;
        }

        public PageParams(string uri)
        {
            PageUri = uri;
        }
    }
}
