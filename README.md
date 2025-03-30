## Hello Everyone!

As I continue my journey as a Junior Game Developer, I want to tell you about my second project, a 2D Action RPG game. I started this project by following the "2D RPG Game Development with Unity - Alex Dev" course on Udemy. Then, I added my own unique mechanics and created a demo. I spent more than 200 hours on this project, working hard on research and development. It was a really challenging process! :)

## Project Details:

### 1) UI Design
The Main Menu has a "Continue" button if there is a saved game.

In the game, there are four different menus:

Character Menu: Shows character stats and equipped items.

Skill Tree: Has 25 different skills that can be managed.

Crafting Menu: Allows crafting new equipment and lists required materials.

Settings: Adjusts sound/music levels and has a Save & Return to Main Menu button.



### 2) Character Movement Mechanics
I designed the character movement with essential mechanics for a 2D game. My goal was to make the gameplay feel smooth and enjoyable.



### 3) Combat System & Epic Items
The combat system changes based on character stats, similar to RPG games.
I also added visual effects, sound effects, and unique item properties to create variety.



### 4) Skill Tree Details
The Skill Tree includes 25 connected and unlockable skills.

Some skills are linked together, like cloning abilities.

This was one of the most detailed parts of my project, and I spent a lot of time developing it.

### [You can check the Figma plan for more details](https://www.figma.com/design/UVJL21hR8sEORcR7B1AjkZ/Yetenek-A%C4%9Fac%C4%B1?node-id=0-1&t=waoRvMj7U3uQa9vD-1)



### 5) Inventory & Crafting System
The Inventory has two categories: Equipment and Materials.

Players can craft different equipment using various materials.

Fixing bugs in this system took a long time, but now it works almost perfectly.

This was the most challenging part of the project for me.



### 6) Save & Load System
The game has a simple save and load system using JSON data format.

All important data is saved and loaded.

If there is no saved data, the "Continue" button in the Main Menu is hidden.

I added small details to improve the save system.

## Technical Details
I developed this project using Unity. I used Finite State Machine (FSM) for development.

## What is FSM?
FSM is a common method in game development. It is used to control:

=> Character behaviors

=> Game mechanics

=> Animation transitions

=> Dialogue systems

=> Menu flow

## FSM has three main components:

States: The current state of a character or object (Example: Idle, Running, Attacking).

Transitions: Conditions for changing states (Example: "If the enemy sees the player, change from Idle to Battle.")

Events & Conditions: Triggers for transitions (Example: "If the player’s health is below 50, switch to Running state.")

FSM helps keep the code organized, efficient, and easy to expand.

### Would You Like to Check It Out?


### [If you want to play my demo](https://drive.google.com/file/d/1RdWIY0h2gS_5gOYK6WXaU4TFYp5_0Nmd/view?usp=sharing)


### [FSM Details](https://medium.com/yaz%C4%B1l%C4%B1m-ve-bili%C5%9Fim-kul%C3%BCb%C3%BC/sonlu-durum-maki%CC%87nesi%CC%87-finite-state-machine-nedi%CC%87r-3d3626e16193)


### [Udemy Course Link](https://www.udemy.com/course/2d-rpg-alexdev/)


### [Watch this project gameplay video on YouTube](https://youtu.be/QVXG2g9C6ko)

[![Watch the video](https://img.youtube.com/vi/QVXG2g9C6ko/maxresdefault.jpg)](https://youtu.be/QVXG2g9C6ko)

