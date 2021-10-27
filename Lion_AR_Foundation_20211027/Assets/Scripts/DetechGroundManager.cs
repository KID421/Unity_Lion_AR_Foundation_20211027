using UnityEngine;
using UnityEngine.XR.ARFoundation;  // �ޥ� Foundation API
using UnityEngine.XR.ARSubsystems;  // �ޥ� Subsystems API
using System.Collections.Generic;   // �ޥ� ���X API�A�]�t�M�� List

namespace KID.ARFoundation
{
    /// <summary>
    /// �˴��a�O�I���޲z��
    /// �I���a�O��B�z���ʦ欰
    /// �ͦ�����P������m
    /// </summary>
    public class DetechGroundManager : MonoBehaviour
    {
        [Header("�I����n�ͦ�������")]
        public GameObject goSpawn;
        [Header("AR �g�u�޲z��"), Tooltip("���޲z���n��b��v�� Origin ����W")]
        public ARRaycastManager arRaycastManager;
        [Header("�ͦ�����n���V����v������")]
        public Transform traCamera;
        [Header("�ͦ����󭱦V�t��"), Range(0, 100)]
        public float speedLookAt = 3.5f;

        private Transform traSpawn;
        private List<ARRaycastHit> hits = new List<ARRaycastHit>();     // �M����� = �s�W ���� �M�檫��

        /// <summary>
        /// �ƹ�����PĲ��
        /// </summary>
        private bool inputMouseLeft { get => Input.GetKeyDown(KeyCode.Mouse0); }

        private void Update()
        {
            ClickAndDetechGround();
            SpawnLoookAtCamera();
        }

        /// <summary>
        /// �I���P�˴��a�O
        /// 1. �����O�_�����w����
        /// 2. �N�I���y�Ь���
        /// 3. �g�u�˴�
        /// 4. ����
        /// </summary>
        private void ClickAndDetechGround()
        {
            if (inputMouseLeft)                                                                         // �p�G ���U���w����
            {
                Vector2 positionMouse = Input.mousePosition;                                            // ���o �I���y��

                // Ray ray = Camera.main.ScreenPointToRay(positionMouse);                               // �N �I���y�� �ର �g�u

                if (arRaycastManager.Raycast(positionMouse, hits, TrackableType.PlaneWithinPolygon))    // �p�G �g�u ������w���a�O����
                {
                    Vector3 positionHit = hits[0].pose.position;                                        // ���o�I�쪺�y��

                    if (traSpawn == null)
                    {
                        traSpawn = Instantiate(goSpawn, positionHit, Quaternion.identity).transform;    // �N����ͦ��b�I�쪺�y�ФW
                        traSpawn.localScale = Vector3.one * 0.5f;                                       // �վ�ؤo
                    }
                    else
                    {
                        traSpawn.position = positionHit;                                                // �_�h ���ͦ��L���� �N��s�y��
                    }
                }
            }
        }

        /// <summary>
        /// �ͦ����󭱦V��v��
        /// </summary>
        private void SpawnLoookAtCamera()
        {
            if (traSpawn == null) return;                                                                   // �p�G �ͦ����󬰪ŭ� ���X
            Quaternion angle = Quaternion.LookRotation(traCamera.position - traSpawn.position);             // ���o�V�q
            traSpawn.rotation = Quaternion.Lerp(traSpawn.rotation, angle, Time.deltaTime * speedLookAt);    // ���״���
            Vector3 angleOrigial = traSpawn.localEulerAngles;                                               // ���o����
            angleOrigial.x = 0;                                                                             // �ᵲ X
            angleOrigial.z = 0;                                                                             // �ᵲ Z
            traSpawn.localEulerAngles = angleOrigial;                                                       // ��s����
        }
    }
}
