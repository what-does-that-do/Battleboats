namespace Battleboats;

public class GameStorage
{
    // Handles the logic behind saving games:
    public static void Save(string gameName, bool creating, char[,] userGrid, char[,] computerGrid, int go) {
        if (creating) { // if game exists and we are creating, check for name conflicts
            DirectoryInfo d = new DirectoryInfo("Games");
            FileInfo[] files = d.GetFiles("*.battle"); // list all .battle files in directory
            foreach (FileInfo file in files) { // loop through file list, if it contains the name of the new game, add a (1) to the game name
                if (file.Name == gameName + ".battle") {
                    gameName += " (1)";
                }
            }
        }
        
        bool saving = true;
        while (saving) // keep trying to save - the following will fail if the computer is being slow, so we wait until it catches up
        {
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open("Games/" + gameName + ".battle", FileMode.Create))) // overwrites contents
                {
                    writer.Write(go); // write the go number
                    for (int i = 0; i < 8; i++)
                    { // write user grid
                        for (int j = 0; j < 8; j++)
                        {
                            writer.Write(userGrid[i, j]);
                        }
                    }
                    for (int i = 0; i < 8; i++)
                    { // write computer grid
                        for (int j = 0; j < 8; j++)
                        {
                            writer.Write(computerGrid[i, j]);
                        }
                    }
                }
                saving = false; // saved successfully, stop retrying
            }
            catch
            {
                Thread.Sleep(100); // saving failed - wait 0.1s and try again
            }
        }
    }
    // Loads games from file into memory and returns variables
    public static (int, char[,], char[,]) Load(string gameName) {
        using (BinaryReader reader = new BinaryReader(File.Open("Games/" + gameName + ".battle", FileMode.Open))) // reads file
        {
            // Create empty variables
            int go = reader.ReadInt32(); // Read go number into int
            char[,] userGrid = new char[8,8];
            char[,] computerGrid = new char[8,8];

            for (int i = 0; i < 8; i++) { // read user grid
                for (int j = 0; j < 8; j++) {
                    userGrid[i,j] = reader.ReadChar();
                }
            }
            for (int i = 0; i < 8; i++) { // read computer grid
                for (int j = 0; j < 8; j++) {
                    computerGrid[i,j] = reader.ReadChar();
                }
            }
            return (go, userGrid, computerGrid); // return loaded variables
        }
    }
}
