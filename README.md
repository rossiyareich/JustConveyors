# JustConveyors (WIP)
Mindustry and Factorio-style conveyors/transport system using SDL, OpenGL, and ImGUI on C#

## Basic rendering workflow (utilising workarounds; very slow)
- ImGUI renders to its buffer and writes/draws to the screen
- Using `SDL_RenderReadPixels` (a slow operation), the pixel data of SDL's (seperate) rendering target is saved onto an `SDL_Surface`
- The pixels data in the `SDL_Surface` is used to generate an OpenGL texture
- A seperate OpenGL shader program is attached and the OpenGL texture is loaded into the shader, which is mapped onto a quad and drawn on the screen

This rendering workflow/pipeline isn't particularly fast or memory-efficient-- quite the contrary. It's the only current viable option for mixing `SDL_Renderer` with ImGUI's OpenGL-bound rendering in .NET as there are no C# bindings for imgui_impl_sdl. 
It's highly recommended to instead generate the required bindings to use imgui_impl_sdl for any real-world usages.

`Please do not do anything like I did in your SDL or OpenGL projects, no matter the language(s) used.`

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
- Fix out-of-place animation frames of assets
- Add conveyors, bridge conveyors, junctions, rubies, routers
- Add logic for all blocks
- Add UI with ImGUI and handle user input with SDL
