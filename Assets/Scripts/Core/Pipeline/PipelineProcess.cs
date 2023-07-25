using System;
namespace Pipeline
{
	public class PipelineProcess<T> : BasePipelineProcess<T,T> 
	{
		public PipelineProcess(Func<T,Func<T>,T> process) : base(process) { }

        public PipelineProcess(Func<T,Func<T>,T> process,IPipelineProcess<T,T> previous) : base(process, previous) { }

        //execute this process
        public override T Execute(T data) {
			return Process(data,() => {
				if (_nextProcess == null) return data;
				return _nextProcess.Execute(data);
			});
		}
	}
}
