using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using Battleboats;

// Created by Oliver Corrigan

/*
WHAT THE FILES DO:
Program.cs -> Display menu and instructions. Is the root of the program.
GameLoader.cs -> Handles game creation, and resuming a previous game.
Game.cs -> Handles all game play and logic. This includes the final win/loose screen.
Grid.cs -> Handles drawing the grid to the screen.
GameStorage.cs -> Handles game save and load logic.
Helpful.cs -> Handles helpful UI features such as drawing the entire line, showing art, showing title and footer bars.

TO START THE GAME:  Just run Program.cs. 
                    Note that the text art is located in the TextArt folder,
                    running games are located in the Games folder.
                    Enjoy!
*/

Console.Clear();
Console.BackgroundColor = ConsoleColor.Black;
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("Welcome to Battleboats!");

static void ShowMenu()
{
    Console.BackgroundColor = ConsoleColor.Black;
    Console.Clear(); // Fill window with black
    
    Helpful.AddWindowHeader("MAIN MENU"); // Display title bar
    Console.ResetColor();
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Magenta;
    Helpful.ShowArt("main_title", -1, -1); // show the main title art "BATTLEBOATS"
    // Save cursor position, when the menu is redrawn, it will start from here - not from 0,0
    int cursorX = Console.CursorLeft;
    int cursorY = Console.CursorTop;

    void DisplayOptions(int selectedItem) // Draw menu options, highlight selected item
    {
        Console.SetCursorPosition(cursorX, cursorY); // grab cursor co-ordinates from earlier.
        // Display menu options, highlight selected item
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        string[] options = { "[1] Play a new game", "[2] Load a saved game", "[3] View the instructions", "[4] Exit the game" };
        int i = 1;
        // display each option, swapping foreground and background colour depending on selection.
        foreach (string option in options)
        {
            if (i == selectedItem)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.DarkYellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Helpful.WriteToEntireLine(option); // adds spaces to the end of the line so it ends up right at the edge of the window
            i++;
        }
    }

    string[] tips = {"You can rotate boats by pressing [R] when placing them.", "You can place boats next to each other.", "Your game is saved automatically. You can close it whenever you like.", "You can load a saved game from the main menu.", "You can view the instructions from the main menu."};
    Random random = new Random();

    // Display a random tip below the menu
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.WriteLine("\n\n\n\n\n\n");
    Console.BackgroundColor = ConsoleColor.Green;
    Console.WriteLine("*** TOP TIP: ***");
    Console.WriteLine(tips[random.Next(0, tips.Length)]);
    Helpful.AddWindowFooter("Use your arrow keys to navigate the menu. Press enter to select an option.");

    int selectedItem = 1; // set selection to Play A New Game
    DisplayOptions(selectedItem); // draw menu
    // Adjust selected item based on arrow key input
    ConsoleKeyInfo keyInfo;
    while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter)
    {
        switch (keyInfo.Key)
        {
            case ConsoleKey.DownArrow:
                if (selectedItem < 4) {
                    selectedItem++;
                }
                break;
            case ConsoleKey.UpArrow:
                if (selectedItem > 1) {
                    selectedItem--;
                }
                break;
        }
        DisplayOptions(selectedItem); // redraw menu with new selection
    }

    Console.BackgroundColor = ConsoleColor.Black;
    Console.Clear(); // clear window with black
    // Act upon which item was selected:
    switch (selectedItem) {
        case 1:
            GameLoader.New(); // start new game
            break;
        case 2:
            GameLoader.Load(); // show game loader
            break;
        case 3:
            Instructions(); // show instructions
            break;
        case 4: // Exit script with a friendly message!
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Thanks for playing! Goodbye.");
            Environment.Exit(0);
            break;
    }
}

