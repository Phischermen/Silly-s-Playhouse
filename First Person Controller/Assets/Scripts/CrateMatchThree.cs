using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateMatchThree : MatchThreeObject
{
    [ContextMenu("Apply Color")]
    public override void apply_color()
    {
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        if(mesh_renderer != null)
        {
            Material[] new_materials = mesh_renderer.sharedMaterials;
            switch (match3_group_id)
            {
                case match3_group_id_names.blue:
                    new_materials[0] = atlas.baseMaterials[0];
                    new_materials[1] = atlas.colorMaterials[0];
                    break;
                case match3_group_id_names.red:
                    new_materials[0] = atlas.baseMaterials[1];
                    new_materials[1] = atlas.colorMaterials[1];
                    break;
                case match3_group_id_names.green:
                    new_materials[0] = atlas.baseMaterials[0];
                    new_materials[1] = atlas.colorMaterials[2];
                    break;
                case match3_group_id_names.yellow:
                    new_materials[0] = atlas.baseMaterials[1];
                    new_materials[1] = atlas.colorMaterials[3];
                    break;
            }
            mesh_renderer.sharedMaterials = new_materials;
        }
    }
}
