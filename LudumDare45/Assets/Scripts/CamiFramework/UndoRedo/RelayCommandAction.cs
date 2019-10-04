using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CamiFramwork.UndoRedo
{
    public class RelayCommandAction : CommandAction
    {
        public RelayCommandAction()
        {
        }
    
        public RelayCommandAction(Action doAction, Action undoAction)
        {
            this.DoAction = doAction;
            this.UndoAction = undoAction;
        }

        public override void Do()
        {
            if(DoAction != null)
                DoAction();
        }

        public override void Undo()
        {
            if (UndoAction != null)
                UndoAction();
        }

        #region Properties
        #endregion Properties

        #region Fields
        public Action DoAction;
        public Action UndoAction;
        #endregion Fields
    }
}