using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commando.ai.planning
{
    class ActionAttackRanged : Action
    {

        internal override bool testPreConditions(SearchNode node)
        {
            return
                node.boolPasses(Variable.Ammo, true);
        }

        internal override SearchNode unifyRegressive(SearchNode node)
        {
            SearchNode parent = node.getPredecessor();
            parent.action = new ActionAttackRanged();
            parent.cost += 3;
            parent.setInt(Variable.TargetHealth, node.values[Variable.TargetHealth].i + 1);
            parent.setBool(Variable.Ammo, true);
            return parent;
        }
    }
}
