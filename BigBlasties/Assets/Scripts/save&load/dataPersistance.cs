using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface dataPersistance
{
    void Load(GameData data);

    void Save(ref GameData data);
}
