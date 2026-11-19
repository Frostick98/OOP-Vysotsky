// ============================================================================
// Лабораторна робота №10. LINQ в дії: фільтри, join, агрегації. Продуктивність.
// Дисципліна: Об'єктно-орієнтоване програмування
// Студент: Висоцький Олександр, група ІПЗ-3/1
// Варіант 2. Сценарій: Управління студентами.
//
// Сутності:
//   Student (Id, Name, Age, CourseId)
//   Course  (Id, Title, Credits)
//
// Тестові дані: 10 курсів та 20 студентів (див. метод Seed).
//
// Запити варіанту:
//   1. Студенти віком до 20 років на курсі "Programming Basics".
//   2. Join студентів з курсами: ім'я студента та назва курсу.
//   3. Кількість студентів на кожному курсі.
//   4. Наймолодший студент.
// Кожен запит реалізований у двох синтаксисах (запитів і методів),
// час виконання вимірюється через Stopwatch.
// ============================================================================

using System.Diagnostics;
using System.Text;

namespace Lab16v2;

public record Student(int Id, string Name, int Age, int CourseId);

public record Course(int Id, string Title, int Credits);

public static class Program
{
    private const string TargetCourse = "Programming Basics";
    private const int MaxAge = 20;

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var (courses, students) = Seed();
        Console.WriteLine($"Курсів: {courses.Count}, студентів: {students.Count}\n");

        // Прогрів: перший виклик LINQ включає JIT-компіляцію,
        // тому щоб не спотворювати вимірювання, виконуємо запити один раз наперед.
        RunAll(courses, students, print: false, measure: false);

        Console.WriteLine("================ Результати та час (малий набір даних) ================");
        RunAll(courses, students, print: true, measure: true);

