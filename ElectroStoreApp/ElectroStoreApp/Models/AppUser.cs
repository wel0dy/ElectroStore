using System;

namespace ElectroStoreApp.Models
{
    public class AppUser
    {
        public int UserId { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;

        public string FullName
        {
            get
            {
                return (LastName + " " + FirstName + " " + Patronymic).Trim();
            }
        }

        // Роль определяем по названию из таблицы user_role
        public UserRole Role
        {
            get
            {
                if (RoleName.IndexOf("Администратор", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return UserRole.Administrator;
                }
                if (RoleName.IndexOf("Менеджер", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return UserRole.Manager;
                }
                return UserRole.Client;
            }
        }
    }
}