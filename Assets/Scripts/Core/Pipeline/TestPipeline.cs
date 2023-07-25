using System;
using UnityEngine;
using Pipeline;
using System.Collections;
public class TestPipeline : MonoBehaviour
{
    public Pipeline<string> AttackPipeline = new Pipeline<string>();
    public PipelineCoroutine<string> AttackPipelineCoroutine = new PipelineCoroutine<string>();
    private float _elapsed = 0;
    // Start is called before the first frame update
    void Start()
    {
        //ExecuteBasicPipeline();
        ExecuteCoroutinePipeline();
    }

    // Update is called once per frame
    void Update()
    {
        if (_elapsed > 1) {
            //Debug.Log($"update loop running " + Time.realtimeSinceStartup);
            _elapsed = 0;
        }
        _elapsed += Time.deltaTime;
    }

    public IEnumerator SelectTargetCoroutine(Action callback)
    {
        bool done = false;
        while(!done) {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space)) {
                done = true;
            }
            yield return null;
        }
        callback();
    }

    public void ExecuteBasicPipeline() {
        //define the attack pipeline
        AttackPipeline.AddProcess((string input,Func<string> next) => {
            Debug.Log("Beginning attack");
            string output = next();
            Debug.Log("Attack Finished");
            return output;
        });

        AttackPipeline.AddProcess((string input,Func<string> next) => {
            Debug.Log("Selecting Target");
            StartCoroutine(SelectTargetCoroutine(() => Debug.Log("Space Pressed")));
            return next();
        });

        AttackPipeline.AddProcess((string input,Func<string> next) => {
            Debug.Log($"Dealing damage to {input}");
            //let player select square and get its name
            return next();
        });

        Debug.Log(AttackPipeline.Execute("Default Target"));
    }

    public void ExecuteCoroutinePipeline() {
        AttackPipelineCoroutine.AddProcess(StartAttack);
        AttackPipelineCoroutine.AddProcess(SelectTarget);
        AttackPipelineCoroutine.AddProcess(ApplyDamage);

        StartCoroutine(AttackPipelineCoroutine.Execute("Bunny"));
        Debug.Log("Coroutine complete?");
    }

    public IEnumerator StartAttack(string data, Func<IEnumerator> next) {
        Debug.Log("Beginning attack");
        yield return next();
        Debug.Log("Attack Finished");
    }

    public IEnumerator SelectTarget(string data,Func<IEnumerator> next) {
        Debug.Log("Selecting Target");
        //yield return SelectTargetCoroutine(() => Debug.Log("Space Pressed"));
        yield return next();
    }

    public IEnumerator ApplyDamage(string data,Func<IEnumerator> next) {
        Debug.Log($"Dealing damage to {data}!");
        yield return next();
    }
}
