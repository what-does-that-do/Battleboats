using System;

namespace Battleboats;

public class Grid
{
    public static void Draw(char[,] grid, int x, int y, bool hideBoats) {
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.ForegroundColor = ConsoleColor.Black;
        // draw top 1-8 header.
        Console.Write(" ");
        for (int a = 1; a < 9; a++) {
            Console.Write(" "+a+" ");
        }

        // draw rows
        for (int i = 0; i < 8; i++) {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(i+1);
            for (int j = 0; j < 8; j++) { // loop through boat grid
                if (x == j && y == i) { // draw cursor in magenta
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.ForegroundColor = ConsoleColor.Black;
                } else { // cursor not there, continue as usual
                    if (grid[i,j] == '.') { // nothing there, blue square
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                    } else if (grid[i,j] == 'B') { // boat there
                        if (hideBoats) { // if hiding boats, show as a blue square
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            Console.ForegroundColor = ConsoleColor.Black;
                        } else { // not hiding boats, show as yellow square
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                    } else if (grid[i,j] == 'H') { // show hit as red square
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                    } else if (grid[i,j] == 'M') { // show miss as green square
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    } else { // something's gone wrong, don't show anything (black). Key is grey for debugging.
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                if (grid[i,j] == 'B' && hideBoats) { // if hiding boats, write a . not a B
                    Console.Write(" . ");
                } else { // write the character with space padding. Colour already set above.
                    Console.Write(" " + grid[i, j] + " ");
                }
            }
        }
    }
}