using System.Collections;
using System.Collections.Generic;

readonly public struct MovieData
{
    public MovieData(string name, List<string> times)
    {
        this.Name = name;
        this.Times = times;
    }

    public string Name { get; }
    public List<string> Times { get; }
}
