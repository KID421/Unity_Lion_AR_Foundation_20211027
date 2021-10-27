using UnityEngine;
using UnityEngine.XR.ARFoundation;  // 引用 Foundation API
using UnityEngine.XR.ARSubsystems;  // 引用 Subsystems API
using System.Collections.Generic;   // 引用 集合 API，包含清單 List

namespace KID.ARFoundation
{
    /// <summary>
    /// 檢測地板點擊管理器
    /// 點擊地板後處理互動行為
    /// 生成物件與控制物件位置
    /// </summary>
    public class DetechGroundManager : MonoBehaviour
    {
        [Header("點擊後要生成的物件")]
        public GameObject goSpawn;
        [Header("AR 射線管理器"), Tooltip("此管理器要放在攝影機 Origin 物件上")]
        public ARRaycastManager arRaycastManager;
        [Header("生成物件要面向的攝影機物件")]
        public Transform traCamera;
        [Header("生成物件面向速度"), Range(0, 100)]
        public float speedLookAt = 3.5f;

        private Transform traSpawn;
        private List<ARRaycastHit> hits = new List<ARRaycastHit>();     // 清單欄位 = 新增 實體 清單物件

        /// <summary>
        /// 滑鼠左鍵與觸控
        /// </summary>
        private bool inputMouseLeft { get => Input.GetKeyDown(KeyCode.Mouse0); }

        private void Update()
        {
            ClickAndDetechGround();
            SpawnLoookAtCamera();
        }

        /// <summary>
        /// 點擊與檢測地板
        /// 1. 偵測是否按指定按鍵
        /// 2. 將點擊座標紀錄
        /// 3. 射線檢測
        /// 4. 互動
        /// </summary>
        private void ClickAndDetechGround()
        {
            if (inputMouseLeft)                                                                         // 如果 按下指定按鍵
            {
                Vector2 positionMouse = Input.mousePosition;                                            // 取得 點擊座標

                // Ray ray = Camera.main.ScreenPointToRay(positionMouse);                               // 將 點擊座標 轉為 射線

                if (arRaycastManager.Raycast(positionMouse, hits, TrackableType.PlaneWithinPolygon))    // 如果 射線 打到指定的地板物件
                {
                    Vector3 positionHit = hits[0].pose.position;                                        // 取得點到的座標

                    if (traSpawn == null)
                    {
                        traSpawn = Instantiate(goSpawn, positionHit, Quaternion.identity).transform;    // 將物件生成在點到的座標上
                        traSpawn.localScale = Vector3.one * 0.5f;                                       // 調整尺寸
                    }
                    else
                    {
                        traSpawn.position = positionHit;                                                // 否則 有生成過物件 就更新座標
                    }
                }
            }
        }

        /// <summary>
        /// 生成物件面向攝影機
        /// </summary>
        private void SpawnLoookAtCamera()
        {
            if (traSpawn == null) return;                                                                   // 如果 生成物件為空值 跳出
            Quaternion angle = Quaternion.LookRotation(traCamera.position - traSpawn.position);             // 取得向量
            traSpawn.rotation = Quaternion.Lerp(traSpawn.rotation, angle, Time.deltaTime * speedLookAt);    // 角度插值
            Vector3 angleOrigial = traSpawn.localEulerAngles;                                               // 取得角度
            angleOrigial.x = 0;                                                                             // 凍結 X
            angleOrigial.z = 0;                                                                             // 凍結 Z
            traSpawn.localEulerAngles = angleOrigial;                                                       // 更新角度
        }
    }
}
