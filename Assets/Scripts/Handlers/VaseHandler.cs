using System.Collections.Generic;
using ModestTree;
using Zenject;

namespace InteractableObject
{
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

        public void AddCrate(Vase vase, Box box)
        {
            _activeVases[vase].Add(box);
        }

        public void RemoveBox(Box box)
        {
            foreach (Vase vase in _activeVases.Keys)
            {
                List<Box> boxes = _activeVases[vase];
                if (!boxes.Contains(box))
                    continue;
            
                boxes.Remove(box);
                if (!boxes.IsEmpty()) 
                    continue;
            
                vase.Destroy();
            }
        }
    }   
}