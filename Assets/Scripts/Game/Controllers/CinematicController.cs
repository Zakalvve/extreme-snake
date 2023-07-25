using Assets.Scripts.Game.Data;
using ExtremeSnake.Game;
using Pipeline;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Assets.Scripts.Game.Controllers
{
    public class CinematicController : MonoBehaviour
    {
        public void PlayIntro(Action onComplete) {
            CinematicPipelineEventArgs cineArgs = new CinematicPipelineEventArgs();
            GameManager.Instance.GameEmitter.Emit("CinematicCreate",this,cineArgs);
            StartCoroutine(IntroCutscene(cineArgs,onComplete));
        }

        private IEnumerator IntroCutscene(CinematicPipelineEventArgs cineArgs,Action onComplete) {
            GameSettings settings = GameManager.Instance.Settings;
            Camera mainCam = Camera.main;
            mainCam.enabled = false;
            cineArgs.Cinecam.enabled = true;
            for (int i = 0; i < settings.ActiveSession.Snakes.Count; i++) {
                PipelineCoroutine<CinematicArgs> panAndDisplayName = new PipelineCoroutine<CinematicArgs>();
                CinematicArgs cinematicArgs = new CinematicArgs();
                cinematicArgs.Name = settings.ActiveSession.Snakes[i].Name;
                cinematicArgs.Subject = settings.ActiveSession.Snakes[i].Segments.First.Value.Segment.transform;
                cinematicArgs.WaitLength = 1.3f;
                panAndDisplayName.AddProcess(cineArgs.Pan);
                panAndDisplayName.AddProcess(cineArgs.Display);
                panAndDisplayName.AddProcess(Wait);
                yield return panAndDisplayName.Execute(cinematicArgs);
            }
            mainCam.transform.position = cineArgs.Cinecam.transform.position;
            mainCam.orthographicSize = cineArgs.Cinecam.orthographicSize;
            mainCam.enabled = true;
            cineArgs.Cinecam.enabled = false;
            onComplete();
        }

        private IEnumerator Wait(CinematicArgs args,Func<IEnumerator> next) {
            yield return new WaitForSeconds(args.WaitLength);
            yield return next();
        }
    }
}
