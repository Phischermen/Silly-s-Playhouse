using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSaveContainerTutorial : UpdateSaveContainer
{
        public override void Add()
        {
            if (!SaveContainer.tutorials.Exists(gameObject.Equals))
            {
                SaveContainer.tutorials.Add(gameObject);
            }
        }
        public override void Remove()
        {
            if (SaveContainer.tutorials.Exists(gameObject.Equals))
            {
                SaveContainer.tutorials.Remove(gameObject);
            }
        }
}
