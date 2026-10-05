public class Program
{
    public static void Main(string[] args)
    {
        // Книга
        Book book = new Book("Кобзар", "Тарас Шевченко", 320);
        book.ReadPages(80);
        Console.WriteLine($"Книга '{book.Title}' ({book.Author}): прочитано {book.CurrentPage} сторінок");
        Console.WriteLine($"Прогрес читання: {book.GetProgressPercent():F1}%");
        Console.WriteLine($"Книгу прочитано повністю: {(book.IsFinished() ? "так" : "ні")}");
        Console.WriteLine();

        // Банківський рахунок
        BankAccount account = new BankAccount("Олександр Висоцький", 1000);
        account.Deposit(500);
        bool success = account.Withdraw(300);
        Console.WriteLine($"Власник рахунку: {account.Owner}");
        Console.WriteLine($"Зняття 300 грн виконано: {(success ? "так" : "ні")}");
        Console.WriteLine($"Баланс: {account.Balance:F2} грн");
        Console.WriteLine($"Відсотки (5%): {account.CalculateInterest(5):F2} грн");
        bool failed = account.Withdraw(5000);
        Console.WriteLine($"Спроба зняти 5000 грн успішна: {(failed ? "так" : "ні")}");
        Console.WriteLine();

        // Плейлист
        Playlist playlist = new Playlist("Улюблене");
        playlist.AddTrack("Стефанія", 245);
        playlist.AddTrack("Образи", 198);
        playlist.AddTrack("Щедрик", 180);
        playlist.PrintInfo();
        Console.WriteLine($"Кількість треків: {playlist.TrackCount}");
        Console.WriteLine($"Загальна тривалість: {playlist.GetTotalMinutes():F2} хв");
    }
}
