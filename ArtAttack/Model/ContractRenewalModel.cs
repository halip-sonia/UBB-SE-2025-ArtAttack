using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using ArtAttack.Domain;

namespace ArtAttack.Model
{
    public class ContractRenewalModel
    {
        private readonly string _connectionString;

        public ContractRenewalModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Adds a renewed contract to the database (through stored procedure AddRenewedContract).
        /// </summary>
        public void AddRenewedContract(Contract contract, byte[] pdfFile)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddRenewedContract", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OrderID", contract.OrderID);
                    cmd.Parameters.AddWithValue("@ContractStatus", contract.ContractStatus);
                    cmd.Parameters.AddWithValue("@ContractContent", contract.ContractContent);
                    cmd.Parameters.AddWithValue("@RenewalCount", contract.RenewalCount);
                    cmd.Parameters.AddWithValue("@PDFFile", pdfFile);
                    cmd.Parameters.AddWithValue("@PDFID", contract.PDFID);

                    if (contract.PredefinedContractID.HasValue)
                        cmd.Parameters.AddWithValue("@PredefinedContractID", contract.PredefinedContractID.Value);
                    else
                        cmd.Parameters.AddWithValue("@PredefinedContractID", DBNull.Value);

                    if (contract.RenewedFromContractID.HasValue)
                        cmd.Parameters.AddWithValue("@RenewedFromContractID", contract.RenewedFromContractID.Value);
                    else
                        cmd.Parameters.AddWithValue("@RenewedFromContractID", DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Retrieves all contracts with status 'RENEWED' (used for history view).
        /// </summary>
        public List<Contract> GetRenewedContracts()
        {
            var contracts = new List<Contract>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetRenewedContracts", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contract = new Contract
                            {
                                ID = reader.GetInt64(reader.GetOrdinal("ID")),
                                OrderID = reader.GetInt32(reader.GetOrdinal("orderID")),
                                ContractStatus = reader.GetString(reader.GetOrdinal("contractStatus")),
                                ContractContent = reader["contractContent"] as string,
                                RenewalCount = reader.GetInt32(reader.GetOrdinal("renewalCount")),
                                PredefinedContractID = reader["predefinedContractID"] != DBNull.Value ? (int?)reader.GetInt32(reader.GetOrdinal("predefinedContractID")) : null,
                                PDFID = reader.GetInt32(reader.GetOrdinal("pdfID")),
                                RenewedFromContractID = reader["renewedFromContractID"] != DBNull.Value ? (long?)reader.GetInt64(reader.GetOrdinal("renewedFromContractID")) : null
                            };
                            contracts.Add(contract);
                        }
                    }
                }
            }
            return contracts;
        }
    }
}
