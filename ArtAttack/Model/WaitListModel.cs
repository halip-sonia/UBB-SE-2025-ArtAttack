using ArtAttack.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.Model
{
    internal class WaitListModel
    {
        private readonly string _connectionString;

        public WaitListModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        // <summary>
        /// Adds a user to the waitlist for a specific product.
        public void AddUserToWaitlist(int userId, int productWaitListId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddUserToWaitlist", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@ProductWaitListID", SqlDbType.Int).Value = productWaitListId;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// Removes a user from the waitlist and adjusts the queue positions.
        public void RemoveUserFromWaitlist(int userId, int productWaitListId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RemoveUserFromWaitlist", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@ProductWaitListID", SqlDbType.Int).Value = productWaitListId;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// Retrieves all users in a waitlist for a given product.
        public List<UserWaitList> GetUsersInWaitlist(int waitListProductId)
        {
            var waitlistEntries = new List<UserWaitList>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetUsersInWaitlist", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@WaitListProductID", SqlDbType.BigInt).Value = waitListProductId;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entry = new UserWaitList
                            {
                                userID = reader.GetInt32(reader.GetOrdinal("userID")),
                                positionInQueue = reader.GetInt32(reader.GetOrdinal("positionInQueue")),
                                joinedTime = reader.GetDateTime(reader.GetOrdinal("joinedTime"))
                            };
                            waitlistEntries.Add(entry);
                        }
                    }
                }
            }

            return waitlistEntries;
        }

        public List<UserWaitList> GetUserWaitlists(int userId)
        {
            var userWaitlists = new List<UserWaitList>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetUserWaitlists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var userWaitlist = new UserWaitList
                            {
                                userID = userId,
                                productWaitListID = reader.GetInt32(reader.GetOrdinal("productWaitListID")),
                                positionInQueue = reader.GetInt32(reader.GetOrdinal("positionInQueue")),
                                joinedTime = reader.GetDateTime(reader.GetOrdinal("joinedTime"))
                            };

                            userWaitlists.Add(userWaitlist);
                        }
                    }
                }
            }

            return userWaitlists;
        }

        public int GetWaitlistSize(int productWaitListId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetWaitlistSize", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ProductWaitListID", SqlDbType.Int).Value = productWaitListId;
                    SqlParameter outputParam = new SqlParameter("@TotalUsers", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    return (int)outputParam.Value;
                }
            }
        }

        public bool IsUserInWaitlist(int userId, int productWaitListId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("CheckUserInWaitlist", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@ProductWaitListID", SqlDbType.Int).Value = productWaitListId;

                    SqlParameter outputParam = new SqlParameter("@IsInWaitlist", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    return (bool)outputParam.Value;
                }
            }
        }

    }
}
