namespace EmployeeManagementSystem.Model
{
    public class UserAuthorizationToken
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Reference to the user
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
