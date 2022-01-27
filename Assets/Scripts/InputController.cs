using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Ball _ball;
    Tube _cacheTube;

    [SerializeField] LayerMask _tubeLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.State == State.PlayerTurn)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            GameManager.Instance.SetState(State.Wait);
            if (Physics.Raycast(ray, out hit, 100, _tubeLayer.value))
            {

                var tube = hit.transform.gameObject.GetComponent<Tube>();

                if (_ball == null)
                {
                    //click source
                    _cacheTube = tube;
                    if (!tube.HasBall()) return;

                    _ball = tube.Pop();
                    StartCoroutine(tube.Hover(_ball, () => GameManager.Instance.SetState(State.PlayerTurn)));

                }
                else
                {
                    if (tube == _cacheTube)
                    {
                        PutBack();
                        return;
                    }

                    if (tube.IsFull())
                    {
                        PutBack();
                        return;
                    }

                    StartCoroutine(MoveBallToAnotherTube(tube, _ball));
                }
            }
            else
            {

                if (_ball != null)
                {
                    PutBack();
                }

            }

        }

    }


    public void PutBack()
    {
        StartCoroutine(_cacheTube.Take(_ball, () => GameManager.Instance.SetState(State.PlayerTurn)));
        _cacheTube.Push(_ball);
        ClearCache();

    }

    public void ClearCache()
    {
        _ball = null;
        _cacheTube = null;
    }

    public IEnumerator MoveBallToAnotherTube(Tube tube, Ball ball)
    {
        yield return StartCoroutine(ball.MoveTo(tube.HoverPoint.position));
        yield return StartCoroutine(tube.Take(ball));
        tube.Push(ball);
        ClearCache();
        if (GameManager.Instance.CheckWin())
        {
            GameManager.Instance.LoadNextLevel();
        }
        GameManager.Instance.SetState(State.PlayerTurn);
    }

}
