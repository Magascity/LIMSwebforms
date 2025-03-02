using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using LIMSwebforms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LIMSwebforms.UserManagement
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        ApplicationUserManager userMgr;
        ApplicationRoleManager roleMgr;

        protected void Page_Load(object sender, EventArgs e)
        {
            userMgr = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            roleMgr = Context.GetOwinContext().Get<ApplicationRoleManager>();

            if(!Page.IsPostBack)
            {

             //   BindUsersDropdown();
                BindRoles();
            }
        }

        private void BindUsersDropdown()
        {
            // Bind dropdown with users
            ddlUsers.DataSource = userMgr.Users.ToList();
            ddlUsers.DataValueField = "Id";
            ddlUsers.DataTextField = "UserName";
            ddlUsers.DataBind();

            // Add default item
            ddlUsers.Items.Insert(0, new ListItem("--Select a User--", ""));
        }

        // Select methods
        public IQueryable<IdentityRole> grdRoles_GetData()
        {
            return roleMgr.Roles;
        }
        public IQueryable<ApplicationUser> grdUsers_GetData()
        {
            return userMgr.Users;
        }
        public Object dvUsers_GetItem([Control] string grdUsers)
        {
            if (grdUsers == null) return new ApplicationUser();
            return (from u in userMgr.Users
                    where u.Id == grdUsers
                    select u).SingleOrDefault();
        }
        public object dvRoles_GetItem([Control] string grdRoles)
        {
            if (grdRoles == null) return new IdentityRole();
            return (from r in roleMgr.Roles
                    where r.Id == grdRoles
                    select r).SingleOrDefault();
        }

        // Update methods
        public void dvUsers_UpdateItem(string Id)
        {
            ApplicationUser user = (from u in userMgr.Users
                                    where u.Id == Id
                                    select u).SingleOrDefault();
            TryUpdateModel(user);
            user.UserName = user.Email; // assign email to username
            IdentityResult result = userMgr.Update(user);
            if (result.Succeeded) Reload();
        }
        public void dvRoles_UpdateItem(string Id)
        {
            IdentityRole role = (from r in roleMgr.Roles
                                 where r.Id == Id
                                 select r).SingleOrDefault();
            TryUpdateModel(role);
            IdentityResult result = roleMgr.Update(role);
            if (result.Succeeded) Reload();
        }

        // Insert methods
        public void dvUsers_InsertItem()
        {
            ApplicationUser user = new ApplicationUser();
            TryUpdateModel(user);
            user.UserName = user.Email; // assign email to username
            IdentityResult result = userMgr.Create(user);
            if (result.Succeeded) Reload();
        }
        public void dvRoles_InsertItem()
        {
            IdentityRole role = new IdentityRole();
            TryUpdateModel(role);
            IdentityResult result = roleMgr.Create(role);
            if (result.Succeeded) Reload();
        }

        // Delete methods
        public void dvUsers_DeleteItem(string Id)
        {
            ApplicationUser user = (from u in userMgr.Users
                                    where u.Id == Id
                                    select u).SingleOrDefault();
            IdentityResult result = userMgr.Delete(user);
            if (result.Succeeded) Reload();
        }
        public void dvRoles_DeleteItem(string Id)
        {
            IdentityRole role = (from r in roleMgr.Roles
                                 where r.Id == Id
                                 select r).SingleOrDefault();
            IdentityResult result = roleMgr.Delete(role);
            if (result.Succeeded) Reload();
        }

        // Add roles to users
        //protected void btnAddRoles_Click(object sender, EventArgs e)
        //{
        //    string userID = ddlUsers.SelectedValue;
        //    foreach (ListItem item in lstRoles.Items)
        //    {
        //        // if role is selected and user is not in it, add user to role
        //        if (item.Selected)
        //        {
        //            if (!userMgr.IsInRole(userID, item.Text))
        //            {
        //                userMgr.AddToRole(userID, item.Text);
        //            }
        //        }
        //        // if role is not selected and user is in it, remove user from role
        //        else
        //        {
        //            if (userMgr.IsInRole(userID, item.Text))
        //            {
        //                userMgr.RemoveFromRole(userID, item.Text);
        //            }
        //        }
        //    }

        //    grdUsers.DataBind();
        //}

        protected void btnAddRoles_Click(object sender, EventArgs e)
        {
            string userID = ddlUsers.SelectedValue;

            // Check if default item is selected
            if (string.IsNullOrEmpty(userID))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastMessage", "toastr.error('Please select a valid user.');", true);
                return;
            }

            foreach (ListItem item in chkRoles.Items)
            {
                // Role assignment logic
                if (item.Selected)
                {
                    if (!userMgr.IsInRole(userID, item.Text))
                    {
                        userMgr.AddToRole(userID, item.Text);
                    }
                }
                else
                {
                    if (userMgr.IsInRole(userID, item.Text))
                    {
                        userMgr.RemoveFromRole(userID, item.Text);
                    }
                }
            }

            grdUsers.DataBind();
        }


        // Helper methods
        public string ListRoles(ICollection<IdentityUserRole> userRoles)
        {
            IdentityRole role;
            var names = new List<string>();

            foreach (var ur in userRoles)
            {
                role = (from r in roleMgr.Roles
                        where r.Id == ur.RoleId
                        select r).SingleOrDefault();
                names.Add(role.Name);
            }
            return string.Join(", ", names);
        }

        private void Reload()
        {
            grdUsers.DataBind();
            // grdRoles.DataBind();
            ddlUsers.DataBind();

            //lstRoles.DataBind();
        }

        // Provide for formatting GridView controls with Bootstrap
        protected void GridView_PreRender(object sender, EventArgs e)
        {
            GridView grd = (GridView)sender;
            if (grd.HeaderRow != null)
                grd.HeaderRow.TableSection = TableRowSection.TableHeader;
        }


        private void BindRoles()
        {
            chkRoles.DataSource = roleMgr.Roles.ToList();
            chkRoles.DataTextField = "Name";
            chkRoles.DataValueField = "Id";
            chkRoles.DataBind();
        }

        protected void grdUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ToggleLock")
            {
                string userId = e.CommandArgument.ToString();

                // Get the user
                var user = userMgr.FindById(userId);
                if (user != null)
                {
                    // Toggle lock status
                    if (user.LockoutEnabled && user.LockoutEndDateUtc > DateTime.UtcNow)
                    {
                        // Unlock the user
                        user.LockoutEndDateUtc = null;
                        userMgr.Update(user);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastMessage", $"toastr.success('{user.Email} has been unlocked.');", true);
                    }
                    else
                    {
                        // Lock the user
                        user.LockoutEndDateUtc = DateTime.UtcNow.AddYears(100); // Lock indefinitely
                        userMgr.Update(user);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastMessage", $"toastr.warning('{user.Email} has been locked.');", true);
                    }

                    // Refresh the GridView to reflect the changes
                    grdUsers.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastMessage", "toastr.error('User not found.');", true);
                }
            }


        }

        protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUserId = ddlUsers.SelectedValue;

            if (string.IsNullOrEmpty(selectedUserId))
            {
                chkRoles.ClearSelection();
                return;
            }

            // Get the roles for the selected user
            var userRoles = userMgr.GetRoles(selectedUserId);

            // Clear any previous selection
            chkRoles.ClearSelection();

            // Select the roles in the CheckBoxList
            foreach (ListItem roleItem in chkRoles.Items)
            {
                if (userRoles.Contains(roleItem.Text))
                {
                    roleItem.Selected = true;
                }
            }
        }

    }
}