        Console.WriteLine("\n================ Аналіз продуктивності (200 000 студентів) ================");
        Benchmark();
    }

    // ------------------------------------------------------------------------
    // Тестові дані
    // ------------------------------------------------------------------------
    private static (List<Course>, List<Student>) Seed()
    {
        var courses = new List<Course>
        {
            new(1, "Programming Basics", 5),
            new(2, "Databases", 6),
            new(3, "Web Development", 4),
            new(4, "Algorithms", 5),
            new(5, "Networks", 4),
            new(6, "Operating Systems", 5),
            new(7, "Discrete Math", 3),
            new(8, "UI/UX Design", 3),
            new(9, "Software Testing", 4),
            new(10, "Cybersecurity", 4),
        };

        var students = new List<Student>
        {
            new(1, "Олександр Коваль", 19, 1),
            new(2, "Марія Шевченко", 18, 1),
            new(3, "Іван Бондаренко", 21, 2),
            new(4, "Анна Мельник", 20, 1),
            new(5, "Дмитро Ткаченко", 17, 3),
            new(6, "Софія Кравченко", 19, 1),
            new(7, "Максим Олійник", 22, 4),
            new(8, "Катерина Лисенко", 18, 2),
            new(9, "Андрій Мороз", 23, 5),
            new(10, "Юлія Поліщук", 19, 1),
            new(11, "Богдан Савчук", 20, 6),
            new(12, "Вікторія Гончар", 21, 3),
            new(13, "Назар Руденко", 18, 7),
            new(14, "Олена Мартинюк", 22, 8),
            new(15, "Тарас Клименко", 19, 9),
            new(16, "Ірина Павленко", 20, 10),
            new(17, "Роман Бойко", 18, 4),
            new(18, "Дарина Король", 19, 2),
            new(19, "Павло Гаврилюк", 21, 1),
            new(20, "Леся Романюк", 18, 1),
        };

        return (courses, students);
    }

    // ------------------------------------------------------------------------
    // Запуск усіх запитів
    // ------------------------------------------------------------------------
    private static void RunAll(List<Course> courses, List<Student> students, bool print, bool measure)
    {
        // ---------------- Запит 1: фільтрація ----------------
        // Синтаксис запитів (join потрібен, бо назва курсу є в Course, а не в Student)
        var (q1Query, t1q) = Time(() =>
            (from s in students
             join c in courses on s.CourseId equals c.Id
             where c.Title == TargetCourse && s.Age < MaxAge
             select s).ToList());

        // Синтаксис методів
        var (q1Method, t1m) = Time(() =>
            students
                .Join(courses, s => s.CourseId, c => c.Id, (s, c) => new { s, c })
                .Where(x => x.c.Title == TargetCourse && x.s.Age < MaxAge)
                .Select(x => x.s)
                .ToList());

        if (print)
        {
            Console.WriteLine($"\n[1] Студенти до {MaxAge} років на курсі \"{TargetCourse}\":");
            foreach (var s in q1Query) Console.WriteLine($"    {s.Name}, {s.Age} р.");
            Report(t1q, t1m, q1Query.SequenceEqual(q1Method));
        }

        // ---------------- Запит 2: join ----------------
        var (q2Query, t2q) = Time(() =>
            (from s in students
             join c in courses on s.CourseId equals c.Id
             select new { Student = s.Name, Course = c.Title }).ToList());

        var (q2Method, t2m) = Time(() =>
            students
                .Join(courses, s => s.CourseId, c => c.Id,
                      (s, c) => new { Student = s.Name, Course = c.Title })
                .ToList());

        if (print)
        {
            Console.WriteLine("\n[2] Студенти та їхні курси (join):");
            foreach (var r in q2Query) Console.WriteLine($"    {r.Student} -> {r.Course}");
            Report(t2q, t2m, q2Query.SequenceEqual(q2Method));
        }

        // ---------------- Запит 3: агрегація (Count по групах) ----------------
        // GroupJoin дозволяє показати і курси без студентів (з нулем).
        var (q3Query, t3q) = Time(() =>
            (from c in courses
             join s in students on c.Id equals s.CourseId into group_
             select new { c.Title, Count = group_.Count() }).ToList());

        var (q3Method, t3m) = Time(() =>
            courses
                .GroupJoin(students, c => c.Id, s => s.CourseId,
                           (c, group_) => new { c.Title, Count = group_.Count() })
                .ToList());

        if (print)
        {
            Console.WriteLine("\n[3] Кількість студентів на кожному курсі:");
            foreach (var r in q3Query) Console.WriteLine($"    {r.Title}: {r.Count}");
            Report(t3q, t3m, q3Query.SequenceEqual(q3Method));
        }

        // ---------------- Запит 4: наймолодший студент ----------------
        var (q4Query, t4q) = Time(() =>
            (from s in students
             orderby s.Age
             select s).First());

        // MinBy (.NET 6+) проходить колекцію один раз, без сортування
        var (q4Method, t4m) = Time(() => students.MinBy(s => s.Age)!);

        if (print)
        {
            Console.WriteLine("\n[4] Наймолодший студент:");
            Console.WriteLine($"    {q4Query.Name}, {q4Query.Age} р.");
            Report(t4q, t4m, q4Query == q4Method);
        }
    }

    // ------------------------------------------------------------------------
    // Вимірювання часу через Stopwatch
    // ------------------------------------------------------------------------
    private static (T Result, double Ms) Time<T>(Func<T> action)
    {
        var sw = Stopwatch.StartNew();
        var result = action();      // у кожному запиті є ToList()/First(): виконання негайне
        sw.Stop();
        return (result, sw.Elapsed.TotalMilliseconds);
    }

    private static void Report(double queryMs, double methodMs, bool same)
    {
        Console.WriteLine($"    Час (синтаксис запитів): {queryMs:F4} мс");
        Console.WriteLine($"    Час (синтаксис методів): {methodMs:F4} мс");
        Console.WriteLine($"    Результати збігаються: {same}");
    }

    // ------------------------------------------------------------------------
    // Порівняння на великому наборі даних (середнє з 5 запусків)
    // ------------------------------------------------------------------------
    private static void Benchmark()
    {
        var (courses, _) = Seed();
        var rnd = new Random(42);
        var big = Enumerable.Range(1, 200_000)
            .Select(i => new Student(i, "Student " + i, rnd.Next(17, 26), rnd.Next(1, courses.Count + 1)))
            .ToList();

        const int runs = 5;

        double Avg(Action a)
        {
            a(); // прогрів
            double total = 0;
            for (int i = 0; i < runs; i++)
            {
                var sw = Stopwatch.StartNew();
                a();
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / runs;
        }

        var results = new List<(string Name, double Ms)>
        {
            ("1. Фільтр + join (запити)", Avg(() =>
                (from s in big join c in courses on s.CourseId equals c.Id
                 where c.Title == TargetCourse && s.Age < MaxAge select s).ToList())),

            ("1. Фільтр + join (методи)", Avg(() =>
                big.Join(courses, s => s.CourseId, c => c.Id, (s, c) => new { s, c })
                   .Where(x => x.c.Title == TargetCourse && x.s.Age < MaxAge)
                   .Select(x => x.s).ToList())),

            ("2. Join (запити)", Avg(() =>
                (from s in big join c in courses on s.CourseId equals c.Id
                 select new { s.Name, c.Title }).ToList())),

            ("2. Join (методи)", Avg(() =>
                big.Join(courses, s => s.CourseId, c => c.Id, (s, c) => new { s.Name, c.Title }).ToList())),

            ("3. GroupJoin + Count (запити)", Avg(() =>
                (from c in courses join s in big on c.Id equals s.CourseId into g
                 select new { c.Title, Count = g.Count() }).ToList())),

            ("3. GroupJoin + Count (методи)", Avg(() =>
                courses.GroupJoin(big, c => c.Id, s => s.CourseId,
                                  (c, g) => new { c.Title, Count = g.Count() }).ToList())),

            ("4. OrderBy + First (запити)", Avg(() =>
                (from s in big orderby s.Age select s).First())),

            ("4. MinBy (методи)", Avg(() => big.MinBy(s => s.Age))),
        };

        foreach (var (name, ms) in results)
            Console.WriteLine($"  {name,-34} {ms,10:F3} мс");

        var slowest = results.MaxBy(r => r.Ms);
        var fastest = results.MinBy(r => r.Ms);
        Console.WriteLine($"\n  Найдовше: {slowest.Name} ({slowest.Ms:F3} мс)");
        Console.WriteLine($"  Найшвидше: {fastest.Name} ({fastest.Ms:F3} мс)");
    }
}

// ============================================================================
// ВИСНОВОК (за результатами запуску, див. вивід програми):
// - Обидва синтаксиси компілюються в однакові виклики методів розширення,
//   тому різниця в часі (кілька мс) лежить у межах похибки вимірювання.
// - На малому наборі (20 студентів) час становить соті частки мілісекунди,
//   на нього впливають JIT і стан процесора, тому порівнювати потрібно
//   на великих даних (200 000 елементів).
// - Найдорожчими виявились запити 1 і 2 (близько 30-37 мс): Join будує
//   хеш-таблицю, а ToList створює багато нових об'єктів (проєкція в
//   анонімний тип та копіювання результату).
// - Агрегація з GroupJoin + Count (5-7 мс) і пошук мінімуму (1-3 мс) значно
//   швидші: вони повертають кілька значень або один елемент.
// - MinBy проходить колекцію один раз (O(n)) і приблизно вдвічі швидший
//   за OrderBy + First.
// ============================================================================
