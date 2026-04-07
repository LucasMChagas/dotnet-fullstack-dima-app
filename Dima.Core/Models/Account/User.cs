namespace Dima.Core.Models.Account;

public class User
{
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; } = false;
    public Dictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();
}