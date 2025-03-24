using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using ArtAttack.Domain;

namespace ArtAttack.Model
{
    public class ContractViewModel
    {
        private readonly string _connectionString;

        public ContractViewModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Retrieves a single contract using the GetContractByID stored procedure.
        /// </summary>
        public Contract GetContractById(long contractId)
        {
            Contract contract = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetContractByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ContractID", SqlDbType.BigInt).Value = contractId; // Explicit SQL type
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows && reader.Read()) // Added HasRows check
                        {
                            contract = new Contract
                            {
                                ID = reader.GetInt64("ID"),
                                OrderID = reader.GetInt32("orderID"),
                                StartDate = reader.GetDateTime("startDate"),
                                EndDate = reader.GetDateTime("endDate"),
                                ContractStatus = reader.GetString("contractStatus"),
                                ContractContent = reader["contractContent"] as string,
                                RenewalCount = reader.GetInt32("renewalCount"),
                                PredefinedContractID = reader["predefinedContractID"] != DBNull.Value
                                    ? (int?)reader.GetInt32("predefinedContractID")
                                    : null,
                                PDFID = reader.GetInt32("pdfID"),
                                RenewedFromContractID = reader["renewedFromContractID"] != DBNull.Value
                                    ? (long?)reader.GetInt64("renewedFromContractID")
                                    : null
                            };
                        }
                    }
                }
            }
            return contract;
        }

        /// <summary>
        /// Retrieves all contracts using GetAllContracts stored procedure.
        /// </summary>
        public List<Contract> GetAllContracts()
        {
            var contracts = new List<Contract>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllContracts", conn))
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
                                StartDate = reader.GetDateTime(reader.GetOrdinal("startDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("endDate")),
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

        /// <summary>
        /// Retrieves the renewal history for a contract using GetContractHistory stored procedure.
        /// </summary>
        public List<Contract> GetContractHistory(long contractId)
        {
            var history = new List<Contract>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetContractHistory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ContractID", contractId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contract = new Contract
                            {
                                ID = reader.GetInt64(reader.GetOrdinal("ID")),
                                OrderID = reader.GetInt32(reader.GetOrdinal("orderID")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("startDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("endDate")),
                                ContractStatus = reader.GetString(reader.GetOrdinal("contractStatus")),
                                ContractContent = reader["contractContent"] as string,
                                RenewalCount = reader.GetInt32(reader.GetOrdinal("renewalCount")),
                                PredefinedContractID = reader["predefinedContractID"] != DBNull.Value ? (int?)reader.GetInt32(reader.GetOrdinal("predefinedContractID")) : null,
                                PDFID = reader.GetInt32(reader.GetOrdinal("pdfID")),
                                RenewedFromContractID = reader["renewedFromContractID"] != DBNull.Value ? (long?)reader.GetInt64(reader.GetOrdinal("renewedFromContractID")) : null
                            };
                            history.Add(contract);
                        }
                    }
                }
            }
            return history;
        }

        /// <summary>
        /// Inserts a new contract and updates the PDF file using AddContract stored procedure.
        /// </summary>
        public void AddContract(Contract contract, byte[] pdfFile)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddContract", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OrderID", contract.OrderID);
                    cmd.Parameters.AddWithValue("@StartDate", contract.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", contract.EndDate);
                    cmd.Parameters.AddWithValue("@ContractStatus", contract.ContractStatus);
                    cmd.Parameters.AddWithValue("@ContractContent", contract.ContractContent);
                    cmd.Parameters.AddWithValue("@RenewalCount", contract.RenewalCount);

                    // Handle nullable PredefinedContractID
                    if (contract.PredefinedContractID.HasValue)
                        cmd.Parameters.AddWithValue("@PredefinedContractID", contract.PredefinedContractID.Value);
                    else
                        cmd.Parameters.AddWithValue("@PredefinedContractID", DBNull.Value);

                    cmd.Parameters.AddWithValue("@PDFID", contract.PDFID);
                    cmd.Parameters.AddWithValue("@PDFFile", pdfFile);

                    // Handle nullable RenewedFromContractID
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
        /// Retrieves seller information for a given contract using GetContractSeller stored procedure.
        /// Returns a tuple of (SellerID, SellerName).
        /// </summary>
        public (int SellerID, string SellerName) GetContractSeller(long contractId)
        {
            (int SellerID, string SellerName) sellerInfo = (0, string.Empty);
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetContractSeller", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ContractID", contractId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            sellerInfo = (
                                SellerID: reader.GetInt32(reader.GetOrdinal("SellerID")),
                                SellerName: reader.GetString(reader.GetOrdinal("SellerName"))
                            );
                        }
                    }
                }
            }
            return sellerInfo;
        }

        /// <summary>
        /// Retrieves buyer information for a given contract using GetContractBuyer stored procedure.
        /// Returns a tuple of (BuyerID, BuyerName).
        /// </summary>
        public (int BuyerID, string BuyerName) GetContractBuyer(long contractId)
        {
            (int BuyerID, string BuyerName) buyerInfo = (0, string.Empty);
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetContractBuyer", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ContractID", contractId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            buyerInfo = (
                                BuyerID: reader.GetInt32(reader.GetOrdinal("BuyerID")),
                                BuyerName: reader.GetString(reader.GetOrdinal("BuyerName"))
                            );
                        }
                    }
                }
            }
            return buyerInfo;
        }

        /// <summary>
        /// Retrieves order summary information for a contract using GetOrderSummaryInformation stored procedure.
        /// Returns the data as a dictionary.
        /// </summary>
        public Dictionary<string, object> GetOrderSummaryInformation(long contractId)
        {
            var orderSummary = new Dictionary<string, object>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetOrderSummaryInformation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ContractID", contractId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Assuming the OrderSummary table has these columns.
                            orderSummary["ID"] = reader["ID"];
                            orderSummary["subtotal"] = reader["subtotal"];
                            orderSummary["warrantyTax"] = reader["warrantyTax"];
                            orderSummary["deliveryFee"] = reader["deliveryFee"];
                            orderSummary["finalTotal"] = reader["finalTotal"];
                            orderSummary["fullName"] = reader["fullName"];
                            orderSummary["email"] = reader["email"];
                            orderSummary["phoneNumber"] = reader["phoneNumber"];
                            orderSummary["address"] = reader["address"];
                            orderSummary["postalCode"] = reader["postalCode"];
                            orderSummary["additionalInfo"] = reader["additionalInfo"];
                            orderSummary["ContractDetails"] = reader["ContractDetails"];
                        }
                    }
                }
            }
            return orderSummary;
        }
    }
}
