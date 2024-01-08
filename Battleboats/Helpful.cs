namespace Battleboats;

public class Helpful
{
    public static void AddWindowHeader(string text) // Draws the header at the top of the window
    {
        // Set red background, red foreground, draw header text
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.Black;
        string header = "Battleboats - " + text;
        Console.Title = header; // Sets console window title
        WriteToEntireLine(header);
    }

    public static void AddWindowFooter(string text) { // Draws the footer at the bottom of the window
        Console.SetCursorPosition(0, Console.WindowHeight - 1); // Set cursor to bottom left
        // Set to navy background with white text, draw text to entire line
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        WriteToEntireLine(text);
    }

    public static void ShowArt(string name, int cursorX, int cursorY) { // show text art from file
        string art = File.ReadAllText("TextArt/" + name + ".txt"); // Read art file
        Console.BackgroundColor = ConsoleColor.Black; // Reset background colour
        // Loop through file line by line
        int i = 0;
        foreach (string line in art.Split("\n"))
        {
            if (cursorX != -1) { // if the cursor is not at the left of the screen (resuming from where it last was). It has been predefined.
                Console.SetCursorPosition(cursorX, cursorY+i); // Set cursor underneath where it last was
            }
            Console.WriteLine(line);
            i++;
        }
    }
    public static void WriteToEntireLine(string text) { // Writes text then pads with spaces until we reach the end of the line
        int toWrite = Console.WindowWidth - text.Length; // get remaining un-drawn chars at the end of the line, add that many spaces to the end of the title
        for (int i = 0; i < toWrite; i++)
        {
            text += " ";
        }
        Console.Write(text); // draw on console
    }
}

