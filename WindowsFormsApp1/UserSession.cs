public static class UserSession
{
    public static int UserID { get; set; }
    public static string Email { get; set; }
    public static string FullName { get; set; }
    public static string Role { get; set; }

    public static bool IsLoggedIn => UserID != 0; // Check if a user is logged in

    public static void ClearSession()
    {
        UserID = 0;
        Email = null;
        FullName = null;
        Role = null;
    }
}
