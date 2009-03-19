using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commando.ai
{
    public abstract class System
    {
        public AI AI_;

        public System(AI ai)
        {
            AI_ = ai;
        }

        abstract public void update();

        abstract public void draw();
    }
}
