# ♟️ ChakraView Chess Engine

ChakraView is a modular chess engine built in C# with a separate GUI project for visualization and interaction. The project is structured into two main components:

* ChakraView (Engine Core)
* ChakraView.GUI (Windows Forms UI)
<img width="903" height="688" alt="image" src="https://github.com/user-attachments/assets/77359d0b-2731-4a6c-822c-9d126a36cd04" />


The ChakraView project contains the complete chess engine logic.

# ✔ Features implemented:

* Full chess rules validation <br>
* Move generation system <br>
* Game state tracking <br>
* Basic AI module (for move decision making) <br>
* Turn management system <br>
* Timer system for gameplay control <br>
* Modular structure for easy expansion <br>

# 🔧 Design Approach:
* Separation of concerns (AI, Rules, Game, Timer) <br>
* Clean architecture for scalability <br>
* Engine is independent of UI (can run standalone) <br>

# 🖥️ ChakraView.GUI (User Interface) <br>

The GUI project is a Windows Forms application that connects to the engine.

✔ Features:
* Interactive chessboard UI <br>
* Piece movement via mouse input <br>
* Visual representation of game state <br>
* Integration with ChakraView engine logic <br>
* Real-time updates after each move <br> 


# 🎯 Purpose:

The GUI acts as a frontend layer, allowing users to play and test the engine visually instead of console-based interaction.

<img width="900" height="231" alt="image" src="https://github.com/user-attachments/assets/b78142f5-d9e9-4e32-85e5-da806e6c047b" /> <br>

* Engine handles all chess logic <br>
* GUI only handles rendering and user input <br>
* Clear separation for maintainability <br>

⚙️ Build & Run
# Prerequisites:
* .NET SDK 8.0
* Visual Studio 2022/2025
# Run:
* Open CHESS ENGINE.sln
* Set ChakraView.GUI as Startup Project
* Press F5 to run
