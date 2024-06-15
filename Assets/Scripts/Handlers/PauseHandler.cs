using System.Collections.Generic;
using Entities;

public class PauseHandler
{
    private readonly List<IPausable> _pausables;
    
    public bool IsGamePaused { get; private set; }

    public PauseHandler()
    {
        _pausables = new List<IPausable>();
    }

    public void Register(IPausable newPausable)
    {
        _pausables.Add(newPausable);
    }

    public void Unregister(IPausable removePausable)
    {
        _pausables.Remove(removePausable);
    }
    
    public void SetPause(bool isPaused)
    {
        IsGamePaused = isPaused;
        foreach (IPausable pausable in _pausables)
        {
            if (isPaused)
            {
                pausable.Pause();
                continue;
            }
            
            pausable.Unpause();
        }
    }
}