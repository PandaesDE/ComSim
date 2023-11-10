using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail
{
    public enum ColorScheme
    {
        DEFAULT,
        DIETARY
    }

    private class Vertex
    {
        public Vector3 position;
        public Color color;
        public Vertex(Vector3 position, Color color)
        {
            this.position = position;
            this.color = color;
        }
    }

    private Creature creature;
    private IDietary dietary;
    private LineRenderer lr;
    private LinkedList<Vertex> verticies = new();

    public Trail (Creature creature, IDietary dietary)
    {
        this.creature = creature;
        this.dietary = dietary;
        lr = creature.GetComponent<LineRenderer>();

        lr.startWidth = 1;
        lr.endWidth = .1f;
        setColor();
    }

    public void FixedUpdate()
    {
        if (Gamevariables.SHOW_TRAIL)
        {
            float x = creature.transform.position.x;
            float y = creature.transform.position.y;
            float z = 0;
            Vector3 position = new Vector3(x, y, z);
            AddVertex(new Vertex(position, getStatusColor()));
            renderLine();
        } else if (verticies.Count > 0)
        {
             verticies.Clear();
        }
    }

    private void renderLine()
    {
        int index = 0;
        lr.positionCount = verticies.Count;
        foreach (Vertex v in verticies)
        {
            lr.SetPosition(index, v.position);
            index++;
        }
    }

    private void AddVertex(Vertex v)
    {
        if (verticies.Count >= Gamevariables.TRAIL_LENGTH)
        {
            int difference = verticies.Count - Gamevariables.TRAIL_LENGTH;
            for (int i = 0; i < difference; i++)
            {
                verticies.RemoveLast();
            }
        }
        verticies.AddFirst(v);
    }

    #region Color
    public void setColor()
    {
        if (Gamevariables.TRAIL_COLOR == ColorScheme.DIETARY)
        {
            setDietaryColor();
            return;
        }
        //if (Gamevariables.TRAIL_COLOR == ColorScheme...)
        //{
        //    setStatusColor();
        //}

        setDefaultColor();
    }

    private void setDefaultColor()
    {
        lr.startColor = new Color(1, 1, 1, .5f);
        lr.endColor = new Color(1, 1, 1, 0);
    }

    private void setStatusColor()
    {
        /*placeholder for gradient
         * Every Vertex has a StatusColor.
         * Unity Gradients can only hold up to 8 Color Keys which is not sufficient.
         *      -> TODO
         */
    }

    private void setDietaryColor()
    {
        if (dietary.specification == IDietary.Specification.OMNIVORE)
        {
            lr.startColor = new Color(1, 1, 0, .5f);
            lr.endColor = new Color(1, 1, 0, 0);
            return;
        }
        if (dietary.specification == IDietary.Specification.CARNIVORE)
        {
            lr.startColor = new Color(1, 0, 0, .5f);
            lr.endColor = new Color(1, 0, 0, 0);
            return;
        }
        if (dietary.specification == IDietary.Specification.HERBIVORE)
        {
            lr.startColor = new Color(0, 1, 0, .5f);
            lr.endColor = new Color(0, 1, 0, 0);
            return;
        }
    }

    private Color getStatusColor()
    {
        float alpha = .25f;
        if (creature.statusManager.status == StatusManager.Status.WANDERING)
            return new Color(.8f, .8f, .8f, alpha);     //LIGHT GREY
        if (creature.statusManager.status == StatusManager.Status.THIRSTY)
            return new Color(.5f, .75f, 1f, alpha);     //LIGHT BLUE
        if (creature.statusManager.status == StatusManager.Status.DEHYDRATED)
            return new Color(0, .28f, .55f, alpha);     //DARK BLUE
        if (creature.statusManager.status == StatusManager.Status.HUNGRY)
            return new Color(1, .83f, .6f, alpha);      //LIGHT ORANGE
        if (creature.statusManager.status == StatusManager.Status.STARVING)
            return new Color(.78f, .52f, .16f, alpha);  //DARK ORANGE
        if (creature.statusManager.status == StatusManager.Status.FLEEING)
            return new Color(.56f, 1, .63f, alpha);     //LIGHT GREEN
        if (creature.statusManager.status == StatusManager.Status.HUNTING)
            return new Color(.9f, .34f, .34f, alpha);   //RED
        if (creature.statusManager.status == StatusManager.Status.LOOKING_FOR_PARTNER)
            return new Color(1, .6f, .84f, alpha);      //PINK
        if (creature.statusManager.status == StatusManager.Status.SLEEPING)
            return new Color(.4f, .4f, .4f, alpha);     //DARK GREY

        return Color.black;                             //ERROR COLOR
    }
    #endregion
}
