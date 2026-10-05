using System.Collections.Generic;

public class Playlist
{
    // Приватні поля
    private string _name;
    private List<string> _trackTitles;
    private int _totalSeconds;

    // Властивість з get та set
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    // Властивість лише для читання
    public int TrackCount
    {
        get { return _trackTitles.Count; }
    }

    // Конструктор
    public Playlist(string name)
    {
        _name = name;
        _trackTitles = new List<string>();
        _totalSeconds = 0;
    }

    // Метод: додавання треку
    public void AddTrack(string title, int durationSeconds)
    {
        _trackTitles.Add(title);
        _totalSeconds += durationSeconds;
    }

    // Метод: загальна тривалість у хвилинах
    public double GetTotalMinutes()
    {
        return _totalSeconds / 60.0;
    }

    // Метод: виведення інформації про плейлист
    public void PrintInfo()
    {
        Console.WriteLine($"Плейлист '{_name}':");
        for (int i = 0; i < _trackTitles.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {_trackTitles[i]}");
        }
    }
}
