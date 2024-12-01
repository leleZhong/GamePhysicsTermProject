using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // �� ��ȯ�� ���� �߰�

public class TypingEffect : MonoBehaviour
{
    public Text uiText; // UI �ؽ�Ʈ
    public float typingSpeed = 0.05f; // ���ڰ� �ϳ��� ��Ÿ���� �ӵ�
    public ParticleSystem typingCompleteEffect; // Ÿ���� �Ϸ� �� ������ ��ƼŬ �ý���

    private string fullText = "E N D"; // ��ü �ؽ�Ʈ
    private string currentText = ""; // ������� ǥ�õ� �ؽ�Ʈ

    void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i); // �ؽ�Ʈ�� �� ���ھ� �߰�
            uiText.text = currentText; // UI �ؽ�Ʈ ����
            yield return new WaitForSeconds(typingSpeed); // ���
        }

        // Ÿ������ ���� �� ��ƼŬ �ý��� ����
        if (typingCompleteEffect != null)
        {
            typingCompleteEffect.Play();
        }

        // 8�� �� ���� ������ �̵�
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene("Main"); // "MainScene"�� ���� ���� �� �̸����� ����
    }
}
