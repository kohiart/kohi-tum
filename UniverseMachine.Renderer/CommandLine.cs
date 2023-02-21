// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

namespace UniverseMachine.Renderer;

internal static class CommandLine
{
    public static async Task MastheadAsync()
    {
        var color = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        await Console.Out.WriteLineAsync(@"  _  __    _    _ 
 | |/ /___| |_ (_)
 | ' </ _ \ ' \| |
 |_|\_\___/_||_|_|
");

        Console.ForegroundColor = color;
    }
}