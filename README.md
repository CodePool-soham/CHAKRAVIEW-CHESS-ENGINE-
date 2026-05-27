♟️ ChakraView Chess Engine

ChakraView is a modular chess engine built in C# with a separate GUI project for visualization and interaction. The project is structured into two main components:

ChakraView (Engine Core)
ChakraView.GUI (Windows Forms UI)
<img width="903" height="688" alt="image" src="https://github.com/user-attachments/assets/77359d0b-2731-4a6c-822c-9d126a36cd04" />


The ChakraView project contains the complete chess engine logic.

✔ Features implemented:
Full chess rules validation
Move generation system
Game state tracking
Basic AI module (for move decision making)
Turn management system
Timer system for gameplay control
Modular structure for easy expansion
🔧 Design Approach:
Separation of concerns (AI, Rules, Game, Timer)
Clean architecture for scalability
Engine is independent of UI (can run standalone)
🖥️ ChakraView.GUI (User Interface)

The GUI project is a Windows Forms application that connects to the engine.

✔ Features:
Interactive chessboard UI
Piece movement via mouse input
Visual representation of game state
Integration with ChakraView engine logic
Real-time updates after each move <br>
🎯 Purpose:

The GUI acts as a frontend layer, allowing users to play and test the engine visually instead of console-based interaction.

🔗 Architecture Overview
ChakraView (Engine)
        ↑
        │  (logic / rules / AI calls)
        ↓
ChakraView.GUI (Frontend)
Engine handles all chess logic
GUI only handles rendering and user input
Clear separation for maintainability
⚙️ Build & Run
Prerequisites:
.NET SDK 10+
Visual Studio 2022/2025
Run:
Open CHESS ENGINE.sln
Set ChakraView.GUI as Startup Project
Press F5 to run
