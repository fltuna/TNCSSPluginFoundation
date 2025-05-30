﻿using CounterStrikeSharp.API;
using Microsoft.Extensions.DependencyInjection;
using TNCSSPluginFoundation.Example.Dependency;
using TNCSSPluginFoundation.Example.Interfaces;
using TNCSSPluginFoundation.Example.Modules;
using TNCSSPluginFoundation.Example.Modules.DI;

namespace TNCSSPluginFoundation.Example;

public sealed class TncssExamplePlugin: TncssPluginBase
{
    // Same as CounterStrikeSharp plugin
    public override string ModuleName => "TNCSSExamplePlugin";
    public override string ModuleVersion => "0.3.1";
    
    // This is a base cfg directory path, but I'm not implemented anything yet.
    public override string BaseCfgDirectoryPath => ModuleDirectory;
    
    // The autogenerated ConVar configuration file path.
    // This config file is executed from game's `exec` command. so you need to place under a `game/csgo/cfg/` directory.
    public override string ConVarConfigPath => Path.Combine(Server.GameDirectory, "csgo/cfg/TNCSSExamplePlugin/TNCSSExamplePlugin.cfg");
    
    // This is a chat prefix. when you use TncssPluginBase::LocalizeStringWithPluginPrefix(), it will return translated string with this prefix.
    // For instance: `[TNCSSExample] This is a translated message!`
    public override string PluginPrefix => "[TNCSSExample]";
    
    // If this enabled, PluginPrefix will be treated as Translation Key.
    public override bool UseTranslationKeyInPluginPrefix => false;


    // These are initialization and unloading methods.
    // I have sorted method order in this class, so execution order is top to down.
    
    
    /// <summary>
    /// In this method, You can add your own required services to DI container.
    /// </summary>
    /// <param name="collection">Plugin's DI Container</param>
    /// <param name="provider">Plugin's DI Container</param>
    protected override void RegisterRequiredPluginServices(IServiceCollection collection, IServiceProvider provider)
    {
        // At this example, We will set the plugin's IDebugLogger to my own implementation of IDebugLogger.
        DebugLogger = new SimpleDebugLogger(provider);
        
        // This is a sample of registering your own service to DI container.
        collection.AddSingleton<IPluginDependencyExample, PluginDependencyExample>();
    }

    protected override void TncssOnPluginLoad(bool hotReload)
    {
        RegisterModule<MapChanger>();
        RegisterModule<PlayerUtility>();
        
        // The initialization order doesn't matter as long as you follow our framework's rules.
        // There is only one rule: You can obtain dependencies from the DI container only when the AllPluginsLoaded method is called in PluginModules.
        RegisterModule<DiTest>();
        RegisterModule<ModuleDependency>();
    }


    protected override void LateRegisterPluginServices(IServiceCollection collection, IServiceProvider provider)
    {
        // This method will call when CounterStrikeSharp's AllPluginsLoaded execution.
        // So you can get other plugins capability and register to this framework's DI container here.
    }

    protected override void TncssAllPluginsLoaded(bool hotReload)
    {
    }


    protected override void TncssOnPluginUnload(bool hotReload)
    {
    }
}