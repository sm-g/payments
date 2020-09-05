using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections;

namespace PaymentsWpfApplication.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        public ViewModelBase()
        {

        }

        #region  INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExpr = propertyExpression.Body as MemberExpression;
            if (memberExpr == null)
            {
                throw new ArgumentException("The expression is not a member access expression.", "propertyExpression");
            }
            string memberName = memberExpr.Member.Name;
            OnPropertyChanged(memberName);
        }

        #endregion


        /// <summary>
        /// Adds the specified error to the errors collection if it is not 
        /// already present, inserting it in the first position if isWarning is 
        /// false.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="error"></param>
        /// <param name="isWarning"></param>
        protected void AddError(string propertyName, string error, bool isWarning = false)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            if (!errors[propertyName].Contains(error))
            {
                if (isWarning)
                {
                    errors[propertyName].Add(error);
                }
                else
                {
                    errors[propertyName].Insert(0, error);
                }
                RaiseErrorsChanged(propertyName);
            }
        }

        /// <summary>
        /// Removes the specified error from the errors collection if it is present. 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="error"></param>
        protected void RemoveError(string propertyName, string error)
        {
            if (errors.ContainsKey(propertyName) &&
                errors[propertyName].Contains(error))
            {
                errors[propertyName].Remove(error);
                if (errors[propertyName].Count == 0)
                {
                    errors.Remove(propertyName);
                }
                RaiseErrorsChanged(propertyName);
            }
        }

        /// <summary>
        /// Removes all errors from the errors collection if it is present. 
        /// </summary>
        protected void RemoveAllErrors(string propertyName)
        {
            if (errors.ContainsKey(propertyName))
            {
                errors.Remove(propertyName);

                RaiseErrorsChanged(propertyName);
            }
        }

        void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        #region INotifyDataErrorInfo Members

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        public IEnumerable GetErrors(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName) || !errors.ContainsKey(propertyName))
            {
                return null;
            }
            return errors[propertyName];
        }

        public bool HasErrors
        {
            get { return errors.Count > 0; }
        }

        #endregion
    }
}