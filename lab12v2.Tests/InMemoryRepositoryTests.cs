using lab12v2;

namespace lab12v2.Tests;

public class InMemoryRepositoryTests
{
    private static User CreateUser(string name = "test", int id = 0) => new()
    {
        Id = id,
        Username = name,
        Email = $"{name}@example.com",
        IsActive = true
    };

    // ---------- Add ----------

    [Fact]
    public void Add_NewItem_AssignsIdAndStoresIt()
    {
        var repo = new InMemoryRepository<User>();
        var user = CreateUser("anna");

        repo.Add(user);

        Assert.Equal(1, user.Id);
        Assert.Single(repo.GetAll());
        Assert.Same(user, repo.GetById(1));
    }

    [Fact]
    public void Add_TwoItems_AssignsDifferentIds()
    {
        var repo = new InMemoryRepository<User>();
        var first = CreateUser("a");
        var second = CreateUser("b");

        repo.Add(first);
        repo.Add(second);

        Assert.NotEqual(first.Id, second.Id);
        Assert.Equal(2, repo.GetAll().Count());
    }

    [Fact]
    public void Add_DuplicateId_ThrowsArgumentException()
    {
        var repo = new InMemoryRepository<User>();
        repo.Add(CreateUser("a", id: 5));

        Assert.Throws<ArgumentException>(() => repo.Add(CreateUser("b", id: 5)));
    }

    [Fact]
    public void Add_Null_ThrowsArgumentNullException()
    {
        var repo = new InMemoryRepository<User>();

        Assert.Throws<ArgumentNullException>(() => repo.Add(null!));
    }

    // ---------- GetById ----------

    [Fact]
    public void GetById_ExistingId_ReturnsItem()
    {
        var repo = new InMemoryRepository<User>();
        var user = CreateUser("anna");
        repo.Add(user);

        var result = repo.GetById(user.Id);

        Assert.Equal("anna", result.Username);
    }

    [Fact]
    public void GetById_NonExistingId_ThrowsKeyNotFoundException()
    {
        var repo = new InMemoryRepository<User>();
        repo.Add(CreateUser("anna"));

        Assert.Throws<KeyNotFoundException>(() => repo.GetById(999));
    }

    // ---------- GetAll ----------

    [Fact]
    public void GetAll_EmptyRepository_ReturnsEmptyCollection()
    {
        var repo = new InMemoryRepository<User>();

        Assert.Empty(repo.GetAll());
    }

    [Fact]
    public void GetAll_WithItems_ReturnsAllItems()
    {
        var repo = new InMemoryRepository<User>();
        repo.Add(CreateUser("a"));
        repo.Add(CreateUser("b"));
        repo.Add(CreateUser("c"));

        var all = repo.GetAll().ToList();

        Assert.Equal(3, all.Count);
        Assert.Contains(all, u => u.Username == "a");
        Assert.Contains(all, u => u.Username == "b");
        Assert.Contains(all, u => u.Username == "c");
    }

    // ---------- Update ----------

    [Fact]
    public void Update_ExistingItem_ChangesStoredData()
    {
        var repo = new InMemoryRepository<User>();
        var user = CreateUser("anna");
        repo.Add(user);

        repo.Update(new User
        {
            Id = user.Id,
            Username = "anna2",
            Email = "new@example.com",
            IsActive = false
        });

        var updated = repo.GetById(user.Id);
        Assert.Equal("anna2", updated.Username);
        Assert.Equal("new@example.com", updated.Email);
        Assert.False(updated.IsActive);
        Assert.Single(repo.GetAll());
    }

    [Fact]
    public void Update_NonExistingItem_ThrowsKeyNotFoundException()
    {
        var repo = new InMemoryRepository<User>();

        Assert.Throws<KeyNotFoundException>(() => repo.Update(CreateUser("ghost", id: 42)));
    }

    // ---------- Delete ----------

    [Fact]
    public void Delete_ExistingItem_RemovesIt()
    {
        var repo = new InMemoryRepository<User>();
        var user = CreateUser("anna");
        repo.Add(user);

        repo.Delete(user.Id);

        Assert.Empty(repo.GetAll());
        Assert.Throws<KeyNotFoundException>(() => repo.GetById(user.Id));
    }

    [Fact]
    public void Delete_NonExistingItem_ThrowsKeyNotFoundException()
    {
        var repo = new InMemoryRepository<User>();
        repo.Add(CreateUser("anna"));

        Assert.Throws<KeyNotFoundException>(() => repo.Delete(999));
        Assert.Single(repo.GetAll());
    }
}
