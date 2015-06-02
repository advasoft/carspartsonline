
namespace StoreAppTest.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Services.Client;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Controls;
    using GalaSoft.MvvmLight.Threading;
    using Model;
    using MSPToolkit.Encodings;
    using Newtonsoft.Json;
    using StoreAppDataService;
    using Utilities;

    public class AdminPanelViewModel : ViewModelBase
    {

        public AdminPanelViewModel()
        {
            InitCommands();
            UsersItems = new ObservableCollection<User>();
            WarehousesItems = new ObservableCollection<Warehouse>();
        }

        public bool IsUsersLoading
        {
            get { return _IsUsersLoading; }
            set
            {
                _IsUsersLoading = value;
                OnPropertyChanged("IsUsersLoading");
            }
        }
        private bool _IsUsersLoading;

        public bool IsWarehousesLoading
        {
            get { return _IsWarehousesLoading; }
            set
            {
                _IsWarehousesLoading = value;
                OnPropertyChanged("IsWarehousesLoading");
            }
        }
        private bool _IsWarehousesLoading;

        public ObservableCollection<User> UsersItems { get; set; }

        public ObservableCollection<Warehouse> WarehousesItems { get; set; }

        public User SelectedUser
        {
            get { return _SelectedUser; }
            set
            {
                _SelectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }
        private User _SelectedUser;

        public Warehouse SelectedWarehouse
        {
            get { return _SelectedWarehouse; }
            set
            {
                _SelectedWarehouse = value;
                OnPropertyChanged("SelectedWarehouse");
            }
        }
        private Warehouse _SelectedWarehouse;


        
        #region Overrides of ViewModelBase

        protected override void LoadViewHandler()
        {
            IsUsersLoading = true;
            IsWarehousesLoading = true;

            string uri = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");

            WarehousesItems.Clear();
            UsersItems.Clear();
            


            Task.Factory.StartNew(() =>
            {
                try
                {

                    StoreDbContext ctx = new StoreDbContext(
                        new Uri(uri
                            , UriKind.Absolute));


                    var warehouses = ctx.ExecuteSyncronous(ctx.Warehouses).ToList();
                    foreach (var warehouse in warehouses)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            WarehousesItems.Add(warehouse);
                        });
                    }
                }
                catch (Exception exception)
                {
                    throw;
                }
            }).ContinueWith(c =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    IsWarehousesLoading = false;
                });
            });

            Task.Factory.StartNew(() =>
            {
                try
                {

                    StoreDbContext ctx = new StoreDbContext(
                        new Uri(uri
                            , UriKind.Absolute));

                    var users = ctx.ExecuteSyncronous(ctx.Users.Expand("Warehouse")).ToList();
                    foreach (var user in users)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            UsersItems.Add(user);
                        });
                    }
                }
                catch (Exception exception)
                {
                    throw;
                }
            }).ContinueWith(c =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    IsUsersLoading = false;
                });
            });
        }

        #endregion



        #region Commands

        public ICommand AddUserCommand { get; set; }
        public ICommand RemoveUserCommand { get; set; }
        public ICommand EditUserCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }



        public ICommand AddWarehouseCommand { get; set; }
        public ICommand RemoveWarehouseCommand { get; set; }
        public ICommand EditWarehouseCommand { get; set; }

        private void InitCommands()
        {

            #region AddUserCommand

            AddUserCommand = new UICommand(a =>
            {
                UserModel model = new UserModel();
                model.WarehouseList = WarehousesItems;

                UserEditControl ctrl = new UserEditControl();
                ctrl.DataContext = model;

                ctrl.Closed += (sender, args) =>
                {
                    if (ctrl.DialogResult == true)
                    {
                        ctrl.DataContext = null;
                        try
                        {

                            User newUser = new User();
                            newUser.DisplayName = model.DisplayName;
                            newUser.UserName = model.UserName;
                            newUser.Warehouse_Id = model.Warehouse_Id;

                            var ctx = GetContext();
                            ctx.AddToUsers(newUser);
                            ctx.SaveChangesSynchronous();

                            UsersItems.Add(newUser);
                        }
                        catch (Exception exception)
                        {
                            MessageChildWindow ms = new MessageChildWindow();
                            ms.Title = "Ошибка";
                            ms.Message = exception.Message;

                            ms.Show();
                        }

                    }
                };

                ctrl.Show();
            });

            #endregion

            #region RemoveUserCommand

            RemoveUserCommand = new UICommand(a =>
            {
                if(SelectedUser == null) return;
                

                AskChildWindow ctrl = new AskChildWindow();
                ctrl.Title = "Удаление";
                ctrl.Message = "Вы уверены, что хотите удалить запись?";

                ctrl.Closed += (sender, args) =>
                {
                    if (ctrl.DialogResult == true)
                    {
                        try
                        {
                            var ctx = GetContext();
                            ctx.AttachTo("Users", SelectedUser);
                            ctx.ChangeState(SelectedUser, EntityStates.Deleted);
                            ctx.SaveChangesSynchronous();
                        }
                        catch (Exception exception)
                        {
                            MessageChildWindow ms = new MessageChildWindow();
                            ms.Title = "Ошибка";
                            ms.Message = exception.Message;

                            ms.Show();
                        }
                    }
                };

                ctrl.Show();
            });

            #endregion

            #region EditUserCommand

            EditUserCommand = new UICommand(a =>
            {
                if (SelectedUser == null) return;

                UserModel model = new UserModel();
                model.WarehouseList = WarehousesItems;
                model.DisplayName = SelectedUser.DisplayName;
                model.UserName = SelectedUser.UserName;
                model.Warehouse_Id = SelectedUser.Warehouse_Id;
                model.IsSupplierVisible = SelectedUser.IsSupplierVisible;


                UserEditControl ctrl = new UserEditControl();
                ctrl.LoginTextEdit.IsEnabled = false;

                ctrl.DataContext = model;

                ctrl.Closed += (sender, args) =>
                {
                    if (ctrl.DialogResult == true)
                    {
                        ctrl.DataContext = null;
                        try
                        {
                            SelectedUser.DisplayName = model.DisplayName;
                            SelectedUser.Warehouse_Id = model.Warehouse_Id;
                            SelectedUser.IsSupplierVisible = model.IsSupplierVisible;

                            var ctx = GetContext();
                            ctx.ChangeState(SelectedUser, EntityStates.Modified);
                            ctx.SaveChangesSynchronous();

                            if (App.CurrentUser.UserName == model.UserName)
                            {
                                App.CurrentUser.DisplayName = model.DisplayName;
                                App.CurrentUser.Warehouse_Id = model.Warehouse_Id;
                                App.CurrentUser.IsSupplierVisible = model.IsSupplierVisible;
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageChildWindow ms = new MessageChildWindow();
                            ms.Title = "Ошибка";
                            ms.Message = exception.Message;

                            ms.Show();
                        }

                    }
                };

                ctrl.Show();
            });

            #endregion

            #region ChangePasswordCommand

            ChangePasswordCommand = new UICommand(a =>
            {
                if (SelectedUser == null) return;

                UserChangePasswordControl ctrl = new UserChangePasswordControl();

                ctrl.Closed += (sender, args) =>
                {
                    if (ctrl.DialogResult == true)
                    {
                        try
                        {
                            Uri apiUri = new Uri(
                                string.Concat(App.AppBaseUrl,
                                string.Format("/api/PriceLists/SetPassword?userName={0}&passwordHash={1}", SelectedUser.UserName, ctrl.AdminPasswordTeBoxEdit.Password))
                                , UriKind.Absolute);

                            new Thread(() =>
                            {
                                try
                                {
                                    var request =
                                      WebRequest.Create(apiUri);
                                    request.Method = "POST";

                                    request.BeginGetResponse((r) =>
                                    {
                                         request.EndGetResponse(r);
                                    },

                                    request);

                                }
                                catch (Exception exception)
                                {
                                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                    {
                                        MessageChildWindow ms = new MessageChildWindow();
                                        ms.Title = "Ошибка";
                                        ms.Message = exception.Message;

                                        ms.Show();
                                    });
                                }


                            }).Start();
                        }
                        catch (Exception exception)
                        {
                            MessageChildWindow ms = new MessageChildWindow();
                            ms.Title = "Ошибка";
                            ms.Message = exception.Message;

                            ms.Show();
                        }

                    }
                };

                ctrl.Show();
            });

            #endregion


            #region AddWarehouseCommand

            AddWarehouseCommand = new UICommand(a =>
            {
                Warehouse model = new Warehouse();
                WarehouseEditControl ctrl = new WarehouseEditControl();
                ctrl.DataContext = model;

                ctrl.Closed += (sender, args) =>
                {
                    if (ctrl.DialogResult == true)
                    {
                        ctrl.DataContext = null;
                        try
                        {
                            var ctx = GetContext();
                            ctx.AddToWarehouses(model);
                            ctx.SaveChangesSynchronous();

                            WarehousesItems.Add(model);
                        }
                        catch (Exception exception)
                        {
                            MessageChildWindow ms = new MessageChildWindow();
                            ms.Title = "Ошибка";
                            ms.Message = exception.Message;

                            ms.Show();
                        }

                    }
                };

                ctrl.Show();
            });

            #endregion

            #region RemoveWarehouseCommand

            RemoveWarehouseCommand = new UICommand(a =>
            {
                if (SelectedWarehouse == null) return;


                AskChildWindow ctrl = new AskChildWindow();
                ctrl.Title = "Удаление";
                ctrl.Message = "Вы уверены, что хотите удалить запись?";

                ctrl.Closed += (sender, args) =>
                {
                    if (ctrl.DialogResult == true)
                    {
                        try
                        {
                            var ctx = GetContext();
                            ctx.AttachTo("Warehouses", SelectedWarehouse);
                            ctx.ChangeState(SelectedWarehouse, EntityStates.Deleted);
                            ctx.SaveChangesSynchronous();
                        }
                        catch (Exception exception)
                        {
                            MessageChildWindow ms = new MessageChildWindow();
                            ms.Title = "Ошибка";
                            ms.Message = exception.Message;

                            ms.Show();
                        }
                    }
                };

                ctrl.Show();
            });

            #endregion

            #region EditWarehouseCommand

            EditWarehouseCommand = new UICommand(a =>
            {

                if (SelectedWarehouse == null) return;

                WarehouseEditControl ctrl = new WarehouseEditControl();
                ctrl.DataContext = SelectedWarehouse;

                ctrl.Closed += (sender, args) =>
                {
                    if (ctrl.DialogResult == true)
                    {
                        ctrl.DataContext = null;
                        try
                        {
                            var ctx = GetContext();
                            ctx.ChangeState(SelectedWarehouse, EntityStates.Modified);
                            ctx.SaveChangesSynchronous();
                        }
                        catch (Exception exception)
                        {
                            MessageChildWindow ms = new MessageChildWindow();
                            ms.Title = "Ошибка";
                            ms.Message = exception.Message;

                            ms.Show();
                        }

                    }
                };

                ctrl.Show();
            });

            #endregion

        }

        #endregion



        private StoreDbContext GetContext()
        {
            string uri = string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/StoreAppDataService.svc/");
                StoreDbContext ctx = new StoreDbContext(
                    new Uri(uri
                        , UriKind.Absolute));
            return ctx;
        }
    }
}
