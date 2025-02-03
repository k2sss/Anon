using UnityEngine;

public class Hammer : MonoBehaviour
{
    // ����UIԪ�ص� RectTransform
    private RectTransform rectTransform;
    private Animator animator;
    public GameObject bonk;

    // ��ʼ��ʱ��ȡ RectTransform ���
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Cursor.visible = false;
        animator = GetComponent<Animator>();
    }

    // ÿ֡���� UI Ԫ�ص�λ�ã�ʹ��������
    private void Update()
    {
        FollowCursor();
        if (Input.GetMouseButtonDown(0))
        {
           
            animator.Play("Click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Mole m = hit.collider.GetComponent<Mole>();
                if ( m!= null)
                {
                   
                    SoundManager.Instance.PlaySound("Sounds/bonk");
                    Bonk();
                    m.Click();
                }
            }
        }
    }
    public void Bonk()
    {
        GameObject b = Instantiate(bonk);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;
        b.transform.position = worldPos;
        TimeDelay.Instance.Delay(0.3f,()=>Destroy(b));
    }
    private void FollowCursor()
    {
        
        rectTransform.position = Input.mousePosition;
    }
}
