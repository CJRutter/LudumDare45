using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamiFramwork.ConsoleUtil
{
    public class ActionCommand : IConsoleCommand
    {
        public ActionCommand(System.Action action, string description = null)
        {
            this.action = action;
            this.description = description;
        }

        public void Execute(Console console, IList<string> args)
        {
            action();
        }

        #region Properties
        public string Description { get { return description; } }
        #endregion Properties

        #region Fields
        private System.Action action;
        private string description;
        #endregion Fields
    }
}
