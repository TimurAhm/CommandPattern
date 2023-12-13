
internal class Program
{
    private static void Main(string[] args)
    {
        Programmer programmer = new Programmer();
        Tester tester = new Tester();
        Markerolog markerolog = new Markerolog();

        List<ICommand> commands = new List<ICommand>
        {
            new CodeCommand(programmer),
            new TestCommand(tester),
            new AdvertizeCommand(markerolog)
        };

        Manager manager = new Manager();
        manager.SetCommand(new MacroCommand(commands));
        manager.StartProject();
        manager.StopProject();

        Console.ReadLine();
    }
}

interface ICommand
{
    void Execute();
    void Undo();
}

class MacroCommand : ICommand
{
    List<ICommand> commands;
    public MacroCommand(List<ICommand> coms)
    {
        commands = coms;
    }

    public void Execute()
    {
        foreach (ICommand c in commands)
        {
            c.Execute();
        }
    }

    public void Undo()
    {
        foreach (ICommand c in commands)
        {
            c.Undo();
        }
    }
}

class Programmer
{
    public void StartCoding()
    {
        Console.WriteLine("Программист начинает писать код");
    }

    public void StopCoding()
    {
        Console.WriteLine("Прогаммист перестает писать код");
    }
}

class Tester
{
    public void StartTest()
    {
        Console.WriteLine("Тестировщик начинает тестирование");
    }

    public void StopTest()
    {
        Console.WriteLine("Тестировщик завершает тестрование");
    }
}

class Markerolog
{
    public void StartAdvertize()
    {
        Console.WriteLine("Маркетолог начинает рекламировать продукт");
    }

    public void StopAdvertize()
    {
        Console.WriteLine("Маректолог прекращает рекламную программу");
    }
}

class CodeCommand : ICommand
{
    Programmer programmer;
    public CodeCommand(Programmer p)
    {
        programmer = p;
    }

    public void Execute()
    {
        programmer.StartCoding();
    }

    public void Undo()
    {
        programmer.StopCoding();
    }
}

class TestCommand : ICommand
{
    Tester tester;

    public TestCommand(Tester t)
    {
        tester = t;
    }

    public void Execute()
    {
        tester.StartTest();
    }

    public void Undo()
    {
        tester.StopTest();
    }
}

class AdvertizeCommand : ICommand
{
    Markerolog markerolog;

    public AdvertizeCommand(Markerolog m)
    {
        markerolog = m;
    }

    public void Execute()
    {
        markerolog.StartAdvertize();
    }

    public void Undo()
    {
        markerolog.StopAdvertize();
    }
}

class Manager
{
    ICommand command;

    public void SetCommand(ICommand com)
    {
        command = com;
    }

    public void StartProject()
    {
        if(command != null)
            command.Execute();
    }

    public void StopProject()
    {
        if (command != null)
            command.Undo();
    }
}