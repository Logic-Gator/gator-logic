using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Threading;
#if PACKAGE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace GatOR.Logic
{
    public static class UnityExtensions
    {
        public static T Or<T>(this T target, T ifNullOrDestroyed) where T : Object =>
            target ? target : ifNullOrDestroyed;

        public static T Or<T>(this T target, Func<T> ifNullOrDestroyed) where T : Object =>
            target ? target : ifNullOrDestroyed();

        public static T OrThrowNullArgument<T>(this T target, string argName) where T : Object =>
            target ? target : throw new ArgumentNullException(argName);

        public static Coroutine StartCoroutine(this MonoBehaviour monoBehaviour,
            Func<IEnumerator> enumeratorFunction) =>
            monoBehaviour.StartCoroutine(enumeratorFunction());

#if PACKAGE_UNITASK
        public static UniTask StartCoroutineAsTask(this MonoBehaviour monoBehaviour, Func<IEnumerator> routine,
            CancellationToken cancellationToken = default)
        {
            return monoBehaviour.StartCoroutineAsTask(routine(), cancellationToken);
        }

        public static UniTask StartCoroutineAsTask(this MonoBehaviour monoBehaviour, IEnumerator routine,
            CancellationToken cancellationToken = default)
        {
            var source = AutoResetUniTaskCompletionSource.Create();
            var coroutine = monoBehaviour.StartCoroutine(Routine(routine, source));
            cancellationToken.Register(() => monoBehaviour.StopCoroutine(coroutine));
            return source.Task;

            static IEnumerator Routine(IEnumerator routine, AutoResetUniTaskCompletionSource task)
            {
                yield return routine;
                task.TrySetResult();
            }
        }
#endif
    }
}
