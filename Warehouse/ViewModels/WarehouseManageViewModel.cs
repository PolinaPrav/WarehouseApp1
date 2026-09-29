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
    public class WarehouseManageViewModel : INotifyPropertyChanged
    {
        private readonly IWarehouseService _whService;
        private readonly IProductService _prodService;
        private readonly IInvoiceService _invService;

        public Organization CurrentOrganization { get; }
        public ICommand ViewInvoiceCommand { get; }

        public ObservableCollection<Warehouse1> Warehouses { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Invoice> Invoices { get; set; }

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                LoadProducts();
            }
        }

        private Warehouse1 _selectedWarehouse;
        public Warehouse1 SelectedWarehouse
        {
            get => _selectedWarehouse;
            set
            {
                _selectedWarehouse = value;
                OnPropertyChanged();
                LoadProducts();
                LoadInvoices();
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        private Invoice _selectedInvoice;
        public Invoice SelectedInvoice
        {
            get => _selectedInvoice;
            set
            {
                _selectedInvoice = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddWarehouseCommand { get; }
        public ICommand EditWarehouseCommand { get; }
        public ICommand DeleteWarehouseCommand { get; }

        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public ICommand CreateIncomingInvoiceCommand { get; }
        public ICommand CreateOutgoingInvoiceCommand { get; }
        public ICommand DeleteInvoiceCommand { get; }

        public WarehouseManageViewModel(Organization org)
        {
            CurrentOrganization = org;

            _whService = DataStore.WarehouseService;
            _prodService = DataStore.ProductService;
            _invService = DataStore.InvoiceService;

            Warehouses = new ObservableCollection<Warehouse1>(
                _whService.GetByOrganizationId(org.Id));
            Products = new ObservableCollection<Product>();
            Invoices = new ObservableCollection<Invoice>();

            AddWarehouseCommand = new RelayCommand(_ => OpenAddEditWarehouse(null));
            EditWarehouseCommand = new RelayCommand(_ => OpenAddEditWarehouse(SelectedWarehouse),
                                                    _ => SelectedWarehouse != null);
            DeleteWarehouseCommand = new RelayCommand(_ => DeleteWarehouse(),
                                                      _ => SelectedWarehouse != null);

            AddProductCommand = new RelayCommand(_ => OpenAddEditProduct(null),
                                                 _ => SelectedWarehouse != null);
            EditProductCommand = new RelayCommand(_ => OpenAddEditProduct(SelectedProduct),
                                                  _ => SelectedProduct != null);
            DeleteProductCommand = new RelayCommand(_ => DeleteProduct(),
                                                    _ => SelectedProduct != null);

            CreateIncomingInvoiceCommand = new RelayCommand(_ => OpenInvoice(InvoiceType.Incoming),
                                                            _ => SelectedWarehouse != null);
            CreateOutgoingInvoiceCommand = new RelayCommand(_ => OpenInvoice(InvoiceType.Outgoing),
                                                            _ => SelectedWarehouse != null);
            DeleteInvoiceCommand = new RelayCommand(_ => DeleteInvoice(),
                                                    _ => SelectedInvoice != null);

            ViewInvoiceCommand = new RelayCommand(_ => ViewInvoice(),
                                      _ => SelectedInvoice != null);
        }

        private void ViewInvoice()
        {
            if (SelectedInvoice == null) return;
            var window = new ViewInvoiceWindow(SelectedInvoice);
            window.ShowDialog();
        }
        private void LoadProducts()
        {
            Products.Clear();
            if (SelectedWarehouse == null) return;

            var list = _prodService.GetByWarehouseId(SelectedWarehouse.Id);

            if (!string.IsNullOrWhiteSpace(SearchText))
                list = list.Where(p => p.Name.ToLower().Contains(SearchText.ToLower())).ToList();

            foreach (var p in list)
                Products.Add(p);
        }

        private void LoadInvoices()
        {
            Invoices.Clear();
            if (SelectedWarehouse == null) return;

            foreach (var i in _invService.GetInvoicesByWarehouse(SelectedWarehouse.Id))
                Invoices.Add(i);
        }

        private void OpenAddEditWarehouse(Warehouse1 wh)
        {
            var window = new AddEditWarehouseWindow(wh, _whService, CurrentOrganization.Id);
            if (window.ShowDialog() == true)
            {
                Warehouses.Clear();
                foreach (var w in _whService.GetByOrganizationId(CurrentOrganization.Id))
                    Warehouses.Add(w);
            }
        }

        private void DeleteWarehouse()
        {
            if (MessageBox.Show($"Удалить склад \"{SelectedWarehouse.Name}\"?", "Подтверждение",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _whService.Delete(SelectedWarehouse.Id);
                Warehouses.Remove(SelectedWarehouse);
                SelectedWarehouse = null;
            }
        }

        private void OpenAddEditProduct(Product prod)
        {
            var window = new AddEditProductWindow(prod, _prodService, SelectedWarehouse.Id);
            if (window.ShowDialog() == true)
                LoadProducts();
        }

        private void DeleteProduct()
        {
            if (MessageBox.Show($"Удалить товар \"{SelectedProduct.Name}\"?", "Подтверждение",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _prodService.Delete(SelectedProduct.Id);
                Products.Remove(SelectedProduct);
            }
        }

        private void OpenInvoice(InvoiceType type)
        {
            var products = _prodService.GetByWarehouseId(SelectedWarehouse.Id);
            if (products.Count == 0)
            {
                MessageBox.Show("Нет товаров на складе.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var window = new CreateInvoiceWindow(SelectedWarehouse.Id, type, products, _invService);
            if (window.ShowDialog() == true)
            {
                LoadProducts();
                LoadInvoices();
            }
        }

        private void DeleteInvoice()
        {
            if (MessageBox.Show($"Удалить накладную №{SelectedInvoice.Id}?", "Подтверждение",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _invService.Delete(SelectedInvoice.Id);
                Invoices.Remove(SelectedInvoice);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}