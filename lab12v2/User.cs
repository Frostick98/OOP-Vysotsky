namespace lab12v2;

/// <summary>
/// Сутність за варіантом 2: користувач.
/// </summary>
public class User : IEntity
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public override string ToString() =>
        $"User #{Id}: {Username}, {Email}, активний: {(IsActive ? "так" : "ні")}";
}
