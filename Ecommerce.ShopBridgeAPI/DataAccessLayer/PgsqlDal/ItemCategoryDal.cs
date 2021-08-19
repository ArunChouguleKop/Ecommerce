using DataAccessLayer.IDal;
using Ecommerce.Model;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;

namespace BusinessLogics.Bal
{
    public  class ItemCategoryDal : IItemCategory
    {
        private readonly IConfiguration con;
       
        string dbconnection;
        public ItemCategoryDal(IConfiguration config)
        {
            con = config;
            dbconnection = con.GetConnectionString("Pgsql");
        }

        public bool CheckRefereceInItem(Guid Id, bool IsUsed = true)
        {
            try
            {
                bool IsRef = false;

                using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
                {
                    bridge.Open();
                    using (NpgsqlCommand com = bridge.CreateCommand())
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.CommandText = @"select ic.id from itemcategory ic
                        inner join item i on i.itemcategoryid=ic.id
                        where ic.id=@id and ic.isactive=@isused and i.isactive=@isused";
                        com.Parameters.AddWithValue("@id", Id);
                        com.Parameters.AddWithValue("@isused", IsUsed);
                        var r=com.ExecuteReader();
                        IsRef=r.HasRows;
                    }
                    bridge.Close();
                }
                return IsRef;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ItemCategory Delete(ItemCategory itemCategory)
        {
            try
            {
                using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
                {
                    bridge.Open();
                    using (NpgsqlCommand com = bridge.CreateCommand())
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.CommandText = @"UPDATE public.itemcategory
	                                        SET id=@id, categoryname=@name, isactive=@isused
	                                        WHERE id=@id;";
                        com.Parameters.AddWithValue("@id", itemCategory.Id);
                        com.Parameters.AddWithValue("@isused", false);
                        com.Parameters.AddWithValue("@name", itemCategory.Name);
                        
                        com.ExecuteScalar();

                    }
                    bridge.Close();
                }
            }
            catch (Exception)
            {

              
            }

            return itemCategory;
        }

        public List<ItemCategory> Fetch(bool IsUsed = true)
        {
            List<ItemCategory> itemCategoryList = new List<ItemCategory>();
            using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
            {
                bridge.Open();
                using (NpgsqlCommand com = bridge.CreateCommand())
                {
                    com.CommandType = System.Data.CommandType.Text;
                    com.CommandText = @"Select * from itemcategory where isactive=@isused";
                    com.Parameters.AddWithValue("@isused", IsUsed);
                    NpgsqlDataReader dr = com.ExecuteReader();
                    while (dr.Read())
                    {

                        itemCategoryList.Add(new ItemCategory()
                        {
                            Id = dr.GetGuid(dr.GetOrdinal("id")),
                            Name = dr.GetString(dr.GetOrdinal("categoryname")),
                            IsUsed = dr.GetBoolean(dr.GetOrdinal("isactive")),
                        });


                    }
                }
                bridge.Close();
            }

            return itemCategoryList;
        }

        public ItemCategory FetchById(Guid Id, bool IsUsed = true)
        {
            ItemCategory itemCategory = new ItemCategory();
            using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
            {
                bridge.Open();
                using (NpgsqlCommand com = bridge.CreateCommand())
                {
                    com.CommandType = System.Data.CommandType.Text;
                    com.CommandText = @"Select * from itemcategory where isactive=@isused and id=@id";
                    com.Parameters.AddWithValue("@isused", IsUsed);
                    com.Parameters.AddWithValue("@id", Id);
                    NpgsqlDataReader dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        itemCategory.Id = dr.GetGuid(dr.GetOrdinal("id"));
                        itemCategory.Name = dr.GetString(dr.GetOrdinal("categoryname"));
                        itemCategory.IsUsed = dr.GetBoolean(dr.GetOrdinal("isactive"));
                    }
                }
                bridge.Close();
            }

            return itemCategory;
        }
       
        public ItemCategory Insert(ItemCategory itemCategory)
        {
            try
            {
                
                using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
                {
                    bridge.Open();
                    using (NpgsqlCommand com = bridge.CreateCommand())
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.CommandText = @"INSERT INTO public.itemcategory(
	                                    id, categoryname, isactive)
	                                    VALUES (@id, @name,  @isused) RETURNING id;";
                        com.Parameters.AddWithValue("@id", Guid.NewGuid());
                        com.Parameters.AddWithValue("@isused", true);
                        com.Parameters.AddWithValue("@name", itemCategory.Name);
                       var r=com.ExecuteScalar();
                        itemCategory.Id = new Guid(r.ToString());
                    }
                    bridge.Close();
                }
            }
            catch (Exception )
            {

              
            }
           
            return itemCategory;
        }

        public ItemCategory Update(ItemCategory itemCategory)
        {
            try
            {
                using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
                {
                    bridge.Open();
                    using (NpgsqlCommand com = bridge.CreateCommand())
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.CommandText = @"UPDATE public.itemcategory
	                                        SET id=@id, categoryname=@name, isactive=@isused
	                                        WHERE id=@id;";
                        com.Parameters.AddWithValue("@id", itemCategory.Id);
                        com.Parameters.AddWithValue("@isused", itemCategory.IsUsed);
                        com.Parameters.AddWithValue("@name", itemCategory.Name);
                        
                        com.ExecuteScalar();

                    }
                    bridge.Close();
                }
            }
            catch (Exception)
            {

               
            }

            return itemCategory;
        }
    }
}
