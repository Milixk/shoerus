using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ShoesRUs
{
    class Register
    {
        //Object that links to the Main Form
        MainForm mForm = Application.OpenForms["MainForm"] as MainForm;

        public void register()
        {
            //Check if any of the input boxes are empty or not selected
            if (mForm.cmbRegTitle.SelectedItem == null || mForm.cmbRegGender.SelectedItem == null || string.IsNullOrEmpty(mForm.txtRegName.Text) ||
                mForm.cmbRegCaType.SelectedItem == null || string.IsNullOrEmpty(mForm.txtRegEmail.Text) || string.IsNullOrEmpty(mForm.txtRegPassword.Text) ||
                string.IsNullOrEmpty(mForm.txtRegPasswordConfirm.Text) || string.IsNullOrEmpty(mForm.txtRegDOB.Text) || string.IsNullOrEmpty(mForm.txtRegPhoneNo.Text) ||
                string.IsNullOrEmpty(mForm.txtRegAddNo.Text) || string.IsNullOrEmpty(mForm.txtRegAddStreet.Text) || string.IsNullOrEmpty(mForm.txtRegAddCity.Text) ||
                string.IsNullOrEmpty(mForm.txtRedAddCountry.Text) || string.IsNullOrEmpty(mForm.txtRegPostCode.Text) || string.IsNullOrEmpty(mForm.txtRegCaName.Text) ||
                string.IsNullOrEmpty(mForm.txtRegCaNo.Text) || string.IsNullOrEmpty(mForm.txtRegCaCVV.Text) || string.IsNullOrEmpty(mForm.txtRegCaExpiry.Text))
            {
                MessageBox.Show("One or more fields are empty.");
            }
            else
            {
                //Check if the email entered already exists
                if (checkEmailExists(mForm.txtRegEmail.Text) == true)
                {
                    MessageBox.Show("This email address is already being used by another account.");
                }
                else
                {
                    //Check if the Date of Birth field is entered correctly
                    DateTime resultDOB;
                    if (DateTime.TryParseExact(mForm.txtRegDOB.Text, new string[] { "d-M-yyyy", "d/M/yyyy", "d.M.yyyy" }, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out resultDOB))
                    {
                        //Check if the Card Expiry field is entered correctly
                        DateTime resultExpiry;
                        if (DateTime.TryParseExact(mForm.txtRegCaExpiry.Text, new string[] { "MM-yy", "MM/yy", "MM.yy" }, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out resultExpiry))
                        {
                            //Check if Phone Number, Card Number and Card CVV input are numbers
                            if (Regex.IsMatch(mForm.txtRegCaNo.Text, @"^\d+$") || Regex.IsMatch(mForm.txtRegCaCVV.Text, @"^\d+$"))
                            {
                                if (mForm.txtRegPassword.Text != mForm.txtRegPasswordConfirm.Text)
                                {
                                    MessageBox.Show("Password doesn't match.");
                                }
                                else
                                {
                                    //Encryption for passwords
                                    Encryption ec = new Encryption();
                                    //Insert registration details into the database
                                    OleDbConnection dbCon = new OleDbConnection(DatabaseConnection.dbconnect);

                                    dbCon.ConnectionString = DatabaseConnection.dbconnect;
                                    OleDbCommand dbCmd = dbCon.CreateCommand();

                                    dbCmd.CommandText = "INSERT INTO Customer(CustomerTitle, CustomerName, CustomerDOB, CustomerGender, CustomerEmail, CustomerPhoneNo, CustomerAddressNo, CustomerAddressStreet, CustomerAddressCity, CustomerAddressCountry, CustomerPostCode, CustomerPaymentCardType, CustomerPaymentCardNo, CustomerPaymentCardCVV, CustomerPaymentCardName, CustomerPaymentCardExpDate, CustomerPassword) VALUES (@CustomerTitle, @CustomerName, @CustomerDOB, @CustomerGender, @CustomerEmail, @CustomerPhoneNo, @CustomerAddressNo, @CustomerAddressStreet, @CustomerAddressCity, @CustomerAddressCountry, @CustomerPostCode, @CustomerPaymentCardType, @CustomerPaymentCardNo, @CustomerPaymentCardCVV, @CustomerPaymentCardName, @CustomerPaymentCardExpDate, @CustomerPassword)";

                                    dbCmd.Parameters.AddWithValue("CustomerTitle", mForm.cmbRegTitle.SelectedItem);
                                    dbCmd.Parameters.AddWithValue("CustomerName", mForm.txtRegName.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerDOB", resultDOB.ToShortDateString());
                                    dbCmd.Parameters.AddWithValue("CustomerGender", mForm.cmbRegGender.SelectedItem);
                                    dbCmd.Parameters.AddWithValue("CustomerEmail", mForm.txtRegEmail.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPhoneNo", mForm.txtRegPhoneNo.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerAddressNo", mForm.txtRegAddNo.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerAddressStreet", mForm.txtRegAddStreet.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerAddressCity", mForm.txtRegAddCity.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerAddressCountry", mForm.txtRedAddCountry.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPostCode", mForm.txtRegPostCode.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardType", mForm.cmbRegCaType.SelectedItem);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardNo", mForm.txtRegCaNo.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardCVV", mForm.txtRegCaCVV.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardName", mForm.txtRegCaName.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardExpDate", resultExpiry.ToShortDateString());
                                    dbCmd.Parameters.AddWithValue("CustomerPassword", ec.Encrypt(mForm.txtRegPassword.Text));

                                    dbCon.Open();
                                    int rowsChanged = dbCmd.ExecuteNonQuery();
                                    dbCon.Close();

                                    MessageBox.Show("Registration Successful!");
                                    clearFields();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Card Number or Card CVV is not a number.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Card Expiry field entered incorrect. Use the format MM/YY.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Date of Birth field entered incorrect. Use the format DD/MM/YYYY.");
                    }
                }
            }
        }

        //Clear the Registration fields
        public void clearFields()
        {
            mForm.cmbRegTitle.SelectedIndex = -1;
            mForm.cmbRegGender.SelectedIndex = -1;
            mForm.cmbRegCaType.SelectedIndex = -1;

            mForm.txtRegName.Clear();
            mForm.txtRegEmail.Clear();
            mForm.txtRegPassword.Clear();
            mForm.txtRegPasswordConfirm.Clear();
            mForm.txtRegPhoneNo.Clear();
            mForm.txtRegAddNo.Clear();
            mForm.txtRegAddStreet.Clear();
            mForm.txtRegAddCity.Clear();
            mForm.txtRedAddCountry.Clear();
            mForm.txtRegPostCode.Clear();
            mForm.txtRegCaName.Clear();
            mForm.txtRegCaNo.Clear();
            mForm.txtRegCaCVV.Clear();

            mForm.txtRegDOB.Text = "DD/MM/YYYY";
            mForm.txtRegCaExpiry.Text = "MM/YY";
        }

        //Check if the Email provided already exists on the Database
        public bool checkEmailExists(string email)
        {
            bool emailExists;

            OleDbConnection dbCon = new OleDbConnection(DatabaseConnection.dbconnect);

            dbCon.ConnectionString = DatabaseConnection.dbconnect;
            OleDbCommand dbCmd = dbCon.CreateCommand();

            dbCmd.CommandText = "SELECT COUNT(*) FROM Customer WHERE CustomerEmail = @CustomerEmail";
            dbCmd.Parameters.AddWithValue("CustomerEmail", email);

            dbCon.Open();
            int emailCount = (int)dbCmd.ExecuteScalar();
            dbCon.Close();

            if (emailCount > 0)
            {
                emailExists = true;
            }
            else
            {
                emailExists = false;
            }

            return emailExists;
        }
    }
}
