using CS5410.TowerDefenseGame;
using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Systems
{
    public class SelectionSystem : System
    {
        static SelectionSystem instance;
        private static object lockObj = new object();
        public static SelectionSystem Instance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance = new SelectionSystem();
                }
            }

            return instance;
        }
        public static void reset()
        {
            lock (lockObj)
            {
                instance = new SelectionSystem();
            }
        }

        protected SelectionSystem()
            : base(typeof(Components.Selectable))
        {
        }


        Entity SelectedTower = null;

        List<Entity> menuEntites = new List<Entity>();

        /// <summary>
        /// Check to see if any movable components collide with any other
        /// collision components.
        ///
        /// Step 1: find all movable components first
        /// Step 2: Test the movable components for collision with other (but not self) collision components
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
        }

        public void click(Entity mouse)
        {
            Entity clickedItem = null;
            foreach (var selectable in m_entities.Values) {
                if (CoordinateSystem.collides(mouse, selectable))
                {
                    clickedItem = selectable;

                    if (clickedItem.ContainsComponent<Components.MenuComponent>()) {
                        break;
                    }
                }
            }


            if (clickedItem == null)
            {
                SelectedTower = null;
                removeEntities();
                blankClick(mouse);
            }
            else
            {
                if (clickedItem.ContainsComponent<Components.TowerComponent>())
                {
                    SelectedTower = clickedItem;
                    towerClick(mouse, clickedItem);
                }
                else
                {
                    var action = clickedItem.GetComponent<Components.MenuComponent>();
                    removeEntities();
                }
            }
        }

        public void blankClick(Entity mouse) { 
            var mousePos = mouse.GetComponent<Components.Position>();

            int gridX = (int)Math.Floor(mousePos.x);
            int gridY = (int)Math.Floor(mousePos.y);


        }

        public void towerClick(Entity mouse, Entity Tower)
        {
            removeEntities();

            var towerComp = Tower.GetComponent<Components.TowerComponent>();
            var mousePos = mouse.GetComponent<Components.Position>();
            int gridX = (int)Math.Floor(mousePos.x);
            int gridY = (int)Math.Floor(mousePos.y);

            string label = towerComp.targetType.ToString();
            var towerLabel = MenuItem.createActionMenuItem(mousePos.x, mousePos.y, label, Components.towerClass.projectile);

            menuEntites.Add(towerLabel);

            addEntities();
        }

        public void removeEntities()
        {
            foreach (var entity in menuEntites) { 
                GameModel.m_removeThese.Add(entity);
            }
            menuEntites.Clear();
        }

        public void addEntities()
        {
            foreach (var entity in menuEntites)
            {
                GameModel.m_addThese.Add(entity);
            }
        }

        public void upgrade() { 
        
        }


        public void sell() {
            if (SelectedTower != null) {
                var funds = SelectedTower.GetComponent<Components.Cost>().cost;
                GameModel.funds += (int)funds;
                GameModel.m_removeThese.Add(SelectedTower);
                removeEntities();

                SelectedTower = null;
            }
        }
    }
}
