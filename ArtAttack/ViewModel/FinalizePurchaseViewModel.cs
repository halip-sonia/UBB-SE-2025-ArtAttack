﻿using ArtAttack.Domain;
using ArtAttack.Model;
using ArtAttack.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.ViewModel
{
    public class FinalizePurchaseViewModel : IFinalizePurchaseViewModel, INotifyPropertyChanged
    {
        private readonly OrderHistoryModel orderHistoryModel;
        private readonly OrderSummaryModel orderSummaryModel;
        private readonly OrderModel orderModel;

        private int orderHistoryID;

        private float _subtotal;
        private float _deliveryFee;
        private float _total;

        private string _fullname;
        private string _phone;
        private string _email;
        private string _paymentMethod;
        private string _orderStatus;



        public ObservableCollection<DummyProduct> ProductList { get; set; }
        public List<DummyProduct> dummyProducts;

        public FinalizePurchaseViewModel(int orderHistoryID)
        {
            orderHistoryModel = new OrderHistoryModel(Configuration._CONNECTION_STRING_);
            orderModel = new OrderModel(Configuration._CONNECTION_STRING_);
            orderSummaryModel = new OrderSummaryModel(Configuration._CONNECTION_STRING_);

            this.orderHistoryID = orderHistoryID;

            _ = InitializeViewModelAsync();

        }

        public async Task InitializeViewModelAsync()
        {
            dummyProducts = await GetDummyProductsFromOrderHistoryAsync(orderHistoryID);
            ProductList = new ObservableCollection<DummyProduct>(dummyProducts);

            OnPropertyChanged(nameof(ProductList));

            OrderSummary orderSummary = await orderSummaryModel.GetOrderSummaryByIDAsync(orderHistoryID);



            await SetOrderHistoryInfo(orderSummary);

        }

        public async Task SetOrderHistoryInfo(OrderSummary orderSummary)
        {
            List<Order> orders = await orderModel.GetOrdersFromOrderHistoryAsync(orderHistoryID);
            Subtotal = orderSummary.Subtotal;
            DeliveryFee = orderSummary.DeliveryFee;
            Total = orderSummary.FinalTotal;

            FullName = orderSummary.FullName;
            Email = orderSummary.Email;
            PhoneNumber = orderSummary.PhoneNumber;
            PaymentMethod = orders[0].PaymentMethod;
            OrderStatus = "Processing";

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<List<DummyProduct>> GetDummyProductsFromOrderHistoryAsync(int orderHistoryID)
        {
            return await orderHistoryModel.GetDummyProductsFromOrderHistoryAsync(orderHistoryID);
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

        public string FullName
        {
            get => _fullname;
            set { _fullname = value; OnPropertyChanged(nameof(FullName)); }
        }

        public string PhoneNumber
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(nameof(PhoneNumber)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string PaymentMethod
        {
            get => _paymentMethod;
            set { _paymentMethod = value; OnPropertyChanged(nameof(PaymentMethod)); }
        }

        public string OrderStatus
        {
            get => _orderStatus;
            set { _orderStatus = value; OnPropertyChanged(nameof(OrderStatus)); }
        }

    }
}
