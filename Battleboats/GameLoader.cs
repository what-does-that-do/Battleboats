namespace Battleboats;

public class GameLoader
{
    public static void New() { // Handles creating a new game
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear(); // clear screen with black

        Helpful.AddWindowHeader("NEW GAME"); // Add title bar
        Console.WriteLine("\n");
        // Create the user's boat grid. Fill with . to represent water.
        char[,] userGrid = new char[8,8];

        void ClearGrid() { // A procedure to clear the user's grid
            for (int i = 0; i < 8; i++) { // loop through grid
                for (int j = 0; j < 8; j++) {
                    userGrid[i,j] = '.'; // set to . to represent water
                }
            }
        }

        int selectedBoat = 0; // Set current boat to Carrier
        // Grab cursor co-ordinates so that only menu must be redrawn
        int cursorX = Console.CursorLeft;
        int cursorY = Console.CursorTop;
        // x,y position of user's cursor
        int x = 0;
        int y = 0;
        int[] boatLengths = {3,2,2,1,1}; // lengths of each boat
        bool[] placedBoats = {false, false, false, false, false}; // whether each boat has been placed yet

        void UpdateWindow(int selectedBoat, int x, int y, bool rotationHorizontal) { // Redraws Menu and grid
            Console.SetCursorPosition(cursorX, cursorY); // cursor to start of menu
            Grid.Draw(userGrid, x, y, false); // draw updated grid

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            // Yellow text showing title
            Console.WriteLine("\n");
            Console.WriteLine("Your available boats:");

            string[] boats = {"[0] ███ <- Carrier", "[1] ██ <- Submarine", "[2] ██ <- Submarine", "[3] █ <- Destroyer", "[4] █ <- Destroyer"};
            
            int i = 0; // for recursion
            foreach (string boat in boats) { // for each boat length
                if (i == selectedBoat) { // if that boat is selected
                    Console.ForegroundColor = ConsoleColor.Black; // text colour = black
                    if (placedBoats[i] == false) { // the boat hasn't been placed. Highlight boat yellow
                        Console.BackgroundColor = ConsoleColor.Yellow;
                    } else { // it has been placed - highlight it red
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                } else {
                    Console.BackgroundColor = ConsoleColor.Black; // not selected, background black
                    if (placedBoats[i] == false) { // boat has been placed, set to green
                        Console.ForegroundColor = ConsoleColor.Green;
                    } else { // boat has not been placed, set to red
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                }

                Console.WriteLine(boat); // Write the boat onto the terminal, with the above settings applied
                i++;
            }
            
            Console.ForegroundColor = ConsoleColor.Black; // return to black text
            if (rotationHorizontal) { // if rotation is set to horizontal
                Console.BackgroundColor = ConsoleColor.Magenta; // display rotation in magenta as horizontal
                Console.WriteLine("Rotation: Horizontal");
            } else { // if rotation set to vertical
                Console.BackgroundColor = ConsoleColor.Cyan; // display rotation in cyan as vertical
                Console.WriteLine("Rotation: Vertical  "); // extra spaces allow the width of the box not to change
            }
        }
        
        UpdateWindow(selectedBoat, x, y, true); // update window with boats and grid
        // Add controls footer
        Helpful.AddWindowFooter("[0-4] select boat, [Arrow Keys] = move boat, [P] = place boat, [R] = change rotation, [DEL] = clear, [ENTER] = start game");
        // Move boats around with arrow keys
        bool canPlace; // if no laws have been broken by the user
        bool placing = true; // while boat is being placed
        bool rotationHorizontal = true; // rotation

        ConsoleKeyInfo keyInfo;
        while (placing)
        {
            keyInfo = Console.ReadKey(true); // grab the latest key pressed
            switch (keyInfo.Key)
            {
                // If keys 0-4 have been pressed, set current boat to that value:
                case ConsoleKey.D0:
                    selectedBoat = 0;
                    break;
                case ConsoleKey.D1:
                    selectedBoat = 1;
                    break;
                case ConsoleKey.D2:
                    selectedBoat = 2;
                    break;
                case ConsoleKey.D3:
                    selectedBoat = 3;
                    break;
                case ConsoleKey.D4:
                    selectedBoat = 4;
                    break;
                // If arrow key pressed move cursor accordingly
                case ConsoleKey.RightArrow:
                    if (x < 7) {
                        x++;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (x > 0) {
                        x--;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (y > 0) {
                        y--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (y < 7) {
                        y++;
                    }
                    break;
                // If P pressed, place the boat on the grid
                case ConsoleKey.P:
                    // checks if boat has been placed yet
                    if (placedBoats[selectedBoat] == false) {
                        canPlace = true;
                    } else {
                        canPlace = false;
                    }
                    // checks if boat is in bounds of the grid
                    if (rotationHorizontal) {
                        if (x + boatLengths[selectedBoat] > 8) {
                            canPlace = false;
                        }
                    } else {
                        if (y + boatLengths[selectedBoat] > 8) {
                            canPlace = false;
                        }
                    }
                    // checks there isn't already a boat there.
                    if (canPlace) { // if the above laws are satisfied
                        for (int i = 0; i < boatLengths[selectedBoat]; i++) { // for each co-ordinate
                            if (rotationHorizontal) { // if rotation is horizontal
                                if (userGrid[y,x+i] != '.') { // if there is already a boat there
                                    canPlace = false; // law broken
                                }
                            } else {
                                if (userGrid[y+i,x] != '.') { // if there is already a boat there
                                    canPlace = false; // law broken
                                }
                            }
                        }
                    }
                    // places boat
                    if (canPlace) { // if no laws have been broken
                        for (int i = 0; i < boatLengths[selectedBoat]; i++) { // for each co-ordinate
                            if (rotationHorizontal) { // if rotation is horizontal
                                userGrid[y,x+i] = 'B'; // place boat
                            } else { // if rotation is vertical
                                userGrid[y+i,x] = 'B'; // place boat
                            }
                        }
                        placedBoats[selectedBoat] = true; // updates array of which boats have been placed
                    }
                    break;
                // Change rotation setting if R pressed
                case ConsoleKey.R:
                    if (rotationHorizontal) {
                        rotationHorizontal = false;
                    } else {
                        rotationHorizontal = true;
                    }
                    break;
                // Delete pressed, clear grid and reset boat placement array
                case ConsoleKey.Delete:
                    ClearGrid();
                    placedBoats = new bool[] {false, false, false, false, false};
                    break;
                case ConsoleKey.Enter: // Enter
                    placing = false; // we're no longer placing boats
                    foreach (bool boat in placedBoats) { // loop through boat placement array
                        if (boat == false) { // if there's an unplaced boat
                            placing = true; // we are still placing
                        }
                    }
                    break;
            }
            UpdateWindow(selectedBoat,x,y, rotationHorizontal); // updates grid & boat selection
        }
        // Clear window to black
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();
        // Add window header
        Helpful.AddWindowHeader("NEW GAME");
        Console.WriteLine("\n");
        // Display loading text, this could take some time
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.WriteLine("The computer is placing their boats.");
        // Remember where cursor is
        cursorX = Console.CursorLeft;
        cursorY = Console.CursorTop;
        // No controls to display, tell the user to be patient!
        Helpful.AddWindowFooter("Please wait...");
        // Create a new separate blank grid for computer's boats
        char[,] computerGrid = new char[8,8];
        for (int i = 0; i < 8; i++) { // loop through grid
            for (int j = 0; j < 8; j++) {
                computerGrid[i,j] = '.'; // set every square to water (.)
            }
        }
        
        Random random = new Random();
        bool notPlaced; // while the computer is placing its' boats

        foreach (int boat in boatLengths) { // loop through each boat
            notPlaced = true; // the boat has not been placed

            while (notPlaced) { // while the boat is being placed
                canPlace = true;
                // randomly decide rotation of boat
                if (random.Next(0,4) % 2 == 1) { // 50% chance
                    rotationHorizontal = true;
                } else {
                    rotationHorizontal = false;
                }
                // randomly decide position of boat
                if (rotationHorizontal) {
                    x = random.Next(0,8-(boat-1));
                    y = random.Next(0,8);
                } else {
                    x = random.Next(0,8);
                    y = random.Next(0,8-(boat-1));
                }
                // checks there isn't already a boat there.
                for (int i = 0; i < boat; i++) { // for each block in boat
                    if (rotationHorizontal) {
                        if (computerGrid[y,x+i] != '.') { // if there is something there
                            canPlace = false; // law broken
                        }
                    } else {
                        if (computerGrid[y+i,x] != '.') { // if there is something there
                            canPlace = false; // law broken
                        }
                    }
                }
                // places boat if no rules broken
                if (canPlace) {
                    for (int i = 0; i < boat; i++) { // for each block in boat
                        if (rotationHorizontal) {
                            computerGrid[y,x+i] = 'B'; // place on grid at co-ordinated decided earlier
                        } else {
                            computerGrid[y+i,x] = 'B'; // place on grid at co-ordinates decided earlier
                        }
                        notPlaced = false; // boat has been placed, quit loop
                    }
                }
            }
        }
        // go to correct part of console window
        Console.SetCursorPosition(cursorX, cursorY);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\nThe computer has placed their boats.\n");
        // Set text to yellow to indicate action
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Please enter a name for your game.");
        // Allow entry of text at the bottom of the window, put cursor in footer
        Helpful.AddWindowFooter("[A-Z][0-9] Game name:");
        Console.SetCursorPosition(17, Console.WindowHeight - 1);
        // Get game name from user's entry
        string gameName = "";
        bool invalid = true;
        while (invalid) { // keep fetching entries until something valid has been entered
            gameName = Console.ReadLine();
            if (gameName.Length > 0) { // if something has been entered
                invalid = false; // innocent until proven guilty
                foreach (char c in gameName) { // check if any characters are invalid
                    if ((char.IsLetter(c) == false) || (char.IsLetter(c) == true)) { // if not a letter or a number
                        invalid = true; // invalid
                        break; // loop can be exited.
                    }
                }
            }
        }
        // A valid name has been entered
        GameStorage.Save(gameName, true, userGrid, computerGrid, 0); // Save
        // Clear window to black
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();
        // Show success text while user waits
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Game written to memory.");
        // Start game!
        Game.Play(gameName, userGrid, computerGrid, 0);
    }

    public static void Load() { // Display game loading menu and load game
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear(); // Clear window to black
        // Show window title
        Helpful.AddWindowHeader("LOAD GAME");
        // Show BATTLEBOATS title in purple. 
        Console.ResetColor();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Helpful.ShowArt("main_title", -1, -1); // Display ASCII art wherever the cursor is now
        // Remember cursor pos for later menu redrawing
        int cursorX = Console.CursorLeft;
        int cursorY = Console.CursorTop;
        // Lists game directory and creates list of options to display (numbered, .battle removed)
        DirectoryInfo d = new DirectoryInfo("Games");
        FileInfo[] files = d.GetFiles("*.battle");
        string[] options = new string[files.Length];
        int a = 0;
        foreach (FileInfo file in files) {
            options[a] = "["+Convert.ToString(a)+"] "+file.Name.Remove(file.Name.Length-7);
            a++;
        }

        void DisplayOptions(int selectedItem) // Draw menu options, highlight selected item
        {
            Console.SetCursorPosition(cursorX, cursorY); // only redraw the menu, not the title bar
            // Display menu options, highlight selected item
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            int i = 0; // recursion

            foreach (string option in options) // for each game save
            {
                if (i == selectedItem) // if selected, highlight option
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                } else { // not selected, don't highlight
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Helpful.WriteToEntireLine(option); // write to entire line length 
                i++;
            }
        }
        // Show controls footer
        Helpful.AddWindowFooter("[Arrow Keys] = select game, [ENTER] = resume game, [DELETE] = delete selected game");

        int selectedItem = 0;
        if (options.Length > 0) { // run as long as there is 1+ active games
            DisplayOptions(selectedItem); // show menu
            bool selecting = true;
            // Adjust selected item based on arrow key input
            ConsoleKeyInfo keyInfo;
            while (selecting) // while no selection has been made
            {
                keyInfo = Console.ReadKey(true); // get keyboard entry
                switch (keyInfo.Key)
                {
                    // adjust selected item by arrow keys, display updated menu
                    case ConsoleKey.DownArrow:
                        if (selectedItem+1 < options.Length) {
                            selectedItem++;
                            DisplayOptions(selectedItem);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (selectedItem > 0) {
                            selectedItem--;
                            DisplayOptions(selectedItem);
                        }
                        break;
                    // Delete game, update menu
                    case ConsoleKey.Delete:
                        File.Delete("Games/"+files[selectedItem].Name); // get index of file matching selected item, delete that file
                        options[selectedItem] = "<game deleted>"; // replace game name with <game deleted>
                        DisplayOptions(selectedItem); // redraw menu
                        break;
                    case ConsoleKey.Escape: // exit menu, return to main menu
                        selecting = false;
                        break;
                    case ConsoleKey.Enter: // select game
                        string gameName = files[selectedItem].Name; // gets selected game filename
                        gameName = gameName.Remove(gameName.Length-7); // removes .battle from end of file name
                        Helpful.AddWindowHeader("LOAD GAME"); // draw window header
                        Console.WriteLine("\n");
                        // show loading
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine("Loading game...");
                        Helpful.AddWindowFooter("Please wait...");
                        // load game into memory
                        (int go, char[,] userGrid, char[,] computerGrid) = GameStorage.Load(gameName);
                        // clear screen
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        // play game
                        Game.Play(gameName, userGrid, computerGrid, go);
                        selecting = false;
                        break;
                }
            }
        } else {
            // Draw window header
            Helpful.AddWindowHeader("LOAD GAME");
            Console.BackgroundColor = ConsoleColor.Black;

            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            // Display message and controls. There are no saved games.
            Console.WriteLine("You have no active games.");
            Console.WriteLine("Press Enter to start a new one.");
            Helpful.AddWindowFooter("[ENTER] = start new game, [ESCAPE] = return to main menu");

            ConsoleKeyInfo keyInfo;
            keyInfo = Console.ReadKey(true); // get keypress
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter: // play new game
                        New();
                        break;
                    case ConsoleKey.Escape: // return to main menu
                        break;
                }
            }
            // clear
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
        }
    }
}
