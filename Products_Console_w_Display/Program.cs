// See https://aka.ms/new-console-template for more information


using System;
using System.Data;
using System.Text.RegularExpressions;
using Npgsql;

class Sample
{
    static void Main(string[] args)
    {
        // Connect to a PostgreSQL database
        NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1:5432;User Id=postgres; " +
           "Password=Temp;Database=prods;");
        conn.Open();

        // Define a query returning a single row result set
        // NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM product", conn);
        NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM product", conn);

        //NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM customer", conn);

        // NpgsqlCommand command = new NpgsqlCommand("SELECT rep_id, cust_balance FROM customer", conn);
        // Execute the query and obtain the value of the first column of the first row
        // Int64 count = (Int64)command.ExecuteScalar();

        NpgsqlDataReader reader = command.ExecuteReader();

        DataTable dt = new DataTable();
       
        dt.Load(reader);

        results7(dt);
        //results20(dt);

        conn.Close();
    }

    static void results7(DataTable dt)
    {
        var query = dt.AsEnumerable()
            .Where(row => row.Field<Int16>("prod_quantity") >= 12 && row.Field<Int16>("prod_quantity") <= 30)
            .Select(row => new
            {
                prodId = row.Field<string>("prod_id"),
                prodDescription = row.Field<string>("prod_desc"),
                prodQuantity = row.Field<Int16>("prod_quantity")
            });

        foreach (var result in query)
        {
            Console.WriteLine(result.prodId + "   " + result.prodDescription + "   " + result.prodQuantity);
        }
    }

    static void results20(DataTable dt)
    {
        var query = dt.AsEnumerable()
            .GroupBy(row => row.Field<string>("rep_id"))
            .Select(repGroup => new
             {
                 repId = repGroup.Key,
                 allBalances = repGroup.Sum(row => row.Field<decimal>("cust_balance"))
              })
             .Where(res => res.allBalances >= 12000)
             .OrderBy(res => res.repId);

        foreach (var result in query)
        {
            Console.WriteLine(result.repId + "   " + result.allBalances);
        }

    }

}

