# Battleboats
A simple Battleboats game created entirely in C# for my Computer Science A Level.

## Downloading and Running
First, clone this repo.

> Note that I have not tested the exe file, if you encounter issues - follow the instructions for Mac/Linux.

- **Windows**: run **Battleboats.exe** which is located in `Battleboats/bin/Debug/net7.0`. Importantly, if you would like to move this exe, you must place the 'TextArt' folder in the same directory as the exe.

- **Mac/Linux**: Run the following commands (provided you have C# dotnet command line tools installed): <br>
`cd Battleboats` <br>
`dotnet bin/Debug/net7.0/Battleboats.dll`

Battleboats should now be running in your terminal window.

## Gameplay
- Place your boats on the grid. <br>
- Fire at the computer's boats to try and sink all of them. <br>
- The computer will then try and sink all of your boats. <br>
- Take it in turns until either you or the computer have sunk all of the opponent's boats. <br>
- The player who sinks all of the opponent's boats first wins!

## Controls
As a rule of thumb, use arrow keys to move the cursor, enter to shoot/select and escape to return to the main menu.

> All relevant controls are always displayed at the bottom of the game window.

## Potential Issues
If you are receiving the "Window Too Small" error, try switching to full screen.

If this does not solve the problem, try zooming in or out of your terminal/console window. Check your computer's documentation but usually this can be achieved with ctrl/cmd + or ctrl/cmd -