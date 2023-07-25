using System;
using System.Collections;

namespace Pipeline
{
    public class PipelineCoroutineProcess<input> : BasePipelineProcess<IEnumerator,input>
    {
        public PipelineCoroutineProcess(Func<input,Func<IEnumerator>,IEnumerator> process) : base(process) { }

        public PipelineCoroutineProcess(Func<input,Func<IEnumerator>,IEnumerator> process,IPipelineProcess<IEnumerator,input> previous) : base(process,previous) { }

        //execute this process
        public override IEnumerator Execute(input data) {
            return Process(data, () => {
                return RunNext(data);
            });
        }

        private IEnumerator RunNext(input data) {
            if (_nextProcess == null) yield return null;
            else yield return _nextProcess.Execute(data);
        }
    }
}
