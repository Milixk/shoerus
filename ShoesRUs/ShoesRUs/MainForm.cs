using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ShoesRUs
{
    public partial class MainForm : Form
    {
        Login login = new Login();
        Register register = new Register();
        public MainForm()
        {
            InitializeComponent();
            Startup su = new Startup();
        }

        //Shows the LoginForm
        private void btnShowLoginGrp_Click(object sender, EventArgs e)
        {
            grpLogin.Visible = true;
        }

        private void btnShowRegisterGrp_Click(object sender, EventArgs e)
        {
            grpRegister.Visible = true;
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {

        }

        private void btnContact_Click(object sender, EventArgs e)
        {
            grpContact.Visible = true;
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            grpProfile.Visible = true;
        }

        private void btnBasket_Click(object sender, EventArgs e)
        {
            grpBasket.Visible = true;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {

        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (login.loggingIn(txtLoginEmail.Text, txtLoginPassword.Text) != -999)
            {
                login.setLoggedIn(login.loggingIn(txtLoginEmail.Text, txtLoginPassword.Text));
                grpLogin.Visible = false;
                btnShowRegisterGrp.Visible = false;
                btnProfile.Visible = true;
                btnBasket.Visible = true;
                btnLogout.Visible = true;
                if (login.checkAdmin() == true)
                {
                    btnAdmin.Visible = true;
                }
                MessageBox.Show("Login successfull!");
                txtLoginEmail.Text = "";
                txtLoginPassword.Text = "";
            }
            else
            {
                MessageBox.Show("Login details incorrect!");
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            //register.register();

            //Check if any of the input boxes are empty or not selected
            if (cmbRegTitle.SelectedItem == null || cmbRegGender.SelectedItem == null || string.IsNullOrEmpty(txtRegName.Text) ||
                cmbRegCaType.SelectedItem == null || string.IsNullOrEmpty(txtRegEmail.Text) || string.IsNullOrEmpty(txtRegPassword.Text) ||
                string.IsNullOrEmpty(txtRegPasswordConfirm.Text) || string.IsNullOrEmpty(txtRegDOB.Text) || string.IsNullOrEmpty(txtRegPhoneNo.Text) ||
                string.IsNullOrEmpty(txtRegAddNo.Text) || string.IsNullOrEmpty(txtRegAddStreet.Text) || string.IsNullOrEmpty(txtRegAddCity.Text) ||
                string.IsNullOrEmpty(txtRedAddCountry.Text) || string.IsNullOrEmpty(txtRegPostCode.Text) || string.IsNullOrEmpty(txtRegCaName.Text) ||
                string.IsNullOrEmpty(txtRegCaNo.Text) || string.IsNullOrEmpty(txtRegCaCVV.Text) || string.IsNullOrEmpty(txtRegCaExpiry.Text))
            {
                MessageBox.Show("One or more fields are empty.");
            }
            else
            {
                //Check if the email entered already exists
                if (register.checkEmailExists(txtRegEmail.Text) == true)
                {
                    MessageBox.Show("This email address is already being used by another account.");
                }
                else
                {
                    //Check if the Date of Birth field is entered correctly
                    DateTime resultDOB;
                    if (DateTime.TryParseExact(txtRegDOB.Text, new string[] { "d-M-yyyy", "d/M/yyyy", "d.M.yyyy" }, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out resultDOB))
                    {
                        //Check if the Card Expiry field is entered correctly
                        DateTime resultExpiry;
                        if (DateTime.TryParseExact(txtRegCaExpiry.Text, new string[] { "MM-yy", "MM/yy", "MM.yy" }, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out resultExpiry))
                        {
                            //Check if Phone Number, Card Number and Card CVV input are numbers
                            if (Regex.IsMatch(txtRegCaNo.Text, @"^\d+$") || Regex.IsMatch(txtRegCaCVV.Text, @"^\d+$"))
                            {
                                if (txtRegPassword.Text != txtRegPasswordConfirm.Text)
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

                                    dbCmd.Parameters.AddWithValue("CustomerTitle", cmbRegTitle.SelectedItem);
                                    dbCmd.Parameters.AddWithValue("CustomerName", txtRegName.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerDOB", resultDOB.ToShortDateString());
                                    dbCmd.Parameters.AddWithValue("CustomerGender", cmbRegGender.SelectedItem);
                                    dbCmd.Parameters.AddWithValue("CustomerEmail", txtRegEmail.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPhoneNo", txtRegPhoneNo.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerAddressNo", txtRegAddNo.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerAddressStreet", txtRegAddStreet.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerAddressCity", txtRegAddCity.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerAddressCountry", txtRedAddCountry.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPostCode", txtRegPostCode.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardType", cmbRegCaType.SelectedItem);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardNo", txtRegCaNo.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardCVV", txtRegCaCVV.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardName", txtRegCaName.Text);
                                    dbCmd.Parameters.AddWithValue("CustomerPaymentCardExpDate", resultExpiry.ToShortDateString());
                                    dbCmd.Parameters.AddWithValue("CustomerPassword", ec.Encrypt(txtRegPassword.Text));

                                    dbCon.Open();
                                    int rowsChanged = dbCmd.ExecuteNonQuery();
                                    dbCon.Close();

                                    MessageBox.Show("Registration Successful!");
                                    register.clearFields();
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

        private void btnCancelRegister_Click(object sender, EventArgs e)
        {
            register.clearFields();
        }        

    }
}
