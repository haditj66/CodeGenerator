using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProtocolsForCommunications.StatePattern
{
    public abstract class State<TContext> where TContext : class //IContext<TContext>
    {

        public TContext _context { get; set; }



    }
}
