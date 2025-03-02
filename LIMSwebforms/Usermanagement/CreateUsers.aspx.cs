using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LIMSwebforms.Models;
using System.Data.SqlClient;
using System.Web.Configuration;
using Microsoft.AspNet.Identity.EntityFramework;


namespace LIMSwebforms.Usermanagement
{
    public partial class CreateUsers : System.Web.UI.Page
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        

        //protected void CreateUser_Click(object sender, EventArgs e)
        //{
        //    // Create an instance of ApplicationDbContext with the CustomConnection string
        //    var customContext = ApplicationDbContext.Create("CustomConnection");

        //    // Create an instance of UserStore with the custom context
        //    var userStore = new UserStore<ApplicationUser>(customContext);

        //    // Create an instance of ApplicationUserManager with the UserStore
        //    var manager = new ApplicationUserManager(userStore);

        //    // Continue with your existing logic
        //  //  var signInManager = new ApplicationSignInManager(manager);

        //    var user = new ApplicationUser()
        //    {
        //        UserName = Email.Text,
        //        Email = Email.Text,
        //        LastName = txtLastname.Text,
        //        Othernames = txtOthernames.Text,
        //        Sbu = ddlSbu.SelectedItem.Value
        //    };

        //    IdentityResult result = manager.Create(user, Password.Text);

        //    if (result.Succeeded)
        //    {
        //        // Your success handling logic

        //        Response.Redirect("ManageUsers");
        //    }
        //    else
        //    {
        //        ErrorMessage.Text = result.Errors.FirstOrDefault();
        //    }
        //}


        protected void CreateUser_Click(object sender, EventArgs e)
        {

            // Create an instance of ApplicationDbContext with the CustomConnection string
            // var customContext = ApplicationDbContext.Create("CustomConnection");

            // Use the custom context to get the ApplicationUserManager and ApplicationSignInManager
            // var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(customContext));




            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = txtUserName.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                //string code = manager.GenerateEmailConfirmationToken(user.Id);
                //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                //signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                //IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);

                Response.Redirect("ManageUsers");
            }
            else
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }


        //protected void CreateUser_Click(object sender, EventArgs e)
        //{
        //    // Initialize User Manager and Sign-In Manager
        //    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();

        //    // Create the user object
        //    var user = new ApplicationUser()
        //    {
        //        UserName = Email.Text,
        //        Email = Email.Text,
        //        LastName = txtLastname.Text,
        //        Othernames = txtOthernames.Text,
        //        Sbu = ddlSbu.SelectedItem.Value
        //    };

        //    // Generate password if needed or use the provided password
        //    string generatedPassword = Password.Text;

        //    // Create the user
        //    IdentityResult result = manager.Create(user, generatedPassword);

        //    if (result.Succeeded)
        //    {
        //        try
        //        {
        //            // Send SMS and Email Notifications
        //            SendMessages messageService = new SendMessages();

        //            // Send Email
        //            string emailSubject = "Your Account Details";
        //            string emailBody = $"Dear {user.LastName} {user.Othernames},<br/><br/>" +
        //                               $"Your account has been created successfully.<br/>" +
        //                               $"Username: {user.Email}<br/>" +
        //                               $"Password: <strong>{generatedPassword}</strong><br/><br/>" +
        //                               "Please change your password upon first login.";
        //            messageService.SendEmail(emailBody, user.Email, emailSubject);

        //            // Send SMS (ensure phone number field is available in the UI and database)
        //            string smsMessage = $"Dear {user.LastName}, your account has been created successfully. Username: {user.Email}, Password: {generatedPassword}";
        //            //string phoneNumber = txtMobile.Text; // Assuming txtMobile is the textbox for phone number input
        //           // messageService.SendSMS(smsMessage, phoneNumber);

        //            // Redirect to Manage Users
        //            Response.Redirect("ManageUsers");
        //        }
        //        catch (Exception ex)
        //        {
        //            // Display error message if notifications fail
        //            ErrorMessage.Text = $"User created successfully, but notifications failed: {ex.Message}";
        //            //ErrorMessage. = System.Drawing.Color.Orange;
        //        }
        //    }
        //    else
        //    {
        //        // Display registration error
        //        ErrorMessage.Text = result.Errors.FirstOrDefault();
        //       // ErrorMessage.ForeColor = System.Drawing.Color.Red;
        //    }
        //}


    }
}