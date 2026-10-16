using Lab6v2;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// 1. Створення об'єктів базового та похідних класів
Shape shape = new Shape("Білий");
Circle circle = new Circle("Червоний", 3);
Rectangle rectangle = new Rectangle("Синій", 4, 5);

// 2. Поліморфна поведінка: масив посилань на базовий клас
Console.WriteLine("=== Поліморфізм (virtual / override) ===");
Shape[] shapes = { shape, circle, rectangle };
foreach (Shape s in shapes)
{
    // Викликається перевизначена версія GetArea() відповідно до реального типу об'єкта
    Console.WriteLine($"{s.GetType().Name,-10} колір: {s.Color,-10} площа: {s.GetArea():F2}");
}

// 3. Власні методи похідних класів (доступні лише через посилання похідного типу)
Console.WriteLine();
Console.WriteLine("=== Власні методи похідних класів ===");
circle.Draw();
rectangle.Draw();

// 4. Різниця між override та new
Console.WriteLine();
Console.WriteLine("=== Різниця між override та new ===");

// Посилання на базовий клас
Shape shapeRef = rectangle;
// Посилання на похідний клас
Rectangle rectRef = rectangle;

// override: однаковий результат незалежно від типу посилання (поліморфізм)
Console.WriteLine($"override | Shape-посилання:     GetArea() = {shapeRef.GetArea()}");
Console.WriteLine($"override | Rectangle-посилання: GetArea() = {rectRef.GetArea()}");

// new: результат залежить від типу посилання (приховування)
Console.WriteLine($"new      | Shape-посилання:     GetShapeType() = {shapeRef.GetShapeType()}");
Console.WriteLine($"new      | Rectangle-посилання: GetShapeType() = {rectRef.GetShapeType()}");
