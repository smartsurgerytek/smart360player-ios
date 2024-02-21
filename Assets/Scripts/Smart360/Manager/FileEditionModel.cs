using Sirenix.Serialization;
using System;
using System.Linq;
using UnityEngine;

//[Serializable]
//public class FileEditionModel : 
//{
//    [OdinSerialize] private IReaderWriter<Edition[]> _accessor;
//    [SerializeField] private Edition[] _cache;
//    Edition[] IEditionModel.data
//    {
//        get
//        {
//            if(_cache == null) _cache = _accessor.Read();
//            return _cache.ToArray();
//        }
//        set => _accessor.Read(value);
//    }
//}

//[SerializeField]
//public class FileModuleModel : IModuleModel
//{
//    [OdinSerialize] private IAccessor<Module[]> _accessor;
//    [SerializeField] private Module[] _cache;
//    Module[] IModuleModel.data
//    {
//        get
//        {
//            if (_cache == null) _cache = _accessor.Read();
//            return _cache.ToArray();
//        }
//        set => _accessor.Write(value);
//    }
//}