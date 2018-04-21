using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrefabLocator : Singleton<PrefabContext>
{
    [SerializeField] private readonly PrefabContext _context;

    public PrefabLocator()
    {
        _context = Resources.Load<PrefabContext>(ResourcePaths.PrefabContext);        
    }

    public GameObject Locate(Prefab prefabId)
    {
        return _context.mapping.First(item => item.key == prefabId).prefab;
    }
}