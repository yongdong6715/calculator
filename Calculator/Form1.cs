using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Calculator.Forms;

namespace Calculator
{
    /// <summary>
    /// Calculator Windows Application
    /// 
    /// Author: Yong Dong
    /// Version: July 26, 2016
    /// Objectives: 
    /// Simplify the mathenatic calculations of most common calculations we would perform daily
    /// 
    /// Disclaimers: Front-end
    /// No calculations should be done at this level. 
    /// This is purely for display and taking user inputs
    /// </summary>
    public partial class Form1 : Form
    {
        // default user credentials (for testing purposes only)
        // as standard practice, the existing user account info should always be pulled from a sql table. 
        // 
        private string username = "user1";
        private string password = "qwerty9876"; 

        private bool isLoggedIn = false; 
        public Form1()
        {
            InitializeComponent();
            // defining some form attributes:
            this.Text = "Calculator";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Events.AddHandler(null,null);
            this.FormClosing += new FormClosingEventHandler(this.formClosingEvent);
            

           // try to get the log in info from the settings.settings
            this.isLoggedIn = Properties.Settings.Default.isLoggedIn; 
           // open up the login dialog if not already logged in 
            if (!isLoggedIn)
            {
                // connect to the database to retrieve user info
                Calculator.Forms.DatabaseConnection dbConnFrm = new Forms.DatabaseConnection();
                dbConnFrm.ShowDialog();
               
                if (!dbConnFrm.dbAccess.onError)
                {
                    // query the user account data table to retrieve user info
                    String query = "SELECT * FROM UserAccount";
                    // display the login form
                    Calculator.Forms.Login loginFrm = new Forms.Login(dbConnFrm.dbAccess.runQuery(query));
                    loginFrm.ShowDialog();
                    if (loginFrm.isValidated)
                    {
                        // Update the settings
                        Properties.Settings.Default.isLoggedIn = loginFrm.isValidated;
                        // load the UI Element for the main form 
                        this.loadUIElements();
                    }
                }
                else
                {
                    // try the default credentials
                    // display the login form
                    Calculator.Forms.Login loginFrm = new Forms.Login(this.username, this.password);
                    loginFrm.Text = loginFrm.Text + " - " + dbConnFrm.errorMsg;
                    loginFrm.lblWarning.Text = dbConnFrm.errorMsg; 
                    loginFrm.ShowDialog();
                    if (loginFrm.isValidated)
                    {
                        // Update the settings
                        Properties.Settings.Default.isLoggedIn = loginFrm.isValidated;
                        // load the UI Element for the main form
                        this.loadUIElements();
                    }
                }
               
            }
            else
            {
                // load the UI Element for the main form
                this.loadUIElements();
            }
        }
        /// Pre-define all the UI Elements below:
        private TabControl tabCtrl; 


        /// <summary>
        /// Load UI Elements for display
        /// Set all the required properties and attributes for each UI elements
        ///
        /// This method will not be called until user is logged in
        /// </summary>
        private void loadUIElements()
        {
            // Define the UI Elements for display
            // 
            this.tabCtrl = new TabControl();
            // Add some tabs....
            TabPage tabHome = new TabPage("HOME");
            tabHome.Name = "tabHome";
            tabHome.Tag = "tabHome";
            // Add some controls to the home page tab...
            /**
            Label lblTemp = new Label();
            lblTemp.Text = "Welcome! The app is currently under construction. Please come back later...";
            float currentSize = lblTemp.Font.Size;
            currentSize += 10.0F;
            lblTemp.Font = new Font(lblTemp.Font.Name, currentSize,
                lblTemp.Font.Style, lblTemp.Font.Unit);
            lblTemp.TextAlign = ContentAlignment.MiddleCenter;
            lblTemp.AutoSize = true;**/
            
            // Defines the x axis and y axis for the home page buttons control 
            int offsetFromOrigin = 15;
            int x = 0;
            int y = 0;
            // Defines the default size of the buttons
            int width = 100;
            int height = 100; 
            // Create some buttons
            Button btnExpenseCalc = new Button();
            btnExpenseCalc.Text = "Expense Calculator";
            btnExpenseCalc.AutoEllipsis = false; 
            btnExpenseCalc.Size = new Size(width, height);
            btnExpenseCalc.Location = new Point(x + offsetFromOrigin, y + offsetFromOrigin);
            btnExpenseCalc.Click += new EventHandler(this.btnExpenseCalcClick);


            Button btnTipCalculator = new Button();
            btnTipCalculator.Text = "Tip Calculator";
            btnTipCalculator.AutoEllipsis = false; 
            btnTipCalculator.Size = new Size(width, height);
            btnTipCalculator.Location = new Point(btnExpenseCalc.Location.X + btnExpenseCalc.Size.Width + offsetFromOrigin,btnExpenseCalc.Location.Y);
            btnTipCalculator.Click += new EventHandler(this.btnTipCalculatorClick);


            Button btnBudgetPlanner = new Button();
            btnBudgetPlanner.Text = "Budget Planner";
            btnBudgetPlanner.AutoEllipsis = false;
            btnBudgetPlanner.Size = new Size(width, height);
            btnBudgetPlanner.Location = new Point(btnTipCalculator.Location.X + btnTipCalculator.Size.Width + offsetFromOrigin, btnTipCalculator.Location.Y);
            btnBudgetPlanner.Click += new EventHandler(this.btnBudgetPlannerClick);


            // Add controls to home page tab
           // tabHome.Controls.Add(lblTemp);
            tabHome.Controls.Add(btnExpenseCalc);
            tabHome.Controls.Add(btnTipCalculator);
            tabHome.Controls.Add(btnBudgetPlanner);

            this.tabCtrl.TabPages.Add(tabHome);
            // set elements location 
            // according to the current windows size 
            this.tabCtrl.Dock = DockStyle.Fill;
            // add elements to the form 
            this.Controls.Add(tabCtrl);
        }
        /// ----------------------------------------------------------------------------
        /// Helper UI functions
        /// 
        private void closeSelectedTab(object sender, System.EventArgs e, TabPage tab)
        {
           // TabPage tab = sender as TabPage;

            this.tabCtrl.Controls.Remove(tab);
            this.tabCtrl.SelectTab("tabHome");
            //this.tabCtrl.SelectedTab = this.tabCtrl.GetControl(this.tabCtrl.Controls.IndexOf())
        }
       
