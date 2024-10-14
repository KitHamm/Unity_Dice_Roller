using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineTargetGroup TargetGroup;
    public GameObject VirtualCamera1;
    public GameObject VirtualCamera2;
    public GameObject VirtualCamera3;
    public int cameraSelect;
    // Start is called before the first frame update
    void Start()
    {
        ClearGroup();
        cameraSelect = 1;
    }

    // Update is called once per frame
    void Update()
    {
        VirtualCamera1.SetActive(cameraSelect == 1);
        VirtualCamera2.SetActive(cameraSelect == 2);
        VirtualCamera3.SetActive(cameraSelect == 3);
    }

    public void ClearGroup()
    {
        for (int i = 0; i < 8; i++)
        {
            if (TargetGroup.m_Targets.Length > 0)
            {
                Transform member = TargetGroup.m_Targets[0].target;
                TargetGroup.RemoveMember(member);
            }
        }
    }

    public void AddTargetToTargetGroup(Transform target)
    {
        TargetGroup.AddMember(target, 1, 1.2f);
    }

}