static void Instructions() {
    Console.BackgroundColor = ConsoleColor.Black;
    Console.Clear(); // Fill window with black

    Helpful.AddWindowHeader("THE INSTRUCTIONS"); // Add window title of "THE INSTRUCTIONS"
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.BackgroundColor = ConsoleColor.Black;
    // Write the following to console, in yellow text on black background
    Console.WriteLine("Welcome to Battleboats!\n");
    Console.WriteLine("You are training for battle with the Dread Pirate Roberts.\nI have studied your enemy well.\nThis computer program simulates all his moves exactly, so you can prepare.");
    // Swap foreground and background colour to show a heading
    Console.BackgroundColor = ConsoleColor.Yellow;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.WriteLine("\nThe Aim Of The Game");
    // Swap foreground and background colour again for normal yellow text
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.BackgroundColor = ConsoleColor.Black;
    Console.WriteLine("Sink all of the computer's boats before the computer sinks all of yours!");
    // How to Play heading
    Console.BackgroundColor = ConsoleColor.Yellow;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.WriteLine("\nHow To Play");
    // Instructions in usual yellow text
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.BackgroundColor = ConsoleColor.Black;
    Console.WriteLine("You will be presented with a blank grid, place your boats in a formation on that grid.\nYou can rotate boats and/or place boats next to each other. You and your opponent have an identical fleet of boats:");
    Console.WriteLine("1x Carrier (███) | 2x Submarine (██) | 2x Destroyer (█)\n");
    Console.WriteLine("The computer will then place their boats on their own grid.\nYou will then take turns firing at each other's grids.\nIf you hit a boat, you will see a red H on your grid. If you miss, you will see a green M on your grid.\n");
    // Write controls in their real colours:
    Console.BackgroundColor = ConsoleColor.Green;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Write("M");
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write(" = Miss | ");
    Console.BackgroundColor = ConsoleColor.Red;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Write("H");
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write(" = Hit | ");
    Console.BackgroundColor = ConsoleColor.Yellow;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Write("B");
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write(" = One of your boats | ");
    Console.BackgroundColor = ConsoleColor.Cyan;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Write(".");
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write(" = Ocean\n");
    // Back to normal text
    Console.WriteLine("\nIf you sink all of the computer's boats, you win!\nIf the computer sinks all of your boats, you lose!");
    // Heading
    Console.BackgroundColor = ConsoleColor.Yellow;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.WriteLine("\nExample Grid:");
    // Normal text
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.BackgroundColor = ConsoleColor.Black;
    Console.WriteLine("NOTE: When navigating the grid, your cursor is represented by the purple square.\nYou can't navigate on this page. Below is an example of a grid you'll see when playing!");
    
    Console.BackgroundColor = ConsoleColor.Black; // Reset background colour
    // An example grid with every possibility shown!
    char[,] exampleGrid = {
        {'.', '.', '.', 'B', 'B', 'B', '.', '.'},
        {'.', 'H', 'M', '.', '.', '.', 'M', '.'},
        {'.', 'H', '.', '.', '.', '.', '.', '.'},
        {'.', 'M', '.', 'M', '.', '.', '.', '.'},
        {'.', '.', '.', '.', '.', 'B', '.', '.'},
        {'.', '.', '.', '.', '.', '.', '.', 'M'},
        {'.', '.', 'B', '.', 'M', '.', 'H', 'B'},
        {'.', '.', '.', '.', '.', '.', '.', '.'},
    };
    Grid.Draw(exampleGrid, 1, 3, false); // Draw grid underneath the above text, with a pretend cursor at (1,3)
    // Green text
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\n\nTIP: always check the bottom of the screen for controls!");
    // Cyan text
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("Good luck!");
    // Show controls footer, then wait until enter is pressed
    Helpful.AddWindowFooter("[ENTER] = return to main menu");
    Console.ReadLine();
}

// If the game storage directory does not exist, create it!
if (Directory.GetDirectories(Directory.GetCurrentDirectory()).Contains("Games") == false)
{
    Directory.CreateDirectory(Directory.GetCurrentDirectory()+"/Games");
}

// Check window sizing - if too small, warn the user (art does not display properly)
bool failing = true;
if (Console.WindowWidth < 130 || Console.WindowHeight < 45)
{
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Helpful.ShowArt("warning", -1, -1); // Show a warning triangle
    // Basic warning text telling user to resize their window (yellow)
    Console.WriteLine("For the best experience...");
    Console.WriteLine("Please resize your console window to be at least 130x45.");
    Console.WriteLine("Drag the window up and to the left to resize it.");
    Console.WriteLine();
    while (failing) { // while window is too small
        // Print an updated warning below with current window sizing to help user get right size.
        Console.WriteLine("Resize your window, then press enter to continue.");
        Console.WriteLine("Your window is currently " + Console.WindowWidth + "x" + Console.WindowHeight + ".");
        Console.ReadLine(); // wait until enter pressed
        if (Console.WindowWidth < 130 || Console.WindowHeight < 45) // check updated window sizing
        {
            // set color to red and display another message with current sizing, window still too small!
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Your window is still too small!");
        }
        else
        {
            failing = false; // window sizing ok, close warning
        }
    }
    
}

while (true) { // always show menu if any later ui stages are exited
    ShowMenu();
}