using UniRx;
using UnityEngine;

public class UniRxTest : MonoBehaviour
{
    void Start()
    {
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ => Debug.Log("Space key pressed"))
            .AddTo(this);
    }
}
