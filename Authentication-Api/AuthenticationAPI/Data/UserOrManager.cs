namespace AuthenticationAPI.Data
{
    public class UserOrManager
    {
        public int userid {  get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public DateTime? Last_Password_Change { get; set; }
        public int roleid { get; set; }
    }
}
