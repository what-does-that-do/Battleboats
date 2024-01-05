using System.ComponentModel;

namespace Battleboats;

public class Game
{
    // Handles game play logic and ui. This will be called once game has been created/loaded
    public static void Play(string gameName, char[,] userGrid, char[,] computerGrid, int go) {
        bool playing = true; 
        int cursorX, cursorY, x, y;
        string thatWasA;
        bool placing;
        string winner = null;

        int userBoatsSunk = 0;
        int computerBoatsSunk = 0;

        for (int i = 0; i < 8; i++) { // find no of boats computer has sunk
            for (int j = 0; j < 8; j++) {
                if (userGrid[i,j] == 'H') {
                    userBoatsSunk++;
                }
            }
        }
        for (int i = 0; i < 8; i++) { // find no of boats user has sunk
            for (int j = 0; j < 8; j++) {
                if (computerGrid[i,j] == 'H') {
                    computerBoatsSunk++;
                }
            }
        }

        void Scoreboard(int go, int computerBoatsSunk, int userBoatsSunk) { // Draws the right hand scoreboard
            Console.SetCursorPosition(cursorX+30, cursorY+1); // Go to cursor pos
            // Write title on a yellow background
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("*** SCOREBOARD ***");
            // Write score info as yellow text on black background. Continues indented from last cursor pos.
            Console.SetCursorPosition(cursorX+30, cursorY+2);
            Console.WriteLine("Current round = "+Convert.ToString(go));
            Console.SetCursorPosition(cursorX+30, cursorY+3);
            Console.WriteLine("Computer boats left = "+Convert.ToString(9 - computerBoatsSunk));
            Console.SetCursorPosition(cursorX+30, cursorY+4);
            Console.WriteLine("Your boats left = "+Convert.ToString(9 - userBoatsSunk));
            Console.SetCursorPosition(0, 11); // prevent cursor misplacement glitches. Returns cursor to underneath grid
        }
        // Gameplay while nobody has won
        while (playing) {
            /*
             * USER'S TURN
             */
            // Clear window
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            // Show title
            Helpful.AddWindowHeader(gameName+" - YOUR TURN");
            Console.WriteLine("\n");
            // remember last console cursor pos - then only partial screen redraw needed
            cursorX = Console.CursorLeft;
            cursorY = Console.CursorTop;
            // Set cursor to 0,0
            x = 0;
            y = 0;
            // Draw scoreboard and grid
            Grid.Draw(computerGrid, 0, 0, true);
            Scoreboard(go, computerBoatsSunk, userBoatsSunk);
            // Draw the boat information underneath scoreboard
            Console.SetCursorPosition(cursorX+30, cursorY+7);
            // Boats title (magenta background)
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("*** BOATS ***");
            // Draw each boat's information (magenta text)
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(cursorX+30, cursorY+8);
            Console.WriteLine("1x Carrier (███)");
            Console.SetCursorPosition(cursorX+30, cursorY+9);
            Console.WriteLine("2x Submarine (██)");
            Console.SetCursorPosition(cursorX+30, cursorY+10);
            Console.WriteLine("2x Destroyer █");
            // Add instruction footer
            Helpful.AddWindowFooter("[Arrow Keys] = move cursor, [ENTER] = fire missile.");
            // While user is choosing somewhere to put their boats, keep checking which key was pressed
            placing = true;
            thatWasA = "";
            ConsoleKeyInfo keyInfo;
            while (placing)
            {
                keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key) // move cursor based on arrow keys
                {
                    // Adjusts co-ordinates based on arrow keys
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
                    // When enter pressed, fire there if the space is blank, otherwise don't do anything
                    case ConsoleKey.Enter:
                        if (computerGrid[y,x] != 'H' && computerGrid[y,x] != 'M') { // check not already fired at
                            if (computerGrid[y,x] == 'B') { // if it's a boat, it's a hit
                                computerGrid[y,x] = 'H';
                                thatWasA = "hit";
                                computerBoatsSunk++;
                            } else { // no boat there, it's a miss
                                computerGrid[y,x] = 'M';
                                thatWasA = "miss";
                            }
                            placing = false; // exit loop
                        }
                        break;
                }
                // Redraw window at the correct position
                Console.SetCursorPosition(cursorX, cursorY);
                Grid.Draw(computerGrid, x, y, true);
                Scoreboard(go, computerBoatsSunk, userBoatsSunk);
            }

            if (computerBoatsSunk >= 9) { // If user has sunk all the computer's boats, they have won
                winner = "user";
            } else { // User hasn't won, we're still playing
                // Write explainer text below of what's happened (yellow)
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("\nThat was a "+thatWasA+"!\nPress enter to continue.");
                Console.ReadLine();
                // Save game
                Console.WriteLine("Saving...");
                GameStorage.Save(gameName, false, userGrid, computerGrid, go);
                // Clear window once saved
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                // Add title
                Helpful.AddWindowHeader(gameName+" - COMPUTER'S TURN");
                Console.WriteLine("\n");
                // Set new cursor position to redraw at
                cursorX = Console.CursorLeft;
                cursorY = Console.CursorTop;
                // draw grid and add window footer
                Grid.Draw(userGrid, 9, 9, false); // uses invalid co-ordinates to hide cursor
                Helpful.AddWindowFooter("The computer has played their turn.");
                // while the computer hasn't found a random place to fire, keep generating random places
                placing = true;
                thatWasA = "";
                Random random = new Random();
                while (placing) {
                    x = random.Next(0,8);
                    y = random.Next(0,8);
                    if (userGrid[y,x] != 'H' && userGrid[y,x] != 'M') { // check square not already fired at
                        if (userGrid[y,x] == 'B') { // if boat there, replace B with H
                            userGrid[y,x] = 'H';
                            thatWasA = "hit";
                            userBoatsSunk++;
                        } else { // if no boat there replace . with M
                            userGrid[y,x] = 'M';
                            thatWasA = "miss";
                        }
                        placing = false; // valid hit, stop looping
                    }
                }
                // update screen
                Console.SetCursorPosition(cursorX, cursorY);
                Grid.Draw(userGrid, 9, 9, false); // uses invalid co-ordinates to hide cursor
                Scoreboard(go, computerBoatsSunk, userBoatsSunk);

                if (userBoatsSunk >= 9) { // if computer has sunk all of user's boats, computer has won
                    winner = "computer";
                } else {
                    // Explainer text of what happened, saving and incrementing go
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("\nThe computer fired and "+thatWasA+"!\nPress enter to continue.");
                    Console.ReadLine();

                    Console.WriteLine("Saving...");
                    go++;
                    GameStorage.Save(gameName, false, userGrid, computerGrid, go);
                }
            }
            if (winner != null) { // if someone has won, exit and go to EndGame
                playing = false;
                Game.End(winner, go, userGrid, computerGrid, gameName, userBoatsSunk, computerBoatsSunk);
            }
        }
    }
    // Handles the win/loose ui
    public static void End(string winner, int go, char[,] userGrid, char[,] computerGrid, string gameName, int userBoatsSunk, int computerBoatsSunk) {
        // Clear screen
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();
        // Add window title
        Helpful.AddWindowHeader(gameName+" - GAME OVER");
        Console.WriteLine("\n");
        // Show user and computer's game grid
        Console.BackgroundColor = ConsoleColor.Green;
        Console.WriteLine("Your Grid:");
        Grid.Draw(userGrid, 9, 9, false); // uses invalid co-ordinates to hide cursor
        Console.BackgroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n\nComputer's Grid:");
        Grid.Draw(computerGrid, 9, 9, false);
        // Draw art depending on who won
        if (winner == "user") {
            Console.ForegroundColor = ConsoleColor.Green;
            Helpful.ShowArt("you_win", 30, 2);
        } else {
            Console.ForegroundColor = ConsoleColor.Red;
            Helpful.ShowArt("you_lost", 30, 2);
        }
        // Go to right side of window, underneath the art
        Console.SetCursorPosition(30, 12);
        Console.ForegroundColor = ConsoleColor.Yellow;
        // Display a random message from the win or loose collection
        string[] lossMessages = {"Mwa ha ha ha! The Dread Pirate Roberts is happy.", "Better luck next time!","Practise makes perfect!","At least it wasn't a real battle...","Keep trying!"};
        string[] winMessages = {"Congratulations! You won!","You beat the Dread Pirate Roberts!","You are a master of Battleboats!","You are a true warrior!","You are a true pirate!"};
        Random random = new Random();
        if (winner == "user") {
            Console.WriteLine(winMessages[random.Next(0,5)]);
        } else {
            Console.WriteLine(lossMessages[random.Next(0,5)]);
        }
        // Display stats in yellow
        Console.SetCursorPosition(30, 15);
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("*** GAME STATS ***");
        Console.SetCursorPosition(30, 16);
        Console.WriteLine("You sank "+Convert.ToString(computerBoatsSunk)+" boats.");
        Console.SetCursorPosition(30, 17);
        Console.WriteLine("The computer sank "+Convert.ToString(userBoatsSunk)+" boats.");
        Console.SetCursorPosition(30, 18);
        Console.WriteLine("The game lasted "+Convert.ToString(go)+" rounds.");
        // Display instructions and next steps
        Console.SetCursorPosition(30,20);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("How about another game?");
        Helpful.AddWindowFooter("[DELETE] = New Game, [ESCAPE] = return to main menu");
        File.Delete("Games/"+gameName+".battle"); // delete game file, it's finished!
        // while user hasn't done something, read key and process
        bool selecting = true;
        ConsoleKeyInfo keyInfo;
        while (selecting)
        {
            keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.Delete: // DEL key = new game
                    GameLoader.New();
                    break;
                case ConsoleKey.Escape: // ESC key = return to main menu
                    selecting = false;
                    break;
            }
        }
    } 
}
