using DataAccessLayer.IDal;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.PgsqlDal
{
    public class ItemDal : IItem
    {
        private readonly IConfiguration con;
        string dbconnection;
        public ItemDal(IConfiguration config)
        {
            con = config;
            dbconnection = con.GetConnectionString("Pgsql");
        }

        public Item AddStock(Item Item)
        {
            try
            {
                using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
                {
                    bridge.Open();
                    using (NpgsqlCommand com = bridge.CreateCommand())
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.CommandText = @"update item 
                        set stockin=(select stockin from item where id=@id)+@stockIn 
                        where  id=@id";
                        com.Parameters.AddWithValue("@id", Item.Id);
                        com.Parameters.AddWithValue("@isused", Item.IsUsed);
                        com.Parameters.AddWithValue("@stockIn", Item.StockIn);
                        com.ExecuteScalar();

                    }
                    bridge.Close();
                }
            }
            catch (Exception)
            {


            }

            return Item;
        }

        public Item ConsumeStock(Item Item)
        {
            try
            {
                using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
                {
                    bridge.Open();
                    using (NpgsqlCommand com = bridge.CreateCommand())
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.CommandText = @"update item 
                        set stockout=((select stockout from item where id=@id)+@stockOut)
                        where id=@id";
                        com.Parameters.AddWithValue("@id", Item.Id);
                        com.Parameters.AddWithValue("@isused", Item.IsUsed);
                        com.Parameters.AddWithValue("@stockOut", Item.StockOut);
                        com.ExecuteScalar();

                    }
                    bridge.Close();
                }
            }
            catch (Exception)
            {


            }

            return Item;
        }

        public Item Delete(Item item)
        {
            try
            {
                using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
                {
                    bridge.Open();
                    using (NpgsqlCommand com = bridge.CreateCommand())
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.CommandText = @"UPDATE public.item
	                                    SET   isactive=@isused
	                                    WHERE id=@id;";
                        com.Parameters.AddWithValue("@id", item.Id);
                        com.Parameters.AddWithValue("@isused", item.IsUsed);
                      com.ExecuteScalar();

                    }
                    bridge.Close();
                }
            }
            catch (Exception)
            {


            }

            return item;
        }

        public List<Item> Fetch( bool IsUsed = true)
        {
            List<Item> itemList = new List<Item>();
            using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
            {
                bridge.Open();
                using (NpgsqlCommand com = bridge.CreateCommand())
                {
                    com.CommandType = System.Data.CommandType.Text;
                    com.CommandText = @"select 
                                    ic.categoryname as itemcategoryname,
                                    i.itemname,
                                    i.stockin-i.stockout as stock,
                                    i.stockin,
                                    i.stockout ,
                                    i.id
                                    from 
                                    itemcategory ic
                                    inner join item i on i.itemcategoryid=ic.id 
                                    where i.stockin > 0  and i.isactive=@isused";
                    com.Parameters.AddWithValue("@isused", IsUsed);
                    NpgsqlDataReader dr = com.ExecuteReader();
                    while (dr.Read())
                    {

                        itemList.Add(new Item()
                        {
                            Id = dr.GetGuid(dr.GetOrdinal("id")),
                            StockIn = dr.GetInt32(dr.GetOrdinal("stockin")),
                            StockOut = dr.GetInt32(dr.GetOrdinal("stockout")),
                            Name = dr.GetString(dr.GetOrdinal("itemname")),
                            Stock= dr.GetInt32(dr.GetOrdinal("stock")),
                            ItemCategoryName = dr.GetString(dr.GetOrdinal("itemCategoryname")),
                        });


                    }
                }
                bridge.Close();
            }

            return itemList;
        }

        public Item FetchById(Guid Id, bool IsUsed = true)
        {
            Item item = new Item();
            using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
            {
                bridge.Open();
                using (NpgsqlCommand com = bridge.CreateCommand())
                {
                    com.CommandType = System.Data.CommandType.Text;
                    com.CommandText = @"Select T.*,ic.categoryname as itemCategoryname from item T
                                        inner join itemcategory ic on ic.id=T.itemcategoryId and ic.isactive=@isused
                                        where T.isactive=@isused and T.id=@id";
                    com.Parameters.AddWithValue("@isused", IsUsed);
                    com.Parameters.AddWithValue("@id", Id);
                    NpgsqlDataReader dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        item.Id = dr.GetGuid(dr.GetOrdinal("id"));
                        item.Name = dr.GetString(dr.GetOrdinal("itemname"));
                        item.StockIn = dr.GetInt32(dr.GetOrdinal("stockin"));
                        item.StockOut = dr.GetInt32(dr.GetOrdinal("stockout"));
                        item.IsUsed = dr.GetBoolean(dr.GetOrdinal("isactive"));
                        item.ItemCategoryName = dr.GetString(dr.GetOrdinal("itemCategoryname"));
                        item.ItemCategoryId = dr.GetGuid(dr.GetOrdinal("itemcategoryid"));
                    }
                }
                bridge.Close();
            }

            return item;
        }

        public Item Insert(Item item)
        {
            try
            {
                using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
                {
                    bridge.Open();
                    using (NpgsqlCommand com = bridge.CreateCommand())
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.CommandText = @"INSERT INTO public.item(
	                                        id, itemname, itemcategoryid, stockin, stockout, isactive)
	                                        VALUES (@id, @name, @itemCategoryId, @stockIn, @stockOut, @isused) RETURNING id;";
                        com.Parameters.AddWithValue("@id", Guid.NewGuid());
                        com.Parameters.AddWithValue("@isused", item.IsUsed);
                        com.Parameters.AddWithValue("@itemCategoryId", item.ItemCategoryId);
                        com.Parameters.AddWithValue("@stockIn", item.StockIn);
                        com.Parameters.AddWithValue("@stockOut", 0);
                        com.Parameters.AddWithValue("@name", item.Name);

                        var r = com.ExecuteScalar();
                        item.Id = new Guid(r.ToString());

                    }
                    bridge.Close();
                }
            }
            catch (Exception )
            {


            }

            return item;
        }

        public Item Update(Item item)
        {
            try
            {
                using (NpgsqlConnection bridge = new NpgsqlConnection(dbconnection))
                {
                    bridge.Open();
                    using (NpgsqlCommand com = bridge.CreateCommand())
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.CommandText = @"UPDATE public.item
	                                    SET  itemname=@name,isactive=@isused
	                                    WHERE id=@id;";
                        com.Parameters.AddWithValue("@id", item.Id);
                        com.Parameters.AddWithValue("@isused", item.IsUsed);
                        
                        com.Parameters.AddWithValue("@name", item.Name);

                        com.ExecuteScalar();

                    }
                    bridge.Close();
                }
            }
            catch (Exception)
            {


            }

            return item;
        }
    }
}
