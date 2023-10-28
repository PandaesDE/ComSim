using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail
{
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
    private LineRenderer lr;
    private LinkedList<Vertex> verticies = new();

    private bool dietaryColor = true;
    private bool statusColor = false;

    public Trail (Creature creature)
    {
        this.creature = creature;
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
        }
    }

    private void renderLine()
    {
        int index = 0;
        lr.positionCount = verticies.Count;
        foreach (Vertex v in verticies)
        {
            lr.SetPosition(index, v.position);
            //lr.SetColors(v.color, v.color);
            index++;
        }
    }

    private void AddVertex(Vertex v)
    {
        if (verticies.Count >= Gamevariables.TRAIL_LENGTH)
        {
            verticies.RemoveLast();
        }
        verticies.AddFirst(v);
    }

    #region Color
    private void setColor()
    {
        if (dietaryColor)
        {
            setDietaryColor();
            return;
        }
        //if (statusColor)
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

    }

    private void setDietaryColor()
    {
        if (creature.dietary.specification == IDietary.Specification.OMNIVORE)
        {
            lr.startColor = new Color(1, 1, 0, .5f);
            lr.endColor = new Color(1, 1, 0, 0);
            return;
        }
        if (creature.dietary.specification == IDietary.Specification.CARNIVORE)
        {
            lr.startColor = new Color(1, 0, 0, .5f);
            lr.endColor = new Color(1, 0, 0, 0);
            return;
        }
        if (creature.dietary.specification == IDietary.Specification.HERBIVORE)
        {
            lr.startColor = new Color(0, 1, 0, .5f);
            lr.endColor = new Color(0, 1, 0, 0);
            return;
        }
    }

    private Color getStatusColor()
    {
        float alpha = .25f;
        if (creature.mission == Creature.Status.WANDERING)
            return new Color(.8f, .8f, .8f, alpha);     //LIGHT GREY
        if (creature.mission == Creature.Status.THIRSTY)
            return new Color(.5f, .75f, 1f, alpha);     //LIGHT BLUE
        if (creature.mission == Creature.Status.DEHYDRATED)
            return new Color(0, .28f, .55f, alpha);     //DARK BLUE
        if (creature.mission == Creature.Status.HUNGRY)
            return new Color(1, .83f, .6f, alpha);      //LIGHT ORANGE
        if (creature.mission == Creature.Status.STARVING)
            return new Color(.78f, .52f, .16f, alpha);  //DARK ORANGE
        if (creature.mission == Creature.Status.FLEEING)
            return new Color(.56f, 1, .63f, alpha);     //LIGHT GREEN
        if (creature.mission == Creature.Status.HUNTING)
            return new Color(.9f, .34f, .34f, alpha);   //RED
        if (creature.mission == Creature.Status.LOOKING_FOR_PARTNER)
            return new Color(1, .6f, .84f, alpha);      //PINK
        if (creature.mission == Creature.Status.SLEEPING)
            return new Color(.4f, .4f, .4f, alpha);     //DARK GREY

        return Color.black; //ERROR COLOR
    }
    #endregion
}
