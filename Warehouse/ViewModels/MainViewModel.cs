using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Warehouse.Commands;
using Warehouse.Core.Data;
using Warehouse.Core.Models;
using Warehouse.Core.Services.Interfaces;
using Warehouse.Views;

namespace Warehouse.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IOrganizationService _orgService;

        public ObservableCollection<Organization> Organizations { get; set; }

        private Organization _selectedOrganization;
        public Organization SelectedOrganization
        {
            get => _selectedOrganization;
            set { _selectedOrganization = value; OnPropertyChanged(); }
        }

        public ICommand OpenWarehouseCommand { get; }
        public ICommand AddOrgCommand { get; }
        public ICommand EditOrgCommand { get; }
        public ICommand DeleteOrgCommand { get; }

        public MainViewModel()
        {
            _orgService = DataStore.OrganizationService;
            Organizations = new ObservableCollection<Organization>(_orgService.GetAll());

            OpenWarehouseCommand = new RelayCommand(_ => OpenWarehouseManage(), _ => SelectedOrganization != null);
            AddOrgCommand = new RelayCommand(_ => OpenAddEditOrganization(null));
            EditOrgCommand = new RelayCommand(_ => OpenAddEditOrganization(SelectedOrganization), _ => SelectedOrganization != null);
            DeleteOrgCommand = new RelayCommand(_ => DeleteOrganization(), _ => SelectedOrganization != null);
        }

        private void OpenWarehouseManage()
        {
            var win = new WarehouseManageWindow(SelectedOrganization);
            win.Show();   // главное окно НЕ закрываем
        }

        private void OpenAddEditOrganization(Organization org)
        {
            var window = new AddEditOrganizationWindow(org, _orgService);
            if (window.ShowDialog() == true)
                RefreshOrganizations();
        }

        private void DeleteOrganization()
        {
            var win = new DeleteConfirmationWindow(SelectedOrganization.Name);
            if (win.ShowDialog() == true)
            {
                _orgService.Delete(SelectedOrganization.Id);
                RefreshOrganizations();
                SelectedOrganization = null;
            }
        }

        private void RefreshOrganizations()
        {
            Organizations.Clear();
            foreach (var org in _orgService.GetAll())
                Organizations.Add(org);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}