// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

namespace UniverseMachine.Renderer;

internal sealed class Manifest
{
    public string UniverseMachineRenderer { get; set; } = null!;
    public string UniverseMachineParameters { get; set; } = null!;

    public static Dictionary<int, int> GetSeeds(string path)
    {
        var lines = File.ReadAllLines(path);
        var dictionary = new Dictionary<int, int>();
        foreach (var line in lines)
        {
            var tokens = line.Split(": ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            dictionary[int.Parse(tokens[0])] = int.Parse(tokens[1]);
        }

        return dictionary;
    }
}