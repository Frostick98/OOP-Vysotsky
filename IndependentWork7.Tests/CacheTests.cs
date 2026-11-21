using IndependentWork7;

namespace IndependentWork7.Tests;

public class CacheTests
{
    // ---------- Конструктор ----------

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_NonPositiveCapacity_ThrowsArgumentOutOfRangeException(int capacity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Cache<int, string>(capacity));
    }

    // ---------- Add ----------

    [Fact]
    public void Add_NewItem_ItemIsStoredInCache()
    {
        var cache = new Cache<string, int>(3);

        cache.Add("one", 1);

        Assert.True(cache.ContainsKey("one"));
        Assert.Equal(1, cache.Get("one"));
    }

    [Fact]
    public void Add_ExistingKey_UpdatesValueWithoutChangingCount()
    {
        var cache = new Cache<string, int>(3);
        cache.Add("one", 1);

        cache.Add("one", 100);

        Assert.Equal(1, cache.Count);
        Assert.Equal(100, cache.Get("one"));
    }

    [Fact]
    public void Add_NullKey_ThrowsArgumentNullException()
    {
        var cache = new Cache<string, int>(3);

        Assert.Throws<ArgumentNullException>(() => cache.Add(null!, 1));
    }

    // ---------- Get ----------

    [Fact]
    public void Get_ExistingKey_ReturnsCorrectValue()
    {
        var cache = new Cache<int, string>(3);
        cache.Add(1, "a");
        cache.Add(2, "b");

        Assert.Equal("a", cache.Get(1));
        Assert.Equal("b", cache.Get(2));
    }

    [Fact]
    public void Get_MissingKey_ThrowsKeyNotFoundException()
    {
        var cache = new Cache<int, string>(3);
        cache.Add(1, "a");

        Assert.Throws<KeyNotFoundException>(() => cache.Get(42));
    }

    [Fact]
    public void Get_OnEmptyCache_ThrowsKeyNotFoundException()
    {
        var cache = new Cache<string, string>(2);

        Assert.Throws<KeyNotFoundException>(() => cache.Get("any"));
    }

    // ---------- ContainsKey ----------

    [Fact]
    public void ContainsKey_ExistingKey_ReturnsTrue()
    {
        var cache = new Cache<int, string>(3);
        cache.Add(1, "a");

        Assert.True(cache.ContainsKey(1));
    }

    [Fact]
    public void ContainsKey_MissingKey_ReturnsFalse()
    {
        var cache = new Cache<int, string>(3);
        cache.Add(1, "a");

        Assert.False(cache.ContainsKey(2));
    }

    [Fact]
    public void ContainsKey_AfterEviction_ReturnsFalseForEvictedKey()
    {
        var cache = new Cache<int, string>(1);
        cache.Add(1, "a");
        cache.Add(2, "b");

        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
    }

    // ---------- Count ----------

    [Fact]
    public void Count_NewCache_IsZero()
    {
        var cache = new Cache<int, string>(3);

        Assert.Equal(0, cache.Count);
    }

    [Fact]
    public void Count_AfterAddingItems_EqualsNumberOfItems()
    {
        var cache = new Cache<int, string>(5);

        cache.Add(1, "a");
        cache.Add(2, "b");
        cache.Add(3, "c");

        Assert.Equal(3, cache.Count);
    }

    [Fact]
    public void Count_WhenCapacityExceeded_NeverExceedsCapacity()
    {
        var cache = new Cache<int, string>(3);

        for (int i = 1; i <= 10; i++)
        {
            cache.Add(i, $"value{i}");
        }

        Assert.Equal(3, cache.Count);
    }

    // ---------- Політика FIFO ----------

    [Fact]
    public void Add_WhenCacheIsFull_RemovesOldestItem()
    {
        var cache = new Cache<int, string>(3);
        cache.Add(1, "a");
        cache.Add(2, "b");
        cache.Add(3, "c");

        cache.Add(4, "d");

        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
        Assert.True(cache.ContainsKey(4));
        Assert.Throws<KeyNotFoundException>(() => cache.Get(1));
    }

    [Fact]
    public void Add_MultipleOverflows_RemovesItemsInInsertionOrder()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "a");
        cache.Add(2, "b");

        cache.Add(3, "c"); // видаляється 1
        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));

        cache.Add(4, "d"); // видаляється 2
        Assert.False(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
        Assert.True(cache.ContainsKey(4));
    }

    [Fact]
    public void Add_CapacityOne_AlwaysKeepsOnlyLatestItem()
    {
        var cache = new Cache<string, int>(1);

        cache.Add("a", 1);
        cache.Add("b", 2);
        cache.Add("c", 3);

        Assert.Equal(1, cache.Count);
        Assert.Equal(3, cache.Get("c"));
        Assert.False(cache.ContainsKey("a"));
        Assert.False(cache.ContainsKey("b"));
    }

    [Fact]
    public void Get_DoesNotChangeEvictionOrder()
    {
        // FIFO (на відміну від LRU): звернення до елемента не робить його "свіжішим".
        var cache = new Cache<int, string>(2);
        cache.Add(1, "a");
        cache.Add(2, "b");

        cache.Get(1);
        cache.Add(3, "c");

        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
    }

    [Fact]
    public void Add_ExistingKeyWhenFull_DoesNotEvictAnything()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "a");
        cache.Add(2, "b");

        cache.Add(2, "B");

        Assert.Equal(2, cache.Count);
        Assert.True(cache.ContainsKey(1));
        Assert.Equal("B", cache.Get(2));
    }
}
