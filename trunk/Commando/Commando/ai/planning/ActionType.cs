using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commando.ai.planning
{
    internal class ActionType
    {
        private Action action_;

        private ActionType() { }

        public ActionType(Action action)
        {
            action_ = action;
        }

        public Action getAction()
        {
            return action_;
        }
    }
}
