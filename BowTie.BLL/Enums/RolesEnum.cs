namespace BowTie.BLL.Enums
{
    public enum RolesEnum
    {
        User,
        Admin,
        Expert
    }

    public static class EnumExtensions
    {
        public static string GetName(this RolesEnum role)
        {
            switch (role)
            {
                case RolesEnum.User:
                    return "Користувач";
                case RolesEnum.Admin:
                    return "Адміністратор";
                case RolesEnum.Expert:
                    return "Експерт";
                default:
                    return "Гость";
            }
        }
    }
}
