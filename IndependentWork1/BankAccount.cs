public class BankAccount
{
    // Приватні поля
    private string _owner;
    private double _balance;

    // Властивість з get та set
    public string Owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    // Властивість лише для читання
    public double Balance
    {
        get { return _balance; }
    }

    // Конструктор
    public BankAccount(string owner, double initialBalance)
    {
        _owner = owner;
        _balance = initialBalance;
    }

    // Метод: поповнення рахунку
    public void Deposit(double amount)
    {
        if (amount > 0)
        {
            _balance += amount;
        }
    }

    // Метод: зняття коштів з перевіркою стану
    public bool Withdraw(double amount)
    {
        if (amount > 0 && amount <= _balance)
        {
            _balance -= amount;
            return true;
        }
        return false;
    }

    // Метод: нарахування відсотків
    public double CalculateInterest(double percentage)
    {
        return _balance * percentage / 100;
    }
}
