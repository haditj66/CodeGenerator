using System.Collections.Generic;

namespace CgenMin.MacroProcesses
{
    public abstract class AOObserver : AOWritableConstructible
    {
        public List<AEFilter> FiltersIflowTo { get; protected set; }

        

        public AESensor SensorIOriginateFrom { get; protected set; }
        public AESPBBase SPBIOriginateFrom { get; protected set; }

        public AOObserver(string fromeLibraryName,  string instanceName, AOTypeEnum aotype) 
            : base(fromeLibraryName,  instanceName, aotype)
        {
            FiltersIflowTo = new List<AEFilter>();

            if (aotype == AOTypeEnum.SPB)
            {
                SPBIOriginateFrom = (AESPBBase)this;   
            }
            else if (aotype == AOTypeEnum.Sensor)
            {
                SensorIOriginateFrom = (AESensor)this;
            } 

        }

        public AEFilter FlowIntoFilter(AEFilter filter)
        {
            if (this.AOType == AOTypeEnum.Sensor)
            {
                filter.SensorIOriginateFrom = (AESensor)this;
                filter.FilterId = filter.SensorIOriginateFrom.FiltersIflowTo.Count + 1;
            }
            if (this.AOType == AOTypeEnum.SPB)
            {
                filter.SPBIOriginateFrom = (AESPBBase)this;
                filter.FilterId = filter.SPBIOriginateFrom.FiltersIflowTo.Count + 1;
            }
            if (this.AOType == AOTypeEnum.Filter)
            {
                filter.FilterICameFrom =  ((AEFilter)this);

                if (this.SensorIOriginateFrom != null)
                {
                    this.SensorIOriginateFrom.FlowIntoFilter(filter);
                }

                if (this.SPBIOriginateFrom != null)
                {
                    this.SPBIOriginateFrom.FlowIntoFilter(filter);
                }
            }

            this.FiltersIflowTo.Add(filter);

            return filter;
        }



        public string GetFilterTemplateArgsValues()
        {
            string templateArgs = "";
            //get filters
            if (this.FiltersIflowTo.Count == 0)
            {

            }
            else
            {
                //treat the first filter differently as it does not have a second param
                if (this.FiltersIflowTo.Count > 0)
                {
                    templateArgs += $" {FiltersIflowTo.Count.ToString()}, {FiltersIflowTo[0].HowItShowsUpInTemplateArg}";
                }

                for (int i =1; i < this.FiltersIflowTo.Count; i++)
                {

                    string secondArg = FiltersIflowTo[i].FilterICameFrom == null ? "0" : FiltersIflowTo[i].FilterICameFrom.FilterId.ToString();
                    templateArgs += $",{FiltersIflowTo[i].HowItShowsUpInTemplateArg} ,{secondArg}  ";
                }
            }
            return templateArgs;
        }


    }
}
