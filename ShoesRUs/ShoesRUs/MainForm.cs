﻿using System;
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

        List<int> basket = new List<int>();
        
        public MainForm()
        {
            InitializeComponent();
            Startup su = new Startup();
            startupLoad();
        }

        //Shows the LoginForm
        private void btnShowLoginGrp_Click(object sender, EventArgs e)
        {
            hideGrp();
            grpLogin.Visible = true;
        }

        private void btnShowRegisterGrp_Click(object sender, EventArgs e)
        {
            hideGrp();
            grpRegister.Visible = true;
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {

        }

        private void btnContact_Click(object sender, EventArgs e)
        {
            hideGrp();
            grpContact.Visible = true;
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            hideGrp();
            grpProfile.Visible = true;
        }

        private void btnBasket_Click(object sender, EventArgs e)
        {
            hideGrp();
            grpBasket.Visible = true;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            login.logOut();
            btnAdmin.Visible = false;
            btnProfile.Visible = false;
            hideGrp();
            grpMain.Visible = true;        }

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

        private void btnMainFilter_Click(object sender, EventArgs e)
        {
            updateListView();
        }


        public void startupLoad()  // PETRs strartup function
        {
            for (int i = 0; i < chckListBoxMainGender.Items.Count; i++)  // this loop checks all the boxes in gender filter
            {
                chckListBoxMainGender.SetItemChecked(i, true);
            }


            /////////////////// NEW START ///////////////////////////////////////////////////////////////////////////////////////////////////////////
            updateBrands();
            for (int i = 0; i < chckListBoxMainBrand.Items.Count; i++)  // this loop checks all the boxes in brand filter
            {
                chckListBoxMainBrand.SetItemChecked(i, true);
            }
            ////////////////// NEW END //////////////////////////////////////////////////////////////////////////////////////////////////////////////


            string queryString = "SELECT * FROM Shoe";
            listViewQuery(queryString);  
        }

        public void listViewQuery(string queryString)
        {
            try
            {

                ListClear();
                using (OleDbConnection dbCon = new OleDbConnection(DatabaseConnection.dbconnect))
                {

                    OleDbCommand command = new OleDbCommand(queryString, dbCon);
                    dbCon.Open();
                    OleDbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ListInsert(reader.GetInt32(0), reader.GetInt32(9).ToString(), reader.GetString(11), reader.GetString(4));

                    }
                    reader.Close();
                    dbCon.Dispose();
                }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to connect to data source (" + ex + ")");
                }

            }

        public void ListClear()
        {
            listViewMain.Items.Clear();
        }

        // public function for adding shoes into listview
        public void ListInsert(int ID, string price, string name, string brand) //   TODO add name functonality
        {
            if (imageListMain.Images.Count > ID) //checks if we have picture
                listViewMain.Items.Add(brand + " " + name + ", £" + price, ID); //add item with name "" and picture id
            else
                listViewMain.Items.Add(brand + " " + name + ", £" + price, 0); //add item with name "" and placeholder picture
        }

        private void cmbMainOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateListView();
        }

        // main function, grabs query strings from everywhere, mashes them together and then calls function to display the results
        private void updateListView()
        {
            string queryString = "SELECT * FROM Shoe";
            string searchQueryString = getSearchText();
            string filterQueryString = getFilterQuery();
            string orderQueryString = cmbMainOrderCaseFunction();

            bool search = false;
            //bool filter = false;
            //bool order = false;

            if (searchQueryString.Length > 0)
            {
                queryString += searchQueryString;
                search = true;
            }

            if (filterQueryString.Length > 0)
            {
                if (search)
                    queryString += " AND";
                else
                    queryString += " WHERE";

                queryString += filterQueryString;
                //filter = true;
            }

            if (orderQueryString.Length > 0)
                queryString += orderQueryString;



            listViewQuery(queryString);  // show items
        }

        //get query text from txtboxmainsearch for searching
        private string getSearchText()
        {
            if (txtBoxMainSearch.TextLength > 0)
                if ("Search" != txtBoxMainSearch.Text)
                    return " WHERE (ShoeName LIKE \"" + txtBoxMainSearch.Text + "%\")";

            return "";
        }

        //get query from filters
        private string getFilterQuery()
        {

            string ret = "";
            string priceString = filterPrice();
            string genderString = filterGender();

            string brandString = filterBrands();
            //bool brand = false;

            bool price = false;
            bool gender = false;

            if (priceString.Length > 0)
            {
                price = true;
                ret += priceString;
            }

            if (genderString.Length > 0)
            {
                gender = true; 
                if (price)
                    ret += " AND";
                ret += genderString;
            }

            if (brandString.Length > 0)
            {
                //brand = true; // we dont really need this one so far
                if (price || gender)
                    ret += " AND";
                ret += brandString;
            }

            return ret;
        }

        private string filterGender()
        {
            string ret = "";


            if (chckListBoxMainGender.CheckedItems.Count == chckListBoxMainGender.Items.Count)
                return "";   // all items are selected, no need to filter

            bool[] gender = new bool[3];

            for (int i = 0; i < chckListBoxMainGender.Items.Count; i++)  // this loop sets gender[i] true if appropriate checkbox is checked
            {
                if (chckListBoxMainGender.GetItemChecked(i))
                    gender[i] = true;
            }

            ret += " (";

            // ugly if statements, dont want to think too hard now
            if (gender[0])
            {
                ret += " ShoeGender='Men'";
                if (gender[1])
                    ret += " OR ShoeGender='Women'";
                if (gender[2])
                    ret += " OR ShoeGender='Kids'";
            }
            else if (gender[1])
            {
                ret += " ShoeGender='Women'";
                if (gender[2])
                    ret += " OR ShoeGender='Kids'";
                else
                    ret += ")";
            }
            else if (gender[2])
                ret += " ShoeGender='Kids'";
            else return "";

            ret += ")";
            return ret;
        }


        private string filterPrice()
        {
            string min;
            string max;
            bool changed = false;


            int n; // just a trash from tryParse
            bool MinIsNumeric = int.TryParse(txtBoxMainPriceMin.Text, out n);
            bool MaxIsNumeric = int.TryParse(txtBoxMainPriceMax.Text, out n);



            if (txtBoxMainPriceMin.TextLength > 0)
                if (MinIsNumeric)
                {
                    min = txtBoxMainPriceMin.Text;
                    changed = true;
                }
                else
                    min = "0";
            else
                min = "0";


            if (txtBoxMainPriceMax.TextLength > 0)
                if (MaxIsNumeric)
                {
                    max = txtBoxMainPriceMax.Text;
                    changed = true;
                }
                else
                    max = "9999";
            else
                max = "9999";


            if (changed)
                return " (ShoePrice BETWEEN " + min + " AND " + max + ")";
            else
                return "";
        }

        private string cmbMainOrderCaseFunction()
        {
            switch (cmbMainOrder.SelectedIndex)
            {
                case 0: return " ORDER BY ShoePrice ASC";  //price ascending   
                case 1: return " ORDER BY ShoePrice DESC"; //price descending  
                case 2: return " ORDER BY ShoeName ASC";   //name ascending  
                case 3: return " ORDER BY ShoeName DESC";  //name descending       
                default: return "";                        //default ID ascending
            }
        }

        private void listViewMain_Click(object sender, EventArgs e)
        {
            if (listViewMain.SelectedItems.Count > 0)
            {
                hideGrp();
                grpViewProduct.Visible = true;
                Populate(listViewMain.SelectedItems[0].ImageIndex);
            }
        }

        private void txtBoxMainSearch_TextChanged(object sender, EventArgs e)
        {
            updateListView();
        }

        private void txtBoxMainPriceMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtBoxMainPriceMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void updateBrands()
        {
            string queryString = "SELECT DISTINCT  ShoeBrand FROM Shoe";

            try
            {
                using (OleDbConnection dbCon = new OleDbConnection(DatabaseConnection.dbconnect))
                {

                    OleDbCommand command = new OleDbCommand(queryString, dbCon);
                    dbCon.Open();
                    OleDbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        chckListBoxMainBrand.Items.Add(reader.GetString(0));
                    }
                    reader.Close();
                    dbCon.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex);
            }

        }





        private string filterBrands()
        {
            string ret = "";


            if (chckListBoxMainBrand.CheckedItems.Count == chckListBoxMainBrand.Items.Count)
                return "";   // all items are selected, no need to filter

            string[] brand = new string[chckListBoxMainBrand.CheckedItems.Count];

            int j = 0;
            for (int i = 0; i < chckListBoxMainBrand.Items.Count; i++)  // this loop sets gender[i] true if appropriate checkbox is checked
            {
                if (chckListBoxMainBrand.GetItemChecked(i))
                {
                    brand[j] = chckListBoxMainBrand.Items[i].ToString();
                    j++;
                }
            }


            //// NO MORE UGLY IF STATEMENTS, add this to the gender selection in the future
            ret += " (";
            bool firsttime = true;
            foreach (string b in brand)
            {
                if (!firsttime)
                    ret += " OR";

                ret += " Shoebrand='" + b + "'";
                firsttime = false;
            }
            ret += ")";
            return ret;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtEmail.Text) ||
               string.IsNullOrEmpty(txtSubj.Text) || string.IsNullOrEmpty(txtMessage.Text))
            {
                MessageBox.Show("One or more fields are empty.");
            }
            else
            {

                Contact contact = new Contact();
                contact.sendMessage(txtName.Text, txtEmail.Text, txtCustNo.Text, txtOrdNo.Text, cmbCategory.SelectedItem.ToString(), txtSubj.Text, txtMessage.Text);
                int chkMessage = contact.checkMessage(txtName.Text, txtEmail.Text, txtCustNo.Text, txtOrdNo.Text, cmbCategory.SelectedItem.ToString(), txtSubj.Text, txtMessage.Text);
                if (chkMessage == 1)
                {
                    MessageBox.Show("Message successfully sent!");
                }
                else
                {

                    MessageBox.Show("Error when sending message!");

                }

                clearFieldsContactForm();


            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clearFieldsContactForm();
        }

        private void clearFieldsContactForm()//function which clear all the fields to be completed (Contact group box)
        {

            txtName.Clear();
            txtName.Clear();
            txtEmail.Clear();
            txtCustNo.Clear();
            txtOrdNo.Clear();
            txtSubj.Clear();
            txtMessage.Clear();
            cmbCategory.SelectedIndex = -1;

        }

        OleDbConnection myConn = new OleDbConnection();

        private void btnViewProfileDetails_Click(object sender, EventArgs e)
        {
            grpProfileDetails.Visible = true;
            grpAddressUpdate.Visible = false;
            grpCardUpdate.Visible = false;
            grpPurchases.Visible = false;

            try
            {
                myConn.ConnectionString = DatabaseConnection.dbconnect; ;
                OleDbCommand myCmd = myConn.CreateCommand();

                myCmd.CommandText = "Select CustomerTitle, CustomerDOB, CustomerGender, CustomerName, CustomerPhoneNo, CustomerEmail From Customer"
                                                           + " Where CustomerID = @customerID";
                myCmd.Parameters.AddWithValue("customerID", login.user.custID);

                myConn.Open();
                OleDbDataReader myDR = myCmd.ExecuteReader();
                myDR.Read();

                //extract information and display through the UI
                txtTitleProfile.Text = myDR[0].ToString();
                txtDOBProfile.Text = myDR[1].ToString();
                txtGenderProfile.Text = myDR[2].ToString();
                txtNameProfile.Text = myDR[3].ToString();
                txtPhoneProfile.Text = myDR[4].ToString();
                txtEmailProfile.Text = myDR[5].ToString();

                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        private void btnUpdateGeneralInfoProfile_Click(object sender, EventArgs e)
        {
            try
            {
                myConn.ConnectionString = DatabaseConnection.dbconnect;
                myConn.Open();

                OleDbCommand myCmd = myConn.CreateCommand();

                myCmd.CommandText = "UPDATE Customer SET CustomerTitle = @ct, CustomerGender = @cGender, CustomerName = @cName, CustomerPhoneNo = @cPhone"
                                                           + " Where CustomerID = " + login.user.custID;
                myCmd.Parameters.AddWithValue("@ct", txtTitleProfile.Text);
                myCmd.Parameters.AddWithValue("@cGender", txtGenderProfile.Text);
                myCmd.Parameters.AddWithValue("@cName", txtNameProfile.Text);
                myCmd.Parameters.AddWithValue("@cPhone", txtPhoneProfile.Text);



                int rowsChanged = myCmd.ExecuteNonQuery();

                myConn.Close();

                clearFieldsGenetalInfo();

                MessageBox.Show("Successfully updated! ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearGeneralInfo_Click(object sender, EventArgs e)
        {
            clearFieldsGenetalInfo();
        }

        private void clearFieldsGenetalInfo()//function which clears all the fields in the GENERAL INFORMATION group box (My profile)
        {

            txtTitleProfile.Clear();
            txtGenderProfile.Clear();
            txtNameProfile.Clear();
            txtPhoneProfile.Clear();
            txtEmailProfile.Clear();
            txtDOBProfile.Clear();


        }

        private void btnShowUpdateAddress_Click(object sender, EventArgs e)
        {
            grpProfileDetails.Visible = false;
            grpAddressUpdate.Visible = true;
            grpCardUpdate.Visible = false;
            grpPurchases.Visible = true;


            try
            {
                myConn.ConnectionString = DatabaseConnection.dbconnect; ;
                OleDbCommand myCmd = myConn.CreateCommand();

                myCmd.CommandText = "Select CustomerAddressNo, CustomerAddressStreet, CustomerAddressCity, CustomerAddressCountry, CustomerPostCode, CustomerPhoneNo From Customer"
                                                           + " Where CustomerID = @customerID";
                myCmd.Parameters.AddWithValue("customerID", login.user.custID);

                myConn.Open();
                OleDbDataReader myDR = myCmd.ExecuteReader();
                myDR.Read();

                //extract information and display through the UI
                txtHouseNoProfile.Text = myDR[0].ToString();
                txtStreetProfile.Text = myDR[1].ToString();
                txtCityProfile.Text = myDR[2].ToString();
                txtCountryProfile.Text = myDR[3].ToString();
                txtPostcodeProfile.Text = myDR[4].ToString();


                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        private void btnUpdateAddress_Click(object sender, EventArgs e)
        {
            try
            {
                myConn.ConnectionString = DatabaseConnection.dbconnect;
                myConn.Open();

                OleDbCommand myCmd = myConn.CreateCommand();

                myCmd.CommandText = "UPDATE Customer SET CustomerAddressNo = @ca, CustomerAddressStreet = @caStreet, CustomerAddressCity = @caCity, CustomerAddressCountry = @caCountry, CustomerPostCode = @caPostcode "
                                                           + " Where CustomerID = " + login.user.custID;
                myCmd.Parameters.AddWithValue("@caNo", txtHouseNoProfile.Text);
                myCmd.Parameters.AddWithValue("@caStreet", txtStreetProfile.Text);
                myCmd.Parameters.AddWithValue("@caCity", txtCityProfile.Text);
                myCmd.Parameters.AddWithValue("@caCountry", txtCountryProfile.Text);
                myCmd.Parameters.AddWithValue("@caPostcode", txtPostcodeProfile.Text);


                int rowsChanged = myCmd.ExecuteNonQuery();

                myConn.Close();

                MessageBox.Show("Successfully updated! ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearAddress_Click(object sender, EventArgs e)
        {
            clearFieldsAddress();
        }

        private void clearFieldsAddress()
        {

            txtHouseNoProfile.Clear();
            txtStreetProfile.Clear();
            txtCityProfile.Clear();
            txtCountryProfile.Clear();
            txtPostcodeProfile.Clear();

        }

        private void btnShowUpdateCardDetails_Click(object sender, EventArgs e)
        {
            grpProfileDetails.Visible = false;
            grpAddressUpdate.Visible = false;
            grpCardUpdate.Visible = true;
            grpPurchases.Visible = false;

            try
            {
                myConn.ConnectionString = DatabaseConnection.dbconnect; ;
                OleDbCommand myCmd = myConn.CreateCommand();

                myCmd.CommandText = "Select  CustomerPaymentCardType, CustomerPaymentCardNo, CustomerPaymentCardCVV, CustomerPaymentCardName,CustomerPaymentCardExpDate From Customer"
                                                           + " Where CustomerID = @customerID";
                myCmd.Parameters.AddWithValue("customerID", login.user.custID);

                myConn.Open();
                OleDbDataReader myDR = myCmd.ExecuteReader();
                myDR.Read();

                //extract information and display through the UI
                txtCardTypeProfile.Text = myDR[0].ToString();
                txtCardNoProfile.Text = myDR[1].ToString();
                txtCVVProfile.Text = myDR[2].ToString();
                txtHolderProfile.Text = myDR[3].ToString();
                txtExpDateProfile.Text = myDR[4].ToString();

                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        private void btnUpdateCardDetails_Click(object sender, EventArgs e)
        {
            try
            {
                myConn.ConnectionString = DatabaseConnection.dbconnect;
                myConn.Open();

                OleDbCommand myCmd = myConn.CreateCommand();

                myCmd.CommandText = "UPDATE Customer SET CustomerPaymentCardType = @cpType , CustomerPaymentCardNo = @cpCardNo, CustomerPaymentCardCVV = @cpCVV, CustomerPaymentCardName = @cpHolder,CustomerPaymentCardExpDate = @cpExpDate"
                                                           + " Where CustomerID = " + login.user.custID;
                myCmd.Parameters.AddWithValue("@cpType", txtCardTypeProfile.Text);
                myCmd.Parameters.AddWithValue("@cpCardNo", txtCardNoProfile.Text);
                myCmd.Parameters.AddWithValue("@cpCVV", txtCVVProfile.Text);
                myCmd.Parameters.AddWithValue("@cpHolder", txtHolderProfile.Text);
                myCmd.Parameters.AddWithValue("@cpExpDate", txtExpDateProfile.Text);


                int rowsChanged = myCmd.ExecuteNonQuery();

                myConn.Close();

                clearFieldsCardNo();

                MessageBox.Show("Successfully updated! ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearCard_Click(object sender, EventArgs e)
        {
            clearFieldsCardNo();
        }

        private void clearFieldsCardNo()//function which clears all the fields in the GENERAL INFORMATION group box (My profile)
        {

            txtCardTypeProfile.Clear();
            txtCardNoProfile.Clear();
            txtExpDateProfile.Clear();
            txtCVVProfile.Clear();
            txtHolderProfile.Clear();

        }

        private void btnViewPurchases_Click(object sender, EventArgs e)
        {
            grpPurchases.Visible = true;
            grpProfileDetails.Visible = false;
            grpAddressUpdate.Visible = false;
            grpCardUpdate.Visible = false;

            try
            {
                myConn.ConnectionString = DatabaseConnection.dbconnect; ;
                OleDbCommand myCmd = myConn.CreateCommand();

                myCmd.CommandText = "SELECT Orders.OrderID, Orders.OrderDate FROM  Orders, Invoice WHERE  Invoice.OrderID=Orders.OrderID AND Invoice.CustomerID = " + login.user.custID;

                MessageBox.Show(myCmd.CommandText);


                myConn.Open();
                OleDbDataReader myDR = myCmd.ExecuteReader();


                lstView.View = View.Details;

                while (myDR.Read())
                {
                    var item = new ListViewItem();
                    item.Text = myDR["OrderID"].ToString();
                    item.SubItems.Add(myDR["OrderDate"].ToString());


                    lstView.Items.Add(item);
                }


                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        private void btnBasketAdd_Click(object sender, EventArgs e)
        {
            lstBasket.Items.Add(this.txtShoeName.Text +", £" + this.txtPrice.Text);

            basket.Add(listViewDisplayProduct.Items[0].ImageIndex);

            MessageBox.Show("Added item to basket.");
        }

        private void btnClearItem_Click(object sender, EventArgs e)
        {
            int selectedIndex = lstBasket.SelectedIndex;
            if (selectedIndex >= 0)
            {
                basket.RemoveAt(selectedIndex);

                listViewDisplayBasket.Clear();
                lstBasket.Items.RemoveAt(selectedIndex);
            }

        }

        private void btnClearBasket_Click(object sender, EventArgs e)
        {
            //clears all the items in the list box
            DialogResult clearAll = MessageBox.Show("Are you sure you want to clear your basket", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (clearAll == DialogResult.Yes)
            {
                basket.Clear();
                lstBasket.Items.Clear();
                listViewDisplayBasket.Clear();
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            //clears all the items in the list box
            DialogResult checkOUt = MessageBox.Show("proceeds to checkout", "Warning!", MessageBoxButtons.OKCancel);
        }

        public void Populate(int shoeID)
        {
            try
            {

                clearViewProduct();
                using (OleDbConnection dbCon = new OleDbConnection(DatabaseConnection.dbconnect))
                {


                    dbCon.ConnectionString = DatabaseConnection.dbconnect;
                    OleDbCommand dbCmd = dbCon.CreateCommand();

                    dbCmd.CommandText = "SELECT * FROM Shoe WHERE ShoeID = @shoeid";
                    dbCmd.Parameters.AddWithValue("shoeid", shoeID);

                    dbCon.Open();
                    OleDbDataReader reader = dbCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        txtShoeName.Text = (reader["ShoeName"].ToString());
                        txtBrand.Text = (reader["ShoeBrand"].ToString());
                        txtShoeType.Text = (reader["ShoeSize"].ToString());
                        txtGender.Text = (reader["ShoeGender"].ToString());
                        txtColour.Text = (reader["ShoeColour"].ToString());
                        txtMaterial.Text = (reader["ShoeMaterial"].ToString());
                        txtPrice.Text = (reader["ShoePrice"].ToString());
                        listViewDisplayProduct.Items.Add("", reader.GetInt32(0));                
                    }

                    dbCon.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex);
            }
        }

        private void clearViewProduct()
        {

            txtShoeName.Clear();
            txtBrand.Clear();
            txtShoeType.Clear();
            txtGender.Clear();
            txtColour.Clear();
            txtMaterial.Clear();
            txtPrice.Clear();
            listViewDisplayProduct.Clear();

        }

        private void lstBasket_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lstBasket.SelectedItems.Count > 0)
            {
                listViewDisplayBasket.Clear();
                listViewDisplayBasket.Items.Add("", basket[lstBasket.SelectedIndex]);
            }
        }

        private void hideGrp()
        {
            grpMain.Visible = false;
            grpLogin.Visible = false;
            grpRegister.Visible = false;
            grpBasket.Visible = false;
            grpContact.Visible = false;
            grpProfile.Visible = false;
            grpViewProduct.Visible = false;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            hideGrp();
            grpMain.Visible = true;
        }


    }
}
