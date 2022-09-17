using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct MovieData
{
    public MovieData(int id, string name, List<string> times)
    {
        this.id = id;
        this.name = name;
        this.times = times;
    }

    public int id;
    public string name;
    public List<string> times;
}
