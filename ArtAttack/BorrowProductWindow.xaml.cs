using ArtAttack.Model;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Windows;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ArtAttack
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BorrowProductWindow : Window
    {
        private readonly string _connectionString;
        private readonly int _currentUserId;
        private readonly int _currentProductId;
        private DateTime? _productEndDate;

        /*public BorrowProductWindow(string connectionString, int userId, int productId)
        {
            this.InitializeComponent();
            _connectionString = connectionString;
            _currentUserId = userId;
            _currentProductId = productId;

            LoadProductDetails();
            CheckWaitListStatus();
        }*/

        public BorrowProductWindow(string connectionString, int userId, int productId)
        {
            this.InitializeComponent();
            _connectionString = connectionString;
            _currentUserId = userId;
            _currentProductId = productId;

            // Subscribe to window loaded event
            this.Activated += BorrowProductWindow_Activated;
        }

        private async void BorrowProductWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            // Unsubscribe immediately so it only runs once
            this.Activated -= BorrowProductWindow_Activated;

            try
            {
                await LoadProductDetails();
                await CheckWaitListStatus();
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Initialization Error", ex.Message);
            }
        }

        private async Task LoadProductDetails()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = @"SELECT p.ID, p.name, p.price, s.name as sellerName, 
                                    p.startDate, p.endDate, p.productType
                                    FROM DummyProduct p
                                    JOIN DummySeller s ON p.SellerID = s.ID
                                    WHERE p.ID = @ProductId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProductId", _currentProductId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Display product details
                        txtProductName.Text = reader["name"].ToString();
                        txtPrice.Text = $"Price: {reader["price"]}";
                        txtSeller.Text = $"Seller: {reader["sellerName"]}";
                        txtType.Text = $"Type: {reader["productType"]}";

                        DateTime? startDate = reader["startDate"] as DateTime?;
                        _productEndDate = reader["endDate"] as DateTime?;

                        txtDates.Text = $"Available: {startDate?.ToShortDateString() ?? "Now"} - " +
                                      $"{(_productEndDate.HasValue ? _productEndDate.Value.ToShortDateString() : "Indefinitely")}";

                        // Set button visibility based on availability
                        bool isAvailable = !_productEndDate.HasValue;
                        btnBorrow.Visibility = isAvailable ? Visibility.Visible : Visibility.Collapsed;
                        btnJoinWaitList.Visibility = !isAvailable ? Visibility.Visible : Visibility.Collapsed;
                        if (!isAvailable)
                        {
                            txtWaitListInfo.Text = $"This product will be available again on {_productEndDate.Value.ToShortDateString()}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Load Error", ex.Message);
            }
        }

        private async Task CheckWaitListStatus()
        {
            try
            {
                WaitListModel waitListModel = new WaitListModel(_connectionString);
                bool isInWaitList = waitListModel.IsUserInWaitlist(_currentUserId, _currentProductId);

                if (isInWaitList)
                {
                    btnJoinWaitList.Content = "Leave WaitList";
                    int position = GetUserPositionInWaitList(_currentUserId, _currentProductId);
                    txtWaitListInfo.Text = $"You are #{position} in the waitlist for this product.";
                }
                else
                {
                    btnJoinWaitList.Content = "Join WaitList";
                }
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Error", $"Error checking waitlist status: {ex.Message}");
            }
        }

        private int GetUserPositionInWaitList(int userId, int productId)
        {
            WaitListModel waitListModel = new WaitListModel(_connectionString);
            var userWaitlists = waitListModel.GetUserWaitlists(userId);

            foreach (var waitlist in userWaitlists)
            {
                if (waitlist.productWaitListID == productId)
                {
                    return waitlist.positionInQueue;
                }
            }
            return -1;
        }

        private async void btnBorrow_Click(object sender, RoutedEventArgs e)
        {
            await ShowMessageAsync("Borrow", "Borrow functionality will be implemented here");
        }

        private async void btnJoinWaitList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WaitListModel waitListModel = new WaitListModel(_connectionString);

                if (btnJoinWaitList.Content.ToString() == "Join WaitList")
                {
                    waitListModel.AddUserToWaitlist(_currentUserId, _currentProductId);
                    await ShowMessageAsync("Success", "You have been added to the waitlist!");
                }
                else
                {
                    waitListModel.RemoveUserFromWaitlist(_currentUserId, _currentProductId);
                    await ShowMessageAsync("Success", "You have been removed from the waitlist.");
                }

                CheckWaitListStatus();
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Error", $"Error updating waitlist: {ex.Message}");
            }
        }

        private async void btnViewWaitList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WaitListModel waitListModel = new WaitListModel(_connectionString);
                var waitlistUsers = waitListModel.GetUsersInWaitlist(_currentProductId);

                string waitlistInfo = "Current Waitlist:\n\n";
                foreach (var user in waitlistUsers)
                {
                    waitlistInfo += $"#{user.positionInQueue} - User ID: {user.userID} (Joined: {user.joinedTime.ToShortDateString()})\n";
                }

                await ShowMessageAsync("Waitlist Details", waitlistInfo);
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Error", $"Error loading waitlist: {ex.Message}");
            }
        }

        /*private async Task ShowMessageAsync(string title, string message)
        {
            // Wait until window is loaded if needed
            if (this.Content.XamlRoot == null)
            {
                await Task.Delay(100); // Small delay
            }

            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot // Now should be available
            };

            await dialog.ShowAsync();
        }
        */

        private async Task ShowMessageAsync(string title, string message)
        {
            if (DispatcherQueue.HasThreadAccess)
            {
                await ShowDialog(title, message);
            }
            else
            {
                // Create a TaskCompletionSource to await the UI thread operation
                var tcs = new TaskCompletionSource<bool>();

                DispatcherQueue.TryEnqueue(() =>
                {
                    try
                    {
                        var dialog = new ContentDialog
                        {
                            Title = title,
                            Content = message,
                            CloseButtonText = "OK",
                            XamlRoot = this.Content.XamlRoot
                        };
                        // Using ContinueWith to properly handle the dialog show operation
                        dialog.ShowAsync().AsTask().ContinueWith(t =>
                        {
                            if (t.IsFaulted)
                                tcs.TrySetException(t.Exception);
                            else
                                tcs.TrySetResult(true);
                        });
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                });

                await tcs.Task;
            }
        }

        private async Task ShowDialog(string title, string message)
        {
            try
            {
                var dialog = new ContentDialog
                {
                    Title = title,
                    Content = message,
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };

                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                // Fallback to simple message if dialog fails
                System.Diagnostics.Debug.WriteLine($"Failed to show dialog: {ex.Message}");
            }

        }
    }
}