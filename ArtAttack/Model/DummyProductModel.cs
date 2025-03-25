﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.Model
{
    public class DummyProductModel
    {
        private readonly string _connectionString;

        public DummyProductModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddDummyProduct(string name, float price, int sellerId, string productType, DateTime startDate, DateTime endDate)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddDummyProduct", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@SellerID", sellerId);
                    cmd.Parameters.AddWithValue("@ProductType", productType);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Updates an existing DummyProduct record.
        /// </summary>
        public void UpdateDummyProduct(int id, string name, float price, int sellerId, string productType, DateTime startDate, DateTime endDate)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateDummyProduct", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@SellerID", sellerId);
                    cmd.Parameters.AddWithValue("@ProductType", productType);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Deletes a DummyProduct record by ID.
        /// </summary>
        public void DeleteDummyProduct(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteDummyProduct", conn))
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

