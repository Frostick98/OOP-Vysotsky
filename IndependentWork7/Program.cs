using IndependentWork7;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація FIFO-кешу Cache<string, string> (ємність = 3) ===");
var cache = new Cache<string, string>(3);

Console.WriteLine("\n1. Додаємо три елементи:");
cache.Add("ua", "Україна");
cache.Add("pl", "Польща");
cache.Add("de", "Німеччина");
Console.WriteLine($"   Count = {cache.Count}");

Console.WriteLine("\n2. Отримуємо елементи за ключем:");
Console.WriteLine($"   Get(\"ua\") = {cache.Get("ua")}");
Console.WriteLine($"   Get(\"pl\") = {cache.Get("pl")}");

Console.WriteLine("\n3. Додаємо четвертий елемент (кеш переповнений):");
cache.Add("fr", "Франція");
Console.WriteLine($"   Count = {cache.Count}");
Console.WriteLine($"   ContainsKey(\"ua\") = {cache.ContainsKey("ua")}  <- найстаріший елемент видалено");
Console.WriteLine($"   ContainsKey(\"pl\") = {cache.ContainsKey("pl")}");
Console.WriteLine($"   ContainsKey(\"de\") = {cache.ContainsKey("de")}");
Console.WriteLine($"   ContainsKey(\"fr\") = {cache.ContainsKey("fr")}");

Console.WriteLine("\n4. Додаємо ще один елемент:");
cache.Add("it", "Італія");
Console.WriteLine($"   ContainsKey(\"pl\") = {cache.ContainsKey("pl")}  <- тепер видалено \"pl\"");
Console.WriteLine($"   Count = {cache.Count}");

Console.WriteLine("\n5. Спроба отримати неіснуючий ключ:");
try
{
    cache.Get("ua");
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"   KeyNotFoundException: {ex.Message}");
}
