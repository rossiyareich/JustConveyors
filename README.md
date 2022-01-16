# This project is no longer being actively developed or maintained. See the stated reasons below

# JustConveyors (WIP)
Mindustry and Factorio-style conveyors/transport system using SDL, OpenGL, and ImGUI on C#

## Basic rendering workflow
- ImGUI renders to its buffer and writes/draws to the screen
- Surfaces of individual components are Blit onto the main surface
- The pixels data in the main surface is used to create an OpenGL texture
- A seperate OpenGL shader program is attached and the OpenGL texture is loaded into the shader, which is mapped onto a quad and drawn on the screen

This rendering workflow/pipeline isn't particularly fast or memory-efficient-- quite the contrary. It's the only current viable option for mixing `SDL_Renderer` with ImGUI's OpenGL-bound rendering in .NET as there are no C# bindings for imgui_impl_sdl. 
It's highly recommended to instead generate the required bindings to use imgui_impl_sdl for any real-world usages.

## Bindings (local builds/copied from source):
- ImGui.NET : https://github.com/mellinoe/ImGui.NET
- ImGuiGL-Renderer : https://github.com/prime31/ImGuiGL-Renderer
- SDL2-CS : https://github.com/flibitijibibo/SDL2-CS

## Native dependencies:
- cimgui.dll : Bundled with ImGui.NET from https://github.com/mellinoe/ImGui.NET
- libpng16-16.dll, SDL2_image.dll, zlib1.dll from https://www.libsdl.org/projects/SDL_image/
- SDL2.dll from https://www.libsdl.org/

## Early versions:
![Testing screenshot](https://raw.githubusercontent.com/rossiyareich/JustConveyors/master/.github/images/screenshot0.png "Early render test")

## Videos

### Conveyor loops
https://user-images.githubusercontent.com/61973036/149662363-b8868bb0-a0a0-4781-9809-245226454d66.mp4

### Drawing guides
https://user-images.githubusercontent.com/61973036/149662369-e95ac476-30b9-43aa-b46f-9a1ef3923c60.mp4

### Zooming
https://user-images.githubusercontent.com/61973036/149662373-0cbc9a5f-77c5-4bf5-a3ea-6d94d6e602cc.mp4

### Pause&Play
https://user-images.githubusercontent.com/61973036/149662379-73c17d4a-7948-483a-b83c-23d23945665f.mp4

## TODO:
- Add multithreading
- Add logic for all blocks
- Add save strings

## This project is no longer being actively developed or maintained for the following reasons:
- I do not have the time and resources to further continue the development of JustConveyors for the being due to school and life in general
- The project is riddled with performance issues; until multithreading is implemented and the logic optimized, I foresee no active development as the final product will be slow and unusable
