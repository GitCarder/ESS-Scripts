using UnityEngine;
using System.Collections;

public class ArrowFlashSign : MonoBehaviour {

    public Color baseColor;
    public Color[] colors;   
    public float speed = 1.0f;
    public float pause = 1.0f;

    private Transform[] arrows;
    private Color[] arrow_colors;
    private int arrow_count;
    private int color_count;
    private float t1;
    private float t2;
    private int iter = 0;
	
	void Start () {
        arrow_count = transform.childCount;
        arrows = new Transform[arrow_count];
        color_count = 2 * arrow_count;
        arrow_colors = new Color[arrow_count * 2];

        for (int i = 0; i < color_count; i++)
        {
            if (i < arrow_count)
            {
                arrows[i] = transform.GetChild(i);                
                arrow_colors[i] = baseColor;
                arrows[i].gameObject.GetComponent<Renderer>().material.color = arrow_colors[i];
            } else
            {
                arrow_colors[i] = colors[i - arrow_count];                
            }
        }

        t1 = 0;
        t2 = 0;
    }
		
	void Update () {
        updateColors();

        if ((Time.fixedTime - t2) > pause)
        {
            if (Time.fixedTime - t1 > speed)
            {
                Color tmp = arrow_colors[0];
                for (int i = 0; i < color_count - 1; i++)
                {
                    arrow_colors[i] = arrow_colors[(i + 1)];
                }
                arrow_colors[color_count - 1] = tmp;

                for (int i = 0; i < arrow_count; i++)
                {                    
                    arrows[i].gameObject.GetComponent<Renderer>().material.color = arrow_colors[i];
                }
                t1 = Time.fixedTime;
                iter++;

                if (iter >= color_count)
                {
                    t2 = Time.fixedTime;
                    iter = 0;
                }
            }
        }
    }   

    void updateColors()
    {
        int offset = iter;        

        for (int i = 0; i < color_count; i++)
        {
            offset = i - iter;
            if (offset < 0)
                offset = color_count - Mathf.Abs(i - iter);

            if (i < arrow_count)
            {                
                arrow_colors[offset] = baseColor;             
            }
            else
            {
                arrow_colors[offset] = colors[i - arrow_count];
            }
        }
    }
}
