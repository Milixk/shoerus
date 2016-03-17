using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace ShoesRUs
{
    class Contact
    {
        OleDbConnection myConn = new OleDbConnection();

        //it takes 7 string parameters and sends the values to the database. The parameters values are given in the mainForm
        public void sendMessage(string name, string email, string custNo, string ordNo, string category, string subject, string message)
        {
            myConn.ConnectionString = DatabaseConnection.dbconnect;
            myConn.Open();

            OleDbCommand myCmd = myConn.CreateCommand();

            //Inserts the customer's name, email address, orderNo, Category, subject, message in the Message table

            myCmd.CommandText = "INSERT INTO Message (Name, Email, CustomerNo, OrderNo, Category, Subject, Message) VALUES (@Name, @Email, @CustomerNo, @OrderNo, @Category, @Subject, @Message)";

            myCmd.Parameters.AddWithValue("Name", name);
            myCmd.Parameters.AddWithValue("Email", email);
            myCmd.Parameters.AddWithValue("CustomerNo", custNo);
            myCmd.Parameters.AddWithValue("OrderNo", ordNo);
            myCmd.Parameters.AddWithValue("Category", category);
            myCmd.Parameters.AddWithValue("Subject", subject);
            myCmd.Parameters.AddWithValue("Message", message);


            int rowsChanged = myCmd.ExecuteNonQuery();


            myConn.Close();

        }

        /*
        it takes the name, emailm orderNo, category, subject and message from the parameters list, which represents the message details. It checks if the
        message details have a corespondent in the database. If the message exists in the database, then the function returns 1, if not the function returns 0,
        which means the message wasn't sent
         */
        public int checkMessage(string name, string email, string custNo, string ordNo, string category, string subject, string message)
        {
            int check = 0;

            myConn.ConnectionString = DatabaseConnection.dbconnect;
            myConn.Open();

            OleDbCommand myCmd = myConn.CreateCommand();

            //selects all columns from the Message table , where each attribute value from the database is equal to each parameter from the parameters list
            //for example: name (from the parameters list) is equal to Name 's value (from the database) etc.

            myCmd.CommandText = "SELECT COUNT(*) FROM Message  "
                                                          + " Where Name = @Name AND Email = @Email AND CustomerNo = @CustomerNo AND OrderNo = @OrderNo AND Category = @Category AND Subject = @Subject AND Message = @Message";
            myCmd.Parameters.AddWithValue("Name", name);
            myCmd.Parameters.AddWithValue("Email", email);
            myCmd.Parameters.AddWithValue("CustomerNo", custNo);
            myCmd.Parameters.AddWithValue("OrderNo", ordNo);
            myCmd.Parameters.AddWithValue("Category", category);
            myCmd.Parameters.AddWithValue("Subject", subject);
            myCmd.Parameters.AddWithValue("Message", message);

            check = (int)myCmd.ExecuteScalar();

            myConn.Close();
            return check;
        }

    }
}
