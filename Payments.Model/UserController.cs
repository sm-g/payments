using Payments.Data;
using System;
using System.Configuration;
using System.Linq;
using System.Security;

namespace Payments.Model
{
    public class UserController
    {
        public string CurrentUserName
        {
            get
            {
                if (CurrentUser != null)
                    return CurrentUser.Login;
                return "";
            }
        }

        public bool IsAdminLoggedIn
        {
            get { return CurrentUser != null && IsAdmin(CurrentUser); }
        }

        public User CurrentUser
        {
            get { return _curUser; }
            private set { _curUser = value; }
        }

        #region User creation

        /// <summary>
        /// Устанавливает новый пароль пользователю.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="user"></param>
        public static void SetNewPassword(SecureString password, User user)
        {
            if (!PasswordMaker.CheckPassword(password))
                return;
            user.Pass = PasswordMaker.MakeHash(password, user.Salt);
        }

        /// <summary>
        /// Проверяет, свободно ли заданное имя пользователя
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private bool IsUsernameAvailable(string username)
        {
            return !(from u in DB.Context.Users
                     where u.Login == username
                     select u).Any();
        }

        /// <summary>
        /// Создаёт пользователя
        /// </summary>
        /// <exception cref="UsernameCollisionException"/>
        /// <exception cref="PasswordStrengthException"/>
        private User CreateUser(string username, SecureString password)
        {
            if (!IsUsernameAvailable(username))
            {
                throw new UsernameCollisionException();
            }

            if (!PasswordMaker.CheckPassword(password))
            {
                throw new PasswordStrengthException();
            }

            var credits = PasswordMaker.MakeSaltedHash(password);

            return new User()
            {
                Login = username,
                Pass = credits.Item1,
                Salt = credits.Item2
            };
        }

        #endregion User creation

        /// <summary>
        /// Пытается авторизовать пользователя по имени и паролю.
        /// </summary>
        /// <exception cref="NoUsernameException" />
        /// <exception cref="FieldConstraintException" />
        public bool TryLogin(string username, SecureString password)
        {
            var user = (from u in DB.Context.Users
                        where u.Login == username
                        select u).SingleOrDefault();

            if (user == null)
            {
                throw new NoUsernameException();
            }

            // сравниваем хеш пароля в базе с вычисленным по паролю
            if (user.Pass == PasswordMaker.MakeHash(password, user.Salt))
            {
                CurrentUser = user;
                return true;
            }
            else
            {
                throw new FieldConstraintException();
            }
        }

#if DEBUG

        public void LoginAdmin()
        {
            CurrentUser = (from u in DB.Context.Users
                           where u.Login == "admin"
                           select u).SingleOrDefault();
        }

#endif

        public void LogOut()
        {
            CurrentUser = null;
        }

        public int GetRegularUserGroup()
        {
            return int.Parse(ConfigurationManager.AppSettings["regularGroupId"]);
        }

        public bool IsAdmin(User user)
        {
            return user.UserGroupID == int.Parse(ConfigurationManager.AppSettings["adminGroupId"]);
        }

        #region Rights cheking

        public bool CanRetrieve(object entity)
        {
            return (GetRights(entity) & CrudRights.Retrieve) == CrudRights.Retrieve;
        }

        public bool CanUpdate(object entity)
        {
            return (GetRights(entity) & CrudRights.Update) == CrudRights.Update;
        }

        public bool CanCreate(Entity type)
        {
            if (CurrentUser == null)
                return false;
            return IsAdmin(CurrentUser) || RightsController.UserCanCreate(type);
        }

        public bool CanDelete(object entity)
        {
            return (GetRights(entity) & CrudRights.Delete) == CrudRights.Delete;
        }

        public static bool OnlyAdmin(Entity type)
        {
            return type == Entity.User ||
                type == Entity.UserGroup;
        }

        /// <summary>
        /// Определяет права авторизованного пользователя на сущность.
        /// </summary>
        /// <param name="table"></param>
        /// <returns>Права текущего пользователя на сущность</returns>
        private CrudRights GetRights(object entity)
        {
            if (CurrentUser != null && entity != null)
            {
                var type = EntityMapper.EntityOf(entity);
                if (IsAdmin(CurrentUser))
                {
                    return CrudRights.All;
                }
                else if (!OnlyAdmin(type) &&
                    (!RightsController.AssociatedWithUser(type) ||
                     RightsController.IsAutorizedRetrievingAllowed(entity, CurrentUser)))
                {
                    // можно просматривать все сущности, которые не содержат данных о плательщике
                    CrudRights rights = CrudRights.Retrieve;

                    if (RightsController.IsAutorizedUpdatingAllowed(entity, CurrentUser))
                    {
                        rights = rights | CrudRights.Update;
                    }
                    if (RightsController.UserCanCreate(type))
                    {
                        rights = rights | CrudRights.Create;
                    }
                    if (RightsController.IsAutorizedDeletingAllowed(entity, CurrentUser))
                    {
                        rights = rights | CrudRights.Delete;
                    }
                    return rights;
                }
            }
            // нет пользователя или сущность может смотреть только администратор
            return CrudRights.None;
        }

        #endregion Rights cheking

        #region Singleton implementation

        private static readonly Lazy<UserController> lazyInstance = new Lazy<UserController>(() => new UserController());
        private User _curUser;

        public static UserController Instance
        {
            get
            {
                return lazyInstance.Value;
            }
        }

        #endregion Singleton implementation
    }
}