using System;
using System.Collections.Generic;

namespace Pipeline
{
    public abstract class BasePipeline<output, input>
    {
        public BasePipeline() {
            processes = new List<IPipelineProcess<output, input>>();
        }
        //the collection of processes which form the pipeline
        public List<IPipelineProcess<output, input>> processes;

        //add a new process to the list
        public abstract void AddProcess(Func<input,Func<output>,output> process);

        //begins execution of the chain of processes
        //each process executes itself and can then choose to either return or execute the next process
        //since the control flow returns back through each process it is possible to execute code both before the endpoint has been reached and after
        public abstract output Execute(input data);
    }
}
