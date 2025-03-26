using System.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.Model
{
    internal class OrderSummaryModel
    {
        private readonly string _connectionString;

        public OrderSummaryModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddOrderSummary(float subtotal, float warrantyTax, float deliveryFee, float finalTotal,
                                    string fullName, string email, string phoneNumber, string address,
                                    string postalCode, string additionalInfo, string contractDetails)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddOrderSummary", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Subtotal", subtotal);
                    cmd.Parameters.AddWithValue("@WarrantyTax", warrantyTax);
                    cmd.Parameters.AddWithValue("@DeliveryFee", deliveryFee);
                    cmd.Parameters.AddWithValue("@FinalTotal", finalTotal);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@PostalCode", postalCode);
                    cmd.Parameters.AddWithValue("@AdditionalInfo", additionalInfo);
                    cmd.Parameters.AddWithValue("@ContractDetails", contractDetails ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateOrderSummary(int id, float subtotal, float warrantyTax, float deliveryFee, float finalTotal,
                                       string fullName, string email, string phoneNumber, string address,
                                       string postalCode, string additionalInfo, string contractDetails)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateOrderSummary", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Subtotal", subtotal);
                    cmd.Parameters.AddWithValue("@WarrantyTax", warrantyTax);
                    cmd.Parameters.AddWithValue("@DeliveryFee", deliveryFee);
                    cmd.Parameters.AddWithValue("@FinalTotal", finalTotal);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@PostalCode", postalCode);
                    cmd.Parameters.AddWithValue("@AdditionalInfo", additionalInfo);
                    cmd.Parameters.AddWithValue("@ContractDetails", contractDetails ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOrderSummary(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteOrderSummary", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
