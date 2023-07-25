using System;
using System.Linq;

namespace Pipeline
{
	//defines a pipeline which takes a mutable input object and performs a series of processes upon it before returning the mutated output
	public class Pipeline<T> : BasePipeline<T, T>
	{
		//the collection of processes which form the pipeline
		public Pipeline() : base() { }

		//add a new process to the list
		public override void AddProcess(Func<T,Func<T>,T> process) {
			processes.Add(new PipelineProcess<T>(process, processes.LastOrDefault()));
		}

		//begins execution of the chain of processes
		//each process executes itself and can then choose to either return or execute the next process
		//since the control flow returns back through each process it is possible to execute code both before the endpoint has been reached and after
		public override T Execute(T data) {
			try {
				return processes[0].Execute(data);
			} catch (IndexOutOfRangeException error) {
				Console.WriteLine(error);
				return data;
			}
		}
	}
}
