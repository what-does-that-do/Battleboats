namespace Battleboats;

public class Helpful
{
    public static void AddWindowHeader(string text)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Clear();
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.Black;
        string header = "Battleboats - " + text;
        WriteToEntireLine(header);
    }

    public static void AddWindowFooter(string text) {
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        WriteToEntireLine(text);
    }

    public static void ShowArt(string name, int cursorX, int cursorY) { // show text art from file
        string art = File.ReadAllText("TextArt/" + name + ".txt");
        Console.BackgroundColor = ConsoleColor.Black;
        int i = 0;
        foreach (string line in art.Split("\n"))
        {
            if (cursorX != -1) {
                Console.SetCursorPosition(cursorX, cursorY+i);
            }
            Console.WriteLine(line);
            i++;
        }
    }
    public static void WriteToEntireLine(string text) {
        int toWrite = Console.WindowWidth - text.Length; // get remaining un-drawn chars at the end of the line, add that many spaces to the end of the title
        for (int i = 0; i < toWrite; i++)
        {
            text += " ";
        }
        Console.Write(text); // draw on console
    }
}

