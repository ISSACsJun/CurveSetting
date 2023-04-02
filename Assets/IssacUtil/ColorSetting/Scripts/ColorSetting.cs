using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IssacUtil.Setting.ForColor
{
    [CreateAssetMenu(fileName = "ColorSetting", menuName = "IssacUtil/Settings/Create Color Setting")]
    public class ColorSetting : ScriptableObject
    {
        // --------------------------------------------------
        // Singleton
        // -------------------------------------------------- 
        // ----- Constructor
        private ColorSetting() { }

        // ----- Instance
        private static ColorSetting _instance = null;

        // ----- Variables
        //If the file location needs to be changed, please modify the corresponding variable.
        private const string SETTING_PATH = "Setting/ColorSetting";

        // ----- Instance Getter
        public static ColorSetting Instance
        {
            get
            {
                if (null != _instance)
                    return _instance;

                _instance = Resources.Load<ColorSetting>(SETTING_PATH);

                if (null == _instance)
                    Debug.LogError("[ColorSetting] Failed To Load Color Setting File. Please Check If The File Exists.");

                _instance._InitColorInfoSet();

                return _instance;
            }
        }

        // --------------------------------------------------
        // Color Data Class
        // --------------------------------------------------
        [System.Serializable]
        private class ColorData
        {
            public EColorType ColorType      = EColorType.unknown;
            public Color      TargetColor    = new Color();
            public Material   TargetColorMat = null;
        }

        // --------------------------------------------------
        // Components
        // --------------------------------------------------
        [SerializeField] private List<ColorData> _dataList;

        // --------------------------------------------------
        // Variables
        // --------------------------------------------------
        private Dictionary<EColorType, ColorData> _infoSet = null;

        // --------------------------------------------------
        // Function - Nomal
        // --------------------------------------------------
        /// <summary>
        /// A Function That Can Receive Color Information based on the Color Type.
        /// Even if a Color Type exists, it may not work if the Color is not registered in the ColorSetting.
        /// The Color Setting File exists in the Resources folder.
        /// </summary>
        public Color GetColor(EColorType colorType)
        {
            if (_infoSet.TryGetValue(colorType, out var info))
                return info.TargetColor;

            if (null == _dataList || 0 == _dataList.Count)
            {
                Debug.LogError("[ColorSetting.GetColor] There is no color information.");
                return default;
            }


            Debug.LogWarning($"[ColorSettings.GetColor] There is no color information for the corresponding type. type: {colorType}");
            return _dataList[0].TargetColor;
        }

        /// <summary>
        /// A function that can receive a registered color material depending on the color type.
        /// Even if a Color Type exists, it may not work if the Color is not registered in the ColorSetting.
        /// The Color Setting File exists in the Resources folder.
        /// </summary>
        public Material GetColorMatarial(EColorType colorType)
        {
            if (_infoSet.TryGetValue(colorType, out var info))
                return info.TargetColorMat;

            if (null == _dataList || 0 == _dataList.Count)
            {
                Debug.LogError("[ColorSettings.GetColorMaterial] There is no color information.");
                return default;
            }

            Debug.LogWarning($"[ColorSettings.GetColorMaterial] There is no color information for the corresponding type. type: {colorType}");
            return _dataList[0].TargetColorMat;
        }

        /// <summary>
        /// This is a function that initializes the Color Info.
        /// </summary>
        private void _InitColorInfoSet()
        {
            _infoSet?.Clear();
            _infoSet = new Dictionary<EColorType, ColorData>();

            for (int i = 0, size = _dataList?.Count ?? 0; i < size; ++i)
            {
                var info = _dataList[i];
                if (null == info)
                    continue;

                _infoSet[info.ColorType] = info;
            }
        }
    }
}
