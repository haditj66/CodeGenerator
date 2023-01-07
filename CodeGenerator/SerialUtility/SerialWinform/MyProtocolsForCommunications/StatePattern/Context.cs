using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProtocolsForCommunications.StatePattern
{

    public abstract class Context<TState> where TState : class
        //, new()//<TStates, UContext>   //where UContext : IContext<TStates, UContext>  //where TStates : State<UContext>
    {
        public TState MyCurrentState { get; set; }

        public Context(TState _myCurrentState)
        {
            MyCurrentState = _myCurrentState;
            InitCrossReference();

        }

        //  function that references the cross references.  
        public abstract void InitCrossReference();

        public void ChangeState(TState stateToChangeTo)
        {
            MyCurrentState = stateToChangeTo;
            InitCrossReference();
        }



    }
}
