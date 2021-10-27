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

        private Transform traSpawn;
        private List<ARRaycastHit> hits = new List<ARRaycastHit>();     // 清單欄位 = 新增 實體 清單物件

        /// <summary>
        /// 滑鼠左鍵與觸控
        /// </summary>
        private bool inputMouseLeft { get => Input.GetKeyDown(KeyCode.Mouse0); }

        private void Update()
        {
            ClickAndDetechGround();
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
            if (inputMouseLeft)
            {
                Vector2 positionMouse = Input.mousePosition;

                print("點擊座標：" + positionMouse);
            }
        }
    }
}
