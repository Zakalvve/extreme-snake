using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pipeline
{
    public abstract class BasePipelineProcess<output, input> : IPipelineProcess<output,input>
    {
        public BasePipelineProcess(Func<input,Func<output>,output> process) : this(process,null) { }

        public BasePipelineProcess(Func<input,Func<output>,output> process,IPipelineProcess<output,input> previous) {
            Process = process;
            previous?.setNextProcess(this);
            _nextProcess = null;
        }

        //the next process in the chain of responsibility
        protected IPipelineProcess<output,input> _nextProcess;

        //the developer defined process logic
        protected Func<input,Func<output>,output> Process { get; set; }

        public abstract output Execute(input data);
        //set the next process in the chain
        public void setNextProcess(IPipelineProcess<output,input> nextProcess) {
            _nextProcess = nextProcess;
        }
    }
}
