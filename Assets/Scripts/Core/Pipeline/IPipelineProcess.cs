namespace Pipeline
{
	//defines the interface for a pipeline process which represents a single step in a pipeline
	//T a mutable reference type which is mutated as the pipeline is executed
	public interface IPipelineProcess<output, input> 
	{
		public void setNextProcess(IPipelineProcess<output, input> next);
		public output Execute(input data);
	}
}
