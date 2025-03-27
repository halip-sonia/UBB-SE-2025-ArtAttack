using ArtAttack.Domain;
using ArtAttack.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ArtAttack.Shared;

namespace ArtAttack.ViewModel
{
    public class BillingInfoModelView : IBillingInfoModelView, INotifyPropertyChanged
    {
        private readonly OrderHistoryModel orderHistoryModel;
        private readonly OrderSummaryModel orderSummaryModel;
        private readonly OrderModel orderModel;
        private int orderHistoryID;
        private string _fullName;
        private string _email;
        private string _phoneNumber;
        private string _address;
        private string _zipCode;
        private string _additionalInfo;
        private string _selectedPaymentMethod;

        private float _subtotal;
        private float _deliveryFee;
        private float _total;
        public ObservableCollection<DummyProduct> ProductList { get; set; }
        public List<DummyProduct> dummyProducts;
        public ICommand FinalizePurchaseCommand { get; }

        public BillingInfoModelView(int orderHistoryID)
        {
            orderHistoryModel = new OrderHistoryModel(Configuration._CONNECTION_STRING_);
            orderModel = new OrderModel(Configuration._CONNECTION_STRING_);
            orderSummaryModel = new OrderSummaryModel(Configuration._CONNECTION_STRING_);

            this.orderHistoryID = orderHistoryID;

            dummyProducts = GetDummyProductsFromOrderHistory(orderHistoryID);
            ProductList = new ObservableCollection<DummyProduct>(dummyProducts);

            CalculateOrderTotal(orderHistoryID);

            FinalizePurchaseCommand = new RelayCommand(onFinalizeButtonClicked);
        }

        private void onFinalizeButtonClicked()
        {
            // Currently, an order summary has the same ID as the order history for simplicity
            orderSummaryModel.UpdateOrderSummary(orderHistoryID, Subtotal, 0, DeliveryFee, Total, FullName, Email, PhoneNumber, Address, ZipCode, AdditionalInfo, null);
        }

        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(nameof(FullName)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(nameof(PhoneNumber)); }
        }
        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(nameof(Address)); }
        }

        public string ZipCode
        {
            get => _zipCode;
            set { _zipCode = value; OnPropertyChanged(nameof(ZipCode)); }
        }

        public string AdditionalInfo
        {
            get => _additionalInfo;
            set { _additionalInfo = value; OnPropertyChanged(nameof(AdditionalInfo)); }
        }

        public string SelectedPaymentMethod
        {
            get => _selectedPaymentMethod;
            set { _selectedPaymentMethod = value; OnPropertyChanged(nameof(SelectedPaymentMethod)); }
        }

        public float Subtotal
        {
            get => _subtotal;
            set { _subtotal = value; OnPropertyChanged(nameof(Subtotal)); }
        }

        public float DeliveryFee
        {
            get => _deliveryFee;
            set { _deliveryFee = value; OnPropertyChanged(nameof(DeliveryFee)); }
        }

        public float Total
        {
            get => _total;
            set { _total = value; OnPropertyChanged(nameof(Total)); }
        }

       

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ApplyBorrowedTax(int productID, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public void CalculateOrderTotal(int orderHistoryID)
        {
            float subtotalProducts = 0;
            foreach(var product in dummyProducts)
                subtotalProducts += product.Price;

            Subtotal = subtotalProducts;
            if (subtotalProducts < 200)
            {
                Total = subtotalProducts + 13.99f;
                DeliveryFee = 13.99f;
            }
            else
                Total = subtotalProducts;
        }

        public List<DummyProduct> GetDummyProductsFromOrderHistory(int orderHistoryID)
        {
            return orderHistoryModel.GetDummyProductsFromOrderHistory(orderHistoryID);
        }

    }
}
