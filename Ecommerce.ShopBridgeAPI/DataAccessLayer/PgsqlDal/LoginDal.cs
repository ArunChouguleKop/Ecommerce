using DataAccessLayer.IDal;
using Ecommerce.Model;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;

namespace DataAccessLayer.PgsqlDal
{
    public class LoginDal : ILogin
    {
        private readonly IConfiguration con;
        string dbconnection;
        public LoginDal(IConfiguration config)
        {
            con = config;
            dbconnection = con.GetConnectionString("Pgsql");
        }
        public User Login(User applicationUser)
        {
            try
            {
                

                using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
                {
                    bridge.Open();
                    using (NpgsqlCommand com = bridge.CreateCommand())
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.CommandText = @"select * from public.user where upper(username)=upper(@username) and upper(password)=upper(@password) and isactive=@isused";
                        com.Parameters.AddWithValue("@username", applicationUser.UserName);
                        com.Parameters.AddWithValue("@password", applicationUser.Password);
                        com.Parameters.AddWithValue("@isused", applicationUser.IsUsed);
                        var dr = com.ExecuteReader();
                        if (!dr.HasRows) 
                        {
                            applicationUser.IsUsed = false;
                            applicationUser.Errors.Add("Wrong credentials.");
                        }
                        while (dr.Read())
                        {
                            applicationUser.IsAdmin = dr.GetBoolean(dr.GetOrdinal("isadmin"));
                            applicationUser.IsUsed = dr.GetBoolean(dr.GetOrdinal("isactive"));
                        }
                    }
                    bridge.Close();
                }
                return applicationUser;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
