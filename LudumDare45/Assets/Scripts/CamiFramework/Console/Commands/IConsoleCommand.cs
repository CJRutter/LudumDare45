using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamiFramwork.ConsoleUtil
{
    public interface IConsoleCommand
    {
        void Execute(Console console, IList<string> args);

        string Description { get; }
    }
}
