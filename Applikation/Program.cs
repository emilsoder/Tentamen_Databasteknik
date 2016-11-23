using System;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using Applikation.EFModel;

namespace Applikation
{
    public class Program
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["NorthwindDBContext"].ConnectionString;

        static void Main(string[] args)
        {
            Console.WriteLine("--------------\n ProductsByCategoryName");
            ProductsByCategoryName("Confections");

            Console.WriteLine("--------------\n SalesByTerritory");
            SalesByTerritory();

            Console.WriteLine("--------------\n EmployeesPerRegion");
            EmployeesPerRegion();

            Console.WriteLine("--------------\n OrdersPerEmployee");
            OrdersPerEmployee();

            Console.WriteLine("--------------\n CustomersWithNamesLongerThan25Characters");
            CustomersWithNamesLongerThan25Characters();

            Console.ReadLine();
        }

        #region UPPGIFT 1
        public static void ProductsByCategoryName(string CategoryName)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand command = sqlConnection.CreateCommand();

            command.CommandText =
                " SELECT products.ProductName, products.UnitPrice, products.UnitsInStock"
                + " FROM Products products"
                + " WHERE p.CategoryID"
                + " = "
                + " (SELECT categories.CategoryID"
                + " FROM Categories categories"
                + " WHERE CategoryName ="
                + " @CategoryName)";

            command.Parameters.AddWithValue("@CategoryName", CategoryName);
            sqlConnection.Open();
            SqlDataReader reader = command.ExecuteReader();
            Console.Clear();
            while (reader.Read())
            {
                Console.WriteLine("{0} \n {1} \n {2}\n ----- \n",
                    reader.GetString(0),
                    reader.GetValue(1),
                    reader.GetValue(2));
            }
            reader.Close();
            sqlConnection.Close();
        }
        #endregion

        #region UPPGIFT 2
        public static void SalesByTerritory()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand command = sqlConnection.CreateCommand();

            command.CommandText =
                  " SELECT TOP 5 COUNT([Order Details].OrderID)"
                + " AS Sales, Territories.TerritoryDescription"
                + " FROM Orders INNER JOIN [Order Details]"
                + " ON Orders.OrderID = [Order Details].OrderID"
                + " INNER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID"
                + " INNER JOIN EmployeeTerritories ON Employees.EmployeeID = EmployeeTerritories.EmployeeID"
                + " INNER JOIN Territories ON EmployeeTerritories.TerritoryID = Territories.TerritoryID "
                + " GROUP BY Territories.TerritoryDescription"
                + " ORDER BY Sales DESC";

            sqlConnection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("{0}: {1}",
                    reader.GetString(1).Trim(),
                    reader.GetValue(0));
            }
            reader.Close();
            sqlConnection.Close();
        }
        #endregion

        #region UPPGIFT 3
        public static void EmployeesPerRegion()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand command = sqlConnection.CreateCommand();

            command.CommandText =
                "SELECT r.RegionDescription, COUNT(emp.EmployeeID)"
                + " FROM Employees emp INNER JOIN EmployeeTerritories et "
                + " ON emp.EmployeeID = et.EmployeeID "
                + " INNER JOIN Territories t ON "
                + " et.TerritoryID = t.TerritoryID INNER JOIN Region r "
                + " ON t.RegionID = r.RegionID GROUP BY r.RegionDescription";

            sqlConnection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("{0}: {1}",
                    reader.GetString(0).Trim(),
                    reader.GetValue(1));
            }
            reader.Close();
            sqlConnection.Close();
        }
        #endregion

        #region UPPGIFT 4
        public static void OrdersPerEmployee()
        {
            var sqlConnection = new SqlConnection(connectionString);
            SqlCommand cmd = sqlConnection.CreateCommand();

            cmd.CommandText =
                "SELECT emp.FirstName +"
                + " ' ' + "
                + " emp.LastName, COUNT(orders.OrderID)"
                + " FROM Employees emp"
                + " INNER JOIN Orders orders "
                + " ON emp.EmployeeID = orders.EmployeeID"
                + " GROUP BY emp.LastName, emp.FirstName";

            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("{0}: {1}", reader.GetString(0), reader.GetValue(1));
            }
            reader.Close();
            sqlConnection.Close();
        }
        #endregion

        #region UPPGIFT 5
        public static void CustomersWithNamesLongerThan25Characters()
        {
            using (var db = new NorthwindDBContext())
            {
                var queryResult = 
                    db.Customers.Where
                    (x => x.CompanyName.Length > 25).Select
                    (x => x.CompanyName);

                foreach (var _companyName in queryResult)
                {
                    Console.WriteLine(_companyName);
                }
            }
        }
        #endregion
    }
}
