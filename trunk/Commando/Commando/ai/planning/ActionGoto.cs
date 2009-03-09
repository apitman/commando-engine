using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commando.levels;

namespace Commando.ai.planning
{
    class ActionGoto : Action
    {
        internal TileIndex target_;

        public ActionGoto(TileIndex target)
        {
            target_ = target;
        }

        internal override bool testPreConditions(SearchNode node)
        {
            // assume we can always get from point A to point B
            return true;
        }

        internal override SearchNode unifyRegressive(SearchNode node)
        {
            SearchNode parent = node.getPredecessor();
            TileIndex target = node.values[Variable.Location].t;
            parent.action = new ActionGoto(target);
            // TODO calculate distance
            float distance = 0;
            parent.cost += 1.0f + distance;
            // TODO
            // determine if GOTO needs to set predecessor position
            // parent.setPosition(Variable.Location,
            parent.resolved[Variable.Location] = false;
            return parent;
        }
    }
}
