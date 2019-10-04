using Cami.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CamiFramwork.UndoRedo
{
    public class CommandStack : BaseBehaviour
    {
        public void Start()
        {
            commandPool.SpawnDelegate = CreateNewCommand;
        }
     
        public void BeginNewCommand()
        {
            if (CommandActive)
                EndCommand();

            Command command = commandPool.Pop();

            if (first == null)
            {
                first = command;
                current = command;
                last = command;
            }
            else
            {
                if (current == null || current != last)
                {
                    RemoveRedoCommands();

                    if (current == null)
                    {
                        current = command;
                        first = command;
                    }
                }
            
                current.Next = command;
                command.Prev = current;
                last = command;
                current = command;
            }

            CommandActive = true;
        }

        public void EndCommand()
        {
            CommandActive = false;
        }

        public void DoAction(Action doAction, Action undoAction)
        {
            DoAction(new RelayCommandAction(doAction, undoAction));
        }

        public void DoAction(CommandAction action)
        {
            if (!CommandActive)
                return;

            current.DoAction(action);
        }

        public void Redo()
        {
            if (CommandActive)
                return;

            if (current == null)
            {
                if (first == null)
                    return;

                current = first;
            }
            else
            {
                if (current == last)
                    return;

                current = current.Next;
            }
            if (current != null)
            {
                current.Do();
            }
        }

        public void Undo()
        {
            if (CommandActive)
                return;

            if (current != null)
            {
                current.Undo();
                current = current.Prev;
            }
        }

        private void RemoveRedoCommands()
        {
            if (CommandActive)
                return;

            // Remove redos
            Command command = null;
            if (current == null)
            {
                command = first;
                first = null;
                last = null;
            }
            else
            {
                command = current.Next;
                last = current;
            }
            while (command != null)
            {
                Command next = command.Next; // store temp as clear nulls next

                command.Clear();
                commandPool.Push(command);

                command = next;
            }
        }
    
        private Command CreateNewCommand()
        {
            return new Command();
        }

        #region Properties
        public bool CommandActive { get; private set; }
        public bool CanRedo
        {
            get
            {
                return !CommandActive &&
                    ((current == null && first != null) ||
                    (current != null && current != last));
            }
        }

        public bool CanUndo
        {
            get
            {
                return !CommandActive && current != null;
            }
        }
        #endregion Properties

        #region Fields
        private Command first;
        private Command current;
        private Command last;

        private Pool<Command> commandPool = new Pool<Command>();
        #endregion Fields
    }
}