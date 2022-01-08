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

## TODO:
- Add zooming and grids and snap-to-grid
- Add UI with ImGUI and handle user input with SDL
- Fix leaks
- Add logic for all blocks
