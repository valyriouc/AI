﻿namespace AIConnector.Gemini;

public readonly struct GeminiFunctionCallPart(string name) : IGeminiRequestPart
{
    public string Name { get; } = name;
    
    // TODO: Impl. args struct 
}