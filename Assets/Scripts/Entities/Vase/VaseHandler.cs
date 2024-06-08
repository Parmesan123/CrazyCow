using System;
using System.Collections.Generic;
using UnityEngine;

public class VaseHandler
{
    private readonly Dictionary<Vase, List<Crate>> _activeVases = new Dictionary<Vase, List<Crate>>();

    public void AddVase(Vase vase)
    {
        _activeVases.Add(vase, new List<Crate>());
    }

    public void AddCrateTo(Vase vase, Crate crate)
    {
        _activeVases[vase].Add(crate);
    }

    public void RemoveCrateFrom(Vase vase, Crate crate)
    {
        List<Crate> crates = _activeVases[vase];
        
        if (!crates.Contains(crate))
            throw new Exception($"Can't remove crate {crate} from vase: Do not contains");

        crates.Remove(crate);
        if (crates.Count == 0)
            Debug.Log("Empty crates on vase. Should remove vase");
    }
}