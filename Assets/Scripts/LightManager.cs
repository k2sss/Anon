using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public Light2D globalLight; // 2D��Դ
    public float fadeSpeed = 1f; // ɫ��仯�ٶ�

    private Coroutine colorChangeRoutine; // ��¼Э�̣�����ֹͣ
    private bool isColorModeActive = false; // ��ɫģʽ�Ƿ񼤻�

    private void Update()
    {

    }

    public void EnableColorMode()
    {
        if (isColorModeActive) return;
        isColorModeActive = true;

        // ������ɫЭ��
        if (colorChangeRoutine != null)
            StopCoroutine(colorChangeRoutine);
        colorChangeRoutine = StartCoroutine(ChangeLightColor());
    }

    public void DisableColorMode()
    {
        isColorModeActive = false;

        // ֹͣ��ɫЭ��
        if (colorChangeRoutine != null)
            StopCoroutine(colorChangeRoutine);

        // ��ɫ�ָ�Ϊ��ɫ
        if (globalLight != null)
            globalLight.color = Color.white;
    }

    private IEnumerator ChangeLightColor()
    {
        float hue = 0f; // ɫ���ʼֵ
        while (isColorModeActive)
        {
            hue += Time.deltaTime * fadeSpeed; // �ۼ�ɫ��ֵ
            if (hue > 1f) hue -= 1f; // ѭ��ɫ��

            // HSL ת��Ϊ RGB ��Ӧ�õ���Դ
            Color newColor = Color.HSVToRGB(hue, 0.2f, 1f);
            globalLight.color = newColor;

            yield return null; // �ȴ�һ֡
        }
    }
}
