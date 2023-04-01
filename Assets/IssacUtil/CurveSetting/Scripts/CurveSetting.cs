using System.Collections.Generic;
using UnityEngine;

namespace Issac.Setting
{
    [CreateAssetMenu(fileName = "CurveSetting", menuName = "IssacUtil/Settings/Create Color Setting")]
    public class CurveSetting : ScriptableObject
    {
        // --------------------------------------------------
        // Singleton
        // --------------------------------------------------
        // ----- Constructor
        private CurveSetting() { }

        // ----- Instance
        private static CurveSetting _instance = null;

        // ----- Variables
        //If the file location needs to be changed, please modify the corresponding variable.
        private const string SETTING_PATH = "Setting/CurveSetting";

        // ----- Instance Getter
        public static CurveSetting Instance
        {
            get
            {
                if (null != _instance)
                    return _instance;

                _instance = Resources.Load<CurveSetting>(SETTING_PATH);  

                if (null == _instance)
                    Debug.LogError("[CurveSetting] Failed To Load Curve Setting File. Please Check If The File Exists.");

                _instance.InitCurveInfoSet();

                return _instance;
            }
        }

        // --------------------------------------------------
        // Curve Info Class
        // --------------------------------------------------
        [System.Serializable]
        public class CurveInfo
        {
            public ECurveType     CurveType;
            public AnimationCurve Curve;
        }

        // --------------------------------------------------
        // Componenst
        // --------------------------------------------------
        [SerializeField] private List<CurveInfo> _curveInfoList;

        // --------------------------------------------------
        // Variables
        // --------------------------------------------------
        private Dictionary<ECurveType, CurveInfo> _infoSet = null;

        // --------------------------------------------------
        // Function - Nomal
        // --------------------------------------------------
        /// <summary>
        /// A Function That Can Receive Curve Information based on the Curve Type.
        /// Even if a Curve Type exists, it may not work if the Curve is not registered in the CurveSetting.
        /// The Curve Setting File exists in the Resources folder.
        /// </summary>
        public AnimationCurve GetCurve(ECurveType type)
        {
            if (_infoSet.TryGetValue(type, out var info))
                return info.Curve;

            if (null == _curveInfoList || 0 == _curveInfoList.Count)
            {
                Debug.LogError("[CurveSettings.GetCurve] There is no curve information.");
                return default;
            }

            Debug.LogWarning($"[CurveSettings.GetCurve] There is no curve information for the corresponding type. type: {type}");
            return _curveInfoList[0].Curve;
        }

        /// <summary>
        /// This is a function that initializes the Curve Info.
        /// </summary>
        private void InitCurveInfoSet()
        {
            _infoSet?.Clear();
            _infoSet = new Dictionary<ECurveType, CurveInfo>();

            for (int i = 0, size = _curveInfoList?.Count ?? 0; i < size; ++i)
            {
                var info = _curveInfoList[i];
                if (null == info)
                    continue;

                _infoSet[info.CurveType] = info;
            }
        }
    }
}