using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Data.Common;
using System.Configuration;
using Authentication_CRUD.Models;

namespace CRUD_ADO.NET.Services
{
    public class UserDAL
    {
        private readonly string _connectionString;

        public UserDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool CreateAccount(UserLogin login)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Sp_Signin", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", login.Username);
                    cmd.Parameters.AddWithValue("@Password", login.Password);
                    cmd.Parameters.AddWithValue("@Age", login.Age);
                    cmd.Parameters.AddWithValue("@Gender", login.Gender);
                    con.Open();
                    int r = cmd.ExecuteNonQuery();
                    return r > 0;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }


        public bool SignIn(UserLogin login)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Sp_login", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", login.Username);
                    cmd.Parameters.AddWithValue("@Password", login.Password);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    bool ret = reader.HasRows;
                    reader.Close();
                    return ret;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public List<UserModelcs> GetList()
        {
            List<UserModelcs> users = new List<UserModelcs>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_Employee_Select", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    UserModelcs user = new UserModelcs();
                    user.First_Name = Convert.ToString(dr["FirstName"]);
                    user.Last_Name = Convert.ToString(dr["LastName"]);
                    user.Age = Convert.ToInt32(dr["Age"]);
                    user.Gender = Convert.ToString(dr["Gender"]);
                    user.Id = Convert.ToInt32(dr["id"]);
                    users.Add(user);
                }
            }
            return users;
        }
        public bool Create(UserModelcs model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_Employe_Add", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", model.First_Name);
                cmd.Parameters.AddWithValue("@LastName", model.Last_Name);
                cmd.Parameters.AddWithValue("@Age", model.Age);
                cmd.Parameters.AddWithValue("@Gender", model.Gender);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool Update(UserModelcs model)
        {

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_Employe_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", model.First_Name);
                cmd.Parameters.AddWithValue("@LastName", model.Last_Name);
                cmd.Parameters.AddWithValue("@Age", model.Age);
                cmd.Parameters.AddWithValue("@Gender", model.Gender);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                con.Open();
                int r = cmd.ExecuteNonQuery();
                return r > 0;
            }
        }
        public UserModelcs GetDetails(int id)
        {
            UserModelcs model = new UserModelcs();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_Details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter adapterr = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapterr.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    model.First_Name = Convert.ToString(dr["FirstName"]);
                    model.Last_Name = Convert.ToString(dr["LastName"]);
                    model.Age = Convert.ToInt32(dr["Age"]);
                    model.Gender = Convert.ToString(dr["Gender"]);
                }
            }
            return model;
        }
        public bool Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_Employee_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}



