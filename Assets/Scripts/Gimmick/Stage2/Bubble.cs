using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] float popOffsetTime;
    [SerializeField] float resetOffsetTime;

    private WaitForSeconds resetSeconds;

    private void Start()
    {
        resetSeconds = new WaitForSeconds(resetOffsetTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(BubblePop());
    }

    private IEnumerator BubblePop()
    {
        // ���⿡ ������ �ִϸ��̼� ����

        yield return resetSeconds;

        // ���⿡ �ٽ� �����Ǵ� �ִϸ��̼� ����
    }
}
