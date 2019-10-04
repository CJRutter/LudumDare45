using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CamiFramwork.UndoRedo
{
    public class Command
    {
        public Command()
        {
        }

        public void Do()
        {
            CommandAction current = first;
            while (current != null)
            {
                current.Do();
                current = current.Next;
            }
        }

        public void Undo()
        {
            CommandAction current = last;
            while (current != null)
            {
                current.Undo();
                current = current.Prev;
            }
        }

        public void DoAction(CommandAction action)
        {
            action.Do();

            if (first == null)
            {
                first = action;
                last = action;
            }
            else
            {
                last.Next = action;
                action.Prev = last;
                last = action;
            }
            ++ActionCount;
        }

        public void Clear()
        {
            Next = null;
            Prev = null;
            ActionCount = 0;
            first = null;
            last = null;
        }

        #region Properties
        public Command Prev { get; set; }
        public Command Next { get; set; }
        public int ActionCount { get; private set; }
        #endregion Properties

        #region Fields
        private CommandAction first;
        private CommandAction last;
        #endregion Fields
    }
}