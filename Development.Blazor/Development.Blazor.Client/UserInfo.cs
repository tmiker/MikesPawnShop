namespace Development.Blazor.Client
{
    public class UserInfo
    {
        // Used by PersistingAuthenticationStateProvider in Server and PersistentAuthenticationStateProvider in Client
        public required string UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Sub { get; set; }
    }
}
