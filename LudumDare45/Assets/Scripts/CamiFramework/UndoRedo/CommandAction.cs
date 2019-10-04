using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CamiFramwork.UndoRedo
{
    public class CommandAction
    {
        public CommandAction()
        {
        }

        public virtual void Do()
        {
        }

        public virtual void Undo()
        {
        }

        #region Properties
        public CommandAction Prev { get; set; }
        public CommandAction Next { get; set; }
        #endregion Properties

        #region Fields
        #endregion Fields
    }
}