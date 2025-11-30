using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml.Linq;
using LocalComponents;

namespace WebApplication1
{
    /// <summary>
    /// Static helper class for managing XML-based account storage with encrypted passwords
    /// </summary>
    public static class AccountStore
    {
        private static readonly string MembersPath = HostingEnvironment.MapPath("~/App_Data/Members.xml");
        private static readonly string StaffPath = HostingEnvironment.MapPath("~/App_Data/Staff.xml");

        /// <summary>
        /// Ensures that both Members.xml and Staff.xml exist with proper structure.
        /// Creates default TA account in Staff.xml if file doesn't exist or has no staff accounts.
        /// </summary>
        public static void EnsureFiles()
        {
            // Ensure Members.xml exists
            if (!File.Exists(MembersPath))
            {
                var membersDoc = new XDocument(
                    new XDeclaration("1.0", "utf-8", null),
                    new XElement("Members")
                );
                membersDoc.Save(MembersPath);
            }

            // Ensure Staff.xml exists with default TA account
            bool needsDefaultStaff = false;
            
            if (!File.Exists(StaffPath))
            {
                needsDefaultStaff = true;
            }
            else
            {
                // Check if Staff.xml exists but has no staff accounts
                try
                {
                    var existingStaffDoc = XDocument.Load(StaffPath);
                    if (!existingStaffDoc.Descendants("Staff").Any())
                    {
                        needsDefaultStaff = true;
                    }
                }
                catch
                {
                    needsDefaultStaff = true;
                }
            }

            if (needsDefaultStaff)
            {
                // Create default staff account with encrypted password
                var defaultPasswordHash = EncryptionUtils.Encrypt("Cse445!");
                
                var staffDoc = new XDocument(
                    new XDeclaration("1.0", "utf-8", null),
                    new XElement("StaffList",
                        new XElement("Staff",
                            new XElement("Username", "TA"),
                            new XElement("PasswordHash", defaultPasswordHash)
                        )
                    )
                );
                staffDoc.Save(StaffPath);
            }
        }

        /// <summary>
        /// Validates staff credentials against Staff.xml
        /// </summary>
        /// <param name="username">Username to validate (case-insensitive)</param>
        /// <param name="plainPassword">Plain text password to validate</param>
        /// <returns>True if credentials are valid, false otherwise</returns>
        public static bool ValidateStaff(string username, string plainPassword)
        {
            EnsureFiles();

            try
            {
                var staffDoc = XDocument.Load(StaffPath);
                var encryptedPassword = EncryptionUtils.Encrypt(plainPassword);

                var matchingStaff = staffDoc.Descendants("Staff")
                    .FirstOrDefault(s => 
                        string.Equals(s.Element("Username")?.Value, username, StringComparison.OrdinalIgnoreCase) &&
                        s.Element("PasswordHash")?.Value == encryptedPassword);

                return matchingStaff != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Validates member credentials against Members.xml
        /// </summary>
        /// <param name="username">Username to validate (case-insensitive)</param>
        /// <param name="plainPassword">Plain text password to validate</param>
        /// <returns>True if credentials are valid, false otherwise</returns>
        public static bool ValidateMember(string username, string plainPassword)
        {
            EnsureFiles();

            try
            {
                var membersDoc = XDocument.Load(MembersPath);
                var encryptedPassword = EncryptionUtils.Encrypt(plainPassword);

                var matchingMember = membersDoc.Descendants("Member")
                    .FirstOrDefault(m => 
                        string.Equals(m.Element("Username")?.Value, username, StringComparison.OrdinalIgnoreCase) &&
                        m.Element("PasswordHash")?.Value == encryptedPassword);

                return matchingMember != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Registers a new member account
        /// </summary>
        /// <param name="username">Username for new account (case-insensitive check for duplicates)</param>
        /// <param name="plainPassword">Plain text password to encrypt and store</param>
        /// <returns>True if registration successful, false if username already exists</returns>
        public static bool RegisterMember(string username, string plainPassword)
        {
            EnsureFiles();

            try
            {
                var membersDoc = XDocument.Load(MembersPath);

                // Check if username already exists (case-insensitive)
                var existingMember = membersDoc.Descendants("Member")
                    .FirstOrDefault(m => 
                        string.Equals(m.Element("Username")?.Value, username, StringComparison.OrdinalIgnoreCase));

                if (existingMember != null)
                {
                    return false; // Username already exists
                }

                // Add new member with encrypted password
                var encryptedPassword = EncryptionUtils.Encrypt(plainPassword);
                var newMember = new XElement("Member",
                    new XElement("Username", username),
                    new XElement("PasswordHash", encryptedPassword)
                );

                membersDoc.Root.Add(newMember);
                membersDoc.Save(MembersPath);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Changes password for an existing member account
        /// </summary>
        /// <param name="username">Username of member account (case-insensitive)</param>
        /// <param name="oldPlainPassword">Current plain text password for verification</param>
        /// <param name="newPlainPassword">New plain text password to set</param>
        /// <returns>True if password changed successfully, false if current password is wrong or user not found</returns>
        public static bool ChangeMemberPassword(string username, string oldPlainPassword, string newPlainPassword)
        {
            // Validate input parameters
            if (string.IsNullOrWhiteSpace(username) || 
                string.IsNullOrWhiteSpace(oldPlainPassword) || 
                string.IsNullOrWhiteSpace(newPlainPassword))
            {
                return false;
            }

            EnsureFiles();

            try
            {
                var membersDoc = XDocument.Load(MembersPath);
                var encryptedOldPassword = EncryptionUtils.Encrypt(oldPlainPassword);
                var encryptedNewPassword = EncryptionUtils.Encrypt(newPlainPassword);

                // Find member with matching username and current password
                var memberElement = membersDoc.Descendants("Member")
                    .FirstOrDefault(m => 
                        string.Equals(m.Element("Username")?.Value, username, StringComparison.OrdinalIgnoreCase) &&
                        m.Element("PasswordHash")?.Value == encryptedOldPassword);

                if (memberElement == null)
                {
                    return false; // Member not found or current password is incorrect
                }

                // Update password hash with new encrypted password
                memberElement.Element("PasswordHash").Value = encryptedNewPassword;
                membersDoc.Save(MembersPath);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}