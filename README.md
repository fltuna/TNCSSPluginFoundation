﻿# [TNCSSPluginFoundation](https://github.com/fltuna/TNCSSPluginFoundation)

## About

A simple framework for CounterStrikeSharp plugin. 

Provides powerful and faster development experience if your plugin feature is separated by module.


## Features

- Automated load/unload life cycle
- Automated ConVar config generation
- Dependency Injection with DI container
- Module oriented development

### Why don't you use CounterStrikeSharp's built-in DI?

Since CounterStrikeSharp's DI requires injection class and definition class.

But I've created this framework for faster development, and convenience.

Also, If we make plugin with module oriented development, there is no way to inject the proper initialized and working module instance to other module.

## How to use

1. Download latest binary from [latest Release](https://github.com/fltuna/TNCSSPluginFoundation/releases/latest)
2. Put this framework to a CounterStrikeSharp's shared folder
3. Develop plugin using this framework or Get plugin uses this framework
4. Install the plugin to your server
5. Run server

## Development Documentation

1. Create a new class library or go to existing project
2. Run `dotnet add package TNCSSPluginFoundation`
3. Ready to develop

### Example

See [Example Plugin](TNCSSPluginFoundation.Example) for module initialization and DI container usage.

Also, [lupercalia-mg-cs2](https://github.com/fltuna/lupercalia-mg-cs2) is heavily uses this framework.



Detailed documentation is not available yet.