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
                if (x == j && y == i) { // draw cursor
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.ForegroundColor = ConsoleColor.Black;
                } else {
                    if (grid[i,j] == '.') {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                    } else if (grid[i,j] == 'B') {
                        
                        if (hideBoats) {
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            Console.ForegroundColor = ConsoleColor.Black;
                        } else {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                    } else if (grid[i,j] == 'H') {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                    } else if (grid[i,j] == 'M') {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    } else {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                if (grid[i,j] == 'B' && hideBoats) {
                    Console.Write(" . ");
                } else {
                    Console.Write(" "+grid[i,j]+" ");
                }
            }
        }
    }
}