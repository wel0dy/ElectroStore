using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroStoreApp.Models
{
    public static class Session
    {
        public static AppUser CurrentUser { get; set; }

        public static UserRole CurrentRole
        {
            get
            {
                if (CurrentUser == null)
                {
                    return UserRole.Guest;
                }
                return CurrentUser.Role;
            }
        }
    }
}