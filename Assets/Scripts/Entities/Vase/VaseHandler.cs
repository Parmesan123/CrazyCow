using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class VaseHandler
{
    public readonly VaseHandlerData Data;
    
    private readonly Dictionary<Vase, List<Crate>> _activeVases;

    [Inject]
    public VaseHandler(VaseHandlerData vaseData)
    {
        _activeVases = new Dictionary<Vase, List<Crate>>();

        Data = vaseData;
    }

    public void AddVase(Vase vase)
    {
        _activeVases.Add(vase, new List<Crate>());
    }

    public void AddCrate(Vase vase, Crate crate)
    {
        _activeVases[vase].Add(crate);
    }

    public void RemoveCrate(Crate crate)
    {
        foreach (Vase vase in _activeVases.Keys)
        {
            List<Crate> crates = _activeVases[vase];
            
            if (!crates.Contains(crate))
                continue;
            
            crates.Remove(crate);
            if (crates.Count == 0)
                vase.Disable();
        }
    }
}