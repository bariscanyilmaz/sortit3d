using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Colors { R = 1, G = 3, B = 5, Y = 7 };

public class Ball : MonoBehaviour
{
    float oneSecond = 1f;
    float _speed = 10f;

    [SerializeField] Colors _color;


    public Colors Color => _color;

    public int GetColorCode() => (int)this._color;


    public IEnumerator MoveTo(Vector3 targetPosition)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = targetPosition;

        float elapsedTime = 0;

        while (elapsedTime < oneSecond)
        {

            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime);
            elapsedTime += Time.deltaTime * _speed;
            yield return null;
        }
    }
}
