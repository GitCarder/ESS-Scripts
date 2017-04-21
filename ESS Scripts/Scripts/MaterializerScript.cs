using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MaterializerScript : MonoBehaviour {

	public Material defaultMaterial;
	public Material concrete;
	public Material steel;
	public Material rail;
    public Material ladder;
    public Material floor;
    public Material liner;
    private Transform hotCell;
    private bool clean = true;

    void Start()
    {
        hotCell = GameObject.Find("Hot Cell").transform;
        //Clear();

        // Make it unnoticably bigger to get rid of overlapping polygons.
		GameObject.Find("Block:_1").transform.localScale = new Vector3(1.0005f, 1.0005f, 1.0005f);

		Vector3 p = GameObject.Find ("Block:_100").transform.localPosition;
		p.y = -0.01f;
		GameObject.Find ("Block:_100").transform.localPosition = p;

        if (clean) {
            // Hide reinforcement
            if (GameObject.Find("Block:_252") != null)
                DestroyImmediate(GameObject.Find("Block:_252"));

            if (GameObject.Find("Block:_316 1") != null)
                DestroyImmediate(GameObject.Find("Block:_316 1"));

            if (GameObject.Find("Block:_317") != null)
                DestroyImmediate(GameObject.Find("Block:_317"));
			
			if (GameObject.Find("Block:_488") != null)
				DestroyImmediate(GameObject.Find("Block:_488"));
			
			if (GameObject.Find("Block:_53") != null)
				DestroyImmediate(GameObject.Find("Block:_53"));
        }

		Apply (concrete, "Block:_1");
		
		Apply(steel, "Block:_53");

		Apply(steel, "Block:_96");
		Apply(steel, "Block:_97");
		Apply(steel, "Block:_98");
		Apply(steel, "Block:_99");
		Apply(steel, "Block:_100");
		Apply(steel, "Block:_102");
		Apply(steel, "Block:_104");
		Apply(steel, "Block:_110");
		Apply(steel, "Block:_232");
		Apply(steel, "Block:_233");
		Apply(steel, "Block:_234");
		Apply(steel, "Block:_222");
		Apply(steel, "Block:_225");
		Apply(steel, "Block:_227");
		Apply(steel, "Block:_228");
		Apply(steel, "Block:_229");
		Apply(steel, "Block:_242");
		Apply(steel, "Block:_223");
		Apply(steel, "Block:_113");
		Apply(steel, "Block:_114");
		Apply(steel, "Block:_235");
		Apply(steel, "Block:_236");
		Apply(steel, "Block:_237");
		Apply(steel, "Block:_238");
		Apply(steel, "Block:_239");

        Apply(ladder, "Block:_2");
        
		Apply(floor, "Block:_61");
        Apply(floor, "Block:_62");
        Apply(floor, "Block:_63");

        Apply(liner, "Block:_101");
        Apply(liner, "Block:_103");
        Apply(liner, "Block:_105");
		Apply(liner, "Block:_106");
		Apply(liner, "Block:_289");
		Apply(liner, "Block:_290");
		Apply(liner, "Block:_245");
		Apply(liner, "Block:_249");
		Apply(liner, "Block:_230");
		Apply(liner, "Block:_246");
		Apply(liner, "Block:_247");
		Apply(liner, "Block:_248");
		Apply(liner, "Block:_250");
		Apply(liner, "Block:_251");
		Apply(liner, "Block:_34");
		
		Apply(concrete, "Block:_291");
		Apply(concrete, "Block:_109");

		Apply(rail, "Block:_3");
		Apply(rail, "Block:_9");
    }

    void Clear() {
        Transform[] children = hotCell.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children) {
            Renderer[] renderers = child.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) {
                renderer.material = defaultMaterial;
            }
        }
    }

    void Apply(Material material, string name) {
        Transform[] children = hotCell.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children) {
            if (child.name.Equals(name) || child.name.StartsWith(name + " ")) {
                Renderer[] renderers = child.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers) {
                    renderer.material = material;
                }
            }
        }
    }
	
}
