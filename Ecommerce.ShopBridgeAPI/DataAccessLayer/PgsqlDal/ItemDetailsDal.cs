using DataAccessLayer.IDal;
using Ecommerce.Model;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.PgsqlDal
{
    public class ItemDetailsDal : IItemDetails
    {
        private readonly IConfiguration con;

        string dbconnection;
        public ItemDetailsDal(IConfiguration config)
        {
            con = config;
            dbconnection = con.GetConnectionString("Pgsql");
        }
        public ItemDetails Delete(ItemDetails ItemDetails)
        {
            try
            {
                using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
                {
                    bridge.Open();
                    using (NpgsqlCommand com = bridge.CreateCommand())
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.CommandText = @"UPDATE public.ItemDetails
	                                        SET  isactive=@isused
	                                        WHERE id=@id;";
                        com.Parameters.AddWithValue("@id", ItemDetails.Id);
                        com.Parameters.AddWithValue("@isused", false);
                        com.ExecuteScalar();

                    }
                    bridge.Close();
                }
            }
            catch (Exception)
            {


            }
           
            return ItemDetails;
        }

        public List<ItemDetails> Fetch(bool IsUsed = true)
        {
            List<ItemDetails> ItemDetails = new List<ItemDetails>();
            using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
            {
                bridge.Open();
                using (NpgsqlCommand com = bridge.CreateCommand())
                {
                    com.CommandType = System.Data.CommandType.Text;
                    com.CommandText = @"Select * from ItemDetails where isactive=@isused";
                    com.Parameters.AddWithValue("@isused", IsUsed);
                    NpgsqlDataReader dr = com.ExecuteReader();
                    while (dr.Read())
                    {


                        ItemDetails.Add(new ItemDetails()
                        {
                            Id = dr.GetGuid(dr.GetOrdinal("id")),
                            ItemId = dr.GetGuid(dr.GetOrdinal("itemid")),
                            Cost = dr.GetDouble(dr.GetOrdinal("price")),
                            IsUsed = dr.GetBoolean(dr.GetOrdinal("isactive")),
                        });


                    }
                }
                bridge.Close();
            }

            return ItemDetails;
        }

        public ItemDetails FetchByItemId(Guid ItemId, bool IsUsed = true)
        {
            ItemDetails ItemDetails = new ItemDetails();
            using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
            {
                bridge.Open();
                using (NpgsqlCommand com = bridge.CreateCommand())
                {
                    com.CommandType = System.Data.CommandType.Text;
                    com.CommandText = @"Select * from ItemDetails where isactive=@isused and itemid=@id";
                    com.Parameters.AddWithValue("@isused", IsUsed);
                    com.Parameters.AddWithValue("@id", ItemId);
                    NpgsqlDataReader dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        ItemDetails.Id = dr.GetGuid(dr.GetOrdinal("id"));
                        ItemDetails.Cost = dr.GetDouble(dr.GetOrdinal("price"));
                        ItemDetails.IsUsed = dr.GetBoolean(dr.GetOrdinal("isactive"));
                    }
                }
                bridge.Close();
            }

            return ItemDetails;
        }

        public ItemDetails Insert(ItemDetails ItemDetails)
        {
            try
            {

                using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
                {
                    bridge.Open();
                    using (NpgsqlCommand com = bridge.CreateCommand())
                    {
                        com.CommandType = System.Data.CommandType.Text;
                                            com.CommandText = @"INSERT INTO public.ItemDetails(
	                    id, itemid, price, isactive)
	                    VALUES (@id, @itemId, @cost, @isused) RETURNING id;";
                        com.Parameters.AddWithValue("@id", Guid.NewGuid());
                        com.Parameters.AddWithValue("@isused", true);
                        com.Parameters.AddWithValue("@cost", ItemDetails.Cost);
                        com.Parameters.AddWithValue("@itemId", ItemDetails.ItemId);
                        var r = com.ExecuteScalar();
                        ItemDetails.Id = new Guid(r.ToString());
                    }
                    bridge.Close();
                }
            }
            catch (Exception)
            {


            }

            return ItemDetails;
        }

        public ItemDetails Update(ItemDetails ItemDetails)
        {
            try
            {
                using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
                {
                    bridge.Open();
                    using (NpgsqlCommand com = bridge.CreateCommand())
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.CommandText = @"UPDATE public.ItemDetails
	                                        SET id=@id, itemid=@itemId, price=@cost, isactive=@isused
	                                       WHERE id=@id;";

                        com.Parameters.AddWithValue("@id", ItemDetails.Id);
                        com.Parameters.AddWithValue("@isused", ItemDetails.IsUsed);
                        com.Parameters.AddWithValue("@itemId", ItemDetails.ItemId);
                        com.Parameters.AddWithValue("@cost", ItemDetails.Cost);

                        com.ExecuteScalar();

                    }
                    bridge.Close();
                }
            }
            catch (Exception)
            {


            }

            return ItemDetails;
        }
    }
}
