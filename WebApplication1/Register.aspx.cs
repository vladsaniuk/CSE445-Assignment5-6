using System;
using System.Drawing;
using System.Web.UI;

namespace WebApplication1
{
    public partial class Register : Page
    {
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // Clear previous messages
            lblRegisterMessage.Text = "";

            // Get input values
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string email = txtEmail.Text.Trim(); // Optional field

            // Basic validation
            if (string.IsNullOrEmpty(username))
            {
                lblRegisterMessage.ForeColor = Color.Red;
                lblRegisterMessage.Text = "Username is required.";
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                lblRegisterMessage.ForeColor = Color.Red;
                lblRegisterMessage.Text = "Password is required.";
                return;
            }

            // Password confirmation validation
            if (!string.Equals(password, confirmPassword))
            {
                lblRegisterMessage.ForeColor = Color.Red;
                lblRegisterMessage.Text = "Password and confirmation do not match.";
                return;
            }

            // Password policy enforcement
            if (password.Length < 6)
            {
                lblRegisterMessage.ForeColor = Color.Red;
                lblRegisterMessage.Text = "Password must be at least 6 characters long.";
                return;
            }

            // Attempt to register the member
            bool registrationSuccess = AccountStore.RegisterMember(username, password);

            if (registrationSuccess)
            {
                lblRegisterMessage.ForeColor = Color.Green;
                lblRegisterMessage.Text = "Registration successful! You can now log in with your new credentials.";
                
                // Clear the form fields
                txtUsername.Text = "";
                txtPassword.Text = "";
                txtConfirmPassword.Text = "";
                txtEmail.Text = "";
            }
            else
            {
                lblRegisterMessage.ForeColor = Color.Red;
                lblRegisterMessage.Text = "Registration failed. Username may already exist.";
            }
        }
    }
}