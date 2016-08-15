using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Calculator.Utilities;
namespace Calculator.Forms
{
    /// <summary>
    /// DatabaseConnection form
    /// A User Interface for entering MySQL database connection info
    /// 
    /// Author: Yong Dong
    /// Version: July 27 2016
    /// </summary>
    public partial class DatabaseConnection : Form
    {
        /// Pre-define all the UI Elements below:
        private Label lblPromptText;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblServerAddress;
        private Label lblDatabase; 
        private Label lblWarning;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtServerAddress;
        private TextBox txtDatabase;
        private Button btnSubmit;

       
        public DatabaseConnection()
        {
            InitializeComponent();

            // define some form attributes:
            this.Text = "Calculator - Database Connection";
            this.MaximumSize = new System.Drawing.Size(500, 400);
            this.Width = 500;
            this.Height = 400;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += new FormClosingEventHandler(this.formClosingEvent);
            // load UI Elements
            this.loadUIElements(); 
        }
        /// <summary>
        /// Load UI Elements for display
        /// Set all the required properties and attributes for each UI elements
        /// </summary>
        private void loadUIElements()
        {
            // Define the UI Elements for display
            // 
            this.lblPromptText = new Label();
            this.lblPromptText.Text = "Please enter your database login credentials.";
            this.lblPromptText.AutoSize = true;

            this.lblServerAddress = new Label();
            this.lblServerAddress.Text = "Server Address: ";
            this.lblServerAddress.TextAlign = ContentAlignment.MiddleRight;

            this.lblDatabase = new Label();
            this.lblDatabase.Text = "Database: ";
            this.lblDatabase.TextAlign = ContentAlignment.MiddleRight;

            this.lblUsername = new Label();
            this.lblUsername.Text = "Username: ";
            this.lblUsername.TextAlign = ContentAlignment.MiddleRight;

            this.lblPassword = new Label();
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = ContentAlignment.MiddleRight;

            this.lblWarning = new Label();
            this.lblWarning.Text = "";
            this.lblWarning.TextAlign = ContentAlignment.MiddleRight;
            this.lblWarning.AutoSize = true;
            this.lblWarning.ForeColor = Color.DarkRed; 

            this.txtServerAddress = new TextBox();

            this.txtDatabase = new TextBox();
            this.txtUsername = new TextBox();

            this.txtPassword = new TextBox();
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.PasswordChar = '*';

            this.btnSubmit = new Button();
            this.btnSubmit.Text = "SUBMIT";
            this.btnSubmit.Size = new Size(100, 25);
            this.btnSubmit.Anchor = AnchorStyles.Bottom;
            this.btnSubmit.Click += new EventHandler(this.formSubmitEvent);

            // set elements location 
            // according to the current windows size 
            int offsetFromOrigin = 15;
            int x = 0;
            int y = 0;

            int locationX = x + offsetFromOrigin;
            int locationY = y + offsetFromOrigin + 5;

            this.lblPromptText.Location = new Point(locationX, locationY);

            this.lblUsername.Location = new Point(locationX, locationY + 20);
            this.txtUsername.Location = new Point(locationX + txtUsername.Size.Width + 10, locationY + 20);

            this.lblPassword.Location = new Point(locationX, locationY + 50);
            this.txtPassword.Location = new Point(locationX + txtPassword.Size.Width + 10, locationY + 50);

            this.lblServerAddress.Location = new Point(locationX, locationY + 80);
            this.txtServerAddress.Location = new Point(locationX + txtServerAddress.Size.Width + 10, locationY + 80);
            
            this.lblDatabase.Location = new Point(locationX, locationY + 110);
            this.txtDatabase.Location = new Point(locationX + txtDatabase.Size.Width + 10, locationY + 110);
           
            this.lblWarning.Location = new Point(locationX, locationY + 140);


            this.btnSubmit.Location = new Point(locationX, this.Size.Height - (btnSubmit.Size.Height + 40));
            // add elements to the form 
            this.Controls.Add(this.lblPromptText);
            this.Controls.Add(this.lblServerAddress);
            this.Controls.Add(this.txtServerAddress);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.txtDatabase);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.lblWarning);
        }
        /// <summary>
        /// This will disabled the close button on top so that we can solely use the submit button 
        /// </summary>
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        ///------------------------------------------------------------------------------
        /// Form events: 
        /// 


        public Utilities.MySQLDatabaseAccess dbAccess;
        public String errorMsg = "";
        private int submissionAttempts = 0;
        public void formSubmitEvent(object sender, System.EventArgs e)
        {
            // check the credentials and then start the connection 
            if (this.txtUsername.Text != "" && this.txtPassword.Text != "" && this.txtDatabase.Text != "" && this.txtServerAddress.Text != "")
            {
                this.dbAccess = new MySQLDatabaseAccess(txtServerAddress.Text,txtUsername.Text,txtPassword.Text,txtDatabase.Text);
                if (dbAccess.onError)
                {
                    /// force to close the form after 3 failure attempts
                    /// but not the application itself
                    if(this.submissionAttempts >= 3){
                        this.errorMsg = " (Invalid database login credentials.)";
                        this.Close();
                    }
                    else
                    {
                        this.lblWarning.Text = "(Invalid database login credentials. Please try again!)";
                    }
                        
                }
            }
            else
            {
                this.lblWarning.Text = " (Please complete all the required fields.)";

            }
            // Counting number of times submit button is clicked
            // the maximum allowed is 3 
            this.submissionAttempts = this.submissionAttempts + 1;

        }
        private void formClosingEvent(object sender, System.EventArgs e)
        {

            Utilities.ErrorLog.writeToFile();
        }
    }
}
