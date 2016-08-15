using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator.Forms
{
    /// <summary>
    /// Login Form 
    /// 
    /// Author: Yong Dong
    /// Version: July 27 2016
    /// Objectives: 
    /// Allows user to login 
    /// </summary>
    public partial class Login : Form
    {
         /// Pre-define all the UI Elements below:
        private Label lblPromptText;
        private Label lblUsername;
        private Label lblPassword;
        public Label lblWarning;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnSubmit;
        // define some properties
        private string username;
        private string password; 
        /// <summary>
        /// Default constructor - Variant 1
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Login(string username = "", string password ="")
        {
            InitializeComponent();
           
           
            // load UI Elements
            this.loadUIElements(); 
            // set the login credentials
            if (username != "" && password != "")
            {
                this.username = username;
                this.password = password;
            }
        }
        ///<summary>
        ///Default constructor - Variant 2
        ///</summary>
        /// <param name="userAccountData">User Account Data as List</param>
        public Login(List<Object> userAccountData = null)
        {
            // load UI Elements
            this.loadUIElements(); 
            // compare the given username and password against the database results
        }
        /// <summary>
        /// Load UI Elements for display
        /// Set all the required properties and attributes for each UI elements
        /// </summary>
        private void loadUIElements()
        {
            // define some form attributes for the form:
            this.Text = "Calculator - User Login";

            this.MaximumSize = new System.Drawing.Size(400, 300);
            this.Width = 400;
            this.Height = 300;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += new FormClosingEventHandler(this.formClosingEvent);
           // Define the UI Elements for display
            // 
            this.lblPromptText = new Label();
            this.lblPromptText.Text = "Please enter your login credentials.";
            this.lblPromptText.AutoSize = true; 
            

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
            this.lblUsername.Location = new Point(locationX, locationY + 15);
            this.txtUsername.Location = new Point(locationX + txtUsername.Size.Width + 10,locationY+15);

            this.lblPassword.Location = new Point(locationX, locationY + 45);
            this.txtPassword.Location = new Point(locationX + txtPassword.Size.Width + 10, locationY + 45);

            this.lblWarning.Location = new Point(locationX, locationY + 70);


            this.btnSubmit.Location = new Point(locationX, this.Size.Height - (btnSubmit.Size.Height + 40));
            // add elements to the form 
            this.Controls.Add(this.lblPromptText);
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
        public bool isValidated = false; 
        private int submissionAttempts = 0;
        private void formSubmitEvent(object sender, System.EventArgs e)
        {
            // check the credentials to set isValidated value
            if (txtUsername.Text != "" && txtPassword.Text != "")
            {
                if (this.username == txtUsername.Text && this.password == txtPassword.Text)
                {
                    this.isValidated = true;
                    this.Close();
                }
                else
                {
                    if(this.submissionAttempts >= 3){
                        // force to terminate the program after 3 failure attempts
                        if (System.Windows.Forms.Application.MessageLoop) 
                        {
                            // WinForms app
                            System.Windows.Forms.Application.Exit();
                        }
                        else
                        {
                            // Console app
                            System.Environment.Exit(1);
                        }
                    }else{
                        this.lblWarning.Text = " (Invalid username or password.)"; 
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
