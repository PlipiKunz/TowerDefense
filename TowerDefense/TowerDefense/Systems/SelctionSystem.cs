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
        int clickx = 0;
        int clicky = 0;

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

            var mousePos = mouse.GetComponent<Components.Position>();
            var adjustedPos = CoordinateSystem.convertGameToPix(mousePos.x, mousePos.y, 0, 0);
            if (clickedItem == null && CoordinateSystem.inGameBounds((int)adjustedPos.X, (int)adjustedPos.Y))
            {
                SelectedTower = null;
                removeEntities();
                blankClick(mouse);
            }
            else if (clickedItem != null)
            {
                if (clickedItem.ContainsComponent<Components.TowerComponent>())
                {
                    if (SelectedTower == clickedItem)
                    {
                        removeEntities();
                        SelectedTower = null;
                    }
                    else
                    {
                        towerClick(mouse, clickedItem);
                        SelectedTower = clickedItem;
                    }


                }
                else
                {
                    var action = clickedItem.GetComponent<Components.MenuComponent>();

                    removeEntities();
                    doAction(action.towerClass);
                }
            }
            else { 
                removeEntities();
            }


        }

        public void doAction(Components.towerClass tc) {
            if (tc == Components.towerClass.Sell)
            {
                sell();
            }
            else if (tc == Components.towerClass.Upgrade)
            {
                upgrade();
            }
            else {
                createNewTower(tc);
            }
        }

        public void blankClick(Entity mouse)
        {
            removeEntities();
            var mousePos = mouse.GetComponent<Components.Position>();

            clickx = (int)Math.Floor(mousePos.x);
            clicky = (int)Math.Floor(mousePos.y);


            var range = box.createCirc(clickx + .5f, clicky + .5f, (int)Tower.STANDARD_RANGE * 2 + 1, (int)Tower.STANDARD_RANGE * 2 + 1);
            menuEntites.Add(range);


            string label = "New Tower";
            var towerLabel = MenuItem.createSimpleMenuItem(11, 0, label);
            menuEntites.Add(towerLabel);

            var nextY = towerLabel.GetComponent<Components.Position>().y + towerLabel.GetComponent<Components.Position>().h * 2;
            label = "Projectile: " + Tower.STANDARD_COST + "g";
            towerLabel = MenuItem.createActionMenuItem(11, nextY, label, Components.towerClass.Projectile);
            menuEntites.Add(towerLabel);

            nextY = towerLabel.GetComponent<Components.Position>().y + towerLabel.GetComponent<Components.Position>().h * 2;
            label = "Missle: " + Tower.COMPLEX_COST + "g";
            towerLabel = MenuItem.createActionMenuItem(11, nextY, label, Components.towerClass.Missle);
            menuEntites.Add(towerLabel);

            nextY = towerLabel.GetComponent<Components.Position>().y + towerLabel.GetComponent<Components.Position>().h * 2;
            label = "Bomb: " + Tower.COMPLEX_COST + "g";
            towerLabel = MenuItem.createActionMenuItem(11, nextY, label, Components.towerClass.Bomb);
            menuEntites.Add(towerLabel);

            nextY = towerLabel.GetComponent<Components.Position>().y + towerLabel.GetComponent<Components.Position>().h * 2;
            label = "Homing: " + Tower.COMPLEX_COST + "g";
            towerLabel = MenuItem.createActionMenuItem(11, nextY, label, Components.towerClass.Other);
            menuEntites.Add(towerLabel);

            addEntities();
        }

        public void towerClick(Entity mouse, Entity Tower)
        {
            removeEntities();

            var towerComp = Tower.GetComponent<Components.TowerComponent>();
            var towerCost = Tower.GetComponent<Components.Cost>();

            var mousePos = mouse.GetComponent<Components.Position>();
            int gridX = (int)Math.Floor(mousePos.x);
            int gridY = (int)Math.Floor(mousePos.y);

            string label = towerComp.tc.ToString() + " Tower";
            var towerLabel = MenuItem.createSimpleMenuItem(11, 0, label);
            menuEntites.Add(towerLabel);

            var nextY = towerLabel.GetComponent<Components.Position>().y + towerLabel.GetComponent<Components.Position>().h;
            label = "Level: " + (towerComp.level + 1).ToString();
            towerLabel = MenuItem.createSimpleMenuItem(11, nextY, label);
            menuEntites.Add(towerLabel);


            nextY = towerLabel.GetComponent<Components.Position>().y + towerLabel.GetComponent<Components.Position>().h*2;
            label = "Sell: " + towerCost.cost.ToString() + "g";
            towerLabel = MenuItem.createActionMenuItem(11, nextY, label, Components.towerClass.Sell);
            menuEntites.Add(towerLabel);

            var range = box.createCirc(gridX + .5f, gridY + .5f, (int)towerComp.range*2+1, (int)towerComp.range*2+1);
            menuEntites.Add(range);

            if (towerComp.level < 2)
            {
                nextY = towerLabel.GetComponent<Components.Position>().y + towerLabel.GetComponent<Components.Position>().h * 2;
                label = "Upgrade: " + ((int)(towerCost.cost * 1.25f)).ToString() + "g";
                towerLabel = MenuItem.createActionMenuItem(11, nextY, label, Components.towerClass.Upgrade);
                menuEntites.Add(towerLabel);
            }


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

        public void upgrade()
        {
            removeEntities();
            if (SelectedTower != null)
            {
                var towerComp = SelectedTower.GetComponent<Components.TowerComponent>();
                if (towerComp.level < 2)
                {
                    int cost = (int)(SelectedTower.GetComponent<Components.Cost>().cost * 1.25);
                    if (GameModel.funds >= cost)
                    {

                        Renderer.buildSound.Play();
                        var towerPos = SelectedTower.GetComponent<Components.Position>();

                        var newTower = createTower((int)towerPos.x, (int)towerPos.y, (int)towerComp.level + 1, towerComp.tc);
                        newTower.GetComponent<Components.Cost>().cost += (uint)cost;

                        GameModel.m_addThese.Add(newTower);
                        GameModel.m_removeThese.Add(SelectedTower);
                        GameModel.funds -= cost;
                    }
                    else
                    {
                        string label = "Insufficient Funds";
                        var towerLabel = MenuItem.createSimpleMenuItem(11, 0, label);
                        menuEntites.Add(towerLabel);
                    }

                    SelectedTower = null;
                }
                else
                {
                    string label = "No Upgrades Left";
                    var towerLabel = MenuItem.createSimpleMenuItem(11, 0, label);
                    menuEntites.Add(towerLabel);
                }
                addEntities();
            }
        }

        public void sell()
        {
            removeEntities();
            if (SelectedTower != null) {

                Renderer.sellSound.Play();
                var funds = SelectedTower.GetComponent<Components.Cost>().cost;
                GameModel.funds += (int)funds;
                GameModel.m_removeThese.Add(SelectedTower);
                Renderer.m_text_emitter.addText(SelectedTower, "$");


                SelectedTower = null;

            }
        }

        public void createNewTower(Components.towerClass tc) {

            uint cost = Tower.COMPLEX_COST;
            if (tc == Components.towerClass.Projectile) {
                cost = Tower.STANDARD_COST;
            }

            if (GameModel.funds >= cost)
            {
                var tower = createTower(clickx, clicky, 0, tc);

                if (CreepMovement.Instance().validTowerLocation(tower))
                {
                    CreepMovement.Instance().upToDate = false;
                    GameModel.m_addThese.Add(tower);
                    GameModel.funds -= (int)cost;

                    Renderer.buildSound.Play();
                }
                else
                {
                    string label = "Invalid Location";
                    var towerLabel = MenuItem.createSimpleMenuItem(11, 0, label);
                    menuEntites.Add(towerLabel);
                    addEntities();
                }
            }
            else
            {
                string label = "Insufficient Funds";
                var towerLabel = MenuItem.createSimpleMenuItem(11, 0, label);
                menuEntites.Add(towerLabel);
                addEntities();
            }
        }

        public Entity createTower(int x, int y, int level, Components.towerClass tc)
        {
            if (tc == Components.towerClass.Projectile)
            {
                var newTower = Tower.createSimpleTower(x, y, level);
                return newTower;
            }
            else if (tc == Components.towerClass.Bomb)
            {
                var newTower = Tower.createBombTower(x, y, level);
                return newTower;
            }
            else if (tc == Components.towerClass.Missle)
            {
                var newTower = Tower.createMissleTower(x, y, level);
                return newTower;
            }
            else
            {
                var newTower = Tower.createHomingTower(x, y, level);
                return newTower;
            }



        }

    }

}
