using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI:MonoBehaviour
{
    public Canvas floatingCan;
    public Image hoverHP;
    public Camera cam;
    public List<GameObject> toDisplay;
    public List<Image> hoverHPs;

    public void AddToList(GameObject gm)
    {
        if (!toDisplay.Contains(gm))
        {
            toDisplay.Add(gm);
            Debug.Log("adding");
        }
    }

    public void AddToList(Image i)
    {
        hoverHPs.Add(i);
    }

    public void HoverHealth()
    {
        for(int i = 0; i<toDisplay.Count;i++)
        {
            GameObject gm = toDisplay[i];
            Image hovImage;

            if (hoverHPs.Count<toDisplay.Count)
            {
                hovImage = Instantiate(hoverHP);
                AddToList(hovImage);
                hovImage.transform.SetParent(floatingCan.transform, false);
            }
            else
            {
                hovImage = hoverHPs[i];
            }
            hovImage.transform.position = cam.WorldToScreenPoint(gm.transform.position);

            if (gm.tag == "Friendly")
            {
                hovImage.color = Color.green;
            //    hovImage.GetComponent<Slider>().value = GetComponent<UnitManager>().GetUnit(gm).Health;
            }
            else if (gm.tag == "Enemy")
            {
                hovImage.color = Color.red;
            //    hovImage.GetComponent<Slider>().value = GetComponent<EnemyHandler>().GetEnemy(gm).Health;
            }
            else if (gm.tag == "Encampment")
            {
                hovImage.color = Color.red;
            //    hovImage.GetComponent<Slider>().value = GetComponent<EncampmentHandler>().GetEncampment(gm).Health;
            }
        }
    }


    public void DeleteHovers()
    {
        toDisplay.Clear();
        hoverHPs.Clear();

        if(floatingCan.transform.childCount<0)
        for(int i =0;i<floatingCan.transform.childCount;i++)
        {
            Destroy(floatingCan.transform.GetChild(i));
        }
    }
}
