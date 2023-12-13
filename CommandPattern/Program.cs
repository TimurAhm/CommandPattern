

internal class Program
{
    private static void Main(string[] args)
    {
        //Controller controller = new Controller();
        //TV tV = new TV();
        //controller.PressOnButton();// ошибка пустого значения обработана
        //controller.Undo();// ошибка пустого значения обработана
        ////сначала обработал условием на пустое значение, позже добавил пустой класс
        //controller.SetCommand(new TVOnCommand(tV)); 
        //controller.PressOnButton();
        //controller.Undo();
        //controller.Undo();

        //Console.WriteLine(new String('-', 30));

        //Microwave microwave = new Microwave();
        //controller.SetCommand(new MicrowaveComand(microwave, 5000));
        //controller.PressOnButton();
        //controller.Undo();

        //Console.WriteLine(new String('-', 30));
        
        //Volume volume = new Volume();
        //controller.SetCommand(new VolumeCommand(volume));
        //controller.PressOnButton();
        //controller.Undo();
        //controller.Undo();
        //controller.Undo();
        //controller.PressOnButton();
        //controller.PressOnButton();
        //сверху весь код рабочий ес че

        TV tv = new TV();
        Volume volume = new Volume();
        MultiControler multiControler = new MultiControler();
        multiControler.SetCommand(0, new TVOnCommand(tv));
        multiControler.SetCommand(1, new VolumeCommand(volume));

        multiControler.PressExecuteButton(0);

        multiControler.PressExecuteButton(1);
        multiControler.PressExecuteButton(1);
        multiControler.PressExecuteButton(1);
        multiControler.PressExecuteButton(1);

        multiControler.PressUndoButton();
        multiControler.PressUndoButton();
        multiControler.PressUndoButton();
        multiControler.PressUndoButton();
        multiControler.PressUndoButton();
        // супер прикольная штука, но в этом случае, пока я не убавлю громкость до 0, то телевизор не выключить
        // но с помощью этого паттерна, можно сделать очень много всего, что должно иметь какое-либо строго пошаговое использование



        Console.Read();
    }
}

//abstract class Command
//{
//    public abstract void Execute();
//    public abstract void Undo();
//}

//class ConcreteCommand : Command
//{
//    Receiver receiver;
//    public ConcreteCommand(Receiver r)
//    {
//        receiver = r;
//    }

//    public override void Execute()
//    {
//        receiver.Operation();
//    }
//    public override void Undo()
//    {    }
//}

//class Receiver
//{
//    public void Operation()
//    { }
//}

//class Invoker
//{
//    Command command;
//    public void SetCommand( Command c)
//    {
//        command = c;
//    }

//    public void Run()
//    {
//        command.Execute();
//    }

//    public void Cancel()
//    {
//        command.Undo();
//    }
//}

interface ICommand
{
    void Execute();
    void Undo();
}

// Receiver ( Получатель )
class TV
{
    public void On()
    {
        Console.WriteLine("Телевизор включен!");
    }
    public void Off()
    {
        Console.WriteLine("Телевизор выключен!");
    }
}

class TVOnCommand : ICommand
{
    TV tv;
    public TVOnCommand(TV _tv)
    {
        tv = _tv;
    }
    public void Execute()
    {
        tv.On();
    }

    public void Undo()
    {
        tv.Off();
    }
}

class Volume
{
    public const int OFF = 0;
    public const int HIGH = 20;
    private int lvl;

    public Volume()
    {
        lvl = OFF;
    }

    public void RaiseLevel()
    {
        if (lvl < HIGH)
            lvl++;
        Console.WriteLine($"Уровень звука - {lvl}");
    }

    public void DropLevel()
    {
        if (lvl > OFF)
            lvl--;
        Console.WriteLine($"Уровень звука - {lvl}");
    }
}

class VolumeCommand : ICommand
{
    Volume volume;

    public VolumeCommand(Volume v)
    {
        volume = v;
    }

    public void Execute()
    {
        volume.RaiseLevel();
    }

    public void Undo()
    {
        volume.DropLevel();
    }
}

// Invoker ( Инициатор )
class Controller
{

    ICommand command;

    public void SetCommand(ICommand _command)
    {
        command = _command;
    }

    public void PressOnButton()
    {
        if (command == null)
            return;
        command.Execute();
    }

    public void Undo()
    {
        if (command == null)
            return;
        command.Undo();
    }
}

class NoCommand : ICommand
{
    public void Execute() { }
    public void Undo() { }
}


class Microwave
{
    public async void StartCooking(int time)
    {
        Console.WriteLine($"Разогреваем фольгу, время ожидания - {time / 1000} секунд");
        Task.Delay(time).GetAwaiter().GetResult();
    }

    public void StopCooking()
    {
        Console.WriteLine("Дамы и господа, квартиры больше нет");
    }
}

class MicrowaveComand : ICommand
{
    Microwave microwave;
    int time;
    public MicrowaveComand(Microwave m, int t)
    {
        microwave = m;
        time = t;
    }

    public void Execute()
    {
        microwave.StartCooking(time);
    }

    public void Undo()
    {
        microwave.StopCooking();
    }
}

class MultiControler
{
    ICommand[] buttons;
    Stack<ICommand> commandsHistory;

    public MultiControler()
    {
        buttons = new ICommand[2];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = new NoCommand();
        }
        commandsHistory = new Stack<ICommand>();
    }

    public void SetCommand(int number, ICommand com)
    {
        buttons[number] = com;
    }

    public void PressExecuteButton(int number)
    {
        buttons[number].Execute();
        commandsHistory.Push(buttons[number]);
    }

    public void PressUndoButton()
    {
        if (commandsHistory.Count > 0)
        {
            ICommand undoCommand = commandsHistory.Pop();
            undoCommand.Undo();
        } 
    }
}