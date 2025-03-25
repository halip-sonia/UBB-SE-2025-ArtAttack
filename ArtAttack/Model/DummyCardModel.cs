using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ArtAttack.Domain;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static QuestPDF.Helpers.Colors;

namespace ArtAttack.Model
{
    public class DummyCardModel
    {
        private readonly string _connectionString;

        public DummyCardModel(string connstring)
        {
            _connectionString = connstring;
        }

        public void AddCard(CardPaymentDetails paymentDetails)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddCard", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@cname", paymentDetails.cardholderName);
                    cmd.Parameters.AddWithValue("@cnumber", paymentDetails.cardNumber);
                    cmd.Parameters.AddWithValue("@cvc", paymentDetails.cvc);
                    cmd.Parameters.AddWithValue("@mon", paymentDetails.month);
                    cmd.Parameters.AddWithValue("@yr", paymentDetails.year);
                    cmd.Parameters.AddWithValue("@country", paymentDetails.country);
                    cmd.Parameters.AddWithValue("@balance", paymentDetails.balance);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCard(string cardNumber)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteCard", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@cardnumber", cardNumber);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateCardBalance(string cardNumber, float balance)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateCardBalance", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@cnumber", cardNumber);
                    cmd.Parameters.AddWithValue("@balance", balance);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public float GetCardBalance(string cardNumber)
        {
            float cardBalance = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateCardBalance", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@cnumber", cardNumber);
                    conn.Open();
                    cmd.ExecuteNonQuery();


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cardBalance = reader.GetFloat(reader.GetOrdinal("balance"));
                        }
                    }

                }

            }
            return cardBalance;
        }
    }
}
