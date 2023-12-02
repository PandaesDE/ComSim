/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule N�rnberg
 *  
 *  Description:
 *      - makes a vizualization of the creature trail
 *  
 *  References:
 *      Scene:
 *          - Indirectly (Component of Creature.cs) for simulation scene(s)
 *      Script:
 *          - One instance per creature
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using System.Collections.Generic;
using UnityEngine;

public class Trail
{
    public enum ColorScheme
    {
        @default,
        dietary
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

    private Creature _creature;
    private IDietary _dietary;
    private LineRenderer _lr;
    private LinkedList<Vertex> _verticies = new();

    public Trail (Creature creature, IDietary dietary)
    {
        this._creature = creature;
        this._dietary = dietary;
        _lr = creature.GetComponent<LineRenderer>();

        _lr.startWidth = 1;
        _lr.endWidth = .1f;
        SetColor();
    }

    public void FixedUpdate()
    {
        if (Gamevariables.ShowTrail)
        {
            AddVertex(new Vertex(_creature.transform.position, GetStatusColor()));
            RenderLine();
        } else if (_verticies.Count > 0)
        {
             _verticies.Clear();
        }
    }

    private void RenderLine()
    {
        int index = 0;
        _lr.positionCount = _verticies.Count;
        foreach (Vertex v in _verticies)
        {
            _lr.SetPosition(index, v.position);
            index++;
        }
    }

    private void AddVertex(Vertex v)
    {
        if (_verticies.Count >= Gamevariables.TrailLength)
        {
            int difference = _verticies.Count - Gamevariables.TrailLength;
            for (int i = 0; i < difference; i++)
            {
                _verticies.RemoveLast();
            }
        }
        _verticies.AddFirst(v);
    }

    #region Color
    public void SetColor()
    {
        if (Gamevariables.TrailColor == ColorScheme.dietary)
        {
            SetDietaryColor();
            return;
        }
        //if (Gamevariables.TRAIL_COLOR == ColorScheme...)
        //{
        //    setStatusColor();
        //}

        SetDefaultColor();
    }

    private void SetDefaultColor()
    {
        _lr.startColor = new Color(1, 1, 1, .5f);
        _lr.endColor = new Color(1, 1, 1, 0);
    }

    private void SetStatusColor()
    {
        /*placeholder for gradient
         * Every Vertex has a StatusColor.
         * Unity Gradients can only hold up to 8 Color Keys which is not sufficient.
         *      -> TODO
         */
    }

    private void SetDietaryColor()
    {
        if (_dietary.specification == IDietary.Specification.OMNIVORE)
        {
            _lr.startColor = new Color(1, 1, 0, .5f);
            _lr.endColor = new Color(1, 1, 0, 0);
            return;
        }
        if (_dietary.specification == IDietary.Specification.CARNIVORE)
        {
            _lr.startColor = new Color(1, 0, 0, .5f);
            _lr.endColor = new Color(1, 0, 0, 0);
            return;
        }
        if (_dietary.specification == IDietary.Specification.HERBIVORE)
        {
            _lr.startColor = new Color(0, 1, 0, .5f);
            _lr.endColor = new Color(0, 1, 0, 0);
            return;
        }
    }

    private Color GetStatusColor()
    {
        float alpha = .25f;
        if (_creature.StatusManager.status == StatusManager.Status.wandering)
            return new Color(.8f, .8f, .8f, alpha);     //LIGHT GREY
        if (_creature.StatusManager.status == StatusManager.Status.thirsty)
            return new Color(.5f, .75f, 1f, alpha);     //LIGHT BLUE
        if (_creature.StatusManager.status == StatusManager.Status.dehydrated)
            return new Color(0, .28f, .55f, alpha);     //DARK BLUE
        if (_creature.StatusManager.status == StatusManager.Status.hungry)
            return new Color(1, .83f, .6f, alpha);      //LIGHT ORANGE
        if (_creature.StatusManager.status == StatusManager.Status.starving)
            return new Color(.78f, .52f, .16f, alpha);  //DARK ORANGE
        if (_creature.StatusManager.status == StatusManager.Status.fleeing)
            return new Color(.56f, 1, .63f, alpha);     //LIGHT GREEN
        if (_creature.StatusManager.status == StatusManager.Status.hunting)
            return new Color(.9f, .34f, .34f, alpha);   //RED
        if (_creature.StatusManager.status == StatusManager.Status.looking_for_partner)
            return new Color(1, .6f, .84f, alpha);      //PINK
        if (_creature.StatusManager.status == StatusManager.Status.sleeping)
            return new Color(.4f, .4f, .4f, alpha);     //DARK GREY

        return Color.black;                             //ERROR COLOR
    }
    #endregion
}
