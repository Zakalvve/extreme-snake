using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pipeline
{
    public class PipelineCoroutine<T> : BasePipeline<IEnumerator,T>
    {
        public PipelineCoroutine() : base() { }
        public override void AddProcess(Func<T,Func<IEnumerator>,IEnumerator> process) {
            processes.Add(new PipelineCoroutineProcess<T>(process,processes.LastOrDefault()));
        }

        public override IEnumerator Execute(T data) {
            if (processes.Count > 0) {
                yield return processes[0].Execute(data);
            }
            else {
                yield return null;
            }
        }
    }
}
