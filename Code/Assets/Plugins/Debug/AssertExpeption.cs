using System;

public class AssertExpeption : Exception
{
    // Methods
    public AssertExpeption()
    {
    }

    public AssertExpeption(string inMsg) : base(inMsg)
    {
    }
}