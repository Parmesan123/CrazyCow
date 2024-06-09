using System.Collections.Generic;
using InteractableObject;
using Zenject;

public class VaseHandler
{
    public readonly VaseHandlerData Data;
    
    private readonly Dictionary<Vase, List<Box>> _activeVases;

    private int _currentVasesOnField;

    [Inject]
    public VaseHandler(VaseHandlerData vaseData)
    {
        _activeVases = new Dictionary<Vase, List<Box>>();

        Data = vaseData;
    }

    public void AddVase(Vase vase)
    {
        _activeVases.Add(vase, new List<Box>());
    }

    public void AddCrate(Vase vase, Box crate)
    {
        _activeVases[vase].Add(crate);
    }

    public void RemoveCrate(Box crate)
    {
        foreach (Vase vase in _activeVases.Keys)
        {
            List<Box> crates = _activeVases[vase];
            if (!crates.Contains(crate))
                continue;
            
            crates.Remove(crate);
            if (crates.Count != 0) 
                continue;
            
            vase.Destroy();
        }
    }
}