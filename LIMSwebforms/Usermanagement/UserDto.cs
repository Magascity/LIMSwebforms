﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCorp.UserManagement
{
    public class UserDto
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public ICollection<IdentityUserRole> Roles { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
    }

}