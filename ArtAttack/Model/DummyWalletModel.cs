﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAttack.Domain;
using Microsoft.Data.SqlClient;

namespace ArtAttack.Model
{
    public class DummyWalletModel
    {
        private readonly string _connectionString;

        public DummyWalletModel(string connstring)
        {
            _connectionString = connstring;
        }

        public async Task AddWallet(DummyWallet dummyWallet)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddWallet", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@balance", dummyWallet.balance);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteWallet(int walletID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteWallet", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", walletID);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateWalletBalance(int walletID, float balance)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateWalletBalance", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", walletID);
                    cmd.Parameters.AddWithValue("@balance", balance);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<float> GetWalletBalanceAsync(int walletID)
        {
            float walletBalance = -1;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetWalletBalance", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", walletID);
                    await conn.OpenAsync();



                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            walletBalance = reader.GetFloat(reader.GetOrdinal("balance"));
                        }
                    }

                }

            }
            return walletBalance;
        }

    }
}
