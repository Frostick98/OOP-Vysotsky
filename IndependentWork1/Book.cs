public class Book
{
    // Приватні поля
    private string _title;
    private string _author;
    private int _totalPages;
    private int _currentPage;

    // Властивість лише для читання (read-only)
    public string Title
    {
        get { return _title; }
    }

    // Властивість з get та set
    public string Author
    {
        get { return _author; }
        set { _author = value; }
    }

    // Властивість лише для читання
    public int CurrentPage
    {
        get { return _currentPage; }
    }

    // Конструктор
    public Book(string title, string author, int totalPages)
    {
        _title = title;
        _author = author;
        _totalPages = totalPages;
        _currentPage = 0;
    }

    // Метод: читання сторінок (зміна стану)
    public void ReadPages(int count)
    {
        _currentPage += count;
        if (_currentPage > _totalPages)
        {
            _currentPage = _totalPages;
        }
    }

    // Метод: обчислення прогресу читання у відсотках
    public double GetProgressPercent()
    {
        return (double)_currentPage / _totalPages * 100;
    }

    // Метод: перевірка стану
    public bool IsFinished()
    {
        return _currentPage == _totalPages;
    }
}
