namespace Battleboats;

public class GameStorage
{
    public static void Save(string gameName, bool creating, char[,] userGrid, char[,] computerGrid, int go) {
        if (creating) { // if game exists and we are creating, check for name conflicts
            DirectoryInfo d = new DirectoryInfo("Games");
            FileInfo[] files = d.GetFiles("*.battle");
            foreach (FileInfo file in files) {
                if (file.Name == gameName + ".battle") {
                    gameName += " (1)";
                }
            }
        }

        using (BinaryWriter writer = new BinaryWriter(File.Open("Games/" + gameName + ".battle", FileMode.Create))) // overwrites contents
        {
            writer.Write(go);
            for (int i = 0; i < 8; i++) { // write user grid
                for (int j = 0; j < 8; j++) {
                    writer.Write(userGrid[i,j]);
                }
            }
            for (int i = 0; i < 8; i++) { // write computer grid
                for (int j = 0; j < 8; j++) {
                    writer.Write(computerGrid[i,j]);
                }
            }
        }
    }

    public static (int, char[,], char[,]) Load(string gameName) {
        using (BinaryReader reader = new BinaryReader(File.Open("Games/" + gameName + ".battle", FileMode.Open))) // overwrites contents
        {
            int go = reader.ReadInt32();
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
            return (go, userGrid, computerGrid);
        }
    }
}
