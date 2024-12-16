using System;
using System.Threading;

public class Stopwatch
{
    // Fields
    private TimeSpan timeElapsed;
    private bool isRunning;
    private Timer timer;

    // Delegate and Events
    public delegate void StopwatchEventHandler(string message);
    public event StopwatchEventHandler OnStarted;
    public event StopwatchEventHandler OnStopped;
    public event StopwatchEventHandler OnReset;

    // Constructor
    public Stopwatch()
    {
        timeElapsed = TimeSpan.Zero;
        isRunning = false;
    }

    // Start the Stopwatch
    public void Start()
    {
        if (!isRunning)
        {
            isRunning = true;
            timer = new Timer(Tick, null, 0, 1000); // Trigger every second
            OnStarted?.Invoke("Stopwatch Started!");
        }
    }

    // Stop the Stopwatch
    public void Stop()
    {
        if (isRunning)
        {
            isRunning = false;
            timer?.Dispose();
            OnStopped?.Invoke("Stopwatch Stopped!");
        }
    }

    // Reset the Stopwatch
    public void Reset()
    {
        isRunning = false;
        timer?.Dispose();
        timeElapsed = TimeSpan.Zero;
        OnReset?.Invoke("Stopwatch Reset!");
    }

    // Tick: Increment time
    private void Tick(object state)
    {
        timeElapsed = timeElapsed.Add(TimeSpan.FromSeconds(1));
        Console.Clear();
        Console.WriteLine($"Time Elapsed: {timeElapsed}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();

        // Subscribe to events
        stopwatch.OnStarted += (message) => Console.WriteLine(message);
        stopwatch.OnStopped += (message) => Console.WriteLine(message);
        stopwatch.OnReset += (message) => Console.WriteLine(message);

        Console.WriteLine("Stopwatch Console Application\n");
        Console.WriteLine("Press S to Start, T to Stop, R to Reset, Q to Quit.");

        bool exit = false;

        while (!exit)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.S:
                        stopwatch.Start();
                        break;

                    case ConsoleKey.T:
                        stopwatch.Stop();
                        break;

                    case ConsoleKey.R:
                        stopwatch.Reset();
                        break;

                    case ConsoleKey.Q:
                        stopwatch.Stop();
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid Key! Use S, T, R, or Q.");
                        break;
                }
            }

            Thread.Sleep(100); // Reduce CPU usage
        }

        Console.WriteLine("Application Exited.");
    }
}
