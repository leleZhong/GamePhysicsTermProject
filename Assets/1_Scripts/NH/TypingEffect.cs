using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 전환을 위해 추가

public class TypingEffect : MonoBehaviour
{
    public Text uiText; // UI 텍스트
    public float typingSpeed = 0.05f; // 글자가 하나씩 나타나는 속도
    public ParticleSystem typingCompleteEffect; // 타이핑 완료 후 실행할 파티클 시스템

    private string fullText = "E N D"; // 전체 텍스트
    private string currentText = ""; // 현재까지 표시된 텍스트

    void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i); // 텍스트를 한 글자씩 추가
            uiText.text = currentText; // UI 텍스트 갱신
            yield return new WaitForSeconds(typingSpeed); // 대기
        }

        // 타이핑이 끝난 후 파티클 시스템 실행
        if (typingCompleteEffect != null)
        {
            typingCompleteEffect.Play();
        }

        // 8초 후 메인 씬으로 이동
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene("Main"); // "MainScene"을 실제 메인 씬 이름으로 변경
    }
}