        ///------------------------------------------------------------------------------
        /// Form events: 
        /// 
        private void formClosingEvent(object sender, System.EventArgs e) 
        {

            Utilities.ErrorLog.writeToFile();
        }

        private void btnExpenseCalcClick(object sender, System.EventArgs e)
        {
            TabPage tabExpenseCal = new TabPage();
            tabExpenseCal.Text = "Expense Calculator";
            tabExpenseCal.Tag = "ExpenseCalculatorTab";
            tabExpenseCal.Name = "ExpenseCalculatorTab";
            ContextMenu ctxMenu = new ContextMenu();
            MenuItem closeCurrentTab = new MenuItem("Close");
            closeCurrentTab.Click += new EventHandler((send, ex) => closeSelectedTab(send, ex, tabExpenseCal));

          
          
            ctxMenu.MenuItems.Add(closeCurrentTab);
            tabExpenseCal.ContextMenu = ctxMenu;
            this.tabCtrl.Controls.Add(tabExpenseCal);
            this.tabCtrl.SelectedTab = tabExpenseCal;

        }

        private void btnTipCalculatorClick(object sender, System.EventArgs e)
        {
            TabPage tabTipCal = new TabPage();
            tabTipCal.Text = "Tip Calculator";
            tabTipCal.Tag = "TipCalculatorTab";
            tabTipCal.Name = "TipCalculatorTab";
            ContextMenu ctxMenu = new ContextMenu();
            MenuItem closeCurrentTab = new MenuItem("Close");
            closeCurrentTab.Click += new EventHandler((send, ex) => closeSelectedTab(send, ex, tabTipCal));

       
            ctxMenu.MenuItems.Add(closeCurrentTab);
            tabTipCal.ContextMenu = ctxMenu;
            this.tabCtrl.Controls.Add(tabTipCal);
            this.tabCtrl.SelectedTab = tabTipCal;
        }
        private void btnBudgetPlannerClick(object sender, System.EventArgs e)
        {
            TabPage tabBudgetPlannerCal = new TabPage();
            tabBudgetPlannerCal.Text = "Budget Planner";
            tabBudgetPlannerCal.Tag = "BudgetPlannerCalculatorTab";
            tabBudgetPlannerCal.Name = "BudgetPlannerCalculatorTab";
            ContextMenu ctxMenu = new ContextMenu();
            MenuItem closeCurrentTab = new MenuItem("Close");
            closeCurrentTab.Click += new EventHandler((send, ex) => closeSelectedTab(send, ex, tabBudgetPlannerCal));

            ctxMenu.MenuItems.Add(closeCurrentTab);
            tabBudgetPlannerCal.ContextMenu = ctxMenu;
            this.tabCtrl.Controls.Add(tabBudgetPlannerCal);
            this.tabCtrl.SelectedTab = tabBudgetPlannerCal;
        }
    }
}
