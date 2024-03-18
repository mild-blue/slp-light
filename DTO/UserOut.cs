namespace slp.light.DTO
{
    public class UserOut
    {
        public required string Username { get; set; }
        public required string FullName { get; set; }
        public required IEnumerable<string> Roles { get; set; }
        public required bool IsActive { get; set; }
    }
}
