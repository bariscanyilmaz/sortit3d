using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    //CONSTANTS
    int capacity = 4;
    float oneSecond = 1f;



    Stack<Ball> _balls;

    [SerializeField] float _speed = 10f;

    [SerializeField]
    Ball[] initialBallPrefabs;

    [SerializeField]
    Transform _pivotPoint, _hoverPoint;

    public Transform HoverPoint => _hoverPoint;

    void Start()
    {
        InitBalls();
    }

    void InitBalls()
    {
        _balls = new Stack<Ball>();
        foreach (var item in initialBallPrefabs)
        {
            Vector3 pos = new Vector3(transform.position.x, _balls.Count, transform.position.z);
            var obj = Instantiate<Ball>(item, pos, Quaternion.identity, _pivotPoint);
            _balls.Push(obj);
        }
    }

    public bool IsFull()
    {
        return _balls.Count >= capacity;
    }

    public bool HasBall()
    {
        return _balls.Count > 0;
    }

    public Ball Pop()
    {
        return _balls.Pop();
    }

    public IEnumerator Take(Ball ball, Action callback = null)
    {
        ball.transform.SetParent(_pivotPoint);
        Vector3 endPos = new Vector3(transform.position.x, _balls.Count, transform.position.z);
        Vector3 startPos = ball.transform.position;


        float elapsedTime = 0;
        while (elapsedTime < oneSecond)
        {
            ball.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime);
            elapsedTime += Time.deltaTime*_speed;
            yield return null;
        }

        if (callback != null)
        {
            callback();
        }


    }

    public IEnumerator Hover(Ball ball, Action callback = null)
    {
        //hover
        Vector3 endPos = _hoverPoint.position;
        Vector3 startPos = ball.transform.position;
        ball.transform.SetParent(null);

        float elapsedTime = 0;
        while (elapsedTime < oneSecond)
        {
            ball.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / oneSecond);
            elapsedTime += Time.deltaTime*_speed;
            yield return null;
        }

        ball.transform.position = endPos;
        if (callback != null)
        {
            callback();
        }

    }

    public void Push(Ball ball)
    {
        _balls.Push(ball);
    }

    public bool IsRightOrder()
    {
        if (_balls.Count == 0) return true;
        if (!IsFull()) return false;

        var balls = _balls.ToArray();
        int colorCode = 0;
        for (int i = 0; i < _balls.Count; i++)
        {
            colorCode ^= balls[i].GetColorCode();
        }

        return colorCode == 0;

    }



}